namespace Bars.GkhCr.Regions.Tatarstan.DomainService
{
    using Bars.B4;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public interface IObjectOutdoorCrService
    {
        IDataResult Recover(BaseParams baseParams);

        IDataResult GetList(IDomainService<ObjectOutdoorCr> domainService, BaseParams baseParams);
    }
}
