namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using B4;

    /// <summary>
    /// Интерфейс для правил формирвоания документов, а также для формирования самих документов ПИР
    /// </summary>
    public interface IClaimWorkDocumentProvider
    {
        /// <summary>
        /// Метод формирвоания документа ПИР
        /// </summary>
        IDataResult CreateDocument(BaseParams baseParams);
    }
}
