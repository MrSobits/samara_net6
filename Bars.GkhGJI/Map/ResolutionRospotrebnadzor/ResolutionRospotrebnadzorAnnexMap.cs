namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Приложения постановления Роспотребнадзора"</summary>
    public class ResolutionRospotrebnadzorAnnexMap : BaseEntityMap<ResolutionRospotrebnadzorAnnex>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX";

        /// <summary>
        /// Наименование
        /// </summary>
        public const string DocumentName = "NAME";

        /// <summary>
        /// Дата документа
        /// </summary>
        public const string DocumentDate = "DOCUMENT_DATE";

        /// <summary>
        /// Описание
        /// </summary>
        public const string Description = "DESCRIPTION";

        /// <summary>
        /// Файл
        /// </summary>
        public const string File = "FILE_ID";

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
#endregion
        public ResolutionRospotrebnadzorAnnexMap() :
                base("Приложения постановления Роспотребнадзора", ResolutionRospotrebnadzorAnnexMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentName, "Наименование").Column(ResolutionRospotrebnadzorAnnexMap.DocumentName);
            this.Property(x => x.DocumentDate, "Дата документа").Column(ResolutionRospotrebnadzorAnnexMap.DocumentDate);
            this.Property(x => x.Description, "Описание").Column(ResolutionRospotrebnadzorAnnexMap.Description);
            this.Reference(x => x.File, "Файл").Column(ResolutionRospotrebnadzorAnnexMap.File).Fetch();
            this.Reference(x => x.Resolution, "Постановление Роспотребнадзора").Column(ResolutionRospotrebnadzorAnnexMap.Resolution).NotNull().Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID").Fetch();
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Property(x => x.MessageCheck, "Статус файла").Column("MESSAGE_CHECK").NotNull();
        }
    }
}
