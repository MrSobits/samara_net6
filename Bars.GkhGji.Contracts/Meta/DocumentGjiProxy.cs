namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class DocumentGjiProxy
    {
        [Display("Год")]
        public int DocYear { get; set; }

        [Display("Номер")]
        public string Number { get; set; }

        [Display("Подномер")]
        public string SubNumber { get; set; }

        [Display("Номер документа")]
        public string DocNumber { get; set; }

        [Display("Дата документа")]
        public DateTime Date { get; set; }

        [Display("Зональное наименование ЗЖИ")]
        public string InspectionNameZoneName { get; set; }

        [Display("Наименование ЗЖИ 1 гос язык")]
        public string InspectionNameLang1 { get; set; }

        [Display("Наименование ЗЖИ 2 гос язык")]
        public string InspectionNameLang2 { get; set; }

        [Display("Адрес ЗЖИ 1 гос язык")]
        public string InspectionAddressLang1 { get; set; }

        [Display("Адрес ЗЖИ 2 гос язык")]
        public string InspectionAddressLang2 { get; set; }

        [Display("Телефон ЗЖИ")]
        public string InspectionPhone { get; set; }

        [Display("Email ЗЖИ")]
        public string InspectionEmail { get; set; }

        [Display("Наименование ЗЖИ 1 гос язык (творительный падеж)")]
        public string InspectionNameLang1_ТворПадеж { get; set; }

        [Display("Наименование ЗЖИ 1 гос язык (родительный падеж)")]
        public string InspectionNameLang1_РодитПадеж { get; set; }

        [Display("Наименование ЗЖИ 1 гос язык (винительный падеж)")]
        public string InspectionNameLang1_ВинитПадеж { get; set; }

        [Display("Контрагент(УО) (родительный падеж)")]
        public string InspectionContragentNameGenitive { get; set; }

        [Display("Адрес контрагента")]
        public string InspectionContragentAddress { get; set; }

        [Display("Корреспондент")]
        public string InspectionAppealCitsCorrespondent { get; set; }

        [Display("Адрес корреспондента")]
        public string InspectionAppealCitsCorrespondentAddress { get; set; }
    }
}
