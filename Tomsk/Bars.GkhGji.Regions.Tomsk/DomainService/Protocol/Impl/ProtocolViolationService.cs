namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ProtocolViolationService : IProtocolViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProtocolViolation> ViolDomain { get; set; }

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public IDataResult ListRealityObject(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * Параметры могут входить следующие:
             * 1. documentId - идентификатор протокола
             * Тут мы по id протокола группируем нарушения по Дому и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = ViolDomain.GetAll()
                .Where(x => x.Document.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : "",
                    RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : "",
                    RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .Filter(loadParam, Container)
                .AsEnumerable()
                .GroupBy(x => new { x.RealityObjectId, x.Municipality, x.RealityObject })
                .Select(x => new
                {
                    Id = x.Key.RealityObjectId,
                    ViolationCount = x.Count(),
                    x.Key.Municipality,
                    RealityObject = x.Key.RealityObject == "" ? "Дом отсутствует" : x.Key.RealityObject
                })
                .AsQueryable();

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }

        public IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var insViolIds = baseParams.Params.GetAs<long[]>("insViolIds");

            var violIds = new long[0];
            
            var protocol = this.ProtocolDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var listProtocolViols = new List<ProtocolViolation>();

            var dictInsViols = InspectionViolDomain.GetAll()
                                    .Where(x => insViolIds.Contains(x.Id))
                                    .Select(x => new { x.Id, violId = x.Violation.Id, roId = x.RealityObject != null ? x.RealityObject.Id : 0 })
                                    .ToList()
                                    .GroupBy(x => x.violId)
                                    .ToDictionary(x => x.Key, y => y.ToList());
            if (dictInsViols.Any())
            {
                violIds = dictInsViols.Keys.ToArray();
            }

            // в этом списке будут id нарушений, которые уже связаны с этим предписанием
            // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
            var listIds =
                ViolDomain.GetAll()
                          .Where(x => x.Document.Id == documentId)
                          .Where(x => violIds.Contains(x.InspectionViolation.Violation.Id))
                          .Select(
                              x =>
                              new
                              {
                                  x.InspectionViolation.Violation.Id,
                                  roId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0
                              })
                          .ToList()
                          .GroupBy(x => x.Id + "_" + x.roId)
                          .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            // получаю словарь на случай если будет выбрано мног онарушений будет работат ьбыстрее чем получение по отдельности Нарушения
            var dictViolations = DictViolationDomain.GetAll()
                                .Where(x => violIds.Contains(x.Id))
                                .Select(x => new { x.Id, x.Name })
                                .ToList()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());


            foreach (var id in violIds)
            {
                // если нет в словаре то тоже выходим
                if (!dictViolations.ContainsKey(id))
                {
                    continue;
                }

                if (insViolIds.Any())
                {
                    // В этом случае имеем дело с нарушениями котоыревыбраны для приказа
                    // из родительского документа, а значит бежим по каждому нарушению и создаем для него комбинацию
                    if (!dictInsViols.ContainsKey(id))
                    {
                        continue;
                    }

                    foreach (var insViol in dictInsViols[id])
                    {
                        // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                        // Если для дома такая комбинация уже есть то тогда пролетаем мимо 
                        if (listIds.ContainsKey(id + "_" + insViol.roId))
                        {
                            continue;
                        }
                        
                        var protocolViol = new ProtocolViolation()
                        {
                            Document = protocol,
                            InspectionViolation = new InspectionGjiViol { Id = insViol.Id},
                            TypeViolationStage = TypeViolationStage.Sentence
                        };

                        listProtocolViols.Add(protocolViol);
                    }
                }
                
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                
                try
                {
                    listProtocolViols.ForEach(x => ViolDomain.Save(x));
                    
                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(new { success = false, message = e.Message });
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}