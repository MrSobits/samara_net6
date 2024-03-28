namespace Bars.GkhDi.Services.Domain
{
    using Bars.GkhDi.Services.DataContracts.GetPeriods;

    public interface IServiceDi
    {
        GetManOrgRealtyObjectInfoResponse GetManOrgRealtyObjectInfo(string houseId, string periodId = null);
    }
}