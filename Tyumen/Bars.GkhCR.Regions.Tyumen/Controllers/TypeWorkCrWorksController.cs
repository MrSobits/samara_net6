using Bars.B4;
using Bars.GkhCr.Entities;
using System;
using System.Linq;


namespace Bars.GkhCR.Regions.Tyumen.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для добавления работ в ПСД работы
    /// </summary>
    public class TypeWorkCrWorksController : B4.Alt.DataController<PSDWorks>
    {
        #region Properties

        public IDomainService<PSDWorks> PSDWorksDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        #endregion

        #region Public methods

        public override ActionResult Update(BaseParams baseParams)
        {
            try
            {
                var result = DomainService.Update(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = new { } });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
          
        }


        /// <summary>
        /// Возвращает список работ этой ПСД
        /// </summary>
        public override ActionResult List(BaseParams baseParams)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();
                long id = loadParams.Filter.GetAs("typeWorkId", 0L);
                if (id == 0)
                    return JsSuccess();//Failure("псд работа не задана");

                var psdWork = TypeWorkCrDomain.Get(id);
                if (psdWork == null)
                    return JsFailure($"работа c id = {id} не найдена");

                if (!psdWork.Work.IsPSD)
                    return Empty();

                var query = PSDWorksDomain.GetAll()
                    .Where(x => x.PSDWork.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        WorkId = x.Work.Id,
                        x.Work.Work.Name,
                        x.Cost,
                        Year = x.Work.YearRepair
                    })
                    .Filter(loadParams, Container);

                return new JsonListResult(query.Order(loadParams).Paging(loadParams), query.Count());

            }
            catch (Exception e)
            {
                //TODO: дописать в стору показ ошибки
                return JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Возвращает список работ, которые можно добавить в ПСД
        /// </summary>
        public ActionResult ListPotential(BaseParams baseParams)
        {
            try
            {
                var loadParams = baseParams.GetLoadParam();
                long id = loadParams.Filter.GetAs("typeWorkId", 0L);
                if (id == 0)
                    return JsSuccess();

                var psdWork = TypeWorkCrDomain.Get(id);
                if (psdWork == null)
                    return JsFailure($"работа c id = {id} не найдена");

                if (!psdWork.Work.IsPSD)
                    return Empty();

                var houseId = psdWork.ObjectCr.RealityObject.Id;
                var alreadyAddedId = PSDWorksDomain.GetAll().Where(x => x.PSDWork.Id == id).Select(x => x.Work.Id).ToList();

                var query = TypeWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.RealityObject.Id == houseId)
                    .Where(x => !x.Work.IsPSD)
                    .Where(x => !alreadyAddedId.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Work.Name,
                        Year = x.YearRepair
                    })
                .Filter(loadParams, Container);

                return new JsonListResult(query.Order(loadParams).Paging(loadParams), query.Count());

            }
            catch (Exception e)
            {
                //TODO: дописать в стору показ ошибки
                return JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Проверяет, является ли работа ПСД
        /// </summary>
        public ActionResult IsPSD(BaseParams baseParams)
        {
            try
            {
                var id = baseParams.Params.GetAs<long>("typeWorkId");
                if (id == 0)
                    return JsSuccess();

                var work = TypeWorkCrDomain.Get(id);
                if (work == null)
                    return JsFailure($"работа c id = {id} не найдена");

                return JsSuccess(work.Work.IsPSD);
            }
            catch (Exception e)
            {
                //TODO: дописать в стору показ ошибки
                return JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Добавить работу к ПСД
        /// </summary>
        public ActionResult AddWork(BaseParams baseParams)
        {
            try
            {
                var id = baseParams.Params.GetAs<long>("typeWorkId");
                if (id == 0)
                    return JsFailure("работа не задана");

                var work = TypeWorkCrDomain.Get(id);
                if (work == null)
                    return JsFailure($"работа c id = {id} не найдена");

                if(work.Work.IsPSD)
                    return JsFailure("работа не должна быть ПСД");

                var psdid = baseParams.Params.GetAs<long>("psdtypeWorkId");
                if (psdid == 0)
                    return JsFailure("ПСД работа не задана");

                var psdwork = TypeWorkCrDomain.Get(psdid);
                if (psdwork == null)
                    return JsFailure($"работа c id = {psdid} не найдена");

                if (!psdwork.Work.IsPSD)
                    return JsFailure("ПСД работа не отмечена как ПСД");

                if (id == psdid)
                    return JsFailure("Нельзя включать работу саму в себя");

                if (!PSDWorksDomain.GetAll().Where(x => x.Work.Id == id).Where(x => x.PSDWork.Id == psdid).Any())
                    PSDWorksDomain.Save(new PSDWorks
                    {
                        PSDWork = psdwork,
                        Work = work
                    });

                return JsSuccess();
            }
            catch (Exception e)
            {
                //TODO: дописать в стору показ ошибки
                return JsFailure(e.Message);
            }
        }

        /// <summary>
        /// Удалить работу из ПСД
        /// </summary>
        public ActionResult DeleteWork(BaseParams baseParams)
        {
            try
            {
                var id = baseParams.Params.GetAs<long>("typeWorkId");
                if (id == 0)
                    return JsFailure("работа не задана");

                var work = TypeWorkCrDomain.Get(id);
                if (work == null)
                    return JsFailure($"работа c id = {id} не найдена");

                if (work.Work.IsPSD)
                    return JsFailure("работа не должна быть ПСД");

                var psdid = baseParams.Params.GetAs<long>("psdtypeWorkId");
                if (psdid == 0)
                    return JsFailure("ПСД работа не задана");

                var psdwork = TypeWorkCrDomain.Get(psdid);
                if (psdwork == null)
                    return JsFailure($"работа c id = {psdid} не найдена");

                if (!psdwork.Work.IsPSD)
                    return JsFailure("ПСД работа не отмечена как ПСД");

                if (id == psdid)
                    return JsFailure("Нельзя включать работу саму в себя");

                var recordsId = PSDWorksDomain.GetAll().Where(x => x.Id == id).Select(x => x.Id).ToList();

                recordsId.ForEach(x => PSDWorksDomain.Delete(x));

                return JsSuccess();
            }
            catch (Exception e)
            {
                //TODO: дописать в стору показ ошибки
                return JsFailure(e.Message);
            }
        }
        #endregion
    }
}
