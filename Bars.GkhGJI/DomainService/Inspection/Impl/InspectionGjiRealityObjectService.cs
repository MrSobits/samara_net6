namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Gkh.Entities;
    using Entities;

    using Castle.Windsor;

    public class InspectionGjiRealityObjectService : IInspectionGjiRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                
                try
                {
                    var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var existRecs =
                        service.GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Where(x => objectIds.Contains(x.RealityObject.Id))
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var inspection = serviceInspection.Load(inspectionId);

                    foreach (var id in objectIds)
                    {
                        // Если среди существующих домов данно инспекции уже есть такой дом то пролетаем мимо
                        if (existRecs.Contains(id))
                            continue;

                        // Если такого дома еще нет то добалвяем
                        var newObj = new InspectionGjiRealityObject
                            {
                                Inspection = inspection,
                                RealityObject = serviceRobject.Load(id)
                            };

                        service.Save(newObj);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceInspection);
                    Container.Release(serviceRobject);
                }
            }
        }
    }
}