using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace CatalogWebApi
{
    public class Program
    {
        /* Global instance of the scopes required by this quickstart.
        If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "catalogwebapi";


        public static void Main(string[] args)
        {
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Gmail API service.
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define parameters of request.
                UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

                // List labels.
                IList<Label> labels = request.Execute().Labels;
                Console.WriteLine("Labels:");
                if (labels == null || labels.Count == 0)
                {
                    Console.WriteLine("No labels found.");
                    return;
                }
                foreach (var labelItem in labels)
                {
                    Console.WriteLine("{0}", labelItem.Name);
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            // Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
