namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IProtocolService
    {
        /// <summary>
        /// Получить наименование документа-основания в определенном формате
        /// </summary>
        /// <param name="protocolParentDocInfo">Информация о родительском документе</param>
        string GetFormattedDocumentBase(ProtocolParentDocInfo protocolParentDocInfo);

        IDataResult GetInfo(long? documentId);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListForStage(BaseParams baseParams);

        IQueryable<ViewProtocol> GetViewList();

        /// <summary>
        /// Выгрузить в Excel
        /// </summary>
        ActionResult Export(BaseParams baseParams);
    }
}