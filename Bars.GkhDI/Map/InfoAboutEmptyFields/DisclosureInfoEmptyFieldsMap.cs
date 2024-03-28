namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;


    /// <summary>
    /// Маппинг журнала незаполненных полей
    /// </summary>
    public class DisclosureInfoEmptyFieldsMap : BaseImportableEntityMap<DisclosureInfoEmptyFields>
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "DI_EMPTY_FIELDS_MANORG_LOG";

        /// <summary>
        /// Название поля
        /// </summary>
        public const string FieldName = "NAME";

        /// <summary>
        /// Идентификатор пути к форме с полем
        /// </summary>
        public const string PathId = "PATH_ID";

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public const string ManOrg = "DI_MAN_ORG_ID";

        public DisclosureInfoEmptyFieldsMap() :
                base("Журнал незаполненных полей", DisclosureInfoEmptyFieldsMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FieldName, "FieldName").Column(DisclosureInfoEmptyFieldsMap.FieldName);
            this.Property(x => x.PathId, "PathId").Column(DisclosureInfoEmptyFieldsMap.PathId);
            this.Reference(x => x.ManOrg, "ManOrg").Column(DisclosureInfoEmptyFieldsMap.ManOrg);
        }
    }
}