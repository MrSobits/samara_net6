namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус ПИР по неплательщику
    /// </summary>
    public enum DebtorState
    {
        /// <summary>
        /// Первый уровень задолженности
        /// </summary>
        [Display("Первый уровень задолженности")]
        StartDebt = 0,

        /// <summary>
        /// Требует формирование уведомления
        /// </summary>
        [Display("Требует формирование уведомления")]
        NotificationNeeded = 10,

        /// <summary>
        /// Сформировано уведомление
        /// </summary>
        [Display("Сформировано уведомление")]
        NotificationFormed = 20,

        /// <summary>
        /// Требует формирование претензии
        /// </summary>
        [Display("Требует формирование претензии")]
        PretensionNeeded = 30,

        /// <summary>
        /// Сформирована претензия
        /// </summary>
        [Display("Сформирована претензия")]
        PretensionFormed = 40,

        /// <summary>
        /// Требует формирование исковой работы
        /// </summary>
        [Display("Требует формирование исковой работы")]
        LawsuitNeeded = 50,

        /// <summary>
        /// Сформировано исковое заявление
        /// </summary>
        [Display("Сформировано исковое заявление")]
        PetitionFormed = 60,

        /// <summary>
        /// Сформировано заявление о выдаче суд.приказа
        /// </summary>
        [Display("Сформировано заявление о выдаче суд.приказа")]
        CourtOrderClaimFormed = 70,

        /// <summary>
        /// Начато исполнительное производство
        /// </summary>
        [Display("Начато исполнительное производство")]
        StartedEnforcement = 80,

        /// <summary>
        /// Задолженность погашена
        /// </summary>
        [Display("Задолженность погашена")]
        PaidDebt = 100,

        /// <summary>
        /// Задолженность погашена
        /// </summary>
        [Display("Приостановлена для корректировки ЛС")]
        PausedChangeAcc = 110,

        /// <summary>
        /// Задолженность погашена
        /// </summary>
        [Display("Приостановлена до получения информации о несовершеннолетнем")]
        PausedGetInfoUnderage = 120,

        /// <summary>
        /// Вынесен судебный приказ
        /// </summary>
        [Display("Вынесен судебный приказ")]
        CourtOrderApproved = 130,


        /// <summary>
        /// Определение об отмене судебного приказа
        /// </summary>
        [Display("Определение об отмене судебного приказа")]
        CourtOrderCancelled = 140,

        /// <summary>
        /// Требует начала исполнительного производства
        /// </summary>
        [Display("Требует начала исполнительного производства")]
        ROSPStartRequired = 150,

        /// <summary>
        /// Поступили оплаты
        /// </summary>
        [Display("Поступили оплаты")]
        PaymentsIncome = 160,

        /// <summary>
        /// Отказ в принятии искового заявления
        /// </summary>
        [Display("Отказ в принятии искового заявления")]
        LawSueenDenied = 170,
        
        
        /// <summary>
        /// Заключено соглашение о погашении долга
        /// </summary>
        [Display("Соглашение о погашении долга")]
        RestructDebtConcluded = 180


    }
}