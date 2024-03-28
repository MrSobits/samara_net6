namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки по поручению руководства
    /// </summary>
    public enum TypeBaseDispHead
    {
        /// <summary>
        /// Поручение руководителей исполнительных органов власти
        /// </summary>
        [Display("Поручение руководителей исполнительных органов власти")]
        ExecutivePower = 10,

        /// <summary>
        /// Выдача предписания в случае неустранения нарушений по ранее выданному предписанию
        /// </summary>
        [Display("Выдача предписания в случае неустранения нарушений по ранее выданному предписанию")]
        FailureRemoveViolation = 20,

        /// <summary>
        /// Передача документов в суд
        /// </summary>
        [Display("Передача документов в суд")]
        TransferToCourt = 30
    }
}