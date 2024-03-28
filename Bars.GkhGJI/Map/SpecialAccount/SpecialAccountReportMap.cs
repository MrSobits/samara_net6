namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>Маппинг для "Решение судебных участков"</summary>
    public class SpecialAccountReportMap : BaseEntityMap<SpecialAccountReport>
    {
        
        public SpecialAccountReportMap() : 
                base("Отчет по спецсчетам", "GJI_SPECIAL_ACCOUNT_REPORT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Property(x => x.Executor, "Ответсвенное лицо").Column("EXECUTOR");
            Property(x => x.MonthEnums, "Отчетный месяц").Column("MONTH").NotNull();
            Property(x => x.AmmountMeasurement, "Рубли/тысячи").Column("AMMOUNT").NotNull();
            Property(x => x.YearEnums, "Отчетный год").Column("YEAR").NotNull();
            Property(x => x.Sertificate, "Сведения о сертификате").Column("SERTIFICATE");
            Property(x => x.Author, "Автор отчета").Column("AUTHOR");
       
            Property(x => x.DateAccept, "Дата отчета").Column("DATE_ACCEPT");
            Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Reference(x => x.SignedXMLFile, "Подписанный XML файл").Column("SIGNED_FILE_ID").Fetch();
        }
    }
}
