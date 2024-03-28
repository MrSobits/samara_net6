namespace Bars.GisIntegration.Base.Map.External.Contragent
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Contragent;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Contragent
    /// </summary>
    public class ExtContragentMap : BaseEntityMap<ExtContragent>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtContragentMap() :
            base("CONTRAGENT")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("CONTRAGENT_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.FullName, "FULL_NAME");
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.ShortName, "SHORT_NAME");
            this.Map(x => x.Ogrn, "OGRN");
            this.Map(x => x.Inn, "INN");
            this.Map(x => x.Kpp, "KPP");
            this.Map(x => x.RegisteredBy, "REGISTERED_BY");
            this.Map(x => x.RegisteredOn, "REGISTERED_ON");
            this.Map(x => x.Okopf, "OKOPF");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.TerminatedOn, "TERMINATED_ON");
            this.Map(x => x.IsActive, "IS_ACTIVE");
            this.Map(x => x.Website, "WEBSITE");
            this.Map(x => x.ChiefId, "CHIEF_ID");
            this.Map(x => x.Phone, "PHONE");
            this.Map(x => x.Fax, "FAX");
            this.Map(x => x.Email, "EMAIL");
            this.Map(x => x.BookerId, "BOOKER_ID");
            this.References(x => x.FiasJurAddress, "FIAS_JUR_ADDRESS");
            this.References(x => x.FiasFactAddress, "FIAS_FACT_ADDRESS");
            this.References(x => x.FiasMailAddress, "FIAS_MAIL_ADDRESS");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }
    }
}
