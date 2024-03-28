namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class GisGmpMap : BaseEntityMap<GisGmp>
    {
        
        public GisGmpMap() : 
                base("Обмен данными с ГИС ГМП", "GJI_CH_GIS_GMP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");            

            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.BillDate, "Дата счета").Column("BILL_DATE").NotNull();
            Property(x => x.BillDateSent, "Дата счета отправленная").Column("BILL_DATE_SENT").NotNull();
            Property(x => x.BillFor, "Назначение платежа").Column("BILL_FOR");
            Property(x => x.BillForSent, "Назначение платежа отправленное").Column("BILL_FOR_SENT");
            Property(x => x.DocumentNumber, "Номер документа ФЛ").Column("DOCUMENT_NUMBER");
            Property(x => x.DocumentSerial, "Серия документа ФД").Column("DOCUMENT_SERIAL");
            Reference(x => x.FLDocType, "Тип документа ФЛ").Column("FLDOCTYPE_ID").Fetch();
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.ReconcileAnswer, "Результат квитирования").Column("RECONCILE_ANSWER");

            Property(x => x.ChargeId, "Состояние запроса").Column("CHARGE_ID");

            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.IdentifierType, "Тип идентификатора ЮЛ").Column("IDENT_TYPE");
            Property(x => x.TypeLicenseRequest, "Тип обращения по лицензиям").Column("LIC_REQ_TYPE");
            Property(x => x.INN, "ИНН").Column("INN");
            Property(x => x.IsRF, "Гражданство").Column("CITIZENSHIP");
            Property(x => x.KBK, "КБК").Column("KBK");
            Property(x => x.KBKSent, "КБК отправленный").Column("KBK_SENT");
            Property(x => x.KIO, "Код иностранной организации").Column("KIO");
            Property(x => x.KPP, "КПП").Column("KPP");
            Property(x => x.OKTMOSent, "ОКТМО отправленный").Column("OKTMO_SENT");
            Property(x => x.OKTMO, "ОКТМО").Column("OKTMO");
            Property(x => x.LegalAct, "Правовой акт").Column("LEGAL_ACT");
            Property(x => x.LegalActSent, "Правовой акт отправленный").Column("LEGAL_ACT_SENT");
            Property(x => x.PayerType, "Тип плательщика").Column("PAYER_TYPE");
            Property(x => x.PaymentType, "Код платежа").Column("PAYMENT_TYPE");
            Property(x => x.GisGmpChargeType, "Тип извещения").Column("CHARGE_TYPE");
            Property(x => x.Reason, "Основание").Column("REASON");
            Property(x => x.PaytReason, "Показатель основания платежа").Column("PAYT_REASON");
            Property(x => x.PaytReasonSent, "Показатель основания платежа отправленное").Column("PAYT_REASON_SENT");
            Property(x => x.TaxPeriod, "Период квитанции").Column("TAX_PERIOD");
            Property(x => x.TaxPeriodSent, "Период квитанции олтправленный").Column("TAX_PERIOD_SENT");
            Property(x => x.TaxDocNumber, "Номер квитанции").Column("TAX_NUM");
            Property(x => x.TaxDocNumberSent, "Номер квитанции отправленный").Column("TAX_NUM_SENT");
            Property(x => x.TaxDocDate, "Дата квитанции").Column("TAX_DATE");
            Property(x => x.TaxDocDateSent, "Дата квитанции отправленная").Column("TAX_DATE_SENT");
            Property(x => x.Status, "Статус плательщика").Column("STATE");
            Property(x => x.StatusSent, "Статус плательщика отправленный").Column("STATE_SENT");
            Property(x => x.TotalAmount, "Сумма начисления").Column("TOTAL_AMMOUNT");
            Property(x => x.TotalAmountSent, "Сумма начисления отправленная").Column("TOTAL_AMMOUNT_SENT");
            Property(x => x.AltPayerIdentifier, "Идентификатор плательщика").Column("ALT_PAYER_IDENTIFIER");
            Property(x => x.AltPayerIdentifierSent, "Идентификатор плательщика отправленный").Column("ALT_PAYER_IDENTIFIER_SENT");
            Property(x => x.PayerName, "Наименование плательщика").Column("PAYER_NAME");
            Property(x => x.PayerNameSent, "Наименование плательщика отправленное").Column("PAYER_NAME_SENT");
            Property(x => x.GisGmpPaymentsKind, "Тип информации о платеже").Column("GIS_GMP_PAYMENTS_KIND");
            Property(x => x.GisGmpPaymentsType, "Тип запроса оплаты").Column("GIS_GMP_PAYMENTS_TYPE");
            Property(x => x.UIN, "УИН").Column("UIN");
            Property(x => x.DocNumDate, "Номера и даты всех документов").Column("DOC_NUM_DATE");
            //
            Reference(x => x.Protocol, "Протокол").Column("PROTOCOL_ID");
            Reference(x => x.ManOrgLicenseRequest, "Запрос на выдачу лицензии").Column("MANORG_LIC_REQUEST_ID").Fetch();
            Reference(x => x.LicenseReissuance, "Запрос на переоформление лицензии").Column("REISSUANCE_ID").Fetch();
            Reference(x => x.GISGMPPayerStatus, "Статус плательщика").Column("PAYER_STATUS_ID").Fetch();
            // для получния оплат
            Property(x => x.PaymentKBK, "Кбк оплат").Column("PAY_KBK");
            Property(x => x.GetPaymentsStartDate, "Дата оплат с").Column("PAY_START_DATE");
            Property(x => x.GetPaymentsEndDate, "Дата оплат по").Column("PAY_END_DATE");
        }
    }
}
