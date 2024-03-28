namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Задание на проектирование"</summary>
    public class SpecialDesignAssignmentMap : BaseImportableEntityMap<SpecialDesignAssignment>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SpecialDesignAssignmentMap() :
            base("Задание на проектирование", "CR_SPECIAL_OBJ_DESIGN_ASSIGNMENT")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWorkCr, "Вид работы").Column("TYPE_WORK_ID").Fetch();
            this.Reference(x => x.DocumentFile, "Файл (документ)").Column("FILE_ID");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.Document, "Документ").Column("DOCUMENT");
            this.Property(x => x.Date, "Дата размещения в системе").Column("DATE");
            this.Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }

    /// <summary>
    /// Маппинг лицевых счетов
    /// </summary>
    public class SpecialPaymentCorrectionSourceNHibernateMapping : ClassMapping<SpecialDesignAssignment>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SpecialPaymentCorrectionSourceNHibernateMapping()
        {
            this.Bag(
                x => x.TypeWorksCr,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.NoLazy);
                    mapper.Key(k => k.Column("ASSIGNMENT_ID"));
                    mapper.Cascade(Cascade.None);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(SpecialDesignAssignmentTypeWorkCr))));
        }
    }
}