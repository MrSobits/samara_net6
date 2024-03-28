namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    using Castle.Windsor;

    using TypeDocumentGji = Bars.GkhGji.Enums.TypeDocumentGji;

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

            var docGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var disposalSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();
            var contragentAuditPurposeDomain = this.Container.ResolveDomain<ContragentAuditPurpose>();
            var auditPurposeSubjDomain = this.Container.ResolveDomain<AuditPurposeSurveySubjectGji>();
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

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave);
            }
            finally
            {
                this.Container.Release(docGjiChildrenDomain);
                this.Container.Release(disposalSubjectDomain);
                this.Container.Release(contragentAuditPurposeDomain);
                this.Container.Release(auditPurposeSubjDomain);
            }

            return ValidateResult.Yes();
        }

        
    }
}
