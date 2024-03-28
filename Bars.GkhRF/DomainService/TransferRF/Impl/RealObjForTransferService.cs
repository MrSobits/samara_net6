namespace Bars.GkhRf.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    using Castle.Windsor;

    public class RealObjForTransferService : IRealObjForTransferService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            var contractRfId = baseParams.Params.ContainsKey("contractRfId") ? baseParams.Params["contractRfId"].ToLong() : 0;
            var contractRfObjectList = this.Container.Resolve<IDomainService<ContractRfObject>>().GetAll().Where(x => x.ContractRf.Id == contractRfId && x.TypeCondition == TypeCondition.Include).Select(x => x.RealityObject.Id).Distinct().ToList();

            // получаем жилые дома вошедшие в заданную программу капремонта. Используются IRepository вместо IDomainService в силу того что нужно получать не переопределенный GetAll
            var programCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToLong() : 0;
            var objectCrList = this.Container.Resolve<IRepository<ObjectCr>>().GetAll().Where(x => x.ProgramCr.Id == programCrId).Select(x => x.RealityObject.Id).Distinct().ToList();

            var service = this.Container.Resolve<IRepository<RealityObject>>();

            var data = service.GetAll()
                .WhereIf(contractRfId > 0, x => contractRfObjectList.Contains(x.Id))
                .Select(x => new { x.Id, Municipality = x.Municipality.Name, x.Address, x.AreaLiving, x.DateLastOverhaul, x.GkhCode })
                .ToList()
                .AsQueryable()
                .WhereIf(programCrId > 0, x => objectCrList.Contains(x.Id))
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}