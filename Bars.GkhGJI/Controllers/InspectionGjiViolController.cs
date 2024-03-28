namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// В контроллере находятся корректирование Нарушений 
    /// </summary>
    public class InspectionGjiViolController : BaseController
    {
        /// <summary>
        /// Корректирование протокольных нарушений (Monjf Протокольные нарушения и нарушения предписания были отдельным сущностями), 
        /// соединяем нарушения предписания и протокола, Удаляем лишнии нарушения в актах
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CorrectProtocolViol(BaseParams baseParams)
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var repInspectionGjiViolStage = Container.Resolve<IRepository<InspectionGjiViolStage>>();
            var servProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var servPrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>();

            // Словарь этапов нарушений протоколов
            var protocolsViolDict = servProtocolViolation.GetAll()
              .Where(x => x.ExternalId != null && x.InspectionViolation.ExternalId != null)
              .Select(x => new { ProtocolId = x.Document.Id, NoCorrectInspectionViol = x.InspectionViolation, InspectionGjiViolStageId = x.Id })
              .AsEnumerable()
              .GroupBy(x => x.ProtocolId)
              .ToDictionary(
              x => x.Key,
              x => x.Select(y =>
                  new TempProtViol
                  {
                      NoCorrectInspectionViol = y.NoCorrectInspectionViol,
                      InspectionGjiViolStageId = y.InspectionGjiViolStageId
                  }).ToList());

            var presctiptionViolsDict = servPrescriptionViol.GetAll()
            .Where(x => x.ExternalId != null)
            .Select(x => new { PrescriptionId = x.Document.Id, CorrectInspectionViol = x.InspectionViolation })
              .AsEnumerable()
              .GroupBy(x => x.PrescriptionId)
              .ToDictionary(x => x.Key, x => x.Select(y => y.CorrectInspectionViol));

            // Связи между протоколом и Предписанием
            var relationsProtocolPresctiption = servDocumentGjiChildren.GetAll()
                .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
               .ToDictionary(x => x.Children.Id, x => x.Parent.Id);

            // Связи между протоколом и Актом проверки
            var relationProtocolActCheck = servDocumentGjiChildren.GetAll()
                .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
               .ToDictionary(x => x.Children.Id, x => x.Parent.Id);

            // Связи между протоколом и Актом устранения нарушения
            var relationProtocolActRemoval = servDocumentGjiChildren.GetAll()
               .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
              .ToDictionary(x => x.Children.Id, x => x.Parent.Id);

            var relationActRemovalPresctiption = servDocumentGjiChildren.GetAll()
              .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null &&
                  x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval &&
                   x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                   .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id })
                   .AsEnumerable()
                   .GroupBy(x => x.ParentId)
                 .ToDictionary(x => x.Key, x => x.Select(y => y.ChildrenId).ToArray());

            // Словарь key идентификатор протокольного stage, value корректный InspectionGjiViol
            var dict = new Dictionary<long, InspectionGjiViol>();
            foreach (var protocol in protocolsViolDict)
            {
                // Ищем связанное с протоколом предписание
                if (relationsProtocolPresctiption.ContainsKey(protocol.Key))
                {
                    var presctiptionId = relationsProtocolPresctiption[protocol.Key];
                    var presctiptionViols = presctiptionViolsDict[presctiptionId];
                    ComparingViols(ref dict, protocol.Value, presctiptionViols);
                    continue;
                }

                // Ищем связанный с Протоколом Акт проверки
                if (relationProtocolActCheck.ContainsKey(protocol.Key))
                {
                    // var actCheckId = relationProtocolActCheck[protocol.Key];

                    // Ищем связанный с этапом
                    var stageParentId = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                        .Where(x => x.Id == protocol.Key)
                        .Select(x => x.Stage.Parent.Id)
                        .First();

                    var prescriptions = Container.Resolve<IDomainService<DocumentGji>>()
                             .GetAll()
                             .Where(x => x.ExternalId != null && x.Stage.Parent.Id == stageParentId && x.TypeDocumentGji == TypeDocumentGji.Prescription)
                             .Select(x => x.Id)
                             .ToArray();

                    if (prescriptions.Length == 0)
                    {
                        // Предписания не найдены (Значит в ветке один Акт проверки - Протокол) и значит тогда все ОК
                        continue;
                    }

                    var presctiptionViols = new List<InspectionGjiViol>();
                    if (prescriptions.Length == 1)
                    {
                        var presctiptionId = prescriptions[0];

                        if (!presctiptionViolsDict.ContainsKey(presctiptionId))
                        {
                            continue;
                        }

                        presctiptionViols.AddRange(presctiptionViolsDict[presctiptionId]);
                    }
                    else
                    {
                        foreach (var presctiptionId in prescriptions.Where(presctiptionViolsDict.ContainsKey))
                        {
                            presctiptionViols.AddRange(presctiptionViolsDict[presctiptionId]);
                        }
                    }

                    ComparingViols(ref dict, protocol.Value, presctiptionViols);
                    continue;
                }

                // Ищем связь протокола с актом устранения
                if (relationProtocolActRemoval.ContainsKey(protocol.Key))
                {
                    var actRemovalId = relationProtocolActRemoval[protocol.Key];

                    if (relationActRemovalPresctiption.ContainsKey(actRemovalId))
                    {
                        var prescriptionList = relationActRemovalPresctiption[actRemovalId];
                        var presctiptionViols = new List<InspectionGjiViol>();
                        if (prescriptionList.Length == 1)
                        {
                            var presctiptionId = prescriptionList[0];

                            if (!presctiptionViolsDict.ContainsKey(presctiptionId))
                            {
                                continue;
                            }

                            presctiptionViols.AddRange(presctiptionViolsDict[presctiptionId]);
                        }
                        else
                        {
                            foreach (var presctiptionId in prescriptionList.Where(presctiptionViolsDict.ContainsKey))
                            {
                                presctiptionViols.AddRange(presctiptionViolsDict[presctiptionId]);
                            }
                        }

                        ComparingViols(ref dict, protocol.Value, presctiptionViols);
                    }
                    else
                    {
                        // Cвязка Акт проверки - Акт устранения нарушения - Протокол
                        var noCorInspViolIds = protocol.Value.Select(x => x.NoCorrectInspectionViol.Id).ToList();
                        var actRemovalViols =
                            Container.Resolve<IDomainService<ActRemovalViolation>>()
                                     .GetAll()
                                     .Where(x => x.ExternalId != null && x.Document.Id == actRemovalId)
                                     .Select(x => x.InspectionViolation)
                                     .ToList();

                        if (noCorInspViolIds.Count < actRemovalViols.Count)
                        {
                            var viols = actRemovalViols
                                          .Where(x => !noCorInspViolIds.Contains(x.Id))
                                          .ToList();
                            ComparingViols(ref dict, protocol.Value, viols);
                        }
                    }
                }
            }

            var start = 1000;
            var tmpProtocolFirst = dict.Keys.Count > start ? dict.Keys.Take(1000).ToArray() : dict.Keys.ToArray();

            // Stage Протоколов на корректировку
            var protocolViolStages = repInspectionGjiViolStage.GetAll()
                .Where(x => tmpProtocolFirst.Contains(x.Id))
                .ToList();

            while (start < dict.Keys.Count)
            {
                var tmpProtocol = dict.Keys.Skip(start).Take(1000).ToArray();

                protocolViolStages.AddRange(repInspectionGjiViolStage.GetAll().Where(x => tmpProtocol.Contains(x.Id)).ToList());

                start += 1000;
            }

            var badInspViolations = protocolViolStages.Select(x => x.InspectionViolation.Id).ToArray();
            var badViolStages = GetLisStageWidthBadInspViolations(repInspectionGjiViolStage, badInspViolations);

            var protocolViolStage = protocolViolStages.ToDictionary(x => x.Id);

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var goodViolStage in dict)
                    {
                        var protocolStage = protocolViolStage[goodViolStage.Key];

                        var noCorrInspectionViolation = protocolStage.InspectionViolation;
                        var corrInspectionViolation = goodViolStage.Value;

                        if (noCorrInspectionViolation.Id == corrInspectionViolation.Id
                            || noCorrInspectionViolation.Violation.Id != corrInspectionViolation.Violation.Id)
                        {
                            continue;
                        }

                        // Находим -не корректные InspectionViolation в других stage и делаем им ссылку на правильный InspectionGjiViol
                        var badStages = badViolStages.ContainsKey(noCorrInspectionViolation.Id)
                                            ? badViolStages[noCorrInspectionViolation.Id].Where(
                                                x => x.Id != protocolStage.Id).ToList()
                                            : new List<InspectionGjiViolStage>();

                        foreach (var badStage in badStages.Where(x => x.Document.TypeDocumentGji != TypeDocumentGji.ActCheck && x.Document.TypeDocumentGji != TypeDocumentGji.ActRemoval))
                        {
                            badStage.InspectionViolation = corrInspectionViolation;
                            repInspectionGjiViolStage.Update(badStage);
                        }

                        foreach (var badActStage in badStages.Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Document.TypeDocumentGji == TypeDocumentGji.ActRemoval))
                        {
                            if (string.IsNullOrEmpty(badActStage.ExternalId))
                            {
                                continue;
                            }

                            repInspectionGjiViolStage.Delete(badActStage.Id);
                        }

                        protocolStage.InspectionViolation = corrInspectionViolation;
                        repInspectionGjiViolStage.Update(protocolStage);
                    }

                    tr.Commit();
                }
                catch (Exception exp)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (Exception e)
                    {
                        var message =
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace);
                        throw new Exception(message, exp);
                    }
                }
            }

            return new JsonNetResult(new { success = true });
        }

        /// <summary>
        /// Дополнительное корректирование (Корректирование нарушений упущенных в первом методе CorrectProtocolViol)
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult DopCorrectProtocolViol(BaseParams baseParams)
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var repInspectionGjiViol = Container.Resolve<IRepository<InspectionGjiViol>>();
            var repInspectionGjiViolStage = Container.Resolve<IRepository<InspectionGjiViolStage>>();
            var servProtocolViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var servPrescriptionViol = Container.Resolve<IDomainService<PrescriptionViol>>();

            var actRemovalViolsDict = Container.Resolve<IDomainService<ActRemovalViolation>>()
                                 .GetAll()
                                 .Where(x => x.ExternalId != null)
                                 .Select(x =>
                                     new
                                     {
                                         actRemovalId = x.Document.Id,
                                         InspectionViolationId = x.InspectionViolation.Id,
                                         ViolationId = x.InspectionViolation.Violation.Id
                                     })
                                 .AsEnumerable()
                                 .GroupBy(x => x.actRemovalId)
                                 .ToDictionary(x => x.Key, x => x.ToList());

            // Словарь этапов нарушений протоколов
            var protocolsViolDict = servProtocolViolation.GetAll()
              .Where(x => x.ExternalId != null && x.InspectionViolation.ExternalId != null)
              .Where(y => Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval))
              .Select(x =>
                  new
                  {
                      ProtocolId = x.Document.Id,
                      NoCorrectViolId = x.InspectionViolation.Violation.Id,
                      InspectionGjiViolId = x.InspectionViolation.Id,
                      InspectionGjiViolStageId = x.Id
                  })
              .AsEnumerable()
              .GroupBy(x => x.ProtocolId)
              .ToDictionary(
              x => x.Key,
              x => x.Select(y =>
                  new
                  {
                      y.NoCorrectViolId,
                      y.InspectionGjiViolStageId,
                      y.InspectionGjiViolId
                  }).ToList());

            // Связи между протоколом и Актом устранения нарушения
            var relationProtocolActRemoval = servDocumentGjiChildren.GetAll()
               .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
              .ToDictionary(x => x.Children.Id, x => x.Parent.Id);

            var presctiptionViolsDict = servPrescriptionViol.GetAll()
            .Where(x => x.ExternalId != null)
            .Where(y => Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval))
            .Select(x => new { PrescriptionId = x.Document.Id, CorrectInspectionViol = x.InspectionViolation })
              .AsEnumerable()
              .GroupBy(x => x.PrescriptionId)
              .ToDictionary(x => x.Key, x => x.Select(y => y.CorrectInspectionViol));

            var relationActRemovalPresctiption = servDocumentGjiChildren.GetAll()
              .Where(x => x.Parent.ExternalId != null && x.Children.ExternalId != null &&
                  x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval &&
                   x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                   .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id })
                   .AsEnumerable()
                   .GroupBy(x => x.ParentId)
                 .ToDictionary(x => x.Key, x => x.Select(y => y.ChildrenId).ToArray());

            // Словарь key идентификатор протокольного stage, value корректный InspectionGjiViol
            var dict = new Dictionary<long, long>();
            foreach (var protocol in protocolsViolDict)
            {
                // Ищем связь протокола с актом устранения
                var actRemovalId = relationProtocolActRemoval[protocol.Key];
                if (!relationActRemovalPresctiption.ContainsKey(actRemovalId))
                {
                    continue;
                }

                if (!actRemovalViolsDict.ContainsKey(actRemovalId))
                {
                    continue;
                }

                var prescriptionList = relationActRemovalPresctiption[actRemovalId];
                var presctiptionViols = new List<InspectionGjiViol>();
                foreach (var presctiptionId in prescriptionList.Where(presctiptionViolsDict.ContainsKey))
                {
                    presctiptionViols.AddRange(presctiptionViolsDict[presctiptionId]);
                }

                // Группируем по violId
                var prescriptionViols = presctiptionViols.GroupBy(x => x.Violation.Id).ToDictionary(x => x.Key, x => x.ToArray());

                // Протокольные нарушения которых нет в предписании
                var protocolViols = protocol.Value
                    .Where(x => !prescriptionViols.ContainsKey(x.NoCorrectViolId))
                    .GroupBy(x => x.NoCorrectViolId)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                if (protocolViols.Count == 0)
                {
                    continue;
                }

                var actRemovalViols = actRemovalViolsDict[actRemovalId]
                    .Where(x => protocolViols.Keys.Contains(x.ViolationId))
                        .ToArray();

                foreach (var protocolViol in protocolViols)
                {
                    var inspViolIds = protocolViol.Value.Select(x => x.InspectionGjiViolId).ToArray();
                    var corr = actRemovalViols.Where(x => !inspViolIds.Contains(x.InspectionViolationId))
                        .GroupBy(x => x.ViolationId)
                        .ToDictionary(x => x.Key, x => x.ToList());

                    if (!corr.ContainsKey(protocolViol.Key)) continue;
                    if (corr[protocolViol.Key].Count == protocolViol.Value.Length)
                    {
                        var i = 0;
                        foreach (var inspectionGjiViolse in protocolViol.Value)
                        {
                            dict.Add(inspectionGjiViolse.InspectionGjiViolStageId, corr[protocolViol.Key][i].InspectionViolationId);
                            i++;
                        }
                    }
                    else if (corr[protocolViol.Key].Count > protocolViol.Value.Length)
                    {
                        var length = corr[protocolViol.Key].Count;
                        var i = 0;
                        foreach (var inspectionGjiViolse in protocolViol.Value)
                        {
                            dict.Add(inspectionGjiViolse.InspectionGjiViolStageId, corr[protocolViol.Key][i].InspectionViolationId);
                            if (i < length - 1)
                            {
                                i++;
                            }
                        }
                    }
                }
            }

            // Stage Протоколов на корректировку
            var protocolViolStages = repInspectionGjiViolStage.GetAll().Where(x => dict.Keys.Contains(x.Id)).ToList();

            var badInspViolations = protocolViolStages.Select(x => x.InspectionViolation.Id).ToArray();
            var badViolStages = GetLisStageWidthBadInspViolations(repInspectionGjiViolStage, badInspViolations);

            var protocolViolStageDict = protocolViolStages.ToDictionary(x => x.Id);

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var goodViolStage in dict)
                    {
                        var protocolStage = protocolViolStageDict[goodViolStage.Key];

                        var noCorrInspectionViolation = protocolStage.InspectionViolation;
                        var corrInspectionViolationId = goodViolStage.Value;

                        var corrInspectionViolation = repInspectionGjiViol.Load(corrInspectionViolationId);

                        if (noCorrInspectionViolation.Id == corrInspectionViolation.Id
                            || noCorrInspectionViolation.Violation.Id != corrInspectionViolation.Violation.Id)
                        {
                            continue;
                        }

                        // Находим -не корректные InspectionViolation в других stage и делаем им ссылку на правильный InspectionGjiViol
                        var badStages = badViolStages.ContainsKey(noCorrInspectionViolation.Id)
                                            ? badViolStages[noCorrInspectionViolation.Id].Where(
                                                x => x.Id != protocolStage.Id).ToList()
                                            : new List<InspectionGjiViolStage>();

                        foreach (var badStage in badStages.Where(x => x.Document.TypeDocumentGji != TypeDocumentGji.ActCheck && x.Document.TypeDocumentGji != TypeDocumentGji.ActRemoval))
                        {
                            badStage.InspectionViolation = corrInspectionViolation;
                            repInspectionGjiViolStage.Update(badStage);
                        }

                        foreach (var badActStage in badStages.Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Document.TypeDocumentGji == TypeDocumentGji.ActRemoval))
                        {
                            if (string.IsNullOrEmpty(badActStage.ExternalId))
                            {
                                continue;
                            }

                            repInspectionGjiViolStage.Delete(badActStage.Id);
                        }

                        protocolStage.InspectionViolation = corrInspectionViolation;
                        repInspectionGjiViolStage.Update(protocolStage);
                    }

                    tr.Commit();
                }
                catch (Exception exp)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (Exception e)
                    {
                        var message =
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace);
                        throw new Exception(message, exp);
                    }
                }
            }

            return new JsonNetResult(new { success = true });
        }

        /// <summary>
        /// Окончательное корректирование нарушений между ветками. Удаление лишних нарушений в проверке
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Correct(BaseParams baseParams)
        {
            var servInspectionGjiViolStage = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
            var servInspectionGjiViol = Container.Resolve<IDomainService<InspectionGjiViol>>();

            var inspections = Container.Resolve<IDomainService<Disposal>>().GetAll()
                .Where(x => x.TypeDisposal == TypeDisposalGji.Base && x.Inspection.ExternalId != null)
                .Select(x => new { StageId = x.Stage.Id, x.Inspection.Id })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.StageId).ToList());

            var disposalsDict = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                         .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal && x.Children.ExternalId != null)
                         .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id, StageId = x.Children.Stage.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.ParentId)
                         .ToDictionary(x => x.Key, x => x.Select(y => new ProxyDisposal { Id = y.ChildrenId, StageId = y.StageId }).ToList());

            var prescriptionDict = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                         .Where(x => x.Stage.Parent != null && x.ExternalId != null && x.TypeDocumentGji == TypeDocumentGji.Prescription)
                         .Select(x => new { x.Id, StageParentId = x.Stage.Parent.Id })
                         .AsEnumerable()
                         .GroupBy(x => x.StageParentId)
                         .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

            // Словарь этапов проверки
            var stagesList = servInspectionGjiViolStage.GetAll()
                       .Where(x => x.ExternalId != null)
                       .Select(x =>
                           new TempViolStage
                           {
                               Id = x.Id,
                               InspectionId = x.InspectionViolation.Inspection.Id,
                               InspectionViolationId = x.InspectionViolation.Id,
                               ViolationId = x.InspectionViolation.Violation.Id,
                               StageParentId = x.Document.Stage.Parent != null ? x.Document.Stage.Parent.Id : (long?)null,
                               DocumentId = x.Document.Id,
                               TypeDocumentGji = x.Document.TypeDocumentGji
                           })
                       .ToList();

            // сгруппированы по проверки
            var stagesDict = stagesList.GroupBy(x => x.InspectionId).ToDictionary(x => x.Key, x => x.ToList());

            // сгруппированны по StageParent и нарушению
            var documentStages = stagesList
                          .Where(x => x.StageParentId != null)
                         .Select(
                             x =>
                             new
                                 {
                                     compositeKey = string.Format("{0}_{1}", x.StageParentId, x.ViolationId),
                                     InspectionGjiViolStage = x
                                 })
                         .AsEnumerable()
                         .GroupBy(x => x.compositeKey)
                         .ToDictionary(
                         x => x.Key, 
                         x => x.Select(y => y.InspectionGjiViolStage)
                             .GroupBy(y => y.DocumentId)
                             .ToDictionary(y => y.Key, y => y.ToList()));

            var inspViolPrescriptionDict = stagesList
                       .Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription && x.StageParentId != null)
                               .Select(x =>
                                   new
                                   {
                                       Id = string.Format("{0}_{1}", x.InspectionId, x.StageParentId),
                                       PrescriptionId = x.DocumentId,
                                       PrescriptionViol = x
                                   })
                               .AsEnumerable()
                               .GroupBy(x => x.Id)
                               .ToDictionary(
                               x => x.Key,
                               x => x.Select(y => y.PrescriptionViol).ToList());

            // Словарь для корректирования Key = InspectionGjiViolStageId, Value - корректный InspectionViolationId
            var correctInspViolStage = new Dictionary<long, long>();
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                foreach (var inspection in inspections)
                {
                    foreach (var stageId in inspection.Value)
                    {
                        // ключ по проверки и этапу проверки
                        var key = string.Format("{0}_{1}", inspection.Key, stageId);

                        if (!inspViolPrescriptionDict.ContainsKey(key))
                        {
                            continue;
                        }

                        // Справочник нарушений по проверке(список по первому этапу)  key - violId
                        var inspViolsDict = inspViolPrescriptionDict[key]
                                 .GroupBy(x => x.ViolationId)
                                 .ToDictionary(x => x.Key, x => x.ToList());

                        // оставляем только одиночные нарушения
                        var oneInspViolsDict = inspViolsDict.Where(x => x.Value.Count == 1).ToDictionary(x => x.Key, x => x.Value[0]);
                        var standartViolStageIds = oneInspViolsDict.Values.Select(x => x.Id).ToArray();

                        // Этапы проверки инспекционной проверки с данным нарушением и не в эталонных этапах нарушений, но этапы из других 
                        TempViolStage[] stages;
                        if (inspection.Value.Count == 1)
                        {
                            // инспекционных проверка с одним базовым распоряжением
                            stages = stagesDict[inspection.Key]
                                .Where(x => !standartViolStageIds.Contains(x.Id) && oneInspViolsDict.ContainsKey(x.ViolationId))
                                .ToArray();
                        }
                        else
                        {
                            // у некоторых инспекционных проверок несколько базовых распоряжений, берем stages из нужной ветки
                            var filterStage = new List<long> { stageId };
                            GetStagesForDisposal(ref filterStage, stageId, prescriptionDict, disposalsDict);
                           stages = stagesDict[inspection.Key]
                               .Where(x => x.StageParentId != null && (filterStage.Contains(x.StageParentId.Value)
                                   && !standartViolStageIds.Contains(x.Id) && oneInspViolsDict.ContainsKey(x.ViolationId)))
                              .ToArray();
                        }

                        foreach (var inspectionGjiViolStage in stages)
                        {
                            var standartInspViol = oneInspViolsDict[inspectionGjiViolStage.ViolationId];
                            if (inspectionGjiViolStage.InspectionViolationId == standartInspViol.InspectionViolationId)
                            {
                                continue;
                            }

                            // заменяем ссылку на эталонную InspectionGjiViol
                            correctInspViolStage.Add(inspectionGjiViolStage.Id, standartInspViol.InspectionViolationId);
                        }

                        // обрабатываем не одиночные нарушения Key - ViolId;  Value - InspectionGjiViolStage
                        var fewInspViolsStageDict = inspViolsDict.Where(x => x.Value.Count > 1).ToArray();
                        foreach (var inspectionGjiViol in fewInspViolsStageDict)
                        {
                            var docsViols = inspectionGjiViol.Value.GroupBy(x => x.DocumentId).ToDictionary(x => x.Key, x => x.ToList());
                            foreach (var docViols in docsViols)
                            {
                                var documentId = docViols.Key;
                                if (!disposalsDict.ContainsKey(documentId))
                                {
                                    // Нет распоряжений у этого предписания
                                    continue;
                                }

                                // Если есть след ветки, то корректируем в них нарушения
                                var standartViols = docViols.Value.Select(x => x.InspectionViolationId).ToList();
                                this.Fill(disposalsDict, disposalsDict[documentId], documentStages, inspectionGjiViol.Key, standartViols, prescriptionDict, ref correctInspViolStage);
                            }
                        }
                    }
                }

                var start = 1000;
                var tmpFirst = correctInspViolStage.Keys.Count > start ? correctInspViolStage.Keys.Take(1000).ToArray() : correctInspViolStage.Keys.ToArray();

                // Stages на корректировку
                var violStages = servInspectionGjiViolStage.GetAll()
                    .Where(x => tmpFirst.Contains(x.Id))
                    .ToList();

                while (start < correctInspViolStage.Keys.Count)
                {
                    var tmp = correctInspViolStage.Keys.Skip(start).Take(1000).ToArray();
                    violStages.AddRange(servInspectionGjiViolStage.GetAll().Where(x => tmp.Contains(x.Id)).ToList());
                    start += 1000;
                }

                var violStagesDict = violStages.ToDictionary(x => x.Id, x => x);
             
                foreach (var inspectionGjiViolStage in correctInspViolStage)
                {
                    var st = violStagesDict[inspectionGjiViolStage.Key];

                    // Пропускаем stages с корректным InspectionViolation
                    if (st.InspectionViolation.Id == inspectionGjiViolStage.Value)
                    {
                        continue;
                    }

                    var inspectionGjiViol = servInspectionGjiViol.Load(inspectionGjiViolStage.Value);
                    var dateFactRemoval = servInspectionGjiViol.GetAll()
                                             .Where(x => x.Id == inspectionGjiViolStage.Value)
                                             .Select(x => x.DateFactRemoval)
                                             .FirstOrDefault();

                   if (st.InspectionViolation.DateFactRemoval > dateFactRemoval)
                   {
                      inspectionGjiViol.DateFactRemoval = st.InspectionViolation.DateFactRemoval;
                      servInspectionGjiViol.Update(inspectionGjiViol);
                   }

                    st.InspectionViolation = inspectionGjiViol;
                    servInspectionGjiViolStage.Update(st);
                }

                 tr.Commit();
                //tr.Rollback();
            }

            return new JsonNetResult(new { success = true });
        }

        private void GetStagesForDisposal(ref List<long> stages, long stageId, Dictionary<long, List<long>> prescriptionDict, Dictionary<long, List<ProxyDisposal>> disposalsDict)
        {
            if (prescriptionDict.ContainsKey(stageId))
            {
                var prescriptions = prescriptionDict[stageId];

                foreach (var prescriptionId in prescriptions)
                {
                    if (disposalsDict.ContainsKey(prescriptionId))
                    {
                        var disposals = disposalsDict[prescriptionId];
                        foreach (var proxyDisposal in disposals)
                        {
                            stages.Add(proxyDisposal.StageId);
                            GetStagesForDisposal(ref stages, proxyDisposal.StageId, prescriptionDict, disposalsDict);
                        }
                    }
                }
            }
        }

        private void ComparingViols(ref Dictionary<long, InspectionGjiViol> result, IEnumerable<TempProtViol> protocolViolStages, IEnumerable<InspectionGjiViol> presctiptionViols)
        {
            // Группируем по violId
            var correctViols = presctiptionViols.GroupBy(x => x.Violation.Id).ToDictionary(x => x.Key, x => x.ToArray());
            var noCorrectViols = protocolViolStages.GroupBy(x => x.NoCorrectInspectionViol.Violation.Id).ToDictionary(x => x.Key, x => x.ToArray());

            foreach (var noCorrectViol in noCorrectViols)
            {
                if (noCorrectViol.Value.Length == 1)
                {
                    if (correctViols.ContainsKey(noCorrectViol.Key))
                    {
                        if (correctViols[noCorrectViol.Key].Length == 1)
                        {
                            result.Add(noCorrectViol.Value[0].InspectionGjiViolStageId, correctViols[noCorrectViol.Key][0]);
                        }
                        else
                        {
                            // берем первый попавшийся
                            result.Add(noCorrectViol.Value[0].InspectionGjiViolStageId, correctViols[noCorrectViol.Key][0]);
                        }
                    }
                }
                else
                {
                    if (correctViols.ContainsKey(noCorrectViol.Key))
                    {
                        if (correctViols[noCorrectViol.Key].Length == noCorrectViol.Value.Length)
                        {
                            var i = 0;
                            foreach (var inspectionGjiViolse in noCorrectViol.Value)
                            {
                                result.Add(inspectionGjiViolse.InspectionGjiViolStageId, correctViols[noCorrectViol.Key][i]);
                                i++;
                            }
                        }
                        else if (correctViols[noCorrectViol.Key].Length > noCorrectViol.Value.Length)
                        {
                            var i = 0;
                            foreach (var inspectionGjiViolse in noCorrectViol.Value)
                            {
                                result.Add(inspectionGjiViolse.InspectionGjiViolStageId, correctViols[noCorrectViol.Key][i]);
                                i++;
                            }
                        }
                        else if (correctViols[noCorrectViol.Key].Length < noCorrectViol.Value.Length)
                        {
                            // Это не верно брать нарушение из не связанного предписания но бывает пример Акт "ПР-14195"
                            var length = correctViols[noCorrectViol.Key].Length;
                            var i = 0;
                            foreach (var inspectionGjiViolse in noCorrectViol.Value)
                            {
                                result.Add(inspectionGjiViolse.InspectionGjiViolStageId, correctViols[noCorrectViol.Key][i]);
                                if (i < length - 1)
                                {
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private class TempProtViol
        {
            public long InspectionGjiViolStageId { get; set; }

            public InspectionGjiViol NoCorrectInspectionViol { get; set; }
        }

        private void Fill(
            Dictionary<long, List<ProxyDisposal>> disposalsDict,
            IEnumerable<ProxyDisposal> disposals,
            Dictionary<string, Dictionary<long, List<TempViolStage>>> documentStages,
            long standartViolId,
            List<long> standartInspViols,
            Dictionary<long, List<long>> prescriptionDict,
            ref Dictionary<long, long> correctInspViolStage)
        {
            foreach (var disposal in disposals)
            {
                var key = string.Format("{0}_{1}", disposal.StageId, standartViolId);

                if (!documentStages.ContainsKey(key))
                {
                    continue;
                }

                // Берем этапы нарушений у след ветки сгрупированные по документам
                foreach (var docStages in documentStages[key])
                {
                    var noCorrectViols = docStages.Value;
                    if (noCorrectViols.Count == standartInspViols.Count)
                    {
                        var i = 0;
                        foreach (var inspectionGjiViolse in noCorrectViols)
                        {
                            if (inspectionGjiViolse.InspectionViolationId != standartInspViols[i])
                            {
                                correctInspViolStage.Add(inspectionGjiViolse.Id, standartInspViols[i]);
                            }

                            i++;
                        }
                    }
                    else if (standartInspViols.Count > noCorrectViols.Count)
                    {
                        var i = 0;
                        foreach (var inspectionGjiViolse in noCorrectViols)
                        {
                            if (inspectionGjiViolse.InspectionViolationId != standartInspViols[i])
                            {
                                correctInspViolStage.Add(inspectionGjiViolse.Id, standartInspViols[i]);
                            }

                            i++;
                        }
                    }
                    else if (noCorrectViols.Count > standartInspViols.Count)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }

                if (!prescriptionDict.ContainsKey(disposal.StageId))
                {
                    continue;
                }

                foreach (var prescriptionId in prescriptionDict[disposal.StageId])
                {
                    if (disposalsDict.ContainsKey(prescriptionId))
                    {
                        this.Fill(
                            disposalsDict,
                            disposalsDict[prescriptionId],
                            documentStages,
                            standartViolId,
                            standartInspViols,
                            prescriptionDict,
                            ref correctInspViolStage);
                    }
                }
            }
        }

        private Dictionary<long, List<InspectionGjiViolStage>> GetLisStageWidthBadInspViolations(IRepository<InspectionGjiViolStage> service, long[] badInspViolations)
        {
            int start = 1000;
            var tmpProtocolFirst = badInspViolations.Length > start ? badInspViolations.Take(1000).ToArray() : badInspViolations.ToArray();

            // Stages на корректировку
            var violStages = service.GetAll().Where(x => tmpProtocolFirst.Contains(x.InspectionViolation.Id)).ToList();

            while (start < badInspViolations.Length)
            {
                var tmpProtocol = badInspViolations.Skip(start).Take(1000).ToArray();

                violStages.AddRange(service.GetAll().Where(x => tmpProtocol.Contains(x.InspectionViolation.Id)).ToList());

                start += 1000;
            }

            return violStages.GroupBy(x => x.InspectionViolation.Id).ToDictionary(x => x.Key, x => x.ToList());
        }

        private class ProxyDisposal
        {
            public long Id { get; set; }

            public long StageId { get; set; }
        }

        private class TempViolStage
        {
            public long Id { get; set; }

            public long InspectionId { get; set; }

            public long InspectionViolationId { get; set; }

            public long ViolationId { get; set; }

            public long? StageParentId { get; set; }

            public long DocumentId { get; set; }

            public TypeDocumentGji TypeDocumentGji { get; set; }
        }
    }
}
