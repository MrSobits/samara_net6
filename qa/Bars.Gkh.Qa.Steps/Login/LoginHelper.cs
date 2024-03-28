namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;

    using Bars.B4;

    class LoginHelper : BindingBase
    {
        public static void CreateUserPassword(string login, string password, string name = "test")
        {
            Container.Resolve<IUserManager>().SaveUser(0, name, login, password);
        }
    }
}
