namespace Bars.Gkh.Overhaul.Hmao.Map.Version
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    /// <summary>
    /// Маппинг для <see cref="VersionActualizeLogRecord"/>
    /// </summary>
    public class VersionActualizeLogRecordMap : BaseEntityMap<VersionActualizeLogRecord>
    {
        /// <inheritdoc />
        public VersionActualizeLogRecordMap()
            : base(typeof(VersionActualizeLogRecord).FullName, "OVRHL_ACTUALIZE_LOG_RECORD")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ActualizeLog, "Лог актуализации версии программы").Column("ACTUALIZE_LOG_ID").NotNull();
            this.Property(x => x.Action, "Действие").Column("ACTION");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID");
            this.Property(x => x.WorkCode, "Код работы").Column("WORK_CODE");
            this.Property(x => x.Ceo, "ООИ").Column("CEO");
            this.Property(x => x.PlanYear, "Плановый год").Column("PLAN_YEAR");
            this.Property(x => x.ChangePlanYear, "Изменение плановый год").Column("CHANGE_PLAN_YEAR");
            this.Property(x => x.PublishYear, "Опубликованный год").Column("PUBLISH_YEAR");
            this.Property(x => x.ChangePublishYear, "Изменение опубликованный год").Column("CHANGE_PUBLISH_YEAR");
            this.Property(x => x.Volume, "Объем").Column("VOLUME");
            this.Property(x => x.ChangeVolume, "Изменение объема").Column("CHANGE_VOLUME");
            this.Property(x => x.Sum, "Сумма").Column("SUM");
            this.Property(x => x.ChangeSum, "Изменение суммы").Column("CHANGE_SUM");
            this.Property(x => x.Number, "Номер").Column("NUMBER");
            this.Property(x => x.ChangeNumber, "Изменение номера").Column("CHANGE_NUMBER");
        }
    }
}