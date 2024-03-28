namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Entities;

    using Castle.Windsor;

    public class AdminCaseViolService : IAdminCaseViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCaseViolation> ViolDomain { get; set; }

        public IDomainService<AdministrativeCase> AdminCaseDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public virtual IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var violIds = baseParams.Params.GetAs<long[]>("violIds");

            var adminCase = this.AdminCaseDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            if (adminCase == null)
            {
                return new BaseDataResult(new { success = false, message = string.Format("Не удалось получить административное дело") });
            }

            if (adminCase.RealityObject == null)
            {
                return new BaseDataResult(new { success = false, message = string.Format("Не указан дом для административного дела") }); 
            }

            var realObjId = adminCase.RealityObject.Id;

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
                                      roId = x.InspectionViolation != null ? x.InspectionViolation.RealityObject.Id : 0
                                  })
                          .ToList()
                          .GroupBy(x => x.Id + "_" + x.roId)
                          .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var listInspectionViols = new List<InspectionGjiViol>();
            var listAdminCaseViols = new List<AdministrativeCaseViolation>();

            // получаю словарь на случай если будет выбрано много нарушений будет работать быстрее, чем получение по отдельности Нарушения
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
                
                // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                // Если для дома такая комбинация уже есть, то тогда пролетаем мимо 
                if ( listIds.ContainsKey( id + "_" + realObjId))
                {
                    continue;
                }

                var viol = dictViolations[id];

                var inspViol = new InspectionGjiViol
                {
                    Inspection = adminCase.Inspection,
                    RealityObject = realObjId > 0 ? new RealityObject { Id = realObjId } : null,
                    Violation = new ViolationGji { Id = id },
                    Description = viol.Name
                };

                listInspectionViols.Add(inspViol);

                var adminCaseViol = new AdministrativeCaseViolation
                {
                    Document = adminCase,
                    InspectionViolation = inspViol,
                    TypeViolationStage = TypeViolationStage.Detection
                };

                listAdminCaseViols.Add(adminCaseViol);    
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    listInspectionViols.ForEach(x => InspectionViolDomain.Save(x));

                    listAdminCaseViols.ForEach(x => ViolDomain.Save(x));

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