namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контроллер "Муниципальный район и Муниципальный образование"
    /// </summary>
    public class MunicipalityController : B4.Alt.DataController<Municipality>
    {
        private IMunicipalityService service;

        private IMunicipalityService Service
        {
            get { return service ?? (service = Container.Resolve<IMunicipalityService>()); }
        }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListWithoutPaging(baseParams);

            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        /// <summary>
        /// Вернуть МО без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSettlementWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListSettlementWithoutPaging(baseParams);

            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        /// <summary>
        /// Список по идентификаторам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListByReg(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByReg(baseParams);

            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        /// <summary>
        /// Возвращает список МО в зависимости от оператора
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список МО</returns>
        public ActionResult ListByOperator(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByOperator(baseParams);

            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        /// <summary>
        /// Добавить МО по номеру ФИАС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult AddMoFromFias(BaseParams baseParams)
        {
            var data = Service.AddMoFromFias(baseParams);

            return new JsonNetResult(new { success = true, data = data.Data });
        }

        /// <summary>
        /// Установить родителский МО для указанного
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult SetMoParent(BaseParams baseParams)
        {
            var data = Service.SetMoParent(baseParams);

            return new JsonNetResult(new { success = data.Success, data = data.Data });
        }

        /// <summary>
        /// Вернуть дерево МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListTree(BaseParams baseParams)
        {
            var data = Service.ListTree(baseParams);

            return new JsonNetResult(data.Data);
        }

        /// <summary>
        /// Вернуть дерево по поисковому запросу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSelectTree(BaseParams baseParams)
        {
            var data = Service.ListSelectTree(baseParams);

            return new JsonNetResult(data.Data);
        }

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListByParam(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByParam(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения (с пагинацией)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListByParamPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByParamPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть по уровню МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListByWorkpriceParam(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByWorkpriceParam(baseParams);
            return new JsonListResult((IList)result.Data);
        }

        /// <summary>
        /// Вернуть Муниципальные районы
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListMoArea(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListMoArea(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть Муниципальные районы (без пагинации)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListMoAreaWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListMoAreaWithoutPaging(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSettlement(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListSettlement(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть муниципальные образования с названием родительского мун. района и гор. округи
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSettlementWithParentMoName(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListSettlementWithParentMo(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть городские округи и муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSettlementAndUrban(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListSettlementAndUrban(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть все с родителями
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListAllWithParent(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListAllWithParent(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        /// <summary>
        /// Вернуть список МР по настройкам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListByParamAndOperator(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByParamAndOperator(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        // TODO: вынести в домен
        /// <summary>
        /// Вернуть МО с указанным родителем
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult ListSettlementByParent(BaseParams baseParams)
        {
            int parentId = baseParams.Params["parentId"].ToInt();

            if (parentId == 0)
            {
                return JsonListResult.EmptyList;
            }

            var result = Container.Resolve<IRepository<Municipality>>().GetAll().Where(x => x.ParentMo.Id == parentId);

            if (!result.Any())
            {
                result = Container.Resolve<IRepository<Municipality>>().GetAll().Where(x => x.Id == parentId);
            }

            return new JsonListResult(result.Paging(GetLoadParam(baseParams)), result.Count());
        }

        /// <summary>
        /// Получение муниципального обазования по FiasAddress.PlaceGuidId
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetByPlaceGuid(BaseParams baseParams)
        {

            var fiasDomain = Container.Resolve<IDomainService<Fias>>();
            var municiplaityRepo = Container.Resolve<IRepository<Municipality>>(); ;

            var placeGuidId = baseParams.Params.GetAs<string>("placeGuidId");

            using (Container.Using(fiasDomain, municiplaityRepo))
            {
                var entityFias = fiasDomain.GetAll()
                    .FirstOrDefault(x => x.ActStatus == FiasActualStatusEnum.Actual && placeGuidId == x.AOGuid);
                var mo = municiplaityRepo.FirstOrDefault(
                    x =>
                        x.FiasId == placeGuidId ||
                        (entityFias != null && entityFias.OKTMO == x.Oktmo.ToString()));
                if (mo != null)
                {
                    return JsSuccess(mo.Name);
                }
            }
            return JsSuccess();
        }

        public ActionResult UnionMo(BaseParams baseParams)
        {
            var result = this.Service.UnionMo(baseParams);

            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}