using System.Linq;
using Castle.Windsor;

namespace Bars.GkhGji.DomainService
{
    using B4;
    using B4.Utils;

    using Bars.Gkh.Entities.Dicts;

    using Entities;

    public class StatSubsubjectGjiService : IStatSubsubjectGjiService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFeature(BaseParams baseParams)
        {
            try
            {
                //идентификаторы характеристик
                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                ? baseParams.Params["objectIds"].ToString()
                                : "";

                //идентификатор подтематики
                var subsubjectId = baseParams.Params.ContainsKey("subsubjectId")
                                       ? baseParams.Params["subsubjectId"].ToLong()
                                       : 0;

                if (string.IsNullOrEmpty(objectIds) || subsubjectId == 0)
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось получить характеристики и/или подтематику" };
                }

                var serviceSubsubjecteature = Container.Resolve<IDomainService<StatSubsubjectFeatureGji>>();

                var existRecords =
                    serviceSubsubjecteature
                             .GetAll()
                             .Where(x => x.Subsubject.Id == subsubjectId)
                             .Select(x => x.FeatureViol.Id)
                             .ToList();

                foreach (var id in objectIds.Split(',').Select(x => x.ToLong()))
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new StatSubsubjectFeatureGji
                            {
                                FeatureViol = new  FeatureViolGji { Id = id },
                                Subsubject = new StatSubsubjectGji { Id = subsubjectId }
                            };

                        serviceSubsubjecteature.Save(newRec);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult AddSubject(BaseParams baseParams)
        {
            try
            {
                //идентификаторы тематик
                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                ? baseParams.Params["objectIds"].ToString()
                                : "";

                //идентификатор подтематики
                var subsubjectId = baseParams.Params.ContainsKey("subsubjectId")
                                       ? baseParams.Params["subsubjectId"].ToLong()
                                       : 0;

                if (string.IsNullOrEmpty(objectIds) || subsubjectId == 0)
                {
                    return new BaseDataResult { Success = false, Message = "Не удалось получить тематики и/или подтематику" };
                }

                var service = Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>();

                var existRecords =
                    service
                             .GetAll()
                             .Where(x => x.Subsubject.Id == subsubjectId)
                             .Select(x => x.Subject.Id)
                             .Distinct()
                             .ToList();

                foreach (var id in objectIds.Split(',').Select(x => x.ToLong()))
                {
                    if (!existRecords.Contains(id))
                    {
                        var newRec = new StatSubjectSubsubjectGji
                        {
                            Subject = new StatSubjectGji { Id = id },
                            Subsubject = new StatSubsubjectGji { Id = subsubjectId }
                        };

                        service.Save(newRec);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}