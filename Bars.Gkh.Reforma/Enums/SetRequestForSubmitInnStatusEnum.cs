namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    public enum SetRequestForSubmitInnStatusEnum {
        /// <summary>
        /// Ошибок нет. Заявка подана.
        /// </summary>
        [Display("Ошибок нет. Заявка подана.")]
        Success = 0,

        /// <summary>
        /// Не указан ИНН
        /// </summary>
        [Display("Не указан ИНН")]
        Missing_INN = 1,

        /// <summary>
        /// Указан некорректный ИНН
        /// </summary>
        [Display("Указан некорректный ИНН")]
        INN_is_not_valid = 2,

        /// <summary>
        /// Запрос на подписку был уже подан ранее
        /// </summary>
        [Display("Запрос на подписку был уже подан ранее")]
        Request_have_been_already_submitted = 3
    }
}