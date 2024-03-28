using System.Configuration;

namespace GisGkhLibrary.Crypto
{
    internal static class Params
    {
        /// <summary>
        /// id блока для подписи в сообщении
        /// </summary>
        internal static string ContainerId { get => "block-to-sign"; }

        /// <summary>
        /// Отпечаток сертификата для подписи
        /// </summary>
        internal static string CertificateThumbprint { get => ConfigurationManager.AppSettings["SignSertificateThumbprint"].Replace(" ", ""); }

        /// <summary>
        /// Наш ОГРН
        /// </summary>
        internal static string OGRN { get => ConfigurationManager.AppSettings["OGRN"].Replace(" ", ""); }

        /// <summary>
        /// Наш КПП
        /// </summary>
        internal static string KPP { get => ConfigurationManager.AppSettings["KPP"].Replace(" ", ""); }

        /// <summary>
        /// Логин
        /// </summary>
        internal static string UserName { get => ConfigurationManager.AppSettings["GisGkhUserName"]; }

        /// <summary>
        /// Пароль
        /// </summary>
        internal static string Password { get => ConfigurationManager.AppSettings["GisGkhPassword"]; }

    }
}