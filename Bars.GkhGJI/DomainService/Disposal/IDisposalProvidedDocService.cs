namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис работы с Предоставляемыми документами
    /// </summary>
    public interface IDisposalProvidedDocService
    {
        /// <summary>
        /// Добавить Предоставляемые документы
        /// </summary>
        IDataResult AddProvidedDocs(BaseParams baseParams);

        /// <summary>
        /// Добавить Предоставляемые документы
        /// </summary>
        IDataResult AddProvidedDocs(long documentId, long[] ids);
    }
}