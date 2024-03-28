namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// !!!Дополнительно при необходимости правит метод GetTypeDocumentCr в ProtocolService
    /// Тип документа КР
    /// Приоритет: 
    /// Протокол об отказе от КР,
    /// Протокол о необходимости проведения КР, 
    /// Протокол о внесение изменений в КР, 
    /// Протокол о завершении капремонта,
    /// Акт сверки данных о расходах,
    /// Акт ввода в эксплуатацию дома после капремонта,
    /// Акт выполненных работ
    /// </summary>
    public enum TypeDocumentCr
    {
        [Display("Акт выполненных работ")]
        Act = 10,

        [Display("Протокол об отказе от КР")]
        ProtocolFailureCr = 20,

        [Display("Протокол о необходимости проведения КР")]
        ProtocolNeedCr = 30,

        [Display("Протокол о внесение изменений в КР")]
        ProtocolChangeCr = 40,

        [Display("Акт ввода в эксплуатацию дома после капремонта")]
        ActExpluatatinAfterCr = 50,

        [Display("Протокол о завершении капремонта")]
        ProtocolCompleteCr = 60,

        [Display("Акт сверки данных о расходах")]
        ActAuditDataExpense = 70
    }
}
