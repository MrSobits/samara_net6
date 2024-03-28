namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class DisposalViolService : IDisposalViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DisposalViolation> ViolDomain { get; set; }

        public IDomainService<ActCheckViolation> ActViolDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }
        
        public IDomainService<DocumentGjiChildren> DcoChildrenDomain { get; set; }

        public IDomainService<Bars.GkhGji.Entities.ActCheck> ActCheckDomain { get; set; }

        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IDomainService<ActCheckViolation> ActCheckViolsDomain { get; set; }

        public virtual IDataResult ListRealityObject(BaseParams baseParams)
        {
            /*
             * Параметры могут входить следующие6
             * 1. documentId - идентификатор приказа
             * Тут мы по id приказа группируем нарушения по Домума и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var service = Container.Resolve<IDomainService<DisposalViolation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var documentId = baseParams.Params.GetAs<long>("documentId");

                var list = service.GetAll()
                    .Where(x => x.Document.Id == documentId)
                    .Select(x => new
                        {
                            x.Id,
                            Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : "",
                            RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : "",
                            RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0
                        })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                    .Filter(loadParam, Container)
                    .ToList();

                var data = list
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

                return new ListDataResult(
                    data.Order(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
           
        }

        public virtual IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var violIds = baseParams.Params.GetAs<long[]>("violIds");
            var insViolIds = baseParams.Params.GetAs<long[]>("insViolIds");

            var disposal = this.DisposalDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var childrenActs = ActCheckDomain.GetAll()
                              .Where(
                                  x =>
                                  x.Stage.Parent.Id
                                  == (disposal != null && disposal.Stage != null ? disposal.Stage.Id : 0))
                              .Select(x => x.Id)
                              .ToList();

            var actCheckRo = ActCheckRoDomain.GetAll()
                                .Where(x => childrenActs.Contains(x.ActCheck.Id))
                                .ToList()
                                .GroupBy(x => (x.RealityObject != null ? x.RealityObject.Id : 0) +"_"+x.ActCheck.Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var listInspectionViols = new List<InspectionGjiViol>();
            var listDisposalViols = new List<DisposalViolation>();
            var listActCheckRo = new List<ActCheckRealityObject>();
            var listActCheckViols = new List<ActCheckViolation>();

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
                        if (listIds.ContainsKey(id+ "_" + insViol.roId))
                        {
                            continue;
                        }

                        var viol = dictViolations[id];

                        var inspViol = new InspectionGjiViol
                        {
                            Inspection = disposal.Inspection,
                            RealityObject = insViol.roId > 0 ? new RealityObject { Id = insViol.roId } : null,
                            Violation = new ViolationGji { Id = id },
                            Description = viol.Name
                        };

                        listInspectionViols.Add(inspViol);

                        var disposalViol = new DisposalViolation
                        {
                            Document = disposal,
                            InspectionViolation = inspViol,
                            TypeViolationStage = TypeViolationStage.Detection
                        };

                        listDisposalViols.Add(disposalViol); 
                    }
                }
                else
                {
                    // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                    // Если для дома такая комбинация уже есть то тогда пролетаем мимо 
                    if ( listIds.ContainsKey(id+"_"+realObjId))
                    {
                        continue;
                    }

                    var viol = dictViolations[id];

                    var inspViol = new InspectionGjiViol
                    {
                        Inspection = disposal.Inspection,
                        RealityObject = realObjId > 0 ? new RealityObject { Id = realObjId } : null,
                        Violation = new ViolationGji { Id = id },
                        Description = viol.Name
                    };

                    listInspectionViols.Add(inspViol);

                    var disposalViol = new DisposalViolation
                    {
                        Document = disposal,
                        InspectionViolation = inspViol,
                        TypeViolationStage = TypeViolationStage.Detection
                    };

                    listDisposalViols.Add(disposalViol);    
                }
            }

            // теперь актуализируем Состояние Актов проверок чтобы в дочерних актах были ткие же нарушения как и в приказе
            foreach (var viol in listInspectionViols)
            {
                foreach (var actId in childrenActs)
                {
                    ActCheckRealityObject actRo = null;

                    var roId = viol.RealityObject != null ? viol.RealityObject.Id : 0;

                    if (actCheckRo.ContainsKey(roId + "_" + actId))
                    {
                        actRo = actCheckRo[roId + "_" + actId];
                    }
                    else
                    {
                        actRo = new ActCheckRealityObject
                        {
                            ActCheck = new GkhGji.Entities.ActCheck { Id = actId },
                            RealityObject = viol.RealityObject,
                            HaveViolation = YesNoNotSet.Yes
                        };
                        listActCheckRo.Add(actRo);

                        // просто фиксируем чтобы вслед раз уже получит ьто что ест ьтакой дом
                        actCheckRo.Add((viol.RealityObject!= null ? viol.RealityObject.Id : 0)+"_"+actId, actRo);
                    }

                    var actViol = new ActCheckViolation
                    {
                        Document = actRo.ActCheck,
                        ActObject = actRo,
                        InspectionViolation = viol,
                        TypeViolationStage = TypeViolationStage.Detection
                    };

                    listActCheckViols.Add(actViol);
                }
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    listInspectionViols.ForEach(x => InspectionViolDomain.Save(x));

                    listDisposalViols.ForEach(x => ViolDomain.Save(x));

                    listActCheckRo.ForEach(x => ActCheckRoDomain.Save(x));

                    listActCheckViols.ForEach(x => ActCheckViolsDomain.Save(x));

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