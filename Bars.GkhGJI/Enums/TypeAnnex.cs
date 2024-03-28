namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип приложения
    /// </summary>
    
    public enum TypeAnnex
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary> 
        /// Распоряжение
        /// </summary>
        [Display("Приказ")]
        Disposal = 10,

        /// <summary>
        /// Распоряжение
        /// </summary>
        [Display("Решение")]
        Decision = 15,

        /// <summary>
        /// Уведомление о проверке
        /// </summary>
        [Display("Уведомление о проверке")]
        DisposalNotice = 20,

        /// <summary>
        /// Уведомление о проверке
        /// </summary>
        [Display("Уведомление заявителя")]
        CorrespondentNotice = 25,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт проверки")]
        ActCheck = 30,

        /// <summary>
        /// Проверочный лист
        /// </summary>
        [Display("Проверочный лист")]
        ControlList = 35,

        /// <summary>
        /// Визуальное обследование
        /// </summary>
        [Display("Предписание")]
        Prescription = 40,

        /// <summary>
        /// Уведомление-вызов
        /// </summary>
        [Display("Уведомление-вызов")]
        PrescriptionNotice = 50,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        Protocol = 60,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Resolution = 70,

        /// <summary>
        /// Мотивированный запрос
        /// </summary>
        [Display("Мотивированный запрос")]
        MotivRequest = 80,

        /// <summary>
        /// уведомления о составлении протокола
        /// </summary>
        [Display("Уведомления о составлении протокола")]
        ProtocolNotification = 90,

        /// <summary>
        /// Предостережение
        /// </summary>
        [Display("Акт профилактического визита")]
        PreventiveVisit = 95,

        /// <summary>
        /// Определение
        /// </summary>
        [Display("Определение")]
        ActDefinition = 100,

        /// <summary>
        /// Определение
        /// </summary>
        [Display("Требование о представлении документов")]
        NeedDoc = 110,

        /// <summary>
        /// Предостережение
        /// </summary>
        [Display("Предостережение")]
        AppealCitsAdmonition = 120,

        /// <summary>
        /// Ответ по обращению
        /// </summary>
        [Display("Ответ по обращению")]
        AppealCitsAnswer = 130,

        /// <summary>
        /// Запрос по обращению
        /// </summary>
        [Display("Запрос по обращению")]
        AppealCitsRequest = 140
    }
}
