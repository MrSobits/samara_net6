namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class InspectorService : IInspectorService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SubcribeToInspectors(BaseParams baseParams)
        {
            try
            {
                var signedInpectorId = baseParams.Params.GetAs<long>("signedInpectorId");
                var inpectorIds = baseParams.Params.GetAs<int[]>("inpectorIds");

                var inspectorSubscriptionService = Container.Resolve<IDomainService<InspectorSubscription>>();

                // получаем у контроллера источники что бы не добавлять их повторно
                var exsisting =
                    inspectorSubscriptionService.GetAll()
                                                .Where(x => x.SignedInspector.Id == signedInpectorId)
                                                .Select(x => x.Inspector.Id)
                                                .ToList();

                foreach (var newId in inpectorIds.Where(id => !exsisting.Contains(id)))
                {
                    var newInspectorSubscription = new InspectorSubscription
                                                       {
                                                           Inspector = new Inspector { Id = newId },
                                                           SignedInspector = new Inspector { Id = signedInpectorId }
                                                       };

                    inspectorSubscriptionService.Save(newInspectorSubscription);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var zonalInspId = baseParams.Params.GetAs<long>("inpectorId");

                var zonalInspectionNames = new StringBuilder();
                var zonalInspectionIds = new StringBuilder();
                
                // Пробегаемся по зон. инспекциям и формируем итоговую строку наименований и строку идентификаторов
                var serviceZonalInspectionInspector = Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                var dataZonalInspection = serviceZonalInspectionInspector.GetAll()
                    .Where(x => x.Inspector.Id == zonalInspId)
                    .Select(x => new
                    {
                        x.ZonalInspection.Id,
                        x.ZonalInspection.ZoneName
                    })
                    .ToArray();

                foreach (var item in dataZonalInspection)
                {
                    if (!string.IsNullOrEmpty(item.ZoneName))
                    {
                        if (zonalInspectionNames.Length > 0)
                            zonalInspectionNames.Append(", ");

                        zonalInspectionNames.Append(item.ZoneName);
                    }

                    if (item.Id > 0)
                    {
                        if (zonalInspectionIds.Length > 0)
                            zonalInspectionIds.Append(", ");

                        zonalInspectionIds.Append(item.Id);
                    }
                }

                Container.Release(serviceZonalInspectionInspector);
                

                return new BaseDataResult(new
                {
                    zonalInspectionNames = zonalInspectionNames.ToString(),
                    zonalInspectionIds = zonalInspectionIds.ToString(),
                   
                });
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        public IDataResult AddZonalInspection(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var zonalInspId = baseParams.Params.GetAs<long>("inpectorId");
                    var zonInsp = Container.Resolve<IDomainService<Inspector>>().Load(zonalInspId);

                    var zonInspIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var serviceZonalInspection = Container.Resolve<IDomainService<ZonalInspection>>();
                    var serviceZonalInspectionInspector = Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                    // В этом словаре будут существующие инсп по зон
                    var dictZonalInspectionInspector =
                        serviceZonalInspectionInspector.GetAll()
                            .Where(x => x.Inspector.Id == zonalInspId)
                            .AsEnumerable()
                            .GroupBy(x => x.ZonalInspection.Id)
                            .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in zonInspIds)
                    {
                        if (dictZonalInspectionInspector.ContainsKey(id))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dictZonalInspectionInspector.Remove(id);
                            continue;
                        }

                        var newObj = new ZonalInspectionInspector
                        {
                            Inspector = zonInsp,
                            ZonalInspection = serviceZonalInspection.Load(id)
                        };

                        serviceZonalInspectionInspector.Save(newObj);
                    }

                    // Если какието зон остались в dictZonalInspectionInspector то их удаляем
                    // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                    foreach (var keyValue in dictZonalInspectionInspector)
                    {
                        serviceZonalInspectionInspector.Delete(keyValue.Value);
                    }

                    Container.Release(serviceZonalInspection);
                    Container.Release(serviceZonalInspectionInspector);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public IDataResult ListZonalInspection(BaseParams baseParams)
        {
            var inspectorId = baseParams.Params.GetAs<long>("inpectorId");
            var service = Container.Resolve<IDomainService<ZonalInspectionInspector>>();

            var data = service.GetAll()
                .Where(x => x.Inspector.Id == inspectorId)
                .Select(x => new
                {
                    x.ZonalInspection.Id,
                    x.ZonalInspection.Name
                });

            int totalCount = data.Count();
            var result = data.ToArray();

            Container.Release(service);

            return new ListDataResult(result, totalCount);
        }
    }
}