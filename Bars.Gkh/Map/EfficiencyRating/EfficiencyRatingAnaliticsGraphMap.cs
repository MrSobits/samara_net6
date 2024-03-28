namespace Bars.Gkh.Map.EfficiencyRating
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Domain.PlotBuilding.Model;
    using Bars.Gkh.Entities.EfficiencyRating;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг <see cref="EfficiencyRatingAnaliticsGraph"/>
    /// </summary>
    public class EfficiencyRatingAnaliticsGraphMap : BaseEntityMap<EfficiencyRatingAnaliticsGraph>
    {
        /// <inheritdoc />
        public EfficiencyRatingAnaliticsGraphMap()
            : base("Bars.Gkh.Entities.EfficiencyRating.EfficiencyRatingAnaliticsGraph", "GKH_EF_ANALITICS_GRAPH")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
            this.Property(x => x.AnaliticsLevel, "Уровень детализации").Column("LEVEL").NotNull();
            this.Property(x => x.Category, "Категория графика").Column("CATEGORY").NotNull();
            this.Property(x => x.ViewParam, "Отображать график в разрезе").Column("VIEW_PARAM").NotNull();
            this.Property(x => x.FactorCode, "Фактор").Column("FACTOR_CODE").NotNull();
            this.Property(x => x.DiagramType, "Тип диаграммы").Column("TYPE_DIAGRAM").NotNull();
        }
    }

    /// <summary>
    /// Маппинг Nh
    /// </summary>
    public class EfficiencyRatingAnaliticsGraphNHibernateMap : ClassMapping<EfficiencyRatingAnaliticsGraph>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public EfficiencyRatingAnaliticsGraphNHibernateMap()
        {
            this.Set(
                x => x.Periods,
                mapper =>
                {
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Cascade(Cascade.Persist);
                    mapper.Key(k => k.Column("ANALITICS_ID"));
                    mapper.Table("GKH_EF_ANALITICS_PERIOD_REL");
                },
                relation => relation.ManyToMany(k => k.Column("PERIOD_ID")));

            this.Set(
                x => x.Municipalities,
                mapper =>
                {
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Cascade(Cascade.Persist);
                    mapper.Key(k => k.Column("ANALITICS_ID"));
                    mapper.Table("GKH_EF_ANALITICS_MU_REL");
                },
                relation => relation.ManyToMany(k => k.Column("MUNICIPALITY_ID")));

            this.Set(
                x => x.ManagingOrganizations,
                mapper =>
                {
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Cascade(Cascade.Persist);
                    mapper.Key(k => k.Column("ANALITICS_ID"));
                    mapper.Table("GKH_EF_ANALITICS_MANORG_REL");
                },
                relation => relation.ManyToMany(k => k.Column("MANORG_ID")));

            this.Property(x => x.Data, m => m.Type<ImprovedBinaryJsonType<BasePlotData>>());
        }
    }
}