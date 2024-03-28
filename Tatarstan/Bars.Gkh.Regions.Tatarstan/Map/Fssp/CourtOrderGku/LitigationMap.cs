namespace Bars.Gkh.Regions.Tatarstan.Map.Fssp.CourtOrderGku
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class LitigationMap : BaseEntityMap<Litigation>
    {
        public LitigationMap()
            : base(typeof(Litigation).FullName, "FSSP_LITIGATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.JurInstitution, "Подразделение ОСП").Column("JUR_INSTITUTION");
            this.Property(x => x.State, "Статус").Column("STATE");
            this.Property(x => x.IndEntrRegistrationNumber, "Регистрационный номер ИП").Column("IND_ENTR_REGISTRATION_NUMBER");
            this.Property(x => x.Debtor, "Должник").Column("DEBTOR");
            this.Reference(x => x.DebtorFsspAddress, "Адрес должника (ФССП)").Column("DEBTOR_FSSP_ADDRESS_ID");
            this.Property(x => x.OuterId, "Внешний Id").Column("OUTER_ID");
            this.Property(x => x.EntrepreneurCreateDate, "Дата создания ИП").Column("ENTREPRENEUR_CREATE_DATE");
            this.Property(x => x.EntrepreneurDebtSum, "Сумма задолженности ИП").Column("ENTREPRENEUR_DEBT_SUM");
        }
    }
}