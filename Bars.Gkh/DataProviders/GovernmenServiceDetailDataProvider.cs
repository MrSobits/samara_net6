namespace Bars.Gkh.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.DataProviders.Meta;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Licensing;
    using Bars.Gkh.Enums.Licensing;

    using Castle.Windsor;

    /// <summary>
    /// Источник данных <see cref="GovernmenServiceDetailMeta"/>
    /// </summary>
    public class GovernmenServiceDetailDataProvider : BaseCollectionDataProvider<GovernmenServiceDetailMeta>
    {
        private readonly ServiceDetailSectionType type;

        /// <inheritdoc />
        public override bool IsHidden => true;

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override string Description { get; }

        /// <inheritdoc />
        public GovernmenServiceDetailDataProvider(IWindsorContainer container, ServiceDetailSectionType type)
            : base(container)
        {
            this.type = type;
        }

        /// <inheritdoc />
        protected override IQueryable<GovernmenServiceDetailMeta> GetDataInternal(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var domain = this.Container.ResolveDomain<GovernmenServiceDetail>();

            using (this.Container.Using(domain))
            {
                var data = domain.GetAll()
                    .Where(x => x.FormGovernmentService.Id == id && x.DetailGroup.ServiceDetailSectionType == this.type)
                    .ToList();

                var result = new List<GovernmenServiceDetailMeta>();
                string lastGroupName = null;

                foreach (var governmenServiceDetail in data)
                {
                    if (governmenServiceDetail.DetailGroup.GroupName != lastGroupName)
                    {
                        result.Add(new GovernmenServiceDetailMeta { DisplayValue = governmenServiceDetail.DetailGroup.GroupName });
                        lastGroupName = governmenServiceDetail.DetailGroup.GroupName;
                    }
                    else
                    {
                        lastGroupName = null;
                        result.Add(new GovernmenServiceDetailMeta
                        {
                            RowNumber = governmenServiceDetail.DetailGroup.RowNumber,
                            Value = governmenServiceDetail.Value,
                            DisplayValue = governmenServiceDetail.DetailGroup.Name
                        });
                    }
                }

                return result.AsQueryable();
            }
        }
    }
}