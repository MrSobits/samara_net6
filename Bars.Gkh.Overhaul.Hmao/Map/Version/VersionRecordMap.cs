namespace Bars.Gkh.Overhaul.Hmao.Map.Version
{
    using System.Collections.Generic;

    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.VersionRecord"</summary>
    public class VersionRecordMap : BaseImportableEntityMap<VersionRecord>
    {
        
        public VersionRecordMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.VersionRecord", "OVRHL_VERSION_REC")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ProgramVersion, "Версия программы").Column("VERSION_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Property(x => x.Year, "Плановый Год").Column("YEAR").NotNull();
            this.Property(x => x.FixedYear, "Год зафиксирован").Column("FIXED_YEAR").NotNull();
            this.Property(x => x.CommonEstateObjects, "Строка объектов общего имущества").Column("CEO_STRING").Length(250).NotNull();
            this.Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            this.Property(x => x.IsChangedYear, "Изменялся ли год ремонта").Column("IS_CHANGED_YEAR").DefaultValue(false);
            this.Property(x => x.IndexNumber, "Порядковый номер").Column("INDEX_NUM").NotNull();
            this.Property(x => x.Point, "Балл").Column("POINT").DefaultValue(0m).NotNull();
            this.Property(x => x.StoredCriteria, "Значения критериев сортировки").Column("CRITERIA");
            this.Property(x => x.StoredPointParams, "Значения параметров очередности по баллам").Column("POINT_PARAMS");
            this.Property(x => x.IsManuallyCorrect, "Была ли ручная корректировка записи").Column("IS_MANUALLY_CORRECT").DefaultValue(false).NotNull();
            this.Property(x => x.IsAddedOnActualize, "Была добавлена при актуализации \"Добавить новые записи\"").Column("IS_ADD_ACTUALIZE");
            this.Property(x => x.IsChangedYearOnActualize, "Была добавлена при актуализации \"Актуализировать год\"").Column("IS_CH_YEAR_ACTUALIZE");
            this.Property(x => x.IsChangeSumOnActualize, "Была добавлена при актуализации \"Актуализировать сумму\"").Column("IS_CH_SUM_ACTUALIZE");
            this.Property(x => x.IsDividedRec, "Была добавлена в результате разделения").Column("IS_DIVIDED_REC");
            this.Property(x => x.PublishYearForDividedRec, "Опубликованный год (только для отщепенцев!!!!)").Column("PUBLISH_YEAR_FOR_DIV_REC");
            this.Property(x => x.Changes, "Изменения записи").Column("CHANGES").Length(250);
            this.Property(x=> x.IsChangedPublishYear, "Опубликованный год изменен").Column("IS_CHANGED_PUBLISH_YEAR").DefaultValue(false).NotNull();
            this.Property(x => x.WorkCode, "Код работы").Column("WORK_CODE");
        }
    }

    public class VersionRecordNHibernateMapping : ClassMapping<VersionRecord>
    {
        public VersionRecordNHibernateMapping()
        {
            this.Property(
                x => x.StoredCriteria,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<StoredPriorityParam>>>();
                    });


            this.Property(
                x => x.StoredPointParams,
                m =>
                    {
                        m.Type<ImprovedJsonSerializedType<List<StoredPointParam>>>();
                    });
        }
    }
}