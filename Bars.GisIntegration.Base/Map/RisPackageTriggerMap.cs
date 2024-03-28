namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.Base.Entities.RisPackageTrigger
    /// </summary>
    public class RisPackageTriggerMap : BaseEntityMap<RisPackageTrigger>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisPackageTriggerMap()
            : base("Bars.GisIntegration.Base.Entities.RisPackageTrigger", "GI_PACKAGE_TRIGGER")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Package, "Package").Column("PACKAGE_ID").Fetch();
            this.Reference(x => x.Trigger, "Trigger").Column("TRIGGER_ID").Fetch();
            this.Property(x => x.State, "State").Column("STATE");
            this.Property(x => x.Message, "Message").Column("MESSAGE").Length(10000);
            this.Property(x => x.ProcessingResult, "ProcessingResult").Column("PROCESSING_RESULT");
            this.Property(x => x.AckMessageGuid, "AckMessageGuid").Column("ACK_MESSAGE_GUID").Length(100);
        }
    }
}
