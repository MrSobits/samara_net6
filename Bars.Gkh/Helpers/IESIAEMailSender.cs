namespace Bars.Gkh.Helpers
{
    public interface IESIAEMailSender
    {
        void SendEmail(string orgname, string ogrn, string name, string login, string password);
    }
}
