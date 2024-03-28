namespace Bars.GkhCr.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;
    using Entities;
    using Gkh.Utils;

    /// <summary>
    /// 
    /// </summary>
    public class CompetitionInterceptor : EmptyDomainInterceptor<Competition>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Competition> service, Competition entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Competition> service, Competition entity)
        {
            var lotDomain = Container.ResolveDomain<CompetitionLot>();
            var protocolDomain = Container.ResolveDomain<CompetitionProtocol>();
            var documentDomain = Container.ResolveDomain<CompetitionDocument>();

            var dependencies = new List<string>();

            if (lotDomain.GetAll().Any(x => x.Competition == entity))
            {
                dependencies.Add("Лоты");
            }

            if (protocolDomain.GetAll().Any(x => x.Competition == entity))
            {
                dependencies.Add("Протоколы");
            }

            if (documentDomain.GetAll().Any(x => x.Competition == entity))
            {
                dependencies.Add("Документы");
            }

            if (dependencies.Any())
            {
                return BaseDataResult.Error("Удаление недоступно. Имеются зависимости: {0}".FormatUsing(dependencies.AggregateWithSeparator(", ")));
            }

            return Success();
        }
    }
}