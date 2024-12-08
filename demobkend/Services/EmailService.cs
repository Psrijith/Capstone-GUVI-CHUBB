using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace demobkend.Services
{
    public class EmailService
    {
        private readonly string _sendGridApiKey ;  

        public EmailService(IConfiguration configuration)
        { 
            _sendGridApiKey = configuration["SendGrid:ApiKey"];
        }


        // Method to send email
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var client = new SendGridClient(_sendGridApiKey);  // Use hardcoded API key
            var from = new EmailAddress("peddireddysrijith@gmail.com", "Your Learning Platform"); // Sender Email
            var to = new EmailAddress(toEmail); // Receiver Email
            var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);

            try
            {
                // Send email
                var response = await client.SendEmailAsync(emailMessage);

                // Check response status code
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    var errorResponse = await response.Body.ReadAsStringAsync();
                    Console.WriteLine($"Error sending email: {response.StatusCode}, Response: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Exception occurred while sending email: {ex.Message}");
            }
        }
    }
}
