namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister
{
    using System.Collections;

    /// <summary>
    /// Интерфейс реестра документов ПИР
    /// </summary>
    public interface IDocumentRegisterService
    {
        /// <summary>
        /// Список типов документов ПИР
        /// </summary>
        /// <returns></returns>
        IList ListTypeDocument();
    }
}