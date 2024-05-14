using Fyp.Dto;

namespace Fyp.Interfaces

{
    public interface IEmailRepository
    {
        void SendVerificationCode(string userEmail);
    }
}
