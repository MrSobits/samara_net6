namespace Bars.Gkh.Ris.Map.External.Dict.SocialSupport
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.Gkh.Ris.Entities.External.Dict.SocialSupport;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Passport
    /// </summary>
    public class PassportMap : BaseEntityMap<Passport>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PassportMap() :
            base("NSI_PASSPORT")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("PASSPORT_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.PassportName, "PASSPORT");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
