namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Договор на пользование общим имуществом
    /// </summary>
    public class DogPoiProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код контрагента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Дом
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 3. Физическое лицо (Арендатор/Наниматель)
        /// </summary>
        public long? IndividualAccountId { get; set; }

        /// <summary>
        /// 4. Организация (Арендатор/Наниматель)
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// 5. Номер договора
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 6. Дата заключения договора
        /// </summary>
        public DateTime? DocumentCreateDate { get; set; }

        /// <summary>
        /// 7. Дата начала действия договора
        /// </summary>
        public DateTime? DocumentStartDate { get; set; }

        /// <summary>
        /// 8. Планируемая дата окончания действия договора
        /// </summary>
        public DateTime? DocumentPlanedEndDate { get; set; }

        /// <summary>
        /// 9. Дата окончания действия
        /// </summary>
        public DateTime? ActionEndDate { get; set; }

        /// <summary>
        /// 10. Предмет договора
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 11. Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 12. Размер платы за предоставление в пользование части общего имущества собственников помещений в МКД
        /// </summary>
        public decimal? CostContract { get; set; }

        /// <summary>
        /// 13. Направления расходования средств, внесённых за пользование частью общего имущества
        /// </summary>
        public string DestinationPayment { get; set; }

        /// <summary>
        /// 14. Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 15. Причина аннулирования
        /// </summary>
        public string RevocationReason { get; set; }

        #region DOGPOIFILES
        /// <summary>
        /// Файл договора на пользование общим имуществом
        /// </summary>
        public FileInfo ContractFile { get; set; }
        #endregion

        #region DOGPOIPROTOCOLOSS
        /// <summary>
        /// Протокол общего собрания собственников
        /// </summary>
        public FileInfo ProtocolFile { get; set; }
        #endregion

        #region POI
        /// <summary>
        /// 2. Наименование общего имущества
        /// </summary>
        public string KindCommomFacilities { get; set; }

        /// <summary>
        /// 3. Назначение общего имущества
        /// </summary>
        public string AppointmentCommonFacilities { get; set; }

        /// <summary>
        /// 4. Площадь общего имущества
        /// </summary>
        public decimal? AreaOfCommonFacilities { get; set; }
        #endregion
    }
}