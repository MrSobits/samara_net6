namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4.Modules.Security;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class HeatSeasonDocService : IHeatSeasonDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = baseParams.Params.ContainsKey("periodId") ? baseParams.Params["periodId"].ToLong() : 0;
            var stateId = baseParams.Params.ContainsKey("stateId") ? baseParams.Params["stateId"].ToLong() : 0;

            var data = Container.Resolve<IDomainService<ViewHeatSeasonDoc>>().GetAll()
                                .Where(x => x.PeriodId == periodId && x.State.Id == stateId && x.ConditionHouse != ConditionHouse.Razed)
                                .Filter(loadParams, Container);
            
            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        /// <summary>
        /// Метод массовой смены статуса документов подготовки к отопительному сезону
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult MassChangeState(BaseParams baseParams)
        {
            /*ids - идентификаторы документов, у которых нужно перевести статус
             * newStateId - идентификатор нового статуса
             */
            var ids = baseParams.Params.ContainsKey("ids") ? baseParams.Params["ids"].As<List<object>>().Select(x => x.ToLong()).ToArray() : new long[] { };
            var newStateId = baseParams.Params.ContainsKey("newStateId") ? baseParams.Params["newStateId"].ToLong() : 0;
            var oldStateId = baseParams.Params.ContainsKey("oldStateId") ? baseParams.Params["oldStateId"].ToLong() : 0;

            if (ids.Length == 0 || newStateId == 0 || oldStateId == 0)
            {
                return new BaseDataResult
                    {
                        Success = false,
                        Message = "Не удалось получить документы по отопительному сезону или статусы"
                    };
            }

            var docService = Container.Resolve<IDomainService<HeatSeasonDoc>>();
            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();
            var stateTransfersCache = Container.Resolve<IStateTransfersCache>();
            var userRoleRepository = Container.ResolveRepository<UserRole>();
            var userIdentity = Container.Resolve<IUserIdentity>();

            try
            {
                // в этот словарь забиваем документы по дому, которых не хватает для перевода статуса
                var dictRo = new Dictionary<string, string>();

                var newState = stateDomain.Load(newStateId);
                var oldState = stateDomain.Load(oldStateId);

                var stateTransfers = stateTransfersCache.GetTransfers().ToArray();

                var roles = userRoleRepository.GetAll()
                    .Where(userRole => userRole.User.Id == (long)userIdentity.UserId)
                    .Select(x => x.Role.Id)
                    .ToList();

                if (
                    roles.All(
                        x =>
                            !stateTransfers.Where(y => y.CurrentState.Id == oldState.Id && y.NewState.Id == newState.Id)
                                .Select(y => y.Role.Id)
                                .Contains(x)))
                {
                    return new BaseDataResult
                    {
                        Success = false,
                        Message = "Нет прав на изменение статуса"
                    };
                }

                foreach (var id in ids.Select(x => x.ToLong()))
                {
                    var doc = docService.Load(id);

                    var addr = doc.HeatingSeason.RealityObject.Address;
                    var heatSeasonId = doc.HeatingSeason.Id;

                    /* если отопление централизованное, 
                 * то в этом периоде по этому дому должны быть загружены:
                 * Акт промывки, Акт опрессовки и Паспорт готовности 
                 * 
                 * если отопление индивидуальное,
                 * то в этом периоде по этому дому должны быть загружены:
                 * Акт проверки вентиляции или Акт проверки дымоходов и Паспорт готовности
                 */

                    //если в словарике уже есть адрес значит по нему нет каких то документов
                    if (dictRo.ContainsKey(addr))
                    {
                        continue;
                    }

                    var missingDocs = "";

                    if (doc.HeatingSeason.HeatingSystem == HeatingSystem.Centralized)
                    {
                        //акт промывки
                        if (
                            !docService.GetAll()
                                .Any(
                                    x =>
                                        x.HeatingSeason.Id == heatSeasonId &&
                                        x.TypeDocument == HeatSeasonDocType.ActFlushingHeatingSystem))
                            missingDocs += "Акт промывки";

                        //акт опрессовки
                        if (
                            !docService.GetAll()
                                .Any(
                                    x =>
                                        x.HeatingSeason.Id == heatSeasonId &&
                                        x.TypeDocument == HeatSeasonDocType.ActPressingHeatingSystem))
                            missingDocs += missingDocs.Length > 0 ? ", Акт опрессовки" : "Акт опрессовки";
                    }
                    else
                    {
                        //акт проверки вентиляции и/или акт проверки дымоходов
                        if (!docService.GetAll()
                            .Any(
                                x =>
                                    x.HeatingSeason.Id == heatSeasonId &&
                                    x.TypeDocument == HeatSeasonDocType.ActCheckVentilation)
                            &&
                            !docService.GetAll()
                                .Any(
                                    x =>
                                        x.HeatingSeason.Id == heatSeasonId &&
                                        x.TypeDocument == HeatSeasonDocType.ActCheckChimney))
                            missingDocs += "Акт проверки вентиляции и/или акт проверки дымоходов";
                    }

                    //паспорт готовности
                    if (
                        !docService.GetAll()
                            .Any(x => x.HeatingSeason.Id == heatSeasonId && x.TypeDocument == HeatSeasonDocType.Passport))
                        missingDocs += missingDocs.Length > 0 ? ", Паспорт готовности" : "Паспорт готовности";

                    if (missingDocs.Length > 0)
                    {
                        dictRo.Add(addr, missingDocs);
                        continue;
                    }

                    //если всё ок, всех документов хватает, то переводим статус
                    stateProvider.ChangeState(id, "gji_heatseason_document", newState, "Массовая смена статуса", true);
                }

                //пройдемся по словарю домов, чтобы сформировать сообщение о недостающих документах
                var strBuilder = new StringBuilder("По следующим домам не загружены необходимые документы: <br>");

                var hasMessage = false;

                foreach (var rec in dictRo)
                {
                    hasMessage = true;

                    strBuilder.AppendFormat("{0} : {1}<br>", rec.Key, rec.Value);
                }

                strBuilder.Append("Перевод статусов для этих документов отменен.");

                return new BaseDataResult {Success = true, Message = hasMessage ? strBuilder.ToString() : ""};
            }
            finally
            {
                Container.Release(docService);
                Container.Release(stateProvider);
                Container.Release(stateDomain);
                Container.Release(stateTransfersCache);
                Container.Release(userRoleRepository);
                Container.Release(userIdentity);
            }
        }

        public IDataResult ListDocumentTypes(BaseParams baseParams)
        {
            /*
             Поскольку в базовый енум добавляется куча типов котоыре не вовсех регионах нужны
             то тогда в этом серверном методе возвращаем типы котоыре нужны только для этого региона
            */

            var list = new List<DocumentTypeProxy>();

            foreach (var type in DocumentTypes())
            {
                var display = type.GetEnumMeta().Display;

                list.Add(new DocumentTypeProxy
                {
                    Id = type.GetHashCode(),
                    Display = display,
                    Name = type.ToString()
                });
            }

            var total = list.Count;

            return new ListDataResult(list, total);
        }

        public virtual HeatSeasonDocType[] DocumentTypes()
        {
            // Вообщем по умолчанию регистрируются только такие типы 
            // в слуаче если в регионе нобходимы другие, то тогда заменяем реализацию
            return new HeatSeasonDocType[]
                {
                    HeatSeasonDocType.ActFlushingHeatingSystem,
                    HeatSeasonDocType.ActPressingHeatingSystem,
                    HeatSeasonDocType.ActCheckVentilation,
                    HeatSeasonDocType.ActCheckChimney,
                    HeatSeasonDocType.Passport
                };
        }

        protected class DocumentTypeProxy
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Display { get; set; }
        }
    }
}