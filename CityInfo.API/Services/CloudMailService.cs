namespace CityInfo.API.Services {
    public class CloudMailService : IMailService {

        private readonly string _mailTo = String.Empty;
        private readonly string _mailFrom = String.Empty;


        // Injecting configuration (IConfiguration - a framework service)
        public CloudMailService(IConfiguration configuration) {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }


        public void Send(string subject, string message) {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with ${nameof(CloudMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }

    }
}
