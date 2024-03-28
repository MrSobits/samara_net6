namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

   
    public interface IEDSDocumentService
    {
        IDataResult ListEDSDocuments(BaseParams baseParams);

        IDataResult ListEDSNotice(BaseParams baseParams);
        IDataResult ListEDSMotivRequst(BaseParams baseParams);

        IDataResult ListEDSDocumentsForSign(BaseParams baseParams);

        IDataResult GetListGjiDoc(BaseParams baseParams);
    }
}

