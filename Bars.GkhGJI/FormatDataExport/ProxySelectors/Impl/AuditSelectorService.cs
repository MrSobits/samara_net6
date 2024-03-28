namespace Bars.GkhGji.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.EntityExtensions;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class AuditSelectorService : BaseProxySelectorService<AuditProxy>
    {
        protected override IDictionary<long, AuditProxy> GetCache()
        {
            var baseJurPersonService = this.Container.Resolve<IBaseJurPersonService>();
            var actCheckRepos = this.Container.Resolve<IRepository<ActCheck>>();
            var baseJurPersonRepos = this.Container.Resolve<IRepository<BaseJurPerson>>();
            var baseDispHeadRepos = this.Container.Resolve<IRepository<BaseDispHead>>();
            var baseProsClaimRepos = this.Container.Resolve<IRepository<BaseProsClaim>>();
            var baseStatementRepos = this.Container.Resolve<IRepository<BaseStatement>>();
            var documentGjiInspectorRepos = this.Container.ResolveRepository<DocumentGjiInspector>();
            var typeSurveyGoalInspGjiRepos = this.Container.ResolveRepository<TypeSurveyGoalInspGji>();
            var typeSurveyTaskInspGjiRepos = this.Container.ResolveRepository<TypeSurveyTaskInspGji>();
            var disposalTypeSurveyRepos = this.Container.ResolveRepository<DisposalTypeSurvey>();

            using (this.Container.Using(baseJurPersonService,
                actCheckRepos,
                baseJurPersonRepos,
                baseDispHeadRepos,
                baseProsClaimRepos,
                baseStatementRepos,
                documentGjiInspectorRepos,
                typeSurveyGoalInspGjiRepos,
                typeSurveyTaskInspGjiRepos,
                disposalTypeSurveyRepos))
            {
                baseJurPersonService.FillPlanNumber();

                var gjiContragent = this.ProxySelectorFactory.GetSelector<GjiProxy>()
                    .ExtProxyListCache
                    .Single();
                var indDict = this.ProxySelectorFactory.GetSelector<IndProxy>()
                    .ExtProxyListCache
                    .GroupBy(x => $"{x.Surname}{x.FirstName}{x.SecondName}".ToUpper(), x => (long?)x.Id)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var inspectionQuery = this.FilterService.GetFiltredQuery<ViewFormatDataExportInspection>();

                var auditList = inspectionQuery
                    .Select(x => new
                    {
                        x.Id,
                        DisposalId = x.Disposal.Id,
                        ActCheckId = x.ActCheck.Id,
                        PlanNumber = (int?) (x.Inspection as BaseJurPerson).PlanNumber,
                        CheckType = x.Inspection.TypeBase == TypeBase.PlanJuridicalPerson ? 1 : 2,
                        CheckState = x.ActCheck.State.Name,
                        AuditPlanId = (long?) (x.Inspection as BaseJurPerson).Plan.Id,
                        RegistrationNumber = (x.Inspection as BaseJurPerson).Plan.UriRegistrationNumber,
                        RegistrationDate = (x.Inspection as BaseJurPerson).UriRegistrationDate,
                        DisposalNumber = x.DocumentNumber,
                        DisposalDate = x.DocumentDate,
                        SubjectContragentId = x.Inspection.Contragent.GetNullableId(),
                        x.Inspection.PhysicalPerson,
                        IsSubjectSmallBusiness = x.Inspection.Contragent.GetNullableId() != null
                            ? x.Inspection.Contragent.TypeEntrepreneurship == TypeEntrepreneurship.Small ? (int?) 1 : 2
                            : null,
                        FactActionPlace = x.Contragent.FactAddress,
                        StartDate = x.Disposal.DateStart,
                        EndDate =  x.Disposal.DateEnd,
                        ProsecutorAgreed =  x.Disposal.TypeAgreementProsecutor == TypeAgreementProsecutor.NotSet ? 2 : 1,
                        IsAgreed =  x.Disposal.TypeAgreementResult == TypeAgreementResult.Agreed ? 1 : 2,
                        AgreedorderNumber = x.DocumentNumber,
                        AgreedorderDate = x.DocumentDate
                    })
                    .ToList();

                var auditFormDict = baseJurPersonRepos.GetAll()
                    .Where(x => inspectionQuery.Any(y => x == y.Inspection))
                    .Select(x => new
                    {
                        x.Id,
                        TypeForm = (TypeFormInspection?)x.TypeForm
                    })
                    .AsEnumerable()
                    .Union(baseDispHeadRepos.GetAll()
                        .Where(x => inspectionQuery.Any(y => x == y.Inspection))
                        .Select(x => new
                        {
                            x.Id,
                            TypeForm = (TypeFormInspection?)x.TypeForm
                        })
                        .AsEnumerable())
                    .Union(baseProsClaimRepos.GetAll()
                        .Where(x => inspectionQuery.Any(y => x == y.Inspection))
                        .Select(x => new
                        {
                            x.Id,
                            TypeForm = (TypeFormInspection?)x.TypeForm
                        })
                        .AsEnumerable())
                    .Select(x => new
                    {
                        x.Id,
                        TypeForm = GetAuditForm(x.TypeForm)
                    })
                    .Union(baseStatementRepos.GetAll()
                        .Where(x => inspectionQuery.Any(y => x == y.Inspection))
                        .Select(x => new
                        {
                            x.Id,
                            FormCheck = x.TypeForm
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            TypeForm = GetAuditForm(x.FormCheck)
                        })
                    )
                    .ToDictionary(x => x.Id, x => x.TypeForm);


                var inspectorDict = documentGjiInspectorRepos.GetAll()
                    .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => inspectionQuery.Any(y => x.DocumentGji == y.Disposal))
                    .WhereNotNull(x => x.Inspector)
                    .Select(x => new
                    {
                        x.DocumentGji.Id,
                        x.Inspector.Fio,
                        x.Inspector.Position
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => $"{x.Fio} {x.Position}")
                    .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

                // Цели проверок
                var purposeDict = typeSurveyGoalInspGjiRepos.GetAll()
                    .Select(x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyPurpose.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Name)
                    .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

                // Задачи проверки
                var taskDict = typeSurveyTaskInspGjiRepos.GetAll()
                    .Select(x => new
                    {
                        x.TypeSurvey.Id,
                        x.SurveyObjective.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Name)
                    .ToDictionary(x => x.Key, x => x.Distinct().AggregateWithSeparator(";"));

                var typeSurveyGjiDict = disposalTypeSurveyRepos.GetAll()
                    .Where(x => inspectionQuery.Any(y => x.Disposal == y.Disposal))
                    .Select(x => new
                    {
                        x.Disposal.Id,
                        TypeSurveyId = x.TypeSurvey.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => new
                        {
                            Purposes = x.Select(y => purposeDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";"),
                            Tasks = x.Select(y => taskDict.Get(y.TypeSurveyId)).Distinct().AggregateWithSeparator(";")
                        });

                return auditList.Select(x =>
                    {
                        var typeSurvey = typeSurveyGjiDict.Get(x.DisposalId);
                        return new AuditProxy
                        {
                            Id = x.Id,
                            ContragentFrguId = gjiContragent.Id,
                            FrguId = gjiContragent.Id,
                            CheckType = x.CheckType,
                            CheckState = this.GetCheckStatus(x.CheckState),
                            AuditPlanId = x.AuditPlanId,

                            PlanNumber = x.PlanNumber,
                            RegistrationNumber = x.RegistrationNumber,
                            RegistrationDate = x.RegistrationDate,

                            //LastAuditDate = endCheckDict.Get(planInspection?.ContragentId ?? 0),
                            MustRegistered = 2, // Поле стало обязательным, пока передаём 2
                            RegistrationReason = 4,
                            AuditKind = 1, // Если проверка ... во всех остальных случаях - 1
                            AuditForm = auditFormDict.Get(x.Id),
                            DisposalNumber = x.DisposalNumber.Cut(255),
                            DisposalDate = x.DisposalDate,
                            Inspectors = inspectorDict.Get(x.DisposalId),
                            Param18 = null,
                            Param19 = null,
                            SubjectContragentId = x.SubjectContragentId,
                            SubjectPhysicalId = indDict.Get(x.PhysicalPerson.ToUpper().Replace(".", "").Replace(" ", "")),
                            IsSubjectSmallBusiness = x.IsSubjectSmallBusiness,
                            FactActionPlace = x.FactActionPlace,
                            Param24 = null,
                            NotificationState = 1,
                            NotificationDate = null,
                            NotificationMethod = null,
                            AuditReason = null,
                            Param29 = null,
                            DependentAuditId = null,
                            AuditReasonPurpose = typeSurvey?.Purposes,
                            Param32 = null,
                            AuditTask = typeSurvey?.Tasks,
                            StartDate = x.StartDate,
                            EndDate = x.EndDate,
                            WorkDaysCount = x.EndDate.HasValue && x.StartDate.HasValue
                                ? ((x.EndDate.Value.Date - x.StartDate.Value.Date).Days + 1)
                                : (int?)null,
                            WorkHoursCount = x.EndDate.HasValue && x.StartDate.HasValue
                                ? (((x.EndDate.Value.Date - x.StartDate.Value.Date).Days + 1) * 8)
                                : (int?)null,
                            AuditCompanion = null,
                            ProsecutorAgreed = x.ProsecutorAgreed,
                            IsAgreed = x.IsAgreed,
                            AgreedorderNumber = x.AgreedorderNumber,
                            AgreedorderDate = x.AgreedorderDate,
                            Param43 = null,
                            Param44 = null,
                            Param45 = null,
                            Param46 = null,
                            Param47 = null,
                            Param48 = null,
                            Param49 = null,
                            Param50 = null,
                            Param51 = null,
                            Param52 = null,
                            Param53 = null,
                            Param54 = null,
                            Param55 = null,
                            Param56 = null,
                            Param57 = null,
                            Param58 = null,

                            InspectionId = x.Id,
                            DisposalId = x.DisposalId,
                            ActCheckId = x.ActCheckId
                        };
                    })
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetAuditForm(TypeFormInspection? typeForm)
        {
            switch (typeForm)
            {
                case TypeFormInspection.Documentary:
                    return 1;
                case TypeFormInspection.Exit:
                    return 2;
                case TypeFormInspection.ExitAndDocumentary:
                    return 3;
                default:
                    return null;
            }
        }

        private int GetCheckStatus(string stateName)
        {
            return stateName == "В работе" ? 1 : 2;
        }
    }
}