namespace Bars.Gkh.Enums
{
    /// <summary>
    /// Перечисление форматов электронной подписи семейства CAdES.
    /// </summary>
    public enum CadesType
    {
        /// <summary>
        /// Тип подписи по умолчанию (CAdES-X Long Type 1)
        /// </summary>
        CADESCOM_CADES_DEFAULT = 0,

        /// <summary>
        /// Тип подписи CAdES BES
        /// </summary>
        CADESCOM_CADES_BES = 1,

        /// <summary>
        /// Тип подписи CAdES T
        /// </summary>
        CADESCOM_CADES_T = 5,

        /// <summary>
        /// Тип подписи CAdES-X Long Type 1
        /// </summary>
        CADESCOM_CADES_X_LONG_TYPE_1 = 93,

        /// <summary>
        /// Тип подписи PKCS7
        /// </summary>
        CADESCOM_PKCS7_TYPE = 0xFFFF
    }
}