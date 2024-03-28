using Bars.B4;
using Bars.B4.Modules.States;
using Bars.Gkh.Domain;
using Bars.Gkh.Overhaul.Hmao.ConfigSections;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Helpers;
using Bars.Gkh.Overhaul.Hmao.Services.ActualizeSubDPKR;
using Bars.Gkh.Utils;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    public class ActualizeSubDPKRController : BaseController
    {
        #region Properties

        public IActualizeSubrecordService Service { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        #endregion

        #region Public methods

        public ActionResult GetAddEntriesList(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            try
            {
                var list = Service.GetAddEntriesList(version).Filter(baseParams).ToList();

                return new JsonListResult(list.Order(baseParams).Paging(baseParams), list.Count);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }

        public ActionResult GetDeleteEntriesList(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            try
            {
                var list = Service.GetDeleteEntriesList(version).Filter(baseParams).ToList();

                return new JsonListResult(list.Order(baseParams).Paging(baseParams), list.Count);
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
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;

            var id = baseParams.Params.GetAs<long>("VersionId");
            var version = VersionDomain.Get(id);
            if (version == null)
                return JsonNetResult.Failure("Версия программы не определена в запросе");

            try
            {
                Service.Actualize(version, endYear);
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
