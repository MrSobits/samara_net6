namespace Bars.Gkh.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IOutdoorWorkService
    {
        IDataResult ListOutdoorWorksByPeriod(BaseParams baseparams);
    }
}
