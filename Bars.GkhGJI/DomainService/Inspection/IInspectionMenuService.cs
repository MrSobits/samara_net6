namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IInspectionMenuService
    {
        IDataResult GetMenu(long inspectionId, long? documentId);
    }
}
