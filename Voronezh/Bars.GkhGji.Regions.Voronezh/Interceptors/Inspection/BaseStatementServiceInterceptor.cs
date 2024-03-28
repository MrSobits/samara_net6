namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class BaseStatementServiceInterceptor : BaseStatementServiceInterceptor<BaseStatement>
    {
    }

    public class BaseStatementServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseStatement
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var serviceNumberRule = Container.Resolve<IBaseStatementNumberRule>();
            try
            {
                // Перед сохранением формируем номер основания проверки
                serviceNumberRule.SetNumber(entity);

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceNumberRule);
            }

        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            // Удаляем все дочерние обращения
            var domainStatAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var domainCheDisp = Container.Resolve<IDomainService<ChelyabinskDisposal>>();
            var domainChelyabinskDisposalProvidedDoc = Container.Resolve<IDomainService<ChelyabinskDisposalProvidedDoc>>();
            var domainDisposalAdditionalDoc = Container.Resolve<IDomainService<DisposalAdditionalDoc>>();
            var domainDisposalControlMeasures = Container.Resolve<IDomainService<DisposalControlMeasures>>();
            var domainDisposalDocConfirm = Container.Resolve<IDomainService<DisposalDocConfirm>>();
            var domainDisposalFactViolation = Container.Resolve<IDomainService<DisposalFactViolation>>();//
            var domainDisposalLongText = Container.Resolve<IDomainService<DisposalLongText>>();

            var domainDisposalAdminRegulation = Container.Resolve<IDomainService<DisposalAdminRegulation>>();
            var domainDisposalAnnex = Container.Resolve<IDomainService<DisposalAnnex>>();
            var domainDisposalExpert = Container.Resolve<IDomainService<DisposalExpert>>();
            var domainDisposalInspFoundation = Container.Resolve<IDomainService<DisposalInspFoundation>>();
            var domainDisposalInspFoundationCheck = Container.Resolve<IDomainService<DisposalInspFoundationCheck>>();//
            var domainDisposalInspFoundCheckNormDocItem = Container.Resolve<IDomainService<DisposalInspFoundCheckNormDocItem>>();

            var domainDisposalProvidedDoc = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var domainDisposalSurveyObjective = Container.Resolve<IDomainService<DisposalSurveyObjective>>();
            var domainDisposalSurveyPurpose = Container.Resolve<IDomainService<DisposalSurveyPurpose>>();
            var domainDisposalTypeSurvey = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var domainDisposalVerificationSubject = Container.Resolve<IDomainService<DisposalVerificationSubject>>();//
            var domainDisposalViolation = Container.Resolve<IDomainService<DisposalViolation>>();

            var domainDecisionAdminRegulation = Container.Resolve<IDomainService<DecisionAdminRegulation>>();
            var domainDecisionAnnex = Container.Resolve<IDomainService<DecisionAnnex>>();
            var domainDecisionControlList = Container.Resolve<IDomainService<DecisionControlList>>();
            var domainDecisionControlMeasures = Container.Resolve<IDomainService<DecisionControlMeasures>>();
            var domainDecisionControlSubjects = Container.Resolve<IDomainService<DecisionControlSubjects>>();//
            var domainDecisionExpert = Container.Resolve<IDomainService<DecisionExpert>>();
            var domainDecisionInspectionReason = Container.Resolve<IDomainService<DecisionInspectionReason>>();
            var domainDecisionProvidedDoc = Container.Resolve<IDomainService<DecisionProvidedDoc>>();
            var domainDecisionVerificationSubject = Container.Resolve<IDomainService<DecisionVerificationSubject>>();
            var domainDecision = Container.Resolve<IDomainService<Decision>>();

            try
            {
                var statAppealIds = domainStatAppeal.GetAll()
                .Where(x => x.Inspection.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

                foreach (var id in statAppealIds)
                {
                    domainStatAppeal.Delete(id);
                }

                var disps = domainCheDisp.GetAll()
                    .Where(x => x.Inspection.Id == entity.Id)
                    .Select(x => x.Id).ToList();
                foreach (long dispId in disps)
                {
                    domainChelyabinskDisposalProvidedDoc.GetAll()
                        .Where(x => x.Disposal.Id == dispId)
                        .Select(x => x.Id).ToList().ForEach(x =>
                         {
                             domainChelyabinskDisposalProvidedDoc.Delete(x);
                         });
                    domainDisposalAdditionalDoc.GetAll()
                      .Where(x => x.Disposal.Id == dispId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDisposalAdditionalDoc.Delete(x);
                      });
                    domainDisposalControlMeasures.GetAll()
                      .Where(x => x.Disposal.Id == dispId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDisposalControlMeasures.Delete(x);
                      });
                    domainDisposalDocConfirm.GetAll()
                      .Where(x => x.Disposal.Id == dispId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDisposalDocConfirm.Delete(x);
                      });
                    domainDisposalFactViolation.GetAll()
                      .Where(x => x.Disposal.Id == dispId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDisposalFactViolation.Delete(x);
                      });
                    domainDisposalLongText.GetAll()
                      .Where(x => x.Disposal.Id == dispId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDisposalLongText.Delete(x);
                      });

                    domainDisposalInspFoundCheckNormDocItem.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalInspFoundCheckNormDocItem.Delete(x);
                   });
                    domainDisposalInspFoundationCheck.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalInspFoundationCheck.Delete(x);
                   });
                    domainDisposalInspFoundation.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalInspFoundation.Delete(x);
                   });
                    domainDisposalExpert.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalExpert.Delete(x);
                   });
                    domainDisposalAnnex.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalAnnex.Delete(x);
                   });
                    domainDisposalAdminRegulation.GetAll()
                   .Where(x => x.Disposal.Id == dispId)
                   .Select(x => x.Id).ToList().ForEach(x =>
                   {
                       domainDisposalAdminRegulation.Delete(x);
                   });

                    domainDisposalProvidedDoc.GetAll()
                 .Where(x => x.Disposal.Id == dispId)
                 .Select(x => x.Id).ToList().ForEach(x =>
                 {
                     domainDisposalProvidedDoc.Delete(x);
                 });
                    domainDisposalSurveyObjective.GetAll()
                 .Where(x => x.Disposal.Id == dispId)
                 .Select(x => x.Id).ToList().ForEach(x =>
                 {
                     domainDisposalSurveyObjective.Delete(x);
                 });
                    domainDisposalSurveyPurpose.GetAll()
                 .Where(x => x.Disposal.Id == dispId)
                 .Select(x => x.Id).ToList().ForEach(x =>
                 {
                     domainDisposalSurveyPurpose.Delete(x);
                 });
                    domainDisposalTypeSurvey.GetAll()
                 .Where(x => x.Disposal.Id == dispId)
                 .Select(x => x.Id).ToList().ForEach(x =>
                 {
                     domainDisposalTypeSurvey.Delete(x);
                 });
                    domainDisposalVerificationSubject.GetAll()
                 .Where(x => x.Disposal.Id == dispId)
                 .Select(x => x.Id).ToList().ForEach(x =>
                 {
                     domainDisposalVerificationSubject.Delete(x);
                 });

                    domainCheDisp.Delete(dispId);

                }

                var decs = domainDecision.GetAll()
                  .Where(x => x.Inspection.Id == entity.Id)
                  .Select(x => x.Id).ToList();
                foreach (long decId in decs)
                {
                    domainDecisionAdminRegulation.GetAll()
                       .Where(x => x.Decision.Id == decId)
                       .Select(x => x.Id).ToList().ForEach(x =>
                       {
                           domainDecisionAdminRegulation.Delete(x);
                       });
                    domainDecisionAnnex.GetAll()
                       .Where(x => x.Decision.Id == decId)
                       .Select(x => x.Id).ToList().ForEach(x =>
                       {
                           domainDecisionAnnex.Delete(x);
                       });
                    domainDecisionControlList.GetAll()
                       .Where(x => x.Decision.Id == decId)
                       .Select(x => x.Id).ToList().ForEach(x =>
                       {
                           domainDecisionControlList.Delete(x);
                       });
                    domainDecisionControlMeasures.GetAll()
                     .Where(x => x.Decision.Id == decId)
                     .Select(x => x.Id).ToList().ForEach(x =>
                     {
                         domainDecisionControlMeasures.Delete(x);
                     });
                    domainDecisionControlSubjects.GetAll()
                     .Where(x => x.Decision.Id == decId)
                     .Select(x => x.Id).ToList().ForEach(x =>
                     {
                         domainDecisionControlSubjects.Delete(x);
                     });
                    domainDecisionExpert.GetAll()
                       .Where(x => x.Decision.Id == decId)
                       .Select(x => x.Id).ToList().ForEach(x =>
                       {
                           domainDecisionExpert.Delete(x);
                       });
                    domainDecisionInspectionReason.GetAll()
                      .Where(x => x.Decision.Id == decId)
                      .Select(x => x.Id).ToList().ForEach(x =>
                      {
                          domainDecisionInspectionReason.Delete(x);
                      });
                    domainDecisionProvidedDoc.GetAll()
                    .Where(x => x.Decision.Id == decId)
                    .Select(x => x.Id).ToList().ForEach(x =>
                    {
                        domainDecisionProvidedDoc.Delete(x);
                    });
                    domainDecisionVerificationSubject.GetAll()
                     .Where(x => x.Decision.Id == decId)
                     .Select(x => x.Id).ToList().ForEach(x =>
                     {
                         domainDecisionVerificationSubject.Delete(x);
                     });
                         domainDecision.Delete(decId);

                }


                    return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(domainStatAppeal);
                Container.Release(domainCheDisp);
                Container.Release(domainChelyabinskDisposalProvidedDoc);
                Container.Release(domainDisposalControlMeasures);
                Container.Release(domainDisposalAdditionalDoc);
                Container.Release(domainDisposalDocConfirm);
                Container.Release(domainDisposalFactViolation);
                Container.Release(domainDisposalLongText);
                Container.Release(domainDisposalAdminRegulation);
                Container.Release(domainDisposalAnnex);
                Container.Release(domainDisposalExpert);
                Container.Release(domainDisposalInspFoundation);
                Container.Release(domainDisposalInspFoundationCheck);
                Container.Release(domainDisposalInspFoundCheckNormDocItem);

                Container.Release(domainDisposalProvidedDoc);
                Container.Release(domainDisposalSurveyObjective);
                Container.Release(domainDisposalSurveyPurpose);
                Container.Release(domainDisposalTypeSurvey);
                Container.Release(domainDisposalVerificationSubject);
                Container.Release(domainDisposalViolation);
            }
        }
    }
}