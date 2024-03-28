namespace Bars.GkhGji.DomainService
{
    using B4;

    using Bars.GkhGji.Enums;

    public interface IHeatSeasonDocService
    {
        IDataResult ListView(BaseParams baseParams);

        IDataResult MassChangeState(BaseParams baseParams);

        IDataResult ListDocumentTypes(BaseParams baseParams);

        HeatSeasonDocType[] DocumentTypes();
    }
}