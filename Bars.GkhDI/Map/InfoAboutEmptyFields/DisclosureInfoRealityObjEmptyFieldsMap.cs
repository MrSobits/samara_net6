namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;


    /// <summary>
    /// Маппинг журнала незаполненных полей
    /// </summary>
    public class DisclosureInfoRealityObjEmptyFieldsMap : BaseImportableEntityMap<DisclosureInfoRealityObjEmptyFields>
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "DI_EMPTY_FIELDS_REALOBJ_LOG";

        /// <summary>
        /// Название поля
        /// </summary>
        public const string FieldName = "NAME";

        /// <summary>
        /// Идентификатор пути к форме с полем
        /// </summary>
        public const string PathId = "PATH_ID";

        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public const string RealityObj = "DI_REALITY_OBJ_ID";

        public DisclosureInfoRealityObjEmptyFieldsMap() :
                base("Журнал незаполненных полей", DisclosureInfoRealityObjEmptyFieldsMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FieldName, "FieldName").Column(DisclosureInfoRealityObjEmptyFieldsMap.FieldName);
            this.Property(x => x.PathId, "PathId").Column(DisclosureInfoRealityObjEmptyFieldsMap.PathId);
            this.Reference(x => x.RealityObj, "RealityObj").Column(DisclosureInfoRealityObjEmptyFieldsMap.RealityObj);
        }
    }
}