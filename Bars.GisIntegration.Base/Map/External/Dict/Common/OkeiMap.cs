namespace Bars.GisIntegration.Base.Map.External.Dict.Common
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Okei
    /// </summary>
    public class OkeiMap : BaseEntityMap<Okei>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public OkeiMap() :
            base("OKEI")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("OKEI_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.OkeiName, "OKEI_NAME");
            this.Map(x => x.NationalSymbol, "NATIONAL_SYMBOL");
            this.Map(x => x.WorldSymbol, "WORLD_SYMBOL");
            this.Map(x => x.NationalCode, "NATIONAL_CODE");
            this.Map(x => x.WorldCode, "WORLD_CODE");
            this.Map(x => x.Comment, "COMMENT");
            this.Map(x => x.OkeiCode, "OKEI_CODE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
