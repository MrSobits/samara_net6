namespace Bars.GisIntegration.Base.Map.External.Housing.PersonalAccount
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Housing.PersonalAccount;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ExtPersonalAccountMap : BaseEntityMap<ExtPersonalAccount>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ExtPersonalAccountMap() :
            base("LS_ACCOUNT")
        {
            //Устанавливаем схему РИС
            this.Schema("DATA");

            this.Id(x => x.Id, m =>
            {
                m.Column("LS_ACCOUNT_ID");
                m.Generator(Generators.Native);
            });
            this.References(x => x.DataSupplier, "DATA_SUPPLIER_ID");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.AbonentType, "ABONENT_TYPE_ID");
            this.Map(x => x.AccountNumber, "ACCOUNT_NUMBER");
            this.References(x => x.LsType, "LS_TYPE_ID");
            this.Map(x => x.OpenedOn, "OPENED_ON");
            this.Map(x => x.Fam, "FAM");
            this.Map(x => x.Name, "NAME");
            this.Map(x => x.FName, "FNAME");
            this.Map(x => x.BornOn, "BORN_ON");
            this.Map(x => x.GilCnt, "GIL_CNT");
            this.Map(x => x.TotalSquare, "TOTAL_SQUARE");
            this.Map(x => x.LiveSquare, "LIVE_SQUARE");
            this.Map(x => x.HeatSquare, "HEAT_SQUARE");
            this.Map(x => x.IsClosed, "IS_CLOSED");
            this.References(x => x.PersonalAccountCloseReason, "LS_CLOSE_REASON_ID");
            this.Map(x => x.ClosedOn, "CLOSED_ON");
            this.Map(x => x.CloseComment, "CLOSE_COMMENT");
            this.References(x => x.PayPerson, "PAY_PERSON_ID");
            this.References(x => x.PayContragent, "PAY_CONTRAGENT_ID");
            this.Map(x => x.IsRenter, "IS_RENTER");
            this.Map(x => x.Geu, "GEU");
            this.Map(x => x.IsDel, "IS_DEL");
            this.References(x => x.UoContragent, "UO_CONTRAGENT_ID");
            this.Map(x => x.OwnType, "OWN_TYPE");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }
    }
}
