namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.Utils;
    using DomainService;
    using Entities;

    public class RealityObjectController : B4.Alt.DataController<RealityObject>
    {

        /// <summary>
        /// Возвращает то же самое что и метод List, но данные берет из ViewRealityObject. 
        /// Т.к. нужны наименования управляющих организаций и договоров с жилыми домами
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult ListView(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var exportName = baseParams.Params.GetAs<string>("exportName") ?? "RealityObjectDataExport";
            var export = Container.Resolve<IDataExportService>(exportName);
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult ListLiftsRegistry(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListLiftsRegistry(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ExportLiftsRegistry(BaseParams baseParams)
        {
            var exportName = baseParams.Params.GetAs<string>("exportName") ?? "RealityObjectLiftRegisterDataExport";
            var export = Container.Resolve<IDataExportService>(exportName);
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult ListExceptDirectManag(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListExceptDirectManag(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListByManOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByManOrg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListByManOrgTumen(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByManOrgTumen(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListByServOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByServOrg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListRoByServOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListRoByServOrg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListByTypeOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByTypeOrg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListGkuInfo(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListGkuInfo(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public virtual ActionResult GetGkuInfo(BaseParams baseParams)
        {
            var result = Resolve<IRealityObjectService>().GetGkuInfo(baseParams);
            return new JsonGetResult(result.Data);
        }

        public ActionResult ListGkuInfoTarifs(BaseParams baseParams)
        {
            var result = Resolve<IRealityObjectService>().ListGkuInfoTarifs(baseParams);
            return new JsonGetResult(result.Data);
        }

        public ActionResult ListMoByContragentId(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListMoByContragentId(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListRoBySupplyResorg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListRoBySupplyResorg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult ListBySupplySerOrg(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListBySupplySerOrg(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }


        public ActionResult ListByLocalityGuid(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByLocalityGuid(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult GetPassportReport(BaseParams baseParams)
        {
            var result = Container.Resolve<IRealityObjectService>().GetPassportReport(baseParams);

            if (result.Success == false)
            {
                return JsonNetResult.Failure(result.Message);
            }

            return new FileStreamResult((MemoryStream)result.Data, "application/vnd.ms-excel") { FileDownloadName = "Passport.xlsx" };
        }

        public ActionResult UpdateContracts()
        {
            var roRepository = Container.Resolve<IRepository<RealityObject>>();
            var roContractManOrgRepository = Container.Resolve<IRepository<ManOrgContractRealityObject>>();

            var now = DateTime.Now.Date;

            var currentContracts = roContractManOrgRepository.GetAll()
                    .Where(x => x.ManOrgContract.StartDate <= now)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= now)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        InnManOrgs = x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                        StartControlDate = x.ManOrgContract.StartDate,
                        x.ManOrgContract.TypeContractManOrgRealObj
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key,
                        y => new
                        {
                            ManOrgs = y.AggregateWithSeparator(x => x.ManOrgName, ", "),
                            InnManOrgs = y.AggregateWithSeparator(x => x.InnManOrgs, ", "),
                            StartControlDate = y.AggregateWithSeparator(x => x.StartControlDate?.ToString("dd.MM.yyyy"), ", "),
                            TypesContract = y.AggregateWithSeparator(x => x.TypeContractManOrgRealObj.GetEnumMeta().Display, ", ")
                        });

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var ro in roRepository.GetAll())
                    {
                        var contract = currentContracts.Get(ro.Id);

                        if (contract != null)
                        {
                            ro.ManOrgs = contract.ManOrgs;
                            ro.InnManOrgs = contract.InnManOrgs;
                            ro.StartControlDate = contract.StartControlDate;
                            ro.TypesContract = contract.TypesContract;
                        }
                        else
                        {
                            ro.ManOrgs = null;
                            ro.InnManOrgs = null;
                            ro.StartControlDate = null;
                            ro.TypesContract = null;
                        }

                        roRepository.Update(ro);
                    }

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return JsonNetResult.Success;
        }

        public ActionResult ListByMoSettlement(BaseParams baseParams)
        {
            var result = (ListDataResult)Resolve<IRealityObjectService>().ListByMoSettlement(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult Back(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IDomainService<RealityObject>>();
            var backForwardService = Container.Resolve<IBackForwardIterator<RealityObject>>();
            using (Container.Using(domainService, backForwardService))
            {
                var currentId = baseParams.Params.GetAs<long>("current");
                var current = domainService.FirstOrDefault(x => x.Id == currentId);
                if (current == null)
                {
                    var any = domainService.GetAll().FirstOrDefault();
                    return JsSuccess(any);
                }

                var previous = backForwardService.Back(current);

                return JsSuccess(previous);
            }
        }
        public ActionResult Forward(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IDomainService<RealityObject>>();
            var backForwardService = Container.Resolve<IBackForwardIterator<RealityObject>>();
            using (Container.Using(domainService, backForwardService))
            {
                var currentId = baseParams.Params.GetAs<long>("current");
                var current = domainService.FirstOrDefault(x => x.Id == currentId);
                if (current == null)
                {
                    var any = domainService.GetAll().FirstOrDefault();
                    return JsSuccess(any);
                }

                var next = backForwardService.Forward(current);

                return JsSuccess(next);
            }
        }

        public ActionResult GetForMap(BaseParams baseParams)
        {
            var ros = Container.Resolve<IRealityObjectService>();
            using (Container.Using(ros))
            {
                var res = ros.GetForMap(baseParams);
                return res.Success ? new JsonGetResult(res.Data) : null;
            }
        }

        public ActionResult GetHistory(BaseParams baseParams)
        {
            var roSeService = Container.Resolve<IRealityObjectService>();
            using (Container.Using(roSeService))
            {
                var result = (ListDataResult)roSeService.GetHistory(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);

            }
        }

        public ActionResult GetHistoryDetail(BaseParams baseParams)
        {
            var roSeService = Container.Resolve<IRealityObjectService>();
            using (Container.Using(roSeService))
            {
                var result = (ListDataResult)roSeService.GetHistoryDetail(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);

            }
        }

        /// <summary>
        /// Получить список для фильтрации в экспорте по формату
        /// </summary>
        public ActionResult FormatDataExportList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IFormatDataExportRepository<RealityObject>>();
            using (this.Container.Using(service))
            {
                return service.List(baseParams).ToJsonResult();
            }
        }
    }
}