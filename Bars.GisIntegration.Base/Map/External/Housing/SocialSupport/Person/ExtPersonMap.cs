namespace Bars.GisIntegration.Base.Map.External.Housing.SocialSupport.Person
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.SocialSupport.Person;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.Person
    /// </summary>
    public class ExtPersonMap : BaseEntityMap<ExtPerson>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtPersonMap() :
            base("PERSON")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("PERSON_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.Fam, "FAM");
            this.Map(x => x.Name, "NAME");
            this.Map(x => x.FName, "FNAME");
            this.Map(x => x.Sex, "SEX");
            this.Map(x => x.BornOn, "BORN_ON");
            this.References(x => x.Passport, "PASSPORT_ID");
            this.Map(x => x.PassSeries, "PASS_SERIES");
            this.Map(x => x.PassNumber, "PASS_NUMBER");
            this.Map(x => x.IssuedOn, "ISSUED_ON");
            this.Map(x => x.Snils, "SNILS");
            this.Map(x => x.BirthPlace, "BIRTH_PLACE");
            //Map(x => x.IsRegistered, "IS_REGISTERED");
            //Map(x => x.IsResident, "IS_RESIDENT");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
