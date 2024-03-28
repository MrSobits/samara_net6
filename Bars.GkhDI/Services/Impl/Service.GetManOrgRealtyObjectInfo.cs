namespace Bars.GkhDi.Services.Impl
{
    using DataContracts.GetPeriods;
    using Domain;

    public partial class Service
    {
        public virtual GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId, string periodId)
        {
            return Container.Resolve<IServiceDi>().GetManOrgRealtyObjectInfo(houseId, periodId);

        }
    }
}