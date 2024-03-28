namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Маппинг сущности <see cref="KnmReason"/>
    /// </summary>
    public class KnmReasonMap : BaseEntityMap<KnmReason>
    {
        public KnmReasonMap()
            : base(nameof(KnmReason), "GJI_KNM_REASON")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ErknmTypeDocument, "Тип документа ЕРКНМ").Column("ERKNM_TYPE_DOCUMENTS_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            this.Reference(x => x.Decision, "Приказ").Column("DECISION_ID").NotNull().Fetch();
            this.Property(x => x.ErknmGuid, "Идентификатор в ЕРКНМ").Column("ERKNM_GUID").Length(36);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION");
        }
    }
}