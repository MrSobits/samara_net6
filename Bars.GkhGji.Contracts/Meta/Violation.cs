namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class Violation
    {
        [Display("Нарушения технического состояния объекта")]
        public bool IsTechnicalStateViolation { get; set; }

        [Display("Адрес")]
        public string Address { get; set; }

        [Display("Номер ПиН")]
        public string RulesNumber { get; set; }

        [Display("Текст нарушения")]
        public string Text { get; set; }

        [Display("Формулировка нарушений")]
        public string Formulation { get; set; }

        [Display("Мероприятия по устранению")]
        public string ActionsForElimination { get; set; }

        [Display("Срок устранения")]
        public DateTime EliminationDate { get; set; }

        [Display("Дата факт. Устранения")]
        public DateTime ActualEliminationDate { get; set; }

        [Display("Описание нарушения (заголовок)")]
        public string DescriptionTitle { get; set; }

        [Display("Описание нарушения (текст)")]
        public string DescriptionText { get; set; }
    }
}
