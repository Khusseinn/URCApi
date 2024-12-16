using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using URCApi.Entitites;

namespace URCApi.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // SMTP server
        private readonly int _smtpPort = 587; // SMTP port
        private readonly string _emailFrom = "mammadovhuseyn313@gmail.com"; // Göndərən email
        private readonly string _emailPassword = "gqhc gdqc dxsq wvps"; // Göndərən emailin şifrəsi
        private readonly string _emailTo = "mammadovhuseyn313@gmail.com"; // Alıcı email (daimi)


        public async Task SendContactEmailAsync(Contact contact)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your Application", _emailFrom));
            email.To.Add(new MailboxAddress("Recipient", _emailTo));
            email.Subject = $"Yeni müraciət: {contact.Subject}";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h3>Yeni müraciət məlumatları:</h3>
                    <p><strong>Ad:</strong> {contact.Name}</p>
                    <p><strong>Email:</strong> {contact.Mail}</p>
                    <p><strong>Mövzu:</strong> {contact.Subject}</p>
                    <p><strong>Mətni:</strong> {contact.Text}</p>"
            };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtpClient = new SmtpClient();
            try
            {
                await smtpClient.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_emailFrom, _emailPassword);
                await smtpClient.SendAsync(email);
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
            }
        }
    }
}