namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.PassportProvider;

    using Castle.Windsor;

    public class RealtyObjectTypeWorkService : IRealtyObjectTypeWorkService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var loadParam =
                baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            var realityObjectId = baseParams.Params["realityObjectId"].ToLong();

            if (Container.Kernel.HasComponent(typeof(IRealtyObjTypeWorkProvider)))
            {
                var objTypeWorkProvider = Container.Resolve<IRealtyObjTypeWorkProvider>();

                var data = objTypeWorkProvider.GetWorks(realityObjectId)
                    .Select(x => new { x.PeriodName, x.WorkName })
                    .AsEnumerable()
                    .Distinct()
                    .ToList();
                
                var totalCount = data.Count();

                return new ListDataResult(data.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }

            return new ListDataResult();
        }
    }
}