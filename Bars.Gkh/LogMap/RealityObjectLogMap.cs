namespace Bars.Gkh.LogMap
{
    using System.Globalization;

    using Bars.B4.Utils;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Маппинг сущности "Жилой дом" для журнала изменений
    /// </summary>
    public class RealityObjectLogMap : AuditLogMap<RealityObject>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RealityObjectLogMap()
        {
            this.Name("Реестр жилых домов");

            this.Description(x => x.Address);

            this.MapProperty(x => x.FiasAddress, "Address", "Адрес", x => x?.AddressName);
            this.MapProperty(x => x.TypeHouse, "TypeHouse", "Тип дома", x => x.GetDisplayName());
            this.MapProperty(x => x.ConditionHouse, "ConditionHouse", "Состояние дома", x => x.GetDisplayName());

            this.MapProperty(x => x.FederalNum, "FederalNum", "Федеральный номер");
            this.MapProperty(x => x.GkhCode, "GkhCode", "Код дома");
            this.MapProperty(x => x.TypeProject, "TypeProject", "Серия, тип проекта", x => x?.Name);
            this.MapProperty(x => x.CapitalGroup, "CapitalGroup", "Группа капитальности", x => x?.Name);
            this.MapProperty(x => x.WebCameraUrl, "WebCameraUrl", "Адрес веб-камеры");
            this.MapProperty(x => x.TypeOwnership, "TypeOwnership", "Форма собственности", x => x?.Name);
            this.MapProperty(x => x.IsInsuredObject, "IsInsuredObject", "Объект застрахован", x => this.BoolToYesNo(x).GetDisplayName());
            this.MapProperty(x => x.PhysicalWear, "PhysicalWear", "Физический износ", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.IsBuildSocialMortgage, "IsBuildSocialMortgage", "Построен по соц. ипотеке", x => x.GetDisplayName());
            this.MapProperty(x => x.CodeErc, "CodeErc", "Код ЕРЦ");

            this.MapProperty(x => x.CadastreNumber, "CadastreNumber", "Кадастровый номер земельного участка");
            this.MapProperty(x => x.TotalBuildingVolume, "TotalBuildingVolume", "Общий строительный объем (куб.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaMkd, "AreaMkd", "Общая площадь МКД (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaCommonUsage, "AreaCommonUsage", "Площадь помещений общего пользования", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaMunicipalOwned, "AreaMunicipalOwned", "Площадь муниципальной собственности (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaGovernmentOwned, "AreaGovernmentOwned", "Площадь государственной собственности (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaLivingOwned, "AreaLivingOwned", "Площадь, находящаяся в собственности граждан", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaLiving, "AreaLiving", "Общая площадь жилых помещений", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaLivingNotLivingMkd, "AreaLivingNotLivingMkd", "Общая площадь жилых и нежилых помещений (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.MaximumFloors, "MaximumFloors", "Максимальная этажность");
            this.MapProperty(x => x.Floors, "Floors", "Минимальная этажность");
            this.MapProperty(x => x.FloorHeight, "FloorHeight", "Высота этажа (м)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.NumberEntrances, "NumberEntrances", "Количество подъездов");
            this.MapProperty(x => x.NumberApartments, "NumberApartments", "Количество квартир");
            this.MapProperty(x => x.NumberLiving, "NumberLiving", "Количество проживающих");
            this.MapProperty(x => x.NumberLifts, "NumberLifts", "Количество лифтов");

            this.MapProperty(x => x.DateLastOverhaul, "DateLastOverhaul", "Дата последнего кап. ремонта");
            this.MapProperty(x => x.IsNotInvolvedCr, "IsNotInvolvedCr", "Дом не участвует в программе КР", x => this.BoolToYesNo(x).GetDisplayName());
            this.MapProperty(x => x.IsRepairInadvisable, "IsRepairInadvisable", "Ремонт нецелесообразен", x => this.BoolToYesNo(x).GetDisplayName());
            this.MapProperty(x => x.IsInvolvedCrTo2, "IsInvolvedCrTo2", "Участвует в программе КР ТО №2", x => this.BoolToYesNo(x ?? false).GetDisplayName());
            this.MapProperty(x => x.BuildYear, "BuildYear", "Год постройки");
            this.MapProperty(x => x.DateCommissioning, "DateCommissioning", "Дата сдачи в эксплуатацию");
            this.MapProperty(x => x.HasPrivatizedFlats, "HasPrivatizedFlats", "Наличие приватизированных квартир", x => this.BoolToYesNo(x ?? false).GetDisplayName());
            this.MapProperty(x => x.PrivatizationDateFirstApartment, "PrivatizationDateFirstApartment", "Дата приватизации первого жилого помещения");
            this.MapProperty(x => x.DateDemolition, "DateDemolition", "Дата сноса");
            this.MapProperty(x => x.IsCulturalHeritage, "IsCulturalHeritage", "Памятник архитектуры", x => this.BoolToYesNo(x).GetDisplayName());
            this.MapProperty(x => x.CadastralHouseNumber, "CadastralHouseNumber", "Кадастровый номер дома");
            this.MapProperty(x => x.AreaOwned, "AreaOwned", "Площадь частной собственности (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaNotLivingFunctional, "AreaNotLivingFunctional", "Общая площадь помещений, входящих в состав общего имущества (кв.м.)", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.AreaCleaning,"AreaCleaning","Уборочная площадь (кв.м.)", x => x?.RoundDecimal(2).ToString());

            this.MapProperty(x => x.NecessaryConductCr, "NecessaryConductCr", "Требовалось проведение КР на дату приватизации первого жилого помещения", x => x.GetDisplayName());
            this.MapProperty(x => x.NumberNonResidentialPremises, "NumberNonResidentialPremises", "Количество нежилых помещений");
            this.MapProperty(x => x.RoofingMaterial, "RoofingMaterial", "Материал кровли", x => x?.Name);
            this.MapProperty(x => x.WallMaterial, "WallMaterial", "Материал стен", x => x?.Name);
            this.MapProperty(x => x.TypeRoof, "TypeRoof", "Тип кровли", x => x.GetDisplayName());
            this.MapProperty(x => x.PercentDebt, "PercentDebt", "Собираемость платежей %", x => x?.RoundDecimal(2).ToString());
            this.MapProperty(x => x.HeatingSystem, "HeatingSystem", "Система отопления", x => x.GetDisplayName());
            this.MapProperty(x => x.HasJudgmentCommonProp, "HasJudgmentCommonProp", "Наличие судебного решения по проведению КР общего имущества", x => x.GetDisplayName());
        }

        private YesNo BoolToYesNo(bool value)
        {
            return value ? YesNo.Yes : YesNo.No;
        }
    }
}
