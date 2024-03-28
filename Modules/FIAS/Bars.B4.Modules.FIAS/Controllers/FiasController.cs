using Newtonsoft.Json;

namespace Bars.B4.Modules.FIAS
{
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FIAS.Enums;
    using Bars.B4.Utils;

    public class FiasController : DataController<Fias>
    {
        public override ActionResult List(StoreLoadParams storeParams)
        {
            /*
             Тут мы получаем все дочерние элементы по уровню
             */

            var parentGuid = storeParams.Params.ContainsKey("parentGuid")
                                   ? storeParams.Params["parentGuid"].ToString()
                                   : "";

            var level = storeParams.Params.ContainsKey("level")
                                   ? storeParams.Params["level"].ToInt()
                                   : 0;

            var service = this.Resolve<IDomainService<Fias>>();

            var data = service.GetAll()
                .Where(x => x.ParentGuid == parentGuid)
                .WhereIf(level > 0, x => x.AOLevel == (FiasLevelEnum)level)
                .Select(x => new
                {
                    x.Id,
                    Name = x.OffName+" "+x.ShortName,
                    x.CodeRecord,
                    x.AOLevel,
                    x.AOGuid,
                    x.TypeRecord,
                    x.ActStatus,
                    x.MirrorGuid
                 })
                .Filter(storeParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(storeParams).Paging(storeParams);

            return new JsonListResult(data.ToList(), totalCount);
        }

        public ActionResult GetObjectsWithoutStreet(StoreLoadParams storeParams)
        {
            var parentGuid = storeParams.Params.GetAs("parentGuid", string.Empty);

            var dataFias = this.DomainService.GetAll()
                .WhereIf(string.IsNullOrEmpty(parentGuid), x => x.ParentGuid == null || x.ParentGuid == parentGuid)
                .WhereIf(!string.IsNullOrEmpty(parentGuid), x => x.ParentGuid == parentGuid)
                .Where(x => x.AOLevel != FiasLevelEnum.Street)
                .Select(x => new
                {
                    x.Id,
                    x.OffName,
                    x.ShortName,
                    x.CodeRecord,
                    x.AOLevel,
                    x.AOGuid,
                    x.ParentGuid,
                    x.TypeRecord,
                    x.ActStatus,
                    x.MirrorGuid,
                    AnyChild = this.DomainService.GetAll().Any(y => y.ParentGuid == x.AOGuid)
                })
                .OrderBy(x => x.AOLevel)
                .ThenBy(x => x.OffName)
                .ToList()
                .Select(x => new FiasProxyTreeItem()
                {
                    Id = x.Id,
                    GuidId = x.AOGuid,
                    Text = x.OffName + " " + x.ShortName + (x.ActStatus == 0 ? "(Не активен)" : ""),
                    TypeRecord = (int) x.TypeRecord,
                    CodeRecord = x.CodeRecord,
                    Level = (int) x.AOLevel,
                    ParentGuidId = x.ParentGuid,
                    MirrorGuid = x.MirrorGuid,
                    Leaf = !x.AnyChild
                });

            return new JsonNetResult(dataFias);
        }

        /// <summary>
        /// метод получения динамических адресов уровней населенных пунктов (без улиц) по переданной строке фильтра
        /// </summary>
        public ActionResult GetPlacesList(string filter)
        {
            var repository = this.Container.Resolve<IFiasRepository>();
            var adrs = repository.GetPlacesDinamicAddress(filter);
            return new JsonListResult(adrs, adrs.Count);
        }

        /// <summary>
        /// метод получения динамических адресов уровня улицы (без других уровней) по переданной строке фильтра и родительскому guid
        /// </summary>
        public ActionResult GetStreetsList(string filter, string parentguid)
        {
            var repository = this.Container.Resolve<IFiasRepository>();
            var adrs = repository.GetStreetsDinamicAddress(filter, parentguid);
            return new JsonListResult(adrs, adrs.Count);
        }

        /// <summary>
        /// Метод получения динамических адресов уровня номер дома, корпуса, строения (без других уровней) по переданной строке фильтра и родительскому guid
        /// </summary>
        public ActionResult GetHousesList(string filter, string parentguid)
        {
            var repository = this.Container.Resolve<IFiasRepository>();
            var adrs = repository.GetHousesDynamicAddress(filter, parentguid);
            return new JsonListResult(adrs, adrs.Count);
        }

        /// <summary>
        /// Метод получения динамических адресов уровня номер дома, корпуса, строения (без других уровней) по переданной строке фильтра, родительскому guid и типу владения
        /// </summary>
        public ActionResult GetHousesListWithEstimate(string filter, string parentguid, FiasEstimateStatusEnum estimatetype)
        {
            var repository = this.Container.Resolve<IFiasRepository>();
            var adrs = repository.GetHousesDynamicAddress(filter, parentguid, estimatetype);
            return new JsonListResult(adrs, adrs.Count);
        }
    }

    public class FiasProxyTreeItem
    {
        public FiasProxyTreeItem()
        {
            this.Expanded = false;
        }

        public FiasProxyTreeItem(string text, long id, string code)
        {
            this.Expanded = false;
            this.Text = text;
            this.Id = id;
            this.CodeRecord = code;
        }
        
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("expanded")]
        public bool Expanded { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("fiasId")]
        public long Id { get; set; }

        [JsonProperty("fiasGuidId")]
        public string GuidId { get; set; }

        [JsonProperty("fiasParentGuidId")]
        public string ParentGuidId { get; set; }

        [JsonProperty("fiasParentId")]
        public int ParentId { get; set; }

        [JsonProperty("fiasCode")]
        public string CodeRecord { get; set; }

        [JsonProperty("typeRecord")]
        public int TypeRecord { get; set; }

        [JsonProperty("mirrorGuid")]
        public string MirrorGuid { get; set; }

        /// <summary>
        /// Флаг: есть ли подчиненный объект
        /// </summary>
        [JsonProperty("leaf")]
        public bool Leaf { get; set; }

        public void Add(FiasProxyTreeItem item)
        {
            item.ParentId = this.ParentId;
        }
    }
}
