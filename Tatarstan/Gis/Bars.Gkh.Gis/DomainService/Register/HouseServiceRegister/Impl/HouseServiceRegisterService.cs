namespace Bars.Gkh.Gis.DomainService.Register.HouseServiceRegister.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;

    using Bars.Gkh.Entities.Dicts;

    using Castle.Windsor;
    using Entities.Register.HouseRegister;
    using Entities.Register.HouseServiceRegister;
    using Helper.HqlHelper;
    using NHibernate.Transform;

    public class HouseServiceRegisterService : IHouseServiceRegisterService
    {
        protected IWindsorContainer Container;
        protected IRepository<HouseServiceRegister> HouseServiceRepository;
        protected ISessionProvider SessionProvider;
        protected IRepository<HouseRegister> HouseRepository;
        protected IRepository<Fias> FiasRepository;
        protected IRepository<FiasAddress> FiasAddressRepository;
        protected IRepository<ServiceDictionary> ServiceDictionaryRepository;
        protected IHqlHelper HqlHelper;

        public HouseServiceRegisterService(IWindsorContainer container, IRepository<HouseServiceRegister> houseServiceRepository
            , ISessionProvider sessionProvider, IRepository<HouseRegister> housepository
            , IRepository<Fias> fiasRepository, IRepository<FiasAddress> fiasAddressRepository
            , IRepository<ServiceDictionary> serviceDictionaryRepository, IHqlHelper hqlHelper)
        {
            Container = container;
            HouseServiceRepository = houseServiceRepository;
            SessionProvider = sessionProvider;
            HouseRepository = housepository;
            FiasRepository = fiasRepository;
            FiasAddressRepository = fiasAddressRepository;
            ServiceDictionaryRepository = serviceDictionaryRepository;
            HqlHelper = hqlHelper;
        }

        /// <summary>
        /// Список поставщиков
        /// </summary>
        public IDataResult SupplierList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = HouseServiceRepository
                .GetAll()
                .Where(x => x.Rso != null && x.Rso != string.Empty)
                .GroupBy(x => x.Rso)
                .Select(x => new { Name = x.Key })
                .Order(loadParam)
                .Filter(loadParam, Container)
                .ToList();

            return new ListDataResult(data.Skip(loadParam.Start).Take(loadParam.Limit).ToList(), data.Count());
        }

        public IDataResult SupplierListWithoutPaging(BaseParams baseParams)
        {
            var data = HouseServiceRepository
                .GetAll()
                .Where(x => x.Rso != null && x.Rso != string.Empty)
                .GroupBy(x => x.Rso)
                .Select(x => new { Name = x.Key })
                .ToList();

            return new ListDataResult(data, data.Count());
        }

        /// <summary>
        /// Список услуг
        /// </summary>
        public IDataResult ServiceList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            string[] restrictionList =
			{
				"ВОДОСНАБЖЕНИЕ",
				"ОТОПЛЕНИЕ",
				"ЭЛЕКТРО",
				"ВОДООТВЕДЕНИЕ"
			};

            var data = ServiceDictionaryRepository
                .GetAll()
                .ToList()
                .Where(x => restrictionList.Any(y => x.Name.Contains(y)))
                .AsQueryable()
                .OrderBy(x => x.Name)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }

        /// <summary>
        /// Список групп услуг
        /// </summary>
        public IDataResult ServiceGroupList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var groups = new[]
			{
				new
				{
					Id = 0,
					Name = "Электричество"
				},
				new
				{
					Id = 1,
					Name = "Тепло"
				},
				new
				{
					Id = 2,
					Name = "ХВС"
				},
				new
				{
					Id = 3, 
					Name = "ГВС"
				},
				new
				{
					Id = 4,
					Name = "Водоотведение"
				}
			};

            var data = groups
                .AsQueryable()
                .OrderBy(x => x.Name)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }

        /// <summary>
        /// Список муниципальных районов
        /// </summary>
        public IDataResult MunicipalAreaList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = FiasRepository
                .GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.KladrCurrStatus == 0
                            && x.AOLevel == FiasLevelEnum.Raion)
                .Select(x => new
                {
                    Name = x.OffName,
                    Id = x.AOGuid
                }).OrderBy(x => x.Name)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }

        /// <summary>
        /// Список населенных пунктов
        /// </summary>
        public IDataResult SettlementList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var parentGuid = baseParams.Params.GetAs<string>("ParentGuid");

            var data = FiasRepository
                .GetAll()
                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.KladrCurrStatus == 0
                            && (x.AOLevel == FiasLevelEnum.City || x.AOLevel == FiasLevelEnum.Place)
                            && x.ParentGuid == parentGuid)
                .Select(x => new
                {
                    Name = x.OffName,
                    Id = x.AOGuid
                }).OrderBy(x => x.Name)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }

        /// <summary>
        /// Список улиц
        /// </summary>
        public IDataResult StreetList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var placeGuid = baseParams.Params.GetAs<string>("PlaceGuid");
            var session = SessionProvider.GetCurrentSession();

            //Т.к. в fias_address могут быть улицы, которых нет в fias, улицы вытаскиваем из fias_address
            //Но, в fias_address у некоторых улиц кривое название => для их корректного отображения пересекаем с b4_fias и уже оттуда тянем наименование
            //Через LINQ nhibernate не поддерживает outer join, а содержание улиц в словаре сильно тормозит => приходится пользоваться HQL

            #region HQL

            var restriction = new StringBuilder();

            if (placeGuid != null)
            {
                restriction.AppendFormat(" and faddr.place_guid = '{0}' ", placeGuid);
            }

            var sql = @"select distinct faddr.street_guid as id
	                        , (case when fias.offname is not null then fias.shortname || '. ' || fias.offname else faddr.street_name end) as name
                        from b4_fias_address faddr
                        left outer join b4_fias fias on faddr.street_guid = fias.aoguid and fias.actstatus = 1 and fias.kladrpcurrstatus = 0
                        where street_guid is not null " + restriction;

            var data = session.CreateSQLQuery(sql)
                .SetResultTransformer(Transformers.AliasToBean<StreetAdapter>())
                .List<StreetAdapter>()
                .AsQueryable()
                .Order(loadParam)
                .Filter(loadParam, Container);

            #endregion

            #region LINQ

            //var data = FiasAddressRepository
            //    .GetAll()
            //    .Where(x => x.PlaceGuidId == placeGuid && x.StreetName != null)
            //    .Select(x => new
            //    {
            //        Id = x.StreetGuidId,
            //        Name = x.StreetName
            //    })
            //    .ToList()
            //    .Distinct()
            //    .AsQueryable()
            //    .Order(loadParam)
            //    .Filter(loadParam, Container);

            #endregion

            return new ListDataResult(data.Paging(loadParam).ToList(), data.Count());
        }

        /// <summary>
        /// Список расхождений объемов
        /// </summary>
        public IDataResult DiscrepancyList(BaseParams baseParams)
        {
            var supplier = baseParams.Params.GetAs<string>("Supplier");
            var showNullValues = baseParams.Params.GetAs<bool>("ShowNullValues");
            var service = baseParams.Params.GetAs<long?>("Service");
            var municipalAreaGuid = baseParams.Params.GetAs<string>("MunicipalAreaGuid");
            var settlementGuid = baseParams.Params.GetAs<string>("SettlementGuid");
            var streetGuid = baseParams.Params.GetAs<string[]>("StreetGuid");
            var period = baseParams.Params.GetAs<DateTime>("Period");

            var loadParam = baseParams.GetLoadParam();
            var session = SessionProvider.GetCurrentSession();

            //#SadButTrue 
            //Nhibernate не поддерживает получение кол-ва записей по сгруппированным строкам, не может адекватно работать со сгруппированными данными
            //по анонимному типу, т.ч. либо все кастить в лист, либо через HQL, ниже реализовано по последнему варианту

            #region Формирование ограничений

            var restrictions = new StringBuilder();

            restrictions.AppendFormat(" AND calculation_date >= '{0}' AND calculation_date < '{1}' ", period, period.AddMonths(1));

            //---
            //if (!string.IsNullOrEmpty(municipalAreaGuid))
            //{
            //    restrictions.AppendFormat(" AND (fias.parentguid = '{0}' OR faddr.place_guid = '{0}') ", municipalAreaGuid);
            //}
            //if (!string.IsNullOrEmpty(settlementGuid))
            //{
            //    restrictions.AppendFormat(" AND faddr.place_guid = '{0}' ", settlementGuid);
            //}
            //if (streetGuid != null && streetGuid.Any())
            //{
            //    restrictions.AppendFormat(" AND faddr.street_guid in ('{0}') ", string.Join("','", streetGuid));
            //}
            //---
            if (!string.IsNullOrEmpty(municipalAreaGuid))
            {
                restrictions.AppendFormat(" AND (municipalareaguid = '{0}' OR placeguid = '{0}') ", municipalAreaGuid);
            }
            if (!string.IsNullOrEmpty(settlementGuid))
            {
                restrictions.AppendFormat(" AND placeguid = '{0}' ", settlementGuid);
            }
            if (streetGuid != null && streetGuid.Any())
            {
                restrictions.AppendFormat(" AND street_guid in ('{0}') ", string.Join("','", streetGuid));
            }

            //Т.к. при отображении группировка производится на первом этапе получения данных, ограничение по группам проводится на втором этапе
            var postRestrictions = new StringBuilder();

            if (service != null)
            {
                postRestrictions.AppendFormat(" AND serviceid = {0}", service);
            }
            //if (!showNullValues)
            //{
            //    postRestrictions.Append(" AND rsodata != 0 AND uodata != 0 ");
            //}
            if (!showNullValues)
            {
                postRestrictions.Append(" AND pole2 != 0 AND uodata != 0 ");
            }
            if (!string.IsNullOrEmpty(supplier))
            {
                postRestrictions.AppendFormat(" AND supplier = '{0}' ", supplier);
            }

            #endregion

            #region Формирование запроса

            //пара кейсов в начале - из-за того, что на выводе группируем несколько услуг, и эта группировка нигде не используется, кроме как здесь
            //Вероятно, есть вопросы по addressSignature - в b4_fias_address есть множество домов, у которых идентичны все поля, кроме идентификатора,
            //следовательно группировать приходится не по идентификатору, а по некой сигнатуре адреса
            //Но так как идентификатор дома нам все равно нужен, берем максимальный из них
            //            var sql = @"WITH data AS (SELECT 
            //								(case when serviceid in (1, 16, 9, 448, 97, 44) then 'Электричество' 
            //									when serviceid in (74, 92, 46) then 'Тепло' 
            //									when serviceid in (90, 13, 83) then 'ХВС' 
            //									when serviceid in (112, 36) then 'ГВС' 
            //									when serviceid in (14, 85) then 'Водоотведение' end) as servicename
            //								, (case when serviceid in (1, 16, 9, 448, 97, 44) then 0
            //									when serviceid in (74, 92, 46) then 1 
            //									when serviceid in (90, 13, 83) then 2 
            //									when serviceid in (112, 36) then 3 
            //									when serviceid in (14, 85) then 4 end) as serviceid
            //								, faddr.address_name AS houseaddress, ispublished, faddr.place_guid AS placeguid, fias.parentguid AS municipalareaguid
            //								, concat('street', faddr.street_guid, 'house', faddr.house, 'housing', faddr.housing, 'building', faddr.building, 'flat', faddr.flat) as addressSignature
            //								, MAX(faddr.id) AS fiashouseid
            //								, MAX(main.rso) AS supplier
            //                              , MAX(main.manorgs) AS manorg
            //								, SUM(CASE WHEN main.manorgs IS NOT NULL AND main.manorgs != '' THEN totalvolume ELSE 0 END) AS uodata 
            //								, SUM(CASE WHEN rso IS NOT NULL AND rso != '' THEN totalvolume ELSE 0 END) AS rsodata
            //							FROM gis_house_service_register MAIN
            //								--JOIN gis_service_dictionary srv ON main.serviceid = srv.id
            //								JOIN gis_house_register house 
            //									JOIN b4_fias_address faddr 
            //										JOIN b4_fias fias 
            //										ON faddr.place_guid = fias.aoguid AND fias.actstatus = 1 and fias.kladrpcurrstatus = 0
            //									ON house.fiasaddress = faddr.id 
            //								ON main.houseid = house.id 
            //							WHERE 1=1 " + restrictions + @" 
            //							GROUP BY 1, 2, 3, 4, 5, 6, 7
            //							ORDER BY 1)
            //						SELECT *, uodata - rsodata AS discrepancy FROM data
            //						WHERE 1=1 " + postRestrictions
            //                      + HqlHelper.GetComplexFilterHqlQuery(loadParam)
            //                      + HqlHelper.GetOrderHqlQuery(loadParam);

            #endregion

            #region костыль

            var sql = @"select servicename, serviceid, houseaddress, ispublished, placeguid, municipalareaguid, addresssignature, pole2 as rsodata, uodata
	                        , pole3 as discrepancy, fiashouseid, supplier, manorg
                        from _anton_
                        where 1=1 " + restrictions + postRestrictions
                      + HqlHelper.GetComplexFilterHqlQuery(loadParam)
                      + HqlHelper.GetOrderHqlQuery(loadParam);

            #endregion

            var result = session.CreateSQLQuery(sql)
                .SetResultTransformer(Transformers.AliasToBean<DiscrepancyAdapter>())
                .List<DiscrepancyAdapter>()
                .AsQueryable();

            return new ListDataResult(result.Paging(loadParam).ToList(), result.Count());
        }

        /// <summary>
        /// Пометить данные как "Опубликовано"
        /// </summary>
        public IDataResult Publish(BaseParams baseParams)
        {
            var session = SessionProvider.GetCurrentSession();
            var data = baseParams.Params.GetAs<List<DiscrepancyAdapter>>("Data");
            foreach (var item in data)
            {
                //var sql = string.Format(" UPDATE gis_house_service_register SET ispublished = true " +
                //                        " WHERE houseaddress = '{0}' AND serviceid = {1} ",
                //    item.HouseAddress, item.ServiceId);
                var sql = string.Format(" UPDATE _anton_ SET ispublished = true " +
                                        " WHERE houseaddress = '{0}' AND serviceid = {1} ",
                    item.HouseAddress, item.ServiceId);

                session.CreateSQLQuery(sql)
                    .ExecuteUpdate();
            }

            return new BaseDataResult(data);
        }

        #region Адаптеры

        private class StreetAdapter
        {
            private string id, name;

            public string Id { get { return id; } set { id = value; } }
            public string Name { get { return name; } set { name = value; } }
        }

        /// <summary>
        /// Адаптер для расхождений объема
        /// </summary>
        private class DiscrepancyAdapter
        {
            private string houseaddress, servicename, supplier, municipalareaguid, placeguid, addresssignature, manorg;
            private long serviceid, fiashouseid;
            private bool ispublished;
            private decimal uodata, rsodata, discrepancy;

            public string HouseAddress { get { return houseaddress; } set { houseaddress = value; } }
            public string ServiceName { get { return servicename; } set { servicename = value; } }
            public string Supplier { get { return supplier; } set { supplier = value; } }
            public string ManOrg { get { return manorg; } set { manorg = value; } }
            public string MunicipalAreaGuid { get { return municipalareaguid; } set { municipalareaguid = value; } }
            public string PlaceGuid { get { return placeguid; } set { placeguid = value; } }
            public string AddressSignature { get { return addresssignature; } set { addresssignature = value; } }
            public long ServiceId { get { return serviceid; } set { serviceid = value; } }
            public long FiasHouseId { get { return fiashouseid; } set { fiashouseid = value; } }
            public bool IsPublished { get { return ispublished; } set { ispublished = value; } }
            public decimal UOData { get { return uodata; } set { uodata = value; } }
            public decimal RSOData { get { return rsodata; } set { rsodata = value; } }
            public decimal Discrepancy { get { return discrepancy; } set { discrepancy = value; } }
        }

        #endregion
    }
}