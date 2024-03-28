namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Voronezh.Entities;


    /// <summary>Маппинг для "Предоставляемый документ заявки на лицензию"</summary>
    public class LicenseReissuanceRPGUMap : BaseImportableEntityMap<LicenseReissuanceRPGU>
    {

        public LicenseReissuanceRPGUMap() : 
                base("Заявка по лицензированию", "GKH_MANORG_REISS_RPGU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicRequest, "Заявка на лицензию").Column("LIC_REQUEST_ID").NotNull();
            Reference(x => x.AnswerFile, "Файл ответа").Column("ANSWER_FILE_ID").Fetch();
            Property(x => x.AnswerText, "Текст ответа").Column("ANSWER_TEXT");
            Property(x => x.MessageId, "Ид обращения в СМЭВ").Column("MESSAGE_ID");
            Property(x => x.Date, "Дата запроса").Column("RPGU_REQ_DATE");
            Property(x => x.RequestRPGUState, "Статус запроса").Column("RPGU_STATE");
            Property(x => x.RequestRPGUType, "Тип запроса").Column("RPGU_TYPE");
            Property(x => x.Text, "Текст запроса").Column("RPGU_TEXT");
            Reference(x => x.File, "Файл предоставляемого документа").Column("LIC_DOC_FILE_ID").Fetch();
        }
    }
}
