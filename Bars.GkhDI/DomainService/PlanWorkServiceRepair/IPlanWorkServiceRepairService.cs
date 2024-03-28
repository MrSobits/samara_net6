namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface IPlanWorkServiceRepairService
    {
        IDataResult AddTemplateService(BaseParams baseParams);

        IDataResult ReloadWorkRepairList(BaseParams baseParams);
    }
}