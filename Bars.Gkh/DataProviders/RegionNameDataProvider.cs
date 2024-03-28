namespace Bars.Gkh.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.DataProviders.Meta;
    using Castle.Windsor;

    using Slepov.Russian.Morpher;

    class RegionNameDataProvider : BaseCollectionDataProvider<RegionNameMeta>
    {
        public IGkhParams GkhParams { get; set; }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }

        public RegionNameDataProvider(IWindsorContainer container) : base(container)
        {
        }

        protected override IQueryable<RegionNameMeta> GetDataInternal(BaseParams baseParams)
        {
            var prm = GkhParams.GetParams();

            var regionName = prm.ContainsKey("RegionName") ? prm["RegionName"].ToStr() : "";

            var name = GetMorpher().Проанализировать(regionName);

            var result = new List<RegionNameMeta>
            {
                new RegionNameMeta
                {
                    Именительный = name != null ? name.Именительный : "",
                    Родительный = name != null ? name.Родительный : "",
                    Дательный = name != null ? name.Дательный : "",
                    Винительный = name != null ? name.Винительный : "",
                    Творительный = name != null ? name.Творительный : "",
                    Предложный = name != null ? name.Предложный : ""
                }
            };

            return result.AsQueryable();
        }

        public override string Name
        {
            get { return "RegionNameDataProvider"; }
        }

        public override string Description
        {
            get { return "Название региона"; }
        }
    }
}
