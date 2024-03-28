namespace Bars.GisIntegration.Gkh.Security
{
    using System;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Gkh;
    using Bars.GisIntegration.Gkh.Cryptography;
    using Bars.Gkh.Authentification;

    using Castle.Windsor;

    /// <summary>
    /// Менджер РИС
    /// </summary>
    public class RisManager
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager GkhUserManager { get; set; }

        /// <summary>
        /// URL RIS
        /// </summary>
        /// <param name="redirect"> Адрес редиректа </param>
        /// <returns>
        /// Путь к системе
        /// </returns>
        public string GetRisUrl(string redirect)
        {
            var risUrl = Settings.RisUrl;
            if (!string.IsNullOrEmpty(risUrl))
            {
                risUrl = risUrl.TrimEnd('/').TrimStart('/') + "/esialogin/index";
            }

            var gkhOperator = this.GkhUserManager.GetActiveOperator();

            if (gkhOperator.IsNull())
            {
                throw new Exception("Текущий пользователь не является оператором");
            }

            var ticket = string.Join(
                ";",
                redirect,
                DateTime.Now.ToString("dd.MM.yyyy H:mm:ss"),
                gkhOperator.RisToken);

            var encryptedTicket = RijndaelCryptoProvider.Encrypt(ticket.ToUtf8Bytes());

            var result = $"{risUrl}?forceAuth={encryptedTicket}";

            return result;
        }
    }
}