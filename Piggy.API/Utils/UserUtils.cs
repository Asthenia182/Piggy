namespace Piggy.API.Utils
{
    public class UserUtils
    {
        public static string GetUsernameOrEmail(User user)
        {
            return string.IsNullOrEmpty(user.Username) ? user.Email : user.Username;
        }

        public static bool IsEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
