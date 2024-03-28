namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    public enum AddressType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Фактический адрес нахождения
        /// </summary>
        [Display("Фактический адрес")]
        FactAddress = 10,

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        [Display("Адрес за пределами субъекта")]
        AddressOutsideSubject = 20,

        /// <summary>
        /// Электронная почта
        /// </summary>
        [Display("Электронная почта")]
        Email = 30,

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [Display("Юридический адрес")]
        JuridicalAddress = 40,

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        [Display("Почтовый адрес")]
        MailingAddress = 50
    }
}
