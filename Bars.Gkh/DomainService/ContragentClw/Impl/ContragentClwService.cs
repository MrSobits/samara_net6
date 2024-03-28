namespace Bars.Gkh.DomainService.ContragentClw.Impl
{
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;

    using Castle.Windsor;
    using Bars.Gkh.Utils;

    public class ContragentClwService : IContragentClwService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var contragentClwDomain = Container.Resolve<IDomainService<ContragentClw>>();
                var contragentClwMunicipalityDomain = Container.Resolve<IDomainService<ContragentClwMunicipality>>();
                var municipalityDomain = Container.Resolve<IDomainService<Municipality>>();

                try
                {
                    var contragentClwId = baseParams.Params.GetAs<long>("contragentClwId");
                    var muIds = baseParams.Params.GetAs<long[]>("muIds");

                    var existRecs =
                        contragentClwMunicipalityDomain.GetAll()
                            .Where(x => x.ContragentClw.Id == contragentClwId)
                            .Select(x => x.Municipality.Id)
                            .Distinct()
                            .AsEnumerable()
                            .ToDictionary(x => x);

                    var contragentClw = contragentClwDomain.Load(contragentClwId);

                    foreach (var id in muIds)
                    {
                        if (existRecs.ContainsKey(id))
                            continue;

                        var newObj = new ContragentClwMunicipality
                        {
                            ContragentClw = contragentClw,
                            Municipality = municipalityDomain.Load(id)
                        };

                        contragentClwMunicipalityDomain.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                finally
                {
                    Container.Release(contragentClwDomain);
                    Container.Release(contragentClwMunicipalityDomain);
                    Container.Release(municipalityDomain);
                }
            }
        }
    }
}