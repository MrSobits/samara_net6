namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Repositories;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System;
    using System.Linq;

    public class MKDLicRequestInterceptor : EmptyDomainInterceptor<MKDLicRequest>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            try
            {
                var indCalendar = Container.Resolve<IIndustrialCalendarService>();

                entity.CheckTime = indCalendar.GetDateAfterWorkDays(entity.StatementDate, 10);

                if (entity.ExtensTime != null && entity.ExtensTime > indCalendar.GetDateAfterWorkDays(entity.StatementDate, 30))
                {
                    entity.ExtensTime = indCalendar.GetDateAfterWorkDays(entity.StatementDate, 30);
                }

                var servStateProvider = Container.Resolve<IStateProvider>();

                try
                {
                    servStateProvider.SetDefaultState(entity);
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<MKDLicRequest>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            try
            {
                var indCalendar = Container.Resolve<IIndustrialCalendarService>();

                entity.CheckTime = indCalendar.GetDateAfterWorkDays(entity.StatementDate, 10);

                if (entity.ExtensTime != null && entity.ExtensTime > indCalendar.GetDateAfterWorkDays(entity.StatementDate, 30))
                {
                    entity.ExtensTime = indCalendar.GetDateAfterWorkDays(entity.StatementDate, 30);
                }

                if (entity.Executant != null && entity.State.StartState)
                {
                    var stateRepo = Container.Resolve<IStateRepository>();
                    var newState = stateRepo.GetAllStates<MKDLicRequest>(x => x.Name == "В работе").FirstOrDefault();

                    if (newState != null)
                    {
                        entity.State = newState;
                    }
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<MKDLicRequest>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            var answerDomain = Container.ResolveDomain<MKDLicRequestAnswer>();
            var executantDomain = Container.ResolveDomain<MKDLicRequestExecutant>();
            var headInspectorDomain = Container.ResolveDomain<MKDLicRequestHeadInspector>();
            var sourceDomain = Container.ResolveDomain<MKDLicRequestSource>();
            var attachmentDomain = Container.ResolveDomain<MKDLicRequestFile>();
            var realityObjDomain = Container.ResolveDomain<MKDLicRequestRealityObject>();

            try
            {
                var ansIds = answerDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    answerDomain.Delete(value);
                }

                var execIds = executantDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    executantDomain.Delete(value);
                }

                var headIds = headInspectorDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    headInspectorDomain.Delete(value);
                }

                var sourceIds = sourceDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    sourceDomain.Delete(value);
                }

                var attachIds = attachmentDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    attachmentDomain.Delete(value);
                }

                var ROIds = realityObjDomain.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in ansIds)
                {
                    realityObjDomain.Delete(value);
                }

                return Success();
            }
            finally
            {
                Container.Release(answerDomain);
                Container.Release(executantDomain);
                Container.Release(headInspectorDomain);
                Container.Release(sourceDomain);
                Container.Release(attachmentDomain);
                Container.Release(realityObjDomain);
            }
        }
    }
}
