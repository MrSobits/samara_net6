namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    using Gkh.Entities;
    using Entities;

    using Castle.Windsor;

    public class RegOperatorMunicipalityService : IRegOperatorMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Municipality> ServiceMunicipality { get; set; }

        public IDomainService<RegOperator> ServiceRegOperator { get; set; }

        public IDomainService<RegOperatorMunicipality> Service { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            var regOperatorId = baseParams.Params.GetAs<long>("regOperatorId");
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existRecs = Service.GetAll()
                        .Where(x => x.RegOperator.Id == regOperatorId)
                        .Where(x => objectIds.Contains(x.Municipality.Id))
                        .Select(x => x.Municipality.Id)
                        .Distinct()
                        .ToList();

                    var regOperator = ServiceRegOperator.Load(regOperatorId);

                    foreach (var id in objectIds)
                    {
                        if(existRecs.Contains(id))
                            continue;

                        var newRec = new RegOperatorMunicipality
                            {
                                Municipality = ServiceMunicipality.Load(id),
                                RegOperator = regOperator
                            };

                        Service.Save(newRec);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult ListMuByRegOp(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var regOpId = baseParams.Params.GetAs<long>("regOpId");

            var data =
                Service.GetAll()
                       .Where(x => x.RegOperator.Id == regOpId)
                       .Select(x => new 
                       {
                           x.Municipality.Id,
                           x.Municipality.Name
                       })
                       .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());

        }
    }
}