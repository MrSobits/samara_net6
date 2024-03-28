namespace Bars.GkhCr.Regions.Tatarstan.Permissions
{
    using Bars.Gkh.DomainService;

    public class GkhCrFieldRequirementMap : FieldRequirementMap
    {
        public GkhCrFieldRequirementMap()
        {
            this.Namespace("GkhCr", "Модуль капитальный ремонт");
            this.Namespace("GkhCr.Dict", "Справочники");

            this.Namespace("GkhCr.Dict.WorkRealityObjectOutdoor", "Виды работ по благоустройству дворов");
            this.Namespace("GkhCr.Dict.WorkRealityObjectOutdoor.Field", "Поля");

            this.Requirement("GkhCr.Dict.WorkRealityObjectOutdoor.Field.Code", "Код");
            this.Requirement("GkhCr.Dict.WorkRealityObjectOutdoor.Field.UnitMeasure", "Единица измерения");

            this.Namespace("GkhCr.Dict.ElementOutdoor", "Элементы двора");
            this.Namespace("GkhCr.Dict.ElementOutdoor.Field", "Поля");

            this.Requirement("GkhCr.Dict.ElementOutdoor.Field.Code", "Код");
            this.Requirement("GkhCr.Dict.ElementOutdoor.Field.UnitMeasure", "Единица измерения");
        }
    }
}
