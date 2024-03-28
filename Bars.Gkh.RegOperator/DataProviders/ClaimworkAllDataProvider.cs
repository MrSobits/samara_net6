namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;

    using Castle.Windsor;

    /// <summary>
    /// Расширенный поставщик данных для документа ПИР
    /// </summary>
    public class ClaimworkAllDataProvider : BaseCollectionDataProvider<ClaimworkAllProxy>
    {
        public ClaimworkAllDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "ClaimworkInfo"; }
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public string ClaimworkId { get; set; }
        public string LawsuitId { get; set; }
        public string OwnerId { get; set; }
        public string FIO { get; set; }
        public string Pos { get; set; }
        public bool Solidary { get; set; }

        protected override IQueryable<ClaimworkAllProxy> GetDataInternal(BaseParams baseParams)
        {
            var records = new List<ClaimworkAllProxy>();
            records.Add(new ClaimworkAllProxy
            {
                ClwId = ClaimworkId.ToString(),
                LawId = LawsuitId.ToString(),
                RloiId = OwnerId.ToString(),
                Solidary = Solidary,
                FIO =FIO
            });

            return records.AsQueryable();         
        }
    }
}