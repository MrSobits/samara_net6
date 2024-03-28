namespace Bars.Gkh.Repair.Map.RepairObject
{
    using System;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Repair.Entities;

    /// <summary>
    /// Мапинг "Bars.Gkh.Repair.Entities.PerformedRepairWorkAct"
    /// </summary>
    public class PerformedRepairWorkActMap : BaseEntityMap<PerformedRepairWorkAct>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PerformedRepairWorkActMap()
            : base("Bars.Gkh.Repair.Entities.PerformedRepairWorkAct", "RP_PERFORMED_REPAIR_WORK_ACT")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ObjectPhotoDescription, "ObjectPhotoDescription").Column("OBJ_PHOTO_DESC");
            this.Property(x => x.ActDate, "ActDate").Column("ACT_DATE");
            this.Property(x => x.ActNumber, "ActNumber").Column("ACT_NUMBER");
            this.Property(x => x.PerformedWorkVolume, "PerformedWorkVolume").NotNull().Column("WORK_VOLUME");
            this.Property(x => x.ActSum, "ActSum").NotNull().Column("ACT_SUM");
            this.Property(x => x.ActDescription, "ActDescription").Column("ACT_DESC");
            this.Reference(x => x.RepairWork, "RepairWork").Column("RP_WORK_ID").NotNull().Fetch();
            this.Reference(x => x.ObjectPhoto, "ObjectPhoto").Column("OBJECT_PHOTO_ID").Fetch();
            this.Reference(x => x.ActFile, "ActFile").Column("ACT_FILE_ID").Fetch();
        }
    }
}
