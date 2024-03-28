namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Оплата штрафов в постановлении Роспотребнадзора"</summary>
    public class ResolutionRospotrebnadzorPayFineMap : BaseEntityMap<ResolutionRospotrebnadzorPayFine>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_PAYFINE";

        /// <summary>
        /// Тип документа оплаты штрафа
        /// </summary>
        public const string TypeDocumentPaid = "KIND_PAY";

        /// <summary>
        /// Номер документа
        /// </summary>
        public const string DocumentNum = "DOCUMENT_NUM";

        /// <summary>
        /// Дата документа
        /// </summary>
        public const string DocumentDate = "DOCUMENT_DATE";

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public const string Amount = "AMOUNT";

        /// <summary>
        /// Код из Гис программы
        /// </summary>
        public const string GisUip = "GIS_UIP";
        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
        #endregion
        public ResolutionRospotrebnadzorPayFineMap() :
                base("Оплата штрафов в постановлении Роспотребнадзора", ResolutionRospotrebnadzorPayFineMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeDocumentPaid, "Тип документа оплаты штрафа").Column(ResolutionRospotrebnadzorPayFineMap.TypeDocumentPaid).NotNull();
            this.Property(x => x.DocumentDate, "Дата документа").Column(ResolutionRospotrebnadzorPayFineMap.DocumentDate);
            this.Property(x => x.DocumentNum, "Номер документа").Column(ResolutionRospotrebnadzorPayFineMap.DocumentNum).Length(50);
            this.Property(x => x.Amount, "Сумма штрафа").Column(ResolutionRospotrebnadzorPayFineMap.Amount);
            this.Property(x => x.GisUip, "Код из Гис программы").Column(ResolutionRospotrebnadzorPayFineMap.GisUip).Length(50);
            this.Reference(x => x.Resolution, "Постановление").Column(ResolutionRospotrebnadzorPayFineMap.Resolution).NotNull().Fetch();
        }
    }
}
