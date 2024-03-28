namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActualizeDisposalViolationsAction : BaseExecutionAction
    {
        public IRepository<Disposal> DisposalDomain { get; set; }

        public IRepository<DisposalViolation> DisposalViolDomain { get; set; }

        public IRepository<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IRepository<ActCheckViolation> ActCheckViolDomain { get; set; }

        public IRepository<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        public IRepository<InspectionGjiViol> InspectionViolDomain { get; set; }

        public override string Description => @"Данный скрипт берет нарушения из предписаний и помещает их в родительский приказ, 
                                                в томс случае если таких нарушений вприказе еще нет. Скрипт можно выполнять повторно";

        public override string Name => "Томск - Актуализация нарушений для приказа";

        public override Func<IDataResult> Action => this.ActualizeDisposalViolations;

        private void DeleteDublicatesViolForDisposal()
        {
            var listToDelete = new List<long>();

            var dispViolations = this.DisposalViolDomain.GetAll()
                .Select(x => new {x.Id, violId = x.InspectionViolation.Id, docId = x.Document.Id})
                .ToList();

            var dictCurViol = new Dictionary<string, long>();

            foreach (var viol in dispViolations)
            {
                var key = viol.violId + "_" + viol.docId;

                if (!dictCurViol.ContainsKey(key))
                {
                    // первый стаким ключем элемент не будем удалять а остальные уже будем удалять
                    dictCurViol.Add(key, viol.Id);
                }
                else
                {
                    listToDelete.Add(viol.Id);
                }
            }

            if (listToDelete.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToDelete.ForEach(x => this.DisposalViolDomain.Delete(x));

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        private void UpdateInspectionViolations()
        {
            var listToUpdate = new List<InspectionGjiViol>();

            var prescr = this.PrescriptionViolDomain.GetAll()
                .Where(x => x.DatePlanRemoval.HasValue || (x.Action != null && x.Action != ""))
                .OrderBy(x => x.Id)
                .ToList();

            var dictUpdate = new Dictionary<long, InspectionGjiViol>();

            foreach (var presViol in prescr)
            {
                var ins = presViol.InspectionViolation;
                if (dictUpdate.ContainsKey(presViol.InspectionViolation.Id))
                {
                    ins = dictUpdate[presViol.InspectionViolation.Id];
                    listToUpdate.Add(ins);
                }
                else
                {
                    dictUpdate.Add(presViol.InspectionViolation.Id, ins);
                }

                if (presViol.DatePlanRemoval.HasValue)
                {
                    ins.DatePlanRemoval = presViol.DatePlanRemoval;
                }

                if (!string.IsNullOrEmpty(presViol.Action))
                {
                    ins.Action = presViol.Action;
                }
            }

            if (listToUpdate.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToUpdate.ForEach(x => this.InspectionViolDomain.Update(x));

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        public BaseDataResult ActualizeDisposalViolations()
        {
            var disposalViolationToSave = new List<DisposalViolation>();

            var notExistList = this.PrescriptionViolDomain.GetAll()
                .Where(x => x.Document.Stage.Parent != null)
                .Where(
                    x =>
                        !this.DisposalViolDomain.GetAll()
                            .Any(
                                y =>
                                    y.InspectionViolation.Id == x.InspectionViolation.Id
                                        && x.Document.Stage.Parent.Id == y.Document.Stage.Id))
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            parentStageId = x.Document.Stage.Parent.Id,
                            violId = x.InspectionViolation.Id
                        })
                .AsEnumerable()
                .GroupBy(x => x.parentStageId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.violId).Distinct().ToList());

            var findStages = notExistList.Keys.ToList();

            var dictStageDisposals = this.DisposalDomain.GetAll()
                .Where(x => findStages.Contains(x.Stage.Id))
                .Select(x => new {stageId = x.Stage.Id, x.Id})
                .AsEnumerable()
                .GroupBy(x => x.stageId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Id).FirstOrDefault());

            // помещаем не найденные нарушения в приказ 
            foreach (var kvp in notExistList)
            {
                if (!dictStageDisposals.ContainsKey(kvp.Key))
                {
                    continue;
                }

                var disposalId = dictStageDisposals[kvp.Key];

                foreach (var violId in kvp.Value)
                {
                    disposalViolationToSave.Add(
                        new DisposalViolation()
                        {
                            InspectionViolation =
                                new InspectionGjiViol {Id = violId},
                            Document = new DocumentGji {Id = disposalId},
                            TypeViolationStage = TypeViolationStage.Detection
                        });
                }
            }

            if (disposalViolationToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        disposalViolationToSave.ForEach(x => this.DisposalViolDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }

            // Дополнительно обновляем нарушения Мероприятие и дата неперешли в главную сущность
            this.UpdateInspectionViolations();

            //удаляем дубли котоыре могли получится
            this.DeleteDublicatesViolForDisposal();

            return new BaseDataResult();
        }
    }
}