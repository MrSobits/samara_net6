namespace Bars.GkhGji.Map.ResolutionRospotrebnadzor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Маппинг для "Постановление Роспотребнадзора"
    /// </summary>
    public class ResolutionRospotrebnadzorMap: JoinedSubClassMap<ResolutionRospotrebnadzor>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR";
        /// <summary>
        /// Документ-основание
        /// </summary>
        public const string DocumentReason = "DOCUMENT_REASON";
        /// <summary>
        /// Дата вручения
        /// </summary>
        public const string DeliveryDate = "DELIVERY_DATE";
        /// <summary>
        /// Код Гис - униклаьный идентификатор начисления
        /// </summary>
        public const string GisUin = "GIS_UIN";
        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public const string RevocationReason = "REVOCATION_REASON";
        /// <summary>
        /// Тип инициативного органа (кем вынесено)
        /// </summary>
        public const string TypeInitiativeOrg = "TYPE_INITIATIVE_ORG";
        /// <summary>
        /// Основание прекращения
        /// </summary>
        public const string ExpireReason = "EXPIRE_REASON";
        /// <summary>
        /// Сумма штрафов
        /// </summary>
        public const string PenaltyAmount = "PENALTY_AMOUNT";
        /// <summary>
        /// Номер санкционного документа
        /// </summary>
        public const string SspDocumentNum = "SSP_DOCUMENT_NUM";
        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public const string Paided = "PAIDED";
        /// <summary>
        /// Дата передачи в ССП
        /// </summary>
        public const string TransferToSspDate = "TRANSFER_SSP_DATE";
        /// <summary>
        /// Физическое лицо
        /// </summary>
        public const string PhysicalPerson = "PHYSICAL_PERSON";
        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public const string PhysicalPersonInfo = "PHYSICAL_PERSON_INFO";
        /// <summary>
        /// МО получателя штрафа
        /// </summary>
        public const string FineMunicipality = "FINE_MUNICIPALITY_ID";
        /// <summary>
        /// Должностное лицо
        /// </summary>
        public const string Official = "OFFICIAL_ID";
        /// <summary>
        /// Местонахождение
        /// </summary>
        public const string LocationMunicipality = "LOCATION_MUNICIPALITY_ID";
        /// <summary>
        /// Вид санкции
        /// </summary>
        public const string Sanction = "SANCTION_ID";
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public const string Executant = "EXECUTANT_ID";
        /// <summary>
        /// Контрагент
        /// </summary>
        public const string Contragent = "CONTRAGENT_ID";
#endregion
        /// <summary>
        /// .ctor
        /// </summary>
        public ResolutionRospotrebnadzorMap()
            : base("Постановление Роспотребнадзора", ResolutionRospotrebnadzorMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentReason, "Документ-основание").Column(ResolutionRospotrebnadzorMap.DocumentReason).Length(150);
            this.Property(x => x.DeliveryDate, "Дата вручения").Column(ResolutionRospotrebnadzorMap.DeliveryDate);
            this.Property(x => x.GisUin, "Код Гис - униклаьный идентификатор начисления").Column(ResolutionRospotrebnadzorMap.GisUin).Length(50);
            this.Property(x => x.RevocationReason, "Причина аннулирования").Column(ResolutionRospotrebnadzorMap.RevocationReason);
            this.Property(x => x.TypeInitiativeOrg, "Тип инициативного органа").Column(ResolutionRospotrebnadzorMap.TypeInitiativeOrg).NotNull().DefaultValue(TypeInitiativeOrgGji.Rospotrebnadzor);
            this.Property(x => x.ExpireReason, "Основание прекращения").Column(ResolutionRospotrebnadzorMap.ExpireReason);
            this.Property(x => x.PenaltyAmount, "Сумма штрафов").Column(ResolutionRospotrebnadzorMap.PenaltyAmount);
            this.Property(x => x.SspDocumentNum, "Номер санкционного документа").Column(ResolutionRospotrebnadzorMap.SspDocumentNum);
            this.Property(x => x.Paided, "Штраф оплачен").Column(ResolutionRospotrebnadzorMap.Paided).NotNull().DefaultValue(YesNoNotSet.NotSet);
            this.Property(x => x.TransferToSspDate, "Дата передачи в ССП").Column(ResolutionRospotrebnadzorMap.TransferToSspDate);
            this.Property(x => x.PhysicalPerson, "Физическое лицо").Column(ResolutionRospotrebnadzorMap.PhysicalPerson);
            this.Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column(ResolutionRospotrebnadzorMap.PhysicalPersonInfo);

            this.Reference(x => x.FineMunicipality, "МО получателя штрафа").Column(ResolutionRospotrebnadzorMap.FineMunicipality).Fetch();
            this.Reference(x => x.Official, "Должностное лицо").Column(ResolutionRospotrebnadzorMap.Official).Fetch();
            this.Reference(x => x.LocationMunicipality, "Местонахождение").Column(ResolutionRospotrebnadzorMap.LocationMunicipality).Fetch();
            this.Reference(x => x.Sanction, "Вид санкции").Column(ResolutionRospotrebnadzorMap.Sanction).Fetch();
            this.Reference(x => x.Executant, "Тип исполнителя документа").Column(ResolutionRospotrebnadzorMap.Executant).Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column(ResolutionRospotrebnadzorMap.Contragent);
        }
    }
}