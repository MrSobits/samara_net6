namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionMap : BaseEntityMap<ActCheckAction>
    {
        public ActCheckActionMap()
            : base("Действие", "GJI_ACTCHECK_ACTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ActionType, "Вид действия").Column("ACTION_TYPE");
            this.Property(x => x.Date, "Дата").Column("DATE").NotNull();
            this.Property(x => x.Number, "Номер").Column("NUMBER");
            this.Property(x => x.StartDate, "Дата начала обследования").Column("START_DATE");
            this.Property(x => x.StartTime, "Время начала обследования").Column("START_TIME");
            this.Property(x => x.EndDate, "Дата окончания обследования").Column("END_DATE");
            this.Property(x => x.EndTime, "Время окончания обследования").Column("END_TIME");
            this.Property(x => x.ContrPersFio, "ФИО контролируемого лица").Column("CONTR_PERS_FIO");
            this.Property(x => x.ContrPersBirthDate, "Дата рождения контролируемого лица").Column("CONTR_PERS_BIRTH_DATE");
            this.Property(x => x.ContrPersBirthPlace, "Место рождения контролируемого лица").Column("CONTR_PERS_BIRTH_PLACE");
            this.Property(x => x.ContrPersRegistrationAddress,
                "Адрес регистрации контролируемого лица").Column("CONTR_PERS_REG_ADDRESS");
            this.Property(x => x.ContrPersLivingAddressMatched,
                "Адрес проживания контролируемого лица совпадает с адресом регистрации?").Column("CONTR_PERS_LIVING_ADDRESS_MATCHED");
            this.Property(x => x.ContrPersLivingAddress,
                "Адрес проживания контролируемого лица").Column("CONTR_PERS_LIVING_ADDRESS");
            this.Property(x => x.ContrPersIsHirer, "Контролируемое лицо наниматель?").Column("CONTR_PERS_IS_HIRER");
            this.Property(x => x.ContrPersPhoneNumber, "Номер телефона контролируемого лица").Column("CONTR_PERS_PHONE_NUMBER");
            this.Property(x => x.ContrPersWorkPlace, "Место работы контролируемого лица").Column("CONTR_PERS_WORK_PLACE");
            this.Property(x => x.ContrPersPost, "Должность контролируемого лица").Column("CONTR_PERS_POST");
            this.Property(x => x.IdentityDocSeries, "Серия документа, удостоверяющего личность").Column("IDENTITY_DOC_SERIES");
            this.Property(x => x.IdentityDocNumber, "Номер документа, удостоверяющего личность").Column("IDENTITY_DOC_NUMBER");
            this.Property(x => x.IdentityDocIssuedOn, "Дата выдачи документа, удостоверяющего личность").Column("IDENTITY_DOC_ISSUED_ON");
            this.Property(x => x.IdentityDocIssuedBy, "Кем выдан документ, удостоверяющий личность").Column("IDENTITY_DOC_ISSUED_BY");
            this.Property(x => x.RepresentFio, "ФИО представителя").Column("REPRESENT_FIO");
            this.Property(x => x.RepresentWorkPlace, "Место работы представителя").Column("REPRESENT_WORK_PLACE");
            this.Property(x => x.RepresentPost, "Должность представителя").Column("REPRESENT_POST");
            this.Property(x => x.RepresentProcurationNumber,
                "Номер доверенности представителя").Column("REPRESENT_PROCURATION_NUMBER");
            this.Property(x => x.RepresentProcurationIssuedOn,
                "Дата выдачи доверенности представителя").Column("REPRESENT_PROCURATION_ISSUED_ON");
            this.Property(x => x.RepresentProcurationValidPeriod,
                "Срок действия доверенности представителя").Column("REPRESENT_PROCURATION_VALID_PERIOD");
            this.Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);

            this.Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID");
            this.Reference(x => x.CreationPlace, "Место составления").Column("CREATION_PLACE_ID");
            this.Reference(x => x.ExecutionPlace, "Место проведения").Column("EXECUTION_PLACE_ID");
            this.Reference(x => x.IdentityDocType, "Тип документа, удостоверяющего личность").Column("IDENTITY_DOC_TYPE_ID");
        }
    }
}