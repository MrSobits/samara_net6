namespace Bars.GkhGji.Regions.Nso.StateChange
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;
    using Gkh.Domain;
    using Gkh.Entities;
    using GkhGji.Entities;
    using GkhGji.Entities.Dict;
    using Regions.Nso.Entities.Disposal;
    using TypeDocumentGji = GkhGji.Enums.TypeDocumentGji;

    public class ActCheckAddDateStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "ActCheckAddDateStateRule"; }
        }

        public string Name { get { return "Заполнение значение поля \"Дата прошлой проверки\" цели"; } }
        public string TypeId { get { return "gji_document_actcheck"; } }
        public string Description
        {
            get
            {
                return "При переводе статуса будет заполнено значение поля \"Дата прошлой проверки\" цели в карточке Контрагента";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var actCheck = statefulEntity as ActCheck;

            var docGjiChildrenDomain = Container.ResolveDomain<DocumentGjiChildren>();
            var disposalSubjectDomain = Container.ResolveDomain<DisposalVerificationSubject>();
            var contragentAuditPurposeDomain = Container.ResolveDomain<ContragentAuditPurpose>();
            var auditPurposeSubjDomain = Container.ResolveDomain<AuditPurposeSurveySubjectGji>();
            try
            {
                if (actCheck == null)
                {
                    return ValidateResult.No("Невозможно определить акт проверки!");
                }

                var disposal = docGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == actCheck.Id)
                    .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Select(x => x.Parent)
                    .FirstOrDefault();

                if (disposal == null)
                {
                    return ValidateResult.No("Невозможно определить документ!");
                }

                var contragent = actCheck.Inspection.Contragent;

                if (contragent == null)
                {
                    return ValidateResult.No("Невозможно определить контрагент!");
                }

                var existAuditPurps = contragentAuditPurposeDomain.GetAll()
                    .Where(x => x.Contragent.Id == contragent.Id)
                    .AsEnumerable()
                    .GroupBy(x => x.AuditPurpose.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var survSubjectIds = disposalSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.SurveySubject.Id)
                    .ToArray();

                var auditPuropses = auditPurposeSubjDomain.GetAll()
                    .Where(x => survSubjectIds.Contains(x.SurveySubject.Id))
                    .Select(x => x.AuditPurpose)
                    .ToArray();

                var listToSave = new List<ContragentAuditPurpose>();
                foreach (var auditPuropse in auditPuropses)
                {
                    var contrAuditPurp = existAuditPurps.Get(auditPuropse.Id) ?? new ContragentAuditPurpose
                    {
                        AuditPurpose = auditPuropse,
                        Contragent = contragent
                    };

                    contrAuditPurp.LastInspDate = actCheck.DocumentDate;

                    listToSave.Add(contrAuditPurp);
                }

                TransactionHelper.InsertInManyTransactions(Container, listToSave);
            }
            finally
            {
                Container.Release(docGjiChildrenDomain);
                Container.Release(disposalSubjectDomain);
                Container.Release(contragentAuditPurposeDomain);
                Container.Release(auditPurposeSubjDomain);
            }

            return ValidateResult.Yes();
        }

        
    }
}
