namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;

    public class DocRequestActionRequestInfoMap : BaseEntityMap<DocRequestActionRequestInfo>
    {
        public DocRequestActionRequestInfoMap()
            : base("Запрошенные сведения действия \"Истребование документов\"", "GJI_ACTCHECK_DOC_REQUEST_ACTION_REQUEST_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.DocRequestAction, "Действие \"Истребование документов\"").Column("DOC_REQUEST_ACTION_ID");
            this.Property(x => x.RequestInfoType, "Тип сведений").Column("REQUEST_INFO_TYPE");
            this.Property(x => x.Name, "Наименование").Column("NAME");
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}