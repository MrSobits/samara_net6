namespace Bars.GisIntegration.Gkh.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;

    using Castle.Windsor;

    /// <summary>
    /// Кросаутентификация
    /// </summary>
    public class CrossAuthentification
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить хеш аутентификации для текущего пользователся
        /// </summary>
        /// <returns>Хеш аутентификации</returns>
        public string GetAuthentificationHash()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(userManager))
            {
                var user = userManager.GetActiveUser();

                var login = user == null
                    ? "anonymous"
                    : user.Login;

                return this.GetAuthentificationHashByLogin(login);
            }
        }

        /// <summary>
        /// Получить хеш аутентификации для переданного пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns>Хеш аутентификации</returns>
        public string GetAuthentificationHash(string login)
        {
            return this.GetAuthentificationHashByLogin(login);
        }

        
        private string GetAuthentificationHashByLogin(string login)
        {
            var serverDateTime = this.Container.Resolve<ISessionProvider>()
                .GetCurrentSession()
                .CreateSQLQuery("SELECT CURRENT_TIMESTAMP")
                .UniqueResult<DateTime>();

            var date = new DateTime(
                serverDateTime.Year,
                serverDateTime.Month,
                serverDateTime.Day,
                serverDateTime.Hour,
                (serverDateTime.Minute / 10) * 10,
                0);

            var targetStr = string.Format("{0} {1:dd.MM.yyyy HH:mm} cross_auth", login, date);
            byte[] bytes = Encoding.UTF8.GetBytes(targetStr);

            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(bytes));
        }
    }
}