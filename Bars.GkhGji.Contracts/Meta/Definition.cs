namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class Definition
    {
        [Display("Номер")]
        public string Number { get; set; }

        [Display("Дата")]
        public DateTime Date { get; set; }

        [Display("Тип определения")]
        public string DefinitionType { get; set; }

        [Display("ДЛ, вынесшее решение")]
        public Inspector Official { get; set; }

        [Display("Дата исполнения")]
        public DateTime ExecutionDate { get; set; }

        [Display("Описание")]
        public string Description { get; set; }
    }
}
