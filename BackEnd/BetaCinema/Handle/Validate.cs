using System.ComponentModel.DataAnnotations;

namespace BetaCinema.Handle
{
    public class Validate
    {
        public static bool IsValidEmail(string email)
        {
            var checkMail = new EmailAddressAttribute();
            return checkMail.IsValid(email);
        }
    }
}
