namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа оплаты ГЖИ
    /// </summary>
    public enum TypeDocumentPaidGji
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Квитанция")]
        Invoice = 20,

        [Display("Письмо")]
        Later = 30,

        [Display("Уведомление")]
        Notice = 40,

        [Display("Платеж с ГИС ГМП")]
        PaymentGisGmp = 50
    }
}