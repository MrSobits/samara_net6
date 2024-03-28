namespace Bars.Gkh.ClaimWork.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Analytics.Data;
    using Castle.Windsor;
    using Meta;

    /// <summary>
    /// Поставщик данных для печати претензии
    /// </summary>
    public class NotificationDataProvider : BaseCollectionDataProvider<DocumentClwProxy>
    {
        public NotificationDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "Уведомление_ПИР"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public string NotificationId { get; set; }

        protected override IQueryable<DocumentClwProxy> GetDataInternal(BaseParams baseParams)
        {
            var record = new DocumentClwProxy { Id = NotificationId };

            var records = new List<DocumentClwProxy> { record };

            return records.AsQueryable();
        }
    }
}