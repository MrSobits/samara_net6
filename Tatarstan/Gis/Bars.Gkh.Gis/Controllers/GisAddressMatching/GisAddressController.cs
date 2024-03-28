namespace Bars.Gkh.Gis.Controllers.GisAddressMatching
{
    using System.Data;
    using System.Data.Common;
    using System.Text;
    using B4.Config;
    using B4.DataAccess;
    using Gkh.Entities;
    using Entities.Kp50;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FIAS;
    using B4.Utils;

    using Bars.Gkh.Gis.KP_legacy;

    using DomainService.GisAddressMatching;

    public class GisAddressController : BaseController
    {
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }
        public IDomainService<Fias> FiasDomain { get; set; }


        public ActionResult AreaListPaging(BaseParams baseParams)
        {
            var result = (ListDataResult) Container.Resolve<IAddressService>().AreaListPaging(baseParams);
            return result.Success
                ? new JsonListResult(result.Data as IEnumerable<object>, result.TotalCount)
                : JsonNetResult.Failure("Ошибка получения списка районов");
        }


        public ActionResult PlaceListPaging(BaseParams baseParams)
        {
            var result = (ListDataResult) Container.Resolve<IAddressService>().PlaceListPaging(baseParams);
            return result.Success
                ? new JsonListResult(result.Data as IEnumerable<object>, result.TotalCount)
                : JsonNetResult.Failure("Ошибка получения списка населенных пунктов");
        }

        public ActionResult StreetShortNameList(BaseParams baseParams)
        {
            var result = (ListDataResult) Container.Resolve<IAddressService>().StreetShortNameList(baseParams);
            return result.Success
                ? new JsonListResult(result.Data as IEnumerable<object>, result.TotalCount)
                : JsonNetResult.Failure("Ошибка получения списка сокращений улиц");
        }

        public ActionResult StreetListPaging(BaseParams baseParams)
        {
            var result = (ListDataResult) Container.Resolve<IAddressService>().StreetListNoGuidPaging(baseParams);
            return result.Success
                ? new JsonListResult(result.Data as IEnumerable<object>, result.TotalCount)
                : JsonNetResult.Failure("Ошибка получения списка улиц");
        }

        /// <summary>
        /// Добавить новый ФИАС адрес
        /// </summary>
        public ActionResult CreateFiasAddress(BaseParams baseParams)
        {
            try
            {
                var areaGuid = baseParams.Params.GetAs<string>("areaCode");
                var placeGuid = baseParams.Params.GetAs<string>("placeCode");
                var streetName = baseParams.Params.GetAs<string>("streetName");
                var streetShortName = baseParams.Params.GetAs<string>("streetShortName");
                var house = baseParams.Params.GetAs<string>("house");
                var building = baseParams.Params.GetAs<string>("building");


                //получаем район или городs
                var fiasRajon = FiasDomain
                    .GetAll()
                    .FirstOrDefault(
                        x =>
                            x.AOGuid == areaGuid
                            &&
                            x.ActStatus == FiasActualStatusEnum.Actual);
                if (fiasRajon == null)
                {
                    return JsonNetResult.Failure("Район/Город не найден");
                }

                //В случае деления Казани на районы - в качестве нас. пункат используем mirrorGuid
                var useMirrorGuid = string.IsNullOrEmpty(placeGuid) && !string.IsNullOrEmpty(fiasRajon.MirrorGuid);

                //получаем нас пункт               
                Fias fiasPlace = null;
                if (!string.IsNullOrWhiteSpace(placeGuid))
                {
                    fiasPlace = FiasDomain
                        .GetAll()
                        .FirstOrDefault(
                            x => x.AOGuid == placeGuid
                                 &&
                                 (useMirrorGuid || x.ActStatus == FiasActualStatusEnum.Actual));

                    if (fiasPlace == null)
                    {
                        return JsonNetResult.Failure("Населенный пункт не найден");
                    }
                }
                else
                {
                    //проверка на возможность добавления без нас. пункта
                    if (!useMirrorGuid && fiasRajon.AOLevel != FiasLevelEnum.City)
                    {
                        return
                            JsonNetResult.Failure(
                                "Для добавления улицы необходимо либо указать город, либо район с населенным пунктом.");
                    }
                }

                //Регион
                var region =
                    FiasDomain.GetAll().FirstOrDefault(x => x.AOLevel == FiasLevelEnum.Region && x.CodeRegion == "16");
                if (region == null)
                {
                    return JsonNetResult.Failure("Регион не найден");
                }

                var fiasAddress = new FiasAddress
                {
                    AddressName =
                        string.Format("{0}. {1}, {2}{3}{4}, {7}. {5}, д.{6}",
                            region.ShortName,
                            region.FormalName,
                            string.IsNullOrEmpty(fiasRajon.ShortName) ? "" : (fiasRajon.ShortName + ". "),
                            useMirrorGuid ? fiasRajon.OffName : fiasRajon.FormalName,
                            fiasPlace != null
                                ? string.Format(", {0}. {1}", fiasPlace.ShortName, fiasPlace.FormalName)
                                : "",
                            streetName,
                            house,
                            streetShortName)
                        + (string.IsNullOrEmpty(building) ? "" : " корп." + building),

                    PlaceAddressName =
                        string.Format("{0}. {1}, {2}{3}{4}",
                            region.ShortName,
                            region.FormalName,
                            string.IsNullOrEmpty(fiasRajon.ShortName) ? "" : (fiasRajon.ShortName + ". "),
                            useMirrorGuid ? fiasRajon.OffName : fiasRajon.FormalName,
                            fiasPlace != null
                                ? string.Format(", {0}. {1}", fiasPlace.ShortName, fiasPlace.FormalName)
                                : ""),

                    PlaceName =
                        fiasPlace != null
                            ? string.IsNullOrEmpty(fiasPlace.ShortName)
                                ? string.Format("{0}", useMirrorGuid ? fiasPlace.OffName : fiasPlace.FormalName)
                                : string.Format("{0}. {1}", fiasPlace.ShortName,
                                    useMirrorGuid ? fiasPlace.OffName : fiasPlace.FormalName)
                            : (string.IsNullOrEmpty(fiasRajon.ShortName)
                                ? string.Format("{0}", useMirrorGuid ? fiasRajon.OffName : fiasRajon.FormalName)
                                : string.Format("{0}. {1}", fiasRajon.ShortName,
                                    useMirrorGuid ? fiasRajon.OffName : fiasRajon.FormalName)),
                    PlaceCode = fiasPlace != null ? fiasPlace.CodePlace : fiasRajon.CodeArea,
                    PlaceGuidId = fiasPlace != null ? fiasPlace.AOGuid : fiasRajon.AOGuid,
                    StreetName = string.Format("{0}. {1}", streetShortName, streetName),
                    //для новой улицы создается новый GUID
                    StreetGuidId = Guid.NewGuid().ToString(),
                    House = house,
                    Housing = building
                };


                //проверка: для одинаковой улицы добавляем тот же GUID                
                var guid =
                    FiasAddressDomain
                        .GetAll()
                        .FirstOrDefault(
                            x =>
                                x.PlaceGuidId == fiasAddress.PlaceGuidId
                                &&
                                x.StreetName.ToUpper() == fiasAddress.StreetName.ToUpper())
                        .Return(x => x.StreetGuidId);
                if (!string.IsNullOrWhiteSpace(guid))
                {
                    fiasAddress.StreetGuidId = guid;
                }


                FiasAddressDomain.Save(fiasAddress);
                return JsonNetResult.Success;
            }
            catch (Exception)
            {
                return JsonNetResult.Failure("Ошибка сохранения нового адреса");
            }
        }

        /// <summary>
        /// Проставить домам ссылки на схемы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CompareHousesWithSchemas(BaseParams baseParams)
        {
            var billingSchemasInfo = Container.Resolve<IRepository<BilDictSchema>>().GetAll();
            var billingHouseCodeStorage = Container.Resolve<IRepository<BilHouseCodeStorage>>();
            var provider = new ConnectionProvider(Container.Resolve<IConfigProvider>());
            var query = "";
            var uncomparedHousesInfo = new StringBuilder();
            bool houseCompared = false;
            var connectionString = "";
            DbConnection dbConnection = provider.CreateConnection();

            query =
                String.Format(
                    " INSERT INTO public.bil_house_code_storage (billing_house_code) " +
                    " SELECT distinct CAST(billing_id AS BIGINT) " +
                    " FROM public.b4_fias_address_uid " +
                    " WHERE billing_id IS NOT NULL " +
                    " AND billing_id != '' " +
                    " AND CAST(billing_id as bigint) NOT IN " +
                    " ( SELECT  billing_house_code FROM public.bil_house_code_storage )");

            dbConnection.Execute(query, commandTimeout: 3600);

            foreach (var code in billingHouseCodeStorage
                .GetAll()
                .Where(x => x.Schema == null)
                .OrderBy(x => x.Schema.ConnectionString)
                .ThenBy(x => x.BillingHouseCode))
            {
                foreach (var schema in billingSchemasInfo.OrderBy(x => x.ConnectionString))
                {
                    if (connectionString != schema.ConnectionString)
                    {
                        if (dbConnection.State == ConnectionState.Open)
                        {
                            dbConnection.Close();
                            dbConnection.Dispose();
                        }
                        connectionString = schema.ConnectionString;
                        provider.Init(connectionString);
                        dbConnection = provider.CreateConnection();
                        dbConnection.Open();
                    }



                    query =
                        String.Format(
                            " SELECT CAST(count(*) AS INTEGER) AS count FROM  {0}_data.prm_4 " +
                            " WHERE nzp_prm = 890 AND TRIM(val_prm)='{1}'",
                            schema.LocalSchemaPrefix.Trim(), code.BillingHouseCode);
                    var count = dbConnection.Query<int>(query).Single();
                    if (count == 0)
                    {
                        continue;
                    }
                    if (count == 1)
                    {
                        houseCompared = true;

                        var oneHouse = billingHouseCodeStorage
                            .GetAll()
                            .FirstOrDefault(x => x.BillingHouseCode == code.BillingHouseCode);
                        oneHouse.Schema = schema;
                        billingHouseCodeStorage.Update(oneHouse);
                        break;
                    }
                }

                if (!houseCompared)
                {
                    uncomparedHousesInfo.AppendFormat("Дом {0} не сопоставлен.", code);
                }
            }

            return new JsonListResult(billingHouseCodeStorage as IEnumerable<object>);
        }
    }
}