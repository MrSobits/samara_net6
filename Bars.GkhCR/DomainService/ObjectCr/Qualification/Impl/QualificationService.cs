namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class QualificationService : IQualificationService
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<string> GetActiveColumns(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId", 0);

            var periodId = Container.Resolve<IDomainService<Entities.ObjectCr>>()
                         .GetAll()
                         .Where(x => x.Id == objectCrId)
                         .Select(x => x.ProgramCr.Period.Id)
                         .FirstOrDefault();

            return Container.Resolve<IDomainService<QualificationMember>>()
                         .GetAll()
                         .Where(x => x.Period.Id == periodId)
                         .Select(x => x.Name)
                         .ToList();
        }

        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var userManager = Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();

            var municipalityList = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => x.Name).ToList();

            var data = Container.Resolve<IDomainService<ViewQualification>>()
                         .GetAll()
                         .Where(x => municipalityList.Contains(x.MunicipalityName))
                         .Select(x =>
                             new
                             {
                                 x.Id,
                                 x.ProgrammName,
                                 x.MunicipalityName,
                                 x.Address,
                                 x.BuilderName,
                                 x.Sum,
                                 Rating = x.Rating ?? "0 из " + x.QualMemberCount
                             })
                         .OrderIf(loadParams.Order.Length == 0, true, x => x.MunicipalityName)
                         .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                         .Filter(loadParams, this.Container);


            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
