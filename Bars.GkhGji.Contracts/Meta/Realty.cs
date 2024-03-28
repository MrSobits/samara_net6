namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class Realty
    {
        [Display("Муниципальное образование")]
        public string Municipality { get; set; }

        [Display("Тип населенного пункта")]
        public string SettlementType { get; set; }

        [Display("Наименование населенного пункта")]
        public string SettlementName { get; set; }

        [Display("Наименование улицы")]
        public string StreetName { get; set; }

        [Display("Тип улицы")]
        public string StreetType { get; set; }

        [Display("Номер дома")]
        public string HouseNumber { get; set; }

        [Display("Номер корпуса")]
        public string HousingNumber { get; set; }

        [Display("Номер секции")]
        public string SectionNumber { get; set; }

        [Display("Кол-во подъездов (секций)")]
        public int EntrancesCount { get; set; }

        [Display("Материал стен")]
        public string WallMaterial { get; set; }

        [Display("Общая площадь")]
        public string TotalArea { get; set; }

        [Display("Максимальная этажность")]
        public string FloorsMax { get; set; }

        [Display("Минимальная этажность")]
        public string FloorsMin { get; set; }

        [Display("Дата сдачи в эксплуатацию")]
        public DateTime DeliveryDate { get; set; }

        [Display("Материал кровли")]
        public string RoofMaterial { get; set; }

        [Display("Адрес")]
        public string Address { get; set; }
    }
}
