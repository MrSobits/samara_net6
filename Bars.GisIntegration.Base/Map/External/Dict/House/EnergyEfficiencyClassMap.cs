namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.EnergyEfficiencyClass
    /// </summary>
    public class EnergyEfficiencyClassMap : BaseEntityMap<EnergyEfficiencyClass>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EnergyEfficiencyClassMap() :
            base("NSI_ENERGY_EFFICIENCY_CLASS")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("ENERGY_EFFICIENCY_CLASS_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "ENERGY_EFFICIENCY_CLASS");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
