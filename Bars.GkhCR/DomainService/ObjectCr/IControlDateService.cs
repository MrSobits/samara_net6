namespace Bars.GkhCr.DomainService
{
    using Bars.B4;

    public interface IControlDateService
    {
        IDataResult AddStageWorks(BaseParams baseParams);

        IDataResult AddWorks(BaseParams baseParams);
    }
}
