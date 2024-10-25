namespace Payrollfix_poc.IRepository
{
    public interface IServicesRepository
    {
        string GenerateRandomPassword();
        void SendResetPasswordEmail(string email, string newPassword);
    }
}
