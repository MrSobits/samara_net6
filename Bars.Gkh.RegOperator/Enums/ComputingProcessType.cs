namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    public enum ComputingProcessType
    {
        [Display("Неподтвержденное начисление")]
        UnacceptedCharge = 10,

        [Display("Подтверждение начисления")]
        AcceptUnaccepted = 20,

        [Display("Документ на оплату")]
        PaymentDocument = 21,

        [Display("Импорт банковских платежй")]
        BankDocumentImport = 22,

        [Display("Закрытие периода")]
        CloseChargePeriod = 23
    }
}
