namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Utils;

    public class ProtocolGjiProxy : DocumentGjiProxy
    {
        [Display("Контрагент")]
        public Contragent Contragent { get; set; }

        [Display("Адрес дома")]
        public List<Realty> RealtyObjs { get; set; }

        [Display("Акт проверки")]
        public ActCheckProxy ActCheck { get; set; }

        [Display("Статьи закона")]
        public List<ArticleLaw> ArticleLaws { get; set; }

        [Display("Инспекторы")]
        public List<Inspector> Inspectors { get; set; }

        [Display("Дата и время рассмотрения дела")]
        public DateTime DateOfProceedings { get; set; }

        [Display("Установил")]
        public string Description { get; set; }

        [Display("Дата и время рассмотрения дела.Час")]
        public int HourOfProceedings { get; set; }

        [Display("Дата и время рассмотрения дела.Мин")]
        public int MinuteOfProceedings { get; set; }

        [Display("Место рассмотрения дела")]
        public string ProceedingsPlace { get; set; }
    }
}