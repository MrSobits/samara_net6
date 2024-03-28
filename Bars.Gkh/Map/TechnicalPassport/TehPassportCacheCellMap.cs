namespace Bars.Gkh.Map.TechnicalPassport
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DomainService.TechPassport.Impl;
    using Bars.Gkh.Entities;

    public class TehPassportCacheCellMap : PersistentObjectMap<TehPassportCacheCell>
    {
        public TehPassportCacheCellMap()
            : base(typeof(TehPassportCacheCell).FullName, TehPassportCacheService.ViewFullName)
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.RealityObjectId, nameof(TehPassportCacheCell.RealityObjectId)).Column("reality_obj_id");
            this.Property(x => x.FormCode, nameof(TehPassportCacheCell.FormCode)).Column("form_code");
            this.Property(x => x.RowId, nameof(TehPassportCacheCell.RowId)).Column("row_id");
            this.Property(x => x.ColumnId, nameof(TehPassportCacheCell.ColumnId)).Column("column_id");
            this.Property(x => x.Value, nameof(TehPassportCacheCell.Value)).Column("value");
        }
    }
}
