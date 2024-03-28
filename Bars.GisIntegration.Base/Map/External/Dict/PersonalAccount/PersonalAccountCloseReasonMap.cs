namespace Bars.GisIntegration.Base.Map.External.Dict.PersonalAccount
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.PersonalAccount;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class PersonalAccountCloseReasonMap : BaseEntityMap<PersonalAccountCloseReason>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersonalAccountCloseReasonMap() :
            base("NSI_LS_CLOSE_REASON")
        {
            //Устанавливаем схему
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("LS_CLOSE_REASON_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "LS_CLOSE_REASON");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }
    }
}
