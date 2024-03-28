namespace Bars.GisIntegration.Base.Map.External.Dict.Common
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.MoTerritory
    /// </summary>
    public class MoTerritoryMap : BaseEntityMap<MoTerritory>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public MoTerritoryMap() :
            base("MO_TERRITORY")
        {
            //Устанавливаем схему РИС
            this.Schema("MASTER");

            this.Id(x => x.Id, m =>
            {
                m.Column("MO_TERRITORY_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.MoName, "MO_NAME");
            this.Map(x => x.Oktmo, "OKTMO");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
