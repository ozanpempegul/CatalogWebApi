using AutoMapper;
using CatalogWebApi.Base;
using CatalogWebApi.Data;
using CatalogWebApi.Dto;
using Hangfire;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CatalogWebApi.Service
{
    public class TokenManagementService : ITokenManagementService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly byte[] _secret;
        private readonly JwtConfig _jwtConfig;
        private readonly IAccountService _accountService;

        public TokenManagementService(IAccountRepository accountRepository, IAccountService accountService, IMapper mapper, IUnitOfWork unitOfWork, IOptionsMonitor<JwtConfig> jwtConfig) : base()
        {
            this._accountRepository = accountRepository;
            this._accountService = accountService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
            this._jwtConfig = jwtConfig.CurrentValue;
            this._secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        }

        public async Task<BaseResponse<TokenResponse>> GenerateTokensAsync(TokenRequest tokenRequest, DateTime now, string userAgent)
        {
            try
            {
                //MD5 hash and salting 
                string MD5Salting(string pwd)
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(pwd));
                    string result = BitConverter.ToString(bytes).Replace("-", String.Empty);
                    return result.ToLower();
                }
                tokenRequest.Password = MD5Salting(tokenRequest.Password);

                // send email after 3 invalid tries
                var tempAccount2 = await _accountRepository.GetByEmailAsync(tokenRequest.Email);
                var tempAccount3 = _mapper.Map<Account, AccountDto>(tempAccount2);
                if (tempAccount2.invalidtries == 3)
                {
                    BackgroundJob.Enqueue(() => _accountService.SendEmail(tempAccount3, "Account is Blocked", "Your Account is Blocked"));
                    return new BaseResponse<TokenResponse>("Account is blocked");
                };

                // Validate Login-request
                var tempAccount = await _accountRepository.ValidateCredentialsAsync(tokenRequest);                
                await _unitOfWork.CompleteAsync();

                if (tempAccount is null)
                    return new BaseResponse<TokenResponse>("Token_Invalid");




                // Get access-token
                var accessToken = GenerateAccessToken(tempAccount, now);

                // Set Last-Activity value
                tempAccount.LastActivity = DateTime.UtcNow;
                _accountRepository.Update(tempAccount);
                await _unitOfWork.CompleteAsync();

                TokenResponse token = new TokenResponse
                {
                    AccessToken = accessToken,
                    ExpireTime = now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                };

                return new BaseResponse<TokenResponse>(token);
            }
            catch (Exception ex)
            {
                throw new MessageResultException($"Token_Saving_Error {ex}");
            }
        }


        private string GenerateAccessToken(Account account, DateTime now)
        {
            // Get claim value
            Claim[] claims = GetClaim(account);

            var shouldAddAudienceClaim = string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);

            var jwtToken = new JwtSecurityToken(
                _jwtConfig.Issuer,
                shouldAddAudienceClaim ? _jwtConfig.Audience : string.Empty,
                claims,
                expires: now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return accessToken;
        }

        private static Claim[] GetClaim(Account account)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Name, account.UserName),
                new Claim("AccountId", account.Id.ToString()),
            };

            return claims;
        }

    }
}
