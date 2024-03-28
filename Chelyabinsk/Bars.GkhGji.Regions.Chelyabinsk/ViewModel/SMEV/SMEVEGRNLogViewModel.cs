namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVEGRNLogViewModel : BaseViewModel<SMEVEGRNLog>
    {
        public override IDataResult List(IDomainService<SMEVEGRNLog> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = domain.GetAll()
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.Login,
                x.UserName,
                x.ObjectCreateDate,
                x.OperationType,
                x.SMEVEGRN.CadastralNUmber,
                RealityObject = x.SMEVEGRN.RealityObject != null ? x.SMEVEGRN.RealityObject.Address : "",
                Room = x.SMEVEGRN.Room != null ? x.SMEVEGRN.Room.RoomNum : "",
                ReqNum = x.SMEVEGRN.Id.ToString()
            }).AsEnumerable()
            .Select(x=> new
            {
                x.Id,
                x.FileInfo,
                x.Login,
                x.UserName,
                x.ObjectCreateDate,
                x.OperationType,
                x.CadastralNUmber,
                x.RealityObject,
                x.Room,
                x.ReqNum
            }).AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}