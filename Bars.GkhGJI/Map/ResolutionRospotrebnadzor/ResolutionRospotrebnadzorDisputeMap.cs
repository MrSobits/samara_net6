namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Оспаривание постановления Роспотребнадзора"</summary>
    public class ResolutionRospotrebnadzorDisputeMap : BaseEntityMap<ResolutionRospotrebnadzorDispute>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_DISPUTE";

        /// <summary>
        /// Номер документа
        /// </summary>
        public const string DocumentNum = "DOCUMENT_NUM";

        /// <summary>
        /// Дата документа
        /// </summary>
        public const string DocumentDate = "DOCUMENT_DATE";

        /// <summary>
        /// Протест прокуратуры
        /// </summary>
        public const string ProsecutionProtest = "PROSECUTION_PROTEST";

        /// <summary>
        /// Постановление обжаловано
        /// </summary>
        public const string Appeal = "APPEAL";

        /// <summary>
        /// Описание
        /// </summary>
        public const string Description = "DESCRIPTION";

        /// <summary>
        /// Вид суда
        /// </summary>
        public const string Court = "COURT_ID";

        /// <summary>
        /// Файл
        /// </summary>
        public const string File = "FILE_ID";

        /// <summary>
        /// Инстанция
        /// </summary>
        public const string Instance = "INSTANTION_ID";

        /// <summary>
        /// Решение суда
        /// </summary>
        public const string CourtVerdict = "COURTVERDICT_ID";

        /// <summary>
        /// Юрист
        /// </summary>
        public const string Lawyer = "INSPECTOR_ID";

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
        #endregion
        public ResolutionRospotrebnadzorDisputeMap() :
                base("Оспаривание постановления Роспотребнадзора", ResolutionRospotrebnadzorDisputeMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentNum, "Номер документа").Column(ResolutionRospotrebnadzorDisputeMap.DocumentNum).Length(50);
            this.Property(x => x.DocumentDate, "Дата документа").Column(ResolutionRospotrebnadzorDisputeMap.DocumentDate);
            this.Property(x => x.ProsecutionProtest, "Протест прокуратуры").Column(ResolutionRospotrebnadzorDisputeMap.ProsecutionProtest).NotNull();
            this.Property(x => x.Description, "Описание").Column(ResolutionRospotrebnadzorDisputeMap.Description);
            this.Property(x => x.Appeal, "Постановление обжаловано").Column(ResolutionRospotrebnadzorDisputeMap.Appeal).NotNull();
            this.Reference(x => x.File, "Файл").Column(ResolutionRospotrebnadzorDisputeMap.File).Fetch();
            this.Reference(x => x.Court, "вид суда").Column(ResolutionRospotrebnadzorDisputeMap.Court).Fetch();
            this.Reference(x => x.Instance, "Инстанция").Column(ResolutionRospotrebnadzorDisputeMap.Instance).Fetch();
            this.Reference(x => x.CourtVerdict, "Решение суда").Column(ResolutionRospotrebnadzorDisputeMap.CourtVerdict).Fetch();
            this.Reference(x => x.Lawyer, "Юрист").Column(ResolutionRospotrebnadzorDisputeMap.Lawyer).Fetch();
            this.Reference(x => x.Resolution, "постановление").Column(ResolutionRospotrebnadzorDisputeMap.Resolution).NotNull().Fetch();
        }
    }
}
