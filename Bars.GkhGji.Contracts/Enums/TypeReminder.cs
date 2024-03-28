﻿namespace Bars.GkhGji.Contracts.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип напоминания
    /// </summary>
    public enum TypeReminder
    {
        [Display("Обращение")]
        Statement = 10,

        [Display("Основание проверки")]
        BaseInspection = 20,

        [Display("Распоряжение")]
        Disposal = 30,


        [Display("Приказ")]
        DisposalPr = 35,

        [Display("Предписание")]
        Prescription = 40,

        // Нужно только в РТ
        [Display("Постановление")]
        Resolution = 50,

        // Нужно только в НСО
        [Display("Акт проверки")]
        ActCheck = 60,

        // Нужно только в НСО
        [Display("Уведомление о начале проверки")]
        NoticeOfInspection = 70,

        // Нужно только в Томске
        [Display("Протокол")]
        Protocol = 80
    }
}
