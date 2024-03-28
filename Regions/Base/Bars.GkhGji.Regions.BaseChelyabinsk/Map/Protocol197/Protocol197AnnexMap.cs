namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// Маппинг для сущности "Приложения протокола ГЖИ 19.7"
    /// </summary>
	public class Protocol197AnnexMap : BaseEntityMap<Protocol197Annex>
    {
		public Protocol197AnnexMap() : 
                base("Приложения протокола ГЖИ", "GJI_PROTOCOL197_ANNEX")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.TypeAnnex, "Тип документа").Column("TYPE_ANNEX");
            this.Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Reference(x => x.Protocol197, "Протокол").Column("PROTOCOL_ID").NotNull().Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_ID").Fetch();

            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.DocumentSend, "Дата документа").Column("DOCUMENT_SEND");
            Property(x => x.DocumentDelivered, "Дата документа").Column("DOCUMENT_DELIV");
        }
    }
}
