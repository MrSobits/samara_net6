namespace Bars.Gkh.DataProviders
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Utils;
    using Bars.Gkh.DataProviders.Meta;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Licensing;

    using Castle.Windsor;

    /// <summary>
    /// Источник данных <see cref="FormGovernmentServiceMeta"/>
    /// </summary>
    public class FormGovernmentServiceDataProvider : BaseCollectionDataProvider<FormGovernmentServiceMeta>
    {
        /// <inheritdoc />
        public override bool IsHidden => true;

        /// <inheritdoc />
        public override string Name { get; }

        /// <inheritdoc />
        public override string Description { get; }

        /// <inheritdoc />
        public FormGovernmentServiceDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        protected override IQueryable<FormGovernmentServiceMeta> GetDataInternal(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var domain = this.Container.ResolveDomain<FormGovernmentService>();

            using (this.Container.Using(domain))
            {
                return domain.GetAll()
                    .Where(x => x.Id == id)
                    .AsEnumerable()
                    .Select(x => new FormGovernmentServiceMeta
                    {
                        PeriodName = $"{x.Quarter.GetDisplayName()} {x.Year}",
                        ServiceName = x.GovernmentServiceType.GetDisplayName()
                    })
                    .AsQueryable();
            }
        }
    }
}