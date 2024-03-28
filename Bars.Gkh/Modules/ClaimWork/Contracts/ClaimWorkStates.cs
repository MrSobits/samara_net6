namespace Bars.Gkh.Modules.ClaimWork.Contracts
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Константы с наименованиями статусов ПИР
    /// </summary>
    public static class ClaimWorkStates
    {
        /// <summary>
        /// "Первый уровень задолженности" - начальный статус для должников
        /// </summary>
        public static string FirstLevelDebt = "Первый уровень задолженности";

        /// <summary>
        /// "Первый уровень нарушений" - начальный статус для подрядчиков
        /// </summary>
        public static string FirstLevelViolations = "Первый уровень нарушений";
        #region Уведомеление

        /// <summary>
        /// "Требует формирование уведомления"
        /// </summary>
        [Display(Order = 1)]
        public static string NotificationNeeded = "Требует формирование уведомления";

        /// <summary>
        /// "Сформировано уведомление"
        /// </summary>
        [Display(Order = 3)]
        public static string NotificationFormed = "Сформировано уведомление";

        #endregion

        #region Претензия

        /// <summary>
        /// "Требует формирование претензии"
        /// </summary>
        [Display(Order = 2)]
        public static string PretensionNeeded = "Требует формирование претензии";

        /// <summary>
        /// "Сформирована претензия"
        /// </summary>
        [Display(Order = 5)]
        public static string PretensionFormed = "Сформирована претензия";

        #endregion

        #region Исковое заявление

        /// <summary>
        /// "Требует формирование искового заявления"
        /// </summary>
        [Display(Order = 4)]
        public static string PetitionNeeded = "Требует формирование искового заявления";

        /// <summary>
        /// "Сформировано исковое заявление"
        /// </summary>
        [Display(Order = 7)]
        public static string PetitionFormed = "Сформировано исковое заявление";

        #endregion

        #region Исковая работа

        /// <summary>
        /// Требует формирования исковой работы
        /// </summary>
        [Display(Order = 6)]
        public static string LawsuitNeeded = "Требует формирования исковой работы";

        /// <summary>
        /// Сформировано заявление о выдаче суд.приказа
        /// </summary>
        [Display(Order = 8)]
        public static string CourtOrderClaimFormed = "Сформировано заявление о выдаче суд.приказа";

        /// <summary>
        /// Начато исполнительное производство
        /// </summary>
        public static string StartedEnforcement = "Начато исполнительное производство";

        #endregion

        #region Акт выявления нарушения

        /// <summary>
        /// "Требует формирование акта выявления нарушений"
        /// </summary>
        public static string ViolationIdentificationActNeeded = "Требует формирование акта выявления нарушений";

        /// <summary>
        /// "Сформирован акт выявления нарушений"
        /// </summary>
        public static string ViolationIdentificationActFormed = "Сформирован акт выявления нарушений";

        #endregion

        /// <summary>
        /// "Требует формирование акта нарушений"
        /// </summary>
        public static string ViolationActNeeded = "Требует формирование акта нарушений";

        #region Задолженность по оплате ЖКУ

        /// <summary>
        /// "Черновик"
        /// </summary>
        public static string UtilityDebtorDraft = "Черновик";

        /// <summary>
        /// "В работе ФССП"
        /// </summary>
        public static string UtilityDebtorFsspInWork = "В работе ФССП";

        /// <summary>
        /// "Утверждено ФССП"
        /// </summary>
        public static string UtilityDebtorFsspApproved = "Утверждено ФССП";

        /// <summary>
        /// "На рассмотрении МСА ЖКХ"
        /// </summary>
        public static string UtilityDebtorMsaInWork = "На рассмотрении МСА ЖКХ";


        /// <summary>
        ///  "Утверждено МСА ЖКХ"
        /// </summary>
        public static string UtilityDebtorMsaApproved = "Утверждено МСА ЖКХ";

        #endregion
    }
}