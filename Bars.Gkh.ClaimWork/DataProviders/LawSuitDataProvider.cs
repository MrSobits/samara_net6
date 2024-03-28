namespace Bars.Gkh.ClaimWork.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.ClaimWork.DataProviders.Meta;
    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для печати претензии
    /// </summary>
    public class LawSuitDataProvider : BaseCollectionDataProvider<DocumentClwProxy>
    {
        public LawSuitDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "Исковое заявление"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public string LawSuitId { get; set; }

        protected override IQueryable<DocumentClwProxy> GetDataInternal(BaseParams baseParams)
        {
            var record = new DocumentClwProxy { Id = LawSuitId };

            var records = new List<DocumentClwProxy> { record };

            return records.AsQueryable();
        }
    }
}