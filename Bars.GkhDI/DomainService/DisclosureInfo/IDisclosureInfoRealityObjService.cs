namespace Bars.GkhDi.DomainService
{
    using Bars.B4;
    using System;

    public interface IDisclosureInfoRealityObjService
    {
        IDataResult GetDisclosureInfo(BaseParams baseParams);

        IDataResult SaveDisclosureInfo(BaseParams baseParams);

        IDataResult GetCopyInfo(BaseParams baseParams);

        IDataResult GetRealtyObjectPassport(BaseParams baseParams);

        IDataResult GetRealtyObjectPassport(long diRoId, DateTime? yearend = null, long? diId = null);

        IDataResult GetRealtyObjectDevices(BaseParams baseParams);

        IDataResult GetRealtyObjectDevices(long diRoId);

        IDataResult GetRealtyObjectLifts(BaseParams baseParams);

        IDataResult GetRealtyObjectLifts(long diRoId);

        IDataResult GetRealtyObjectStructElements(BaseParams baseParams);

        IDataResult GetRealtyObjectStructElements(long diRoId);

        IDataResult GetRealtyObjectEngineerSystems(BaseParams baseParams);

        IDataResult GetRealtyObjectEngineerSystems(long diRoId);

        IDataResult GetRealtyObjectHouseManaging(BaseParams baseParams);
    }
}