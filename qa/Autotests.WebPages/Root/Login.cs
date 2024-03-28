using System.Diagnostics.Contracts;
using Autotests.Utilities;
using Autotests.Utilities.WebElement;
using Autotests.WebPages.Properties;

namespace Autotests.WebPages.Root
{
    public class Login : PageBase
    {
        #region DataLogIn
        private static readonly WebElement loginText = new WebElement().ByName("login");
        private static readonly WebElement passwordText = new WebElement().ByName("password");
        private static readonly WebElement submitButton = new WebElement().ByClass("submit");
        #endregion

        public void Open()
        {
            Contract.Requires(BaseUrl != null);
            Contract.Ensures(Browser.Url == BaseUrl | Browser.Url.ToString() == BaseUrl + "?ReturnUrl=%2f");

            Browser.WaitAjax();

            if (Browser.Url == BaseUrl |
                Browser.Url.ToString() == BaseUrl + "?ReturnUrl=%2f") return;

            Browser.Navigate(BaseUrl);

            Contract.Assert((Browser.Url == BaseUrl | Browser.Url.ToString() == BaseUrl + "?ReturnUrl=%2f"), string.Format("{0} != {1}", Browser.Url, BaseUrl));
        }
        public bool DoLogin(string login, string password)
        {
            Contract.Requires(!string.IsNullOrEmpty(login) & !string.IsNullOrEmpty(password));
            loginText.Text = login;
            passwordText.Text = password;
            submitButton.Click(false);
            Browser.WaitReadyState();
            return Browser.Url.ToString() == Settings.Default.TestEnvironment;
        }
    }
}
