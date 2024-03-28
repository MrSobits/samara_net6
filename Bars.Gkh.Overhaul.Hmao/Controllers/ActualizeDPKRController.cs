using Bars.B4;
using Bars.B4.Modules.States;
using Bars.Gkh.Domain;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Helpers;
using Bars.Gkh.Overhaul.Hmao.Services.ActualizeDPKR;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    public class ActualizeDPKRController : BaseController
    {
        #region Properties

        public IActualizeDPKRService Service { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Возвращает список статусов домов
        /// </summary>
        public ActionResult ListMKDStates(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var query = StateDomain.GetAll()
                .Where(x => x.TypeId == "gkh_real_obj")
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, Container);

            return new JsonListResult(
                query
                .Order(loadParams)
                .Paging(loadParams),
                query.Count()
                );
        }


        /// <summary>
        /// Возвращает список статусов домов
        /// </summary>
        public ActionResult ListSEStates(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var query = StateDomain.GetAll()
                .Where(x => x.TypeId == "ovrhl_ro_struct_el")
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, Container);

            return new JsonListResult(
                query
                .Order(loadParams)
                .Paging(loadParams),
                query.Count()
                );
        }

        public ActionResult GetAddEntriesList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");            

            var startYear = loadParams.Filter.GetAs<short>("StartYear", 0);
            try
            {
                var list = Service.GetAddEntriesList(version, startYear).Filter(baseParams).ToList();

                return new JsonListResult(list.Order(baseParams).Paging(baseParams), list.Count);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult GetDeleteEntriesList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var startYear = loadParams.Filter.GetAs<short>("StartYear", 0);

            try
            {
                var list = Service.GetDeleteEntriesList(version, startYear).Filter(baseParams).ToList();

                return new JsonListResult(list.Paging(baseParams), list.Count);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult removeHouseForAdd(BaseParams baseParams, Int64 houseId)
        {
            try
            {
                Service.RemoveHouseForAdd(houseId);

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult removeHouseForDelete(BaseParams baseParams, Int64 houseId)
        {
            try
            {
                Service.RemoveHouseForDelete(houseId);

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult ClearCache(BaseParams baseParams)
        {
            try
            {
                Service.ClearCache();

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult Actualize(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var startYear = baseParams.Params.GetAs<short>("StartYear", 0);

            var grid = baseParams.Params.GetAs("Grid", "Both");

            var SelectedAddId = baseParams.Params.GetAs<string>("SelectedAddId").ToLongArray();
            var SelectedDeleteId = baseParams.Params.GetAs<string>("SelectedDeleteId").ToLongArray();

            try
            {
                if (grid == "Both")
                    Service.Actualize(version, startYear);
                else if(grid == "Add" && SelectedAddId.Count()>0)
                        Service.Actualize(version, startYear, SelectedAddId, new long[0]);
                else if (grid == "Delete" && SelectedDeleteId.Count() > 0)
                    Service.Actualize(version, startYear, new long[0], SelectedDeleteId);

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }

        }

        public ActionResult RemoveSelected(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            var SelectedAddId = baseParams.Params.GetAs<string>("SelectedAddId").ToLongArray();
            var SelectedDeleteId = baseParams.Params.GetAs<string>("SelectedDeleteId").ToLongArray();

            if (SelectedAddId.Count() == 0 && SelectedDeleteId.Count() == 0)
                return JsSuccess();

            try
            {
                Service.RemoveSelected(version, SelectedAddId, SelectedDeleteId);

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        #endregion
    }
}
