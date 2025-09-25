namespace FitnessCenter1.Services.Abstract
{
    public interface IEmailService
    {
        Task SendRegistrationEmail(string email, string name);
        Task SendOTPEmail(string email, string otp);
        Task SendOrderConfirmationEmail(string email, string orderDetails);
        Task SendPasswordResetEmail(string email);
        Task SendMembershipConfirmationEmail(string email, string membershipDetails);
        Task SendPaymentConfirmationEmail(string email, string paymentDetails);
    }
}