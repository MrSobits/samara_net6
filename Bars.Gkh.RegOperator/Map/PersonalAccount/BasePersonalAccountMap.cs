namespace Bars.Gkh.RegOperator.Map
{
    using System;
    using System.Linq.Expressions;

    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Лицевой счет"</summary>
    public class BasePersonalAccountMap : BaseImportableEntityMap<BasePersonalAccount>
    {
        
        public BasePersonalAccountMap() : 
                base("Лицевой счет", "REGOP_PERS_ACC")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            this.Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ TransportGUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            this.Property(x => x.UnifiedAccountNumber, "Единый ЛС").Column("UNIFIED_ACC_NUM").Length(10);
            this.Property(x => x.ServiceId, "Идентификатор жилищно-коммунальной услуги").Column("SERVICE_ID").Length(13);
            this.Reference(x => x.Room, "Помещение").Column("ROOM_ID").NotNull().Fetch();
            this.Reference(x => x.AccountOwner, "Абонент").Column("ACC_OWNER_ID").NotNull().Fetch();
            this.Property(x => x.IntNumber, "Целочисленный номер для генерации номера ркц").Column("INT_NUMBER").NotNull();
            this.Property(x => x.PersonalAccountNum, "Номер Л/С").Column("ACC_NUM").Length(20).NotNull();
            this.Property(x => x.PersAccNumExternalSystems, "Номер Л/С во внешних системах").Column("REGOP_PERS_ACC_EXTSYST").Length(250);
            this.Property(x => x.AreaShare, "Доля собственности").Column("AREA_SHARE").DefaultValue(1m).NotNull();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            this.Property(x => x.OpenDate, "Дата открытия ЛС").Column("OPEN_DATE");
            this.Property(x => x.CloseDate, "Дата закрытия ЛС").Column("CLOSE_DATE");
            this.Property(x => x.Tariff, "Удалить после прочтения").Column("TARIFF");
            this.Property(x => x.ContractNumber, "Номер заключения договора").Column("CONTRACT_NUMBER").Length(250);
            this.Property(x => x.ContractDate, "Дата заключения договора").Column("CONTRACT_NUMBER_DATE");
            this.Property(x => x.ContractSendDate, "Дата отправки договора").Column("CONTRACT_SEND_DATE");
            this.Reference(x => x.ContractDocument, "Документ договора").Column("CONTRACT_FILE_ID").Fetch();
            this.Property(x => x.OwnershipDocumentType, "Тип документа о праве собственности").Column("OWNERSHIP_DOC_TYPE").Length(250);
            this.Property(x => x.ServiceType, "Тип услуги").Column("PERSACC_SERV_TYPE").DefaultValue(PersAccServiceType.NotSelected).NotNull();
            this.Property(x => x.DocumentNumber, "Код/номер документа").Column("DOCUMENT_NUMBER").Length(250);
            this.Property(x => x.DocumentRegistrationDate, "Дата оформления документа").Column("DOCUMENT_REG_DATE");
            this.Property(x => x.Area, "Площадь, относящаяся к лс").Column("CAREA").NotNull();
            this.Property(x => x.LivingArea, "Жилая площадь, относящаяся к лс").Column("LAREA");

            this.Property(x => x.TariffChargeBalance, "Текущая задолженность по базовому тарифу").Column("TARIFF_CHARGE_BALANCE");
            this.Property(x => x.DecisionChargeBalance, "Текущая задолженность по тарифу решения").Column("DECISION_CHARGE_BALANCE");
            this.Property(x => x.PenaltyChargeBalance, "Текущая задолженность по пеням").Column("PENALTY_CHARGE_BALANCE");

            this.Property(x => x.IsNotDebtor, "Флаг при установке которого ЛС не попадает в реестр должников").Column("IS_NOT_DEBTOR");
            this.Property(x => x.InstallmentPlan, "Заключен договор о рассрочке").Column("INSTALLMENTPLAN");
            this.Property(x => x.DigitalReceipt, "Признак электронная квитанция").Column("DIGITAL_RECEIPT").NotNull();
        }
    }

    public class BasePersonalAccountNHibernateMapping : ClassMapping<BasePersonalAccount>
    {
        public BasePersonalAccountNHibernateMapping()
        {
            this.WalletMap(x => x.BaseTariffWallet, "BT_WALLET_ID");
            this.WalletMap(x => x.DecisionTariffWallet, "DT_WALLET_ID");
            this.WalletMap(x => x.PenaltyWallet, "P_WALLET_ID");
            this.WalletMap(x => x.RentWallet, "R_WALLET_ID");
            this.WalletMap(x => x.SocialSupportWallet, "SS_WALLET_ID");
            this.WalletMap(x => x.PreviosWorkPaymentWallet, "PWP_WALLET_ID");
            this.WalletMap(x => x.AccumulatedFundWallet, "AF_WALLET_ID");
            this.WalletMap(x => x.RestructAmicableAgreementWallet, "RAA_WALLET_ID");

            this.Bag(x => x.Charges, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Fetch(CollectionFetchMode.Select);
                m.Cascade(Cascade.Remove);
                m.Lazy(CollectionLazy.Extra);
                m.Key(k => k.Column("PERS_ACC_ID"));
            }, action => action.OneToMany());

            this.Bag(x => x.Summaries, m =>
            {
                m.Access(Accessor.NoSetter);
                m.Fetch(CollectionFetchMode.Select);
                m.Cascade(Cascade.Remove);
                m.Lazy(CollectionLazy.Extra);
                m.Key(k => k.Column("ACCOUNT_ID"));
                m.Inverse(true);
            }, action => action.OneToMany());
        }

        public void WalletMap<TWallet>(Expression<Func<BasePersonalAccount, TWallet>> prop, string columnName) where TWallet : class
        {
            this.ManyToOne(prop, m =>
            {
                m.Column(columnName);
                m.Lazy(LazyRelation.Proxy);
                m.Cascade(Cascade.Remove);
                m.Fetch(FetchKind.Join);
                m.NotNullable(true);
            });
        }
    }
}
