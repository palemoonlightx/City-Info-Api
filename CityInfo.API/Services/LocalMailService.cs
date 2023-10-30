using System.Net.Mail;

namespace CityInfo.API.Services {
    public class LocalMailService : IMailService {


        private readonly string _mailTo = String.Empty;
        private readonly string _mailFrom = String.Empty;


        // Injecting configuration (IConfiguration - a framework service)
        public LocalMailService(IConfiguration configuration) {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }


        public void Send(string subject, string message) {
            // send mail - output to console window
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with ${nameof(LocalMailService)}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");
        }

    }
}
