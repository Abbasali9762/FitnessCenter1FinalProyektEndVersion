using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using FitnessCenter1.Services.Abstract;

namespace FitnessCenter1.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "abbaseli.sixelizade.2010@gmail.com";
        private readonly string _smtpPassword = "twaz vxxe kteo dnre";
        private readonly bool _enableSsl = true;

        public EmailService()
        {
        }

        public async Task SendRegistrationEmail(string email, string name)
        {
            string subject = "Fitness Center'a Xoş Gəlmisiniz!";
            string body = $@"
Salam {name},

Fitness Center'a qeydiyyatınız uğurla tamamlandı. Xidmətlərimizdən yüksək səmərə ilə istifadə etməyinizə şərait yaratmaq üçün buradayıq.

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOTPEmail(string email, string otp)
        {
            string subject = "Fitness Center - Təhlükəsizlik Kodu";
            string body = $@"
Təhlükəsizlik kodunuz: {otp}

Bu kodu 10 dəqiqə ərzində istifadə edin. Kodun təhlükəsizliyini qoruyun və heç kimlə paylaşmayın.

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendOrderConfirmationEmail(string email, string orderDetails)
        {
            string subject = "Fitness Center - Sifariş Təsdiqi";
            string body = $@"
Sifarişiniz uğurla qəbul edildi.

Sifariş Detalları:
{orderDetails}

Bizi seçdiyiniz üçün təşəkkürlər!

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmail(string email)
        {
            string subject = "Fitness Center - Şifrə Yeniləmə";
            string body = $@"
Şifrə yeniləmə tələbiniz alındı. Şifrəniz uğurla yeniləndi.

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendMembershipConfirmationEmail(string email, string membershipDetails)
        {
            string subject = "Fitness Center - Üzvlük Təsdiqi";
            string body = $@"
Üzvlüyünüz uğurla aktivləşdirildi.

Üzvlük Detalları:
{membershipDetails}

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPaymentConfirmationEmail(string email, string paymentDetails)
        {
            string subject = "Fitness Center - Ödəniş Təsdiqi";
            string body = $@"
Ödənişiniz uğurla tamamlandı.

Ödəniş Detalları:
{paymentDetails}

Hörmətlə,
Fitness Center Komandası
";

            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpUsername);
                    message.To.Add(new MailAddress(toEmail));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = false;

                    using (var client = new SmtpClient(_smtpServer, _smtpPort))
                    {
                        client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                        client.EnableSsl = _enableSsl;

                        await client.SendMailAsync(message);
                    }
                }

                Console.WriteLine($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {toEmail}: {ex.Message}");
                // Email göndərmə xətası baş versə də, proqramın davam etməsinə icazə ver
            }
        }
    }
}