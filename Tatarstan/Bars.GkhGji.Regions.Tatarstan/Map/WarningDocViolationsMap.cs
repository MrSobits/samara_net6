namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг полей сущности <see cref="WarningDocViolations"/>
    /// </summary>
    public class WarningDocViolationsMap : BaseEntityMap<WarningDocViolations>
    {
        /// <inheritdoc />
        public WarningDocViolationsMap()
            : base("Нарушения требований (Предостережение)", "GJI_WARNING_DOC_VIOLATIONS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Проверяемый объект").Column("REALITY_OBJECT_ID");
            this.Reference(x => x.WarningDoc, "Предостережение ГЖИ").Column("WARNING_DOC_ID");
            this.Reference(x => x.NormativeDoc, "Нормативный документ").Column("NORMATIVE_DOC_ID");
            
            this.Property(x => x.Description, "Описание нарушения").Column("DESCRIPTION");
            this.Property(x => x.TakenMeasures, "Принятые меры").Column("TAKEN_MEASURES");
            this.Property(x => x.DateSolution, "Срок устранения нарушения").Column("DATE_SOLUTION");
        }
    }

    public class WarningDocViolationsNhMap : ClassMapping<WarningDocViolations>
    {
        /// <inheritdoc />
        public WarningDocViolationsNhMap()
        {
            this.Bag(x => x.ViolationList,
                m =>
                {
                    m.Access(Accessor.Property);
                    m.Fetch(CollectionFetchMode.Select);
                    m.Lazy(CollectionLazy.Lazy);
                    m.Key(x => x.Column("WARNING_DOC_VIOL_ID"));
                    m.Cascade(Cascade.DeleteOrphans | Cascade.Persist);
                    m.Inverse(true);
                },
                x => x.OneToMany());
        }
    }
}