using System.Linq;

namespace Bars.GkhCr.ViewModel
{
    using Bars.GkhCr.DomainService;
    using Bars.B4;
    using Entities;

    public class BuilderViolatorViewModel : BaseViewModel<BuilderViolator>
    {
        public override IDataResult List(IDomainService<BuilderViolator> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IBuilderViolatorService>();

            try
            {
                var totalCount = 0;
                var result = service.GetList(baseParams, true, ref totalCount);
                return new ListDataResult(result, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public override IDataResult Get(IDomainService<BuilderViolator> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", 0);
            var x = domainService.GetAll().FirstOrDefault(y => y.Id == id);

            return x != null ? new BaseDataResult(new
            {
                x.Id,
                Builder = x.BuildContract.Builder.Contragent.Name,
                ObjCrId = x.BuildContract.ObjectCr.Id,
                x.BuildContract.ObjectCr.RealityObject.Address,
                BuildContract = x.BuildContract.Id,
                x.BuildContract.Builder.Contragent.Inn,
                x.BuildContract.DocumentNum,
                x.BuildContract.DocumentDateFrom,
                x.BuildContract.DateEndWork,
                x.CreationType,
                x.CountDaysDelay,
                x.StartingDate
            }) : new BaseDataResult();
        }
    }
}