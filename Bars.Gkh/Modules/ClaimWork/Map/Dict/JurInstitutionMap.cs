/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class JurInstitutionMap : BaseEntityMap<JurInstitution>
///     {
///         public JurInstitutionMap()
///             : base("CLW_JUR_INSTITUTION")
///         {
///             Map(x => x.JurInstitutionType, "JUR_INST_TYPE", true, (object)10);
///             Map(x => x.CourtType, "COURT_TYPE", true, (object)10);
///             Map(x => x.Name, "NAME", false, 250);
///             Map(x => x.ShortName, "SHORT_NAME", false, 250);
///             Map(x => x.Address, "ADDRESS", false, 500);
///             Map(x => x.PostCode, "POST_CODE", false, 50);
///             Map(x => x.Phone, "PHONE", false, 250);
///             Map(x => x.Email, "EMAIL", false, 250);
///             Map(x => x.Website, "WEBSITE", false, 250);
///             Map(x => x.JudgeSurname, "JUDGE_SURNAME", false, 250);
///             Map(x => x.JudgeName, "JUDGE_NAME", false, 250);
///             Map(x => x.JudgePatronymic, "JUDGE_PATRONOMYC", false, 250);
///             Map(x => x.JudgeShortFio, "JUDGE_SHORT_FIO", false, 250);
/// 
///             References(x => x.FiasAddress, "FIAS_ADDRESS_ID", ReferenceMapConfig.Fetch);
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Учреждения в судебной практике (JurisprudenceInstitution)"</summary>
    public class JurInstitutionMap : BaseEntityMap<JurInstitution>
    {
        
        public JurInstitutionMap() : 
                base("Учреждения в судебной практике (JurisprudenceInstitution)", "CLW_JUR_INSTITUTION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.JurInstitutionType, "Тип учреждения в судебной практике").Column("JUR_INST_TYPE").DefaultValue(JurInstitutionType.Court).NotNull();
            this.Property(x => x.CourtType, "Тип суда").Column("COURT_TYPE").DefaultValue(CourtType.Magistrate).NotNull();
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
            this.Property(x => x.Name, "Полное наименование").Column("NAME").Length(250);
            this.Property(x => x.ShortName, "Краткое наименование").Column("SHORT_NAME").Length(250);
            this.Reference(x => x.FiasAddress, "Aдрес ФИАС").Column("FIAS_ADDRESS_ID").Fetch();
            this.Property(x => x.Address, "Адрес").Column("ADDRESS").Length(500);
            this.Property(x => x.OutsideAddress, "Адрес за пределами субъекта").Column("OUTSIDE_ADDRESS").Length(500);
            this.Property(x => x.PostCode, "Почтовый индекс").Column("POST_CODE").Length(50);
            this.Property(x => x.Phone, "Телефон").Column("PHONE").Length(250);
            this.Property(x => x.Email, "Электронная почта").Column("EMAIL").Length(250);
            this.Property(x => x.Website, "Сайт").Column("WEBSITE").Length(250);
            this.Property(x => x.JudgePosition, "Судья - должность").Column("JUDGE_POSITION").Length(250);
            this.Property(x => x.JudgeSurname, "Судья - Фамилия").Column("JUDGE_SURNAME").Length(250);
            this.Property(x => x.JudgeName, "Судья - Имя").Column("JUDGE_NAME").Length(250);
            this.Property(x => x.JudgePatronymic, "Судья - Отчество").Column("JUDGE_PATRONOMYC").Length(250);
            this.Property(x => x.JudgeShortFio, "Судья - Фамилия и инициалы").Column("JUDGE_SHORT_FIO").Length(250);
            this.Property(x => x.Code, "Код").Column("CODE").Length(50);
            this.Property(x => x.HeaderText, "Заголовок").Column("HEADER_TEXT").Length(5000);
        }
    }
}
