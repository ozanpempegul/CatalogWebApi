using System.Security.Cryptography;
using System.Text;

namespace CatalogWebApi.Service
{
    public class MD5AndSaltingService
    {
        public string MD5Salting(string pwd, string email)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(pwd + email));
            string result = BitConverter.ToString(bytes).Replace("-", String.Empty);
            return result.ToLower();
        }
    }
}
