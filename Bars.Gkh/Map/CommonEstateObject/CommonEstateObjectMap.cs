namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.Gkh.Entities.CommonEstateObject;

    /// <summary>Маппинг для "Объект общего имущества"</summary>
    public class CommonEstateObjectMap : BaseImportableEntityMap<CommonEstateObject>
    {

        /// <summary>
        /// .ctor
        /// </summary>
        public CommonEstateObjectMap() :
                base("Объект общего имущества", "OVRHL_COMMON_ESTATE_OBJECT")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CEO_CODE").Length(200).NotNull();
            this.Property(x => x.ReformCode, "Код реформы").Column("REFORM_CODE").Length(10);
            this.Property(x => x.GisCode, "Код ГИС").Column("GIS_CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(500).NotNull();
            this.Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(300).NotNull();
            this.Property(x => x.IsMatchHc, "Флаг: Соответствует ЖК РФ").Column("IS_MATCH_HC").DefaultValue(false).NotNull();
            this.Property(x => x.IncludedInSubjectProgramm, "Флаг: Включен в программу субъекта").Column("INC_IN_SUBJ_PRG").DefaultValue(false).NotNull();
            this.Property(x => x.IsEngineeringNetwork, "Флаг: Является инженерной сетью").Column("IS_ENG_NETWORK").DefaultValue(false).NotNull();
            this.Property(x => x.MultipleObject, "Множественный объект").Column("MULT_OBJECT").DefaultValue(false).NotNull();
            this.Property(x => x.Weight, "Вес").Column("WEIGHT").NotNull();
            this.Property(x => x.IsMain, "Является основным").Column("IS_MAIN").DefaultValue(false).NotNull();
            this.Reference(x => x.GroupType, "Тип группы").Column("CEO_GROUP_TYPE_ID").NotNull().Fetch();
        }
    }

    public class CommonEstateObjectNhMap : BaseHaveExportIdMapping<CommonEstateObject>
    {
    }
}
