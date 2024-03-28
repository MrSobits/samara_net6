namespace Bars.GkhGji.Regions.BaseChelyabinsk.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    public class ActCheckAddDateAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Name => "Заполнение дат проверок по целям";

        public override string Description => "Заполнение дат проверок по целям";

        public override Func<IDataResult> Action => this.ActCheckAddDate;

        public BaseDataResult ActCheckAddDate()
        {
            var docGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var disposalSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();
            var contragentAuditPurposeDomain = this.Container.ResolveDomain<ContragentAuditPurpose>();
            var auditPurposeSubjDomain = this.Container.ResolveDomain<AuditPurposeSurveySubjectGji>();
            try
            {
                var actChecks = docGjiChildrenDomain.GetAll()
                    .Where(
                        y => y.Parent.TypeDocumentGji == TypeDocumentGji.Disposal &&
                            y.Parent.Inspection.TypeBase == TypeBase.PlanJuridicalPerson &&
                            y.Children.TypeDocumentGji == TypeDocumentGji.ActCheck &&
                            y.Children.State.FinalState)
                    .Select(
                        x => new
                        {
                            DisposalId = x.Parent.Id,
                            x.Children.DocumentDate,
                            x.Children.Inspection.Contragent
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.DisposalId)
                    .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DocumentDate).First());

                var disposalSubjs = disposalSubjectDomain.GetAll()
                    .Where(
                        x => docGjiChildrenDomain.GetAll().Any(
                            y => y.Parent.Id == x.Disposal.Id
                                &&
                                y.Children.TypeDocumentGji ==
                                    TypeDocumentGji.ActCheck &&
                                y.Children.State.FinalState))
                    .Select(
                        x => new
                        {
                            x.Disposal.Id,
                            SurveySubjectId = x.SurveySubject.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.SurveySubjectId).ToArray());

                var existAuditPurps = contragentAuditPurposeDomain.GetAll()
                    .ToList()
                    .GroupBy(x => x.Contragent.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => y
                            .GroupBy(x => x.AuditPurpose.Id)
                            .ToDictionary(
                                x => x.Key,
                                z => z.First()));

                var auditPurposes = auditPurposeSubjDomain.GetAll()
                    .GroupBy(x => x.SurveySubject.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.AuditPurpose).ToArray());

                var fakeDict = new Dictionary<long, ContragentAuditPurpose>();
                var listToSave = new List<ContragentAuditPurpose>();
                foreach (var disposalSubj in disposalSubjs)
                {
                    var actCheck = actChecks.Get(disposalSubj.Key);

                    if (actCheck == null)
                    {
                        continue;
                    }

                    foreach (var surveySubjectId in disposalSubj.Value)
                    {
                        var tempAuditPurposes = auditPurposes.Get(surveySubjectId);

                        if (tempAuditPurposes == null)
                        {
                            continue;
                        }

                        var tempExistAuditPurps = existAuditPurps.Get(actCheck.Contragent.Id) ?? fakeDict;

                        foreach (var auditPuropse in tempAuditPurposes)
                        {
                            var contrAuditPurp = tempExistAuditPurps.Get(auditPuropse.Id) ?? new ContragentAuditPurpose
                            {
                                AuditPurpose = auditPuropse,
                                Contragent = actCheck.Contragent
                            };

                            if (contrAuditPurp.LastInspDate == null || contrAuditPurp.LastInspDate <= actCheck.DocumentDate)
                            {
                                contrAuditPurp.LastInspDate = actCheck.DocumentDate;
                            }

                            listToSave.Add(contrAuditPurp);
                        }
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave);
            }
            finally
            {
                this.Container.Release(docGjiChildrenDomain);
                this.Container.Release(disposalSubjectDomain);
                this.Container.Release(contragentAuditPurposeDomain);
                this.Container.Release(auditPurposeSubjDomain);
            }

            return new BaseDataResult();
        }
    }
}