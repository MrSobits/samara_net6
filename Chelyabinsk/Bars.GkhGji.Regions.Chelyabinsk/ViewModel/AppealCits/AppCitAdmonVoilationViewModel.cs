namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Entities;
    using Bars.GkhGji.Entities;
    using B4;
    using B4.Utils;

    public class AppCitAdmonVoilationViewModel : BaseViewModel<AppCitAdmonVoilation>
    {
        public override IDataResult List(IDomainService<AppCitAdmonVoilation> appCitAdmonDomain, BaseParams baseParams)
        {
            var appCitAdmonAppealDomain = this.Container.Resolve<IDomainService<AppCitAdmonAppeal>>();
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("AppealCitsAdmonition", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);
            

            var data = appCitAdmonDomain.GetAll()
             .Where(x => x.AppealCitsAdmonition.Id == id)
            .Select(x => new
            {
                x.Id,
                x.PlanedDate,
                x.FactDate,
                ViolationGjiName = x.ViolationGji.Name,
                CodesPin = x.ViolationGji.NormativeDocNames,
                ViolationGjiId = x.ViolationGji.Id,
                ViolationGjiPin = x.ViolationGji.CodePin,
                PayerType = x.AppealCitsAdmonition.PayerType,
                INN = x.AppealCitsAdmonition.INN,
                KPP = x.AppealCitsAdmonition.KPP,
                DocumentNumberFiz = x.AppealCitsAdmonition.DocumentNumberFiz,
                DocumentSerial = x.AppealCitsAdmonition.DocumentSerial,
                PhysicalPersonDocType = x.AppealCitsAdmonition.PhysicalPersonDocType,
                FIO = x.AppealCitsAdmonition.FIO
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());


            //if (id != 0)
            //{

            //}
            //else 
            //{
            //    var data2 = domain.GetAll()
            //   .Select(x => new
            //   {
            //       x.Id,
            //       x.PlanedDate,
            //       x.FactDate,
            //       ViolationGji = x.ViolationGji.Name
            //   })
            //   .AsQueryable()
            //   .Filter(loadParams, Container);

            //    return new ListDataResult(data2.Order(loadParams).Paging(loadParams).ToList(), data2.Count());
            //}

        }

        public override IDataResult Get(IDomainService<AppCitAdmonVoilation> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       x.PlanedDate,
                       x.FactDate,
                       ViolationGji = x.ViolationGji.Name
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}