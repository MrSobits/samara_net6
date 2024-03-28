namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание прекращения деятелньости лицензии
    /// </summary>
    public enum TypeManOrgTerminationLicense
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Решение суда об аннулировании
        /// </summary>
        [Display("Решение суда об аннулировании")]
        CourtDecisionAnnul = 10,

        /// <summary>
        /// Прекращение деятельности
        /// </summary>
        [Display("Прекращение деятельности")]
        RequestTerminationActivity = 20,

        /// <summary>
        /// Заявление лицензиата
        /// </summary>
        [Display("Заявление лицензиата")]
        StatementByTheLicensee = 30,

        /// <summary>
        /// Решение уполномоченного органа
        /// </summary>
        [Display("Решение уполномоченного органа")]
        DecisionAuthorizedBody = 40,

        /// <summary>
        /// Ошибка ввода данных
        /// </summary>
        [Display("Ошибка ввода данных")]
        InputError = 50,

        /// <summary>
        /// Решение ОГК в связи с назначением два и более раз админ. наказания за неисполнение или ненадлежащее исполнение предписания
        /// </summary>
        [Display("Решение ОГК в связи с назначением два и более раз админ. наказания за неисполнение или ненадлежащее исполнение предписания")]
        DecisionStateControlBody = 60
    }
}
