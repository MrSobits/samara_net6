namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Owner;

	/// <summary>Маппинг для "Данные о собственнике"</summary>
	public class PersonalAccountOwnerInformationMap : BaseImportableEntityMap<PersonalAccountOwnerInformation>
    {
        
        public PersonalAccountOwnerInformationMap() : 
                base("Данные о собственнике", "REGOP_PERS_ACC_OWNER_INFO")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentNumber, "Номер документа о собственности").Column("DOCUMENT_NUMBER");
            Property(x => x.AreaShare, "Доля собственности из документа о собственности").Column("AREA_SHARE");
            Property(x => x.StartDate, "Дата начала документа о собственности").Column("START_DATE");
            Property(x => x.EndDate, "Дата окончания документа о собственности").Column("END_DATE");
			
			Reference(x => x.BasePersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
			Reference(x => x.Owner, "Абонент").Column("ACCOUNT_OWNER_ID").NotNull().Fetch();
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
        }
    }
}
