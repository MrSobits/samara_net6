namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Modules.Gkh1468.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Региональные операторы капитального ремонта (regop.csv)
    /// </summary>
    public class RegopSelectorService : BaseProxySelectorService<RegopProxy>
    {
        /// <inheritdoc />
        protected override bool CanGetFullData()
        {
            return true;
        }

        /// <inheritdoc />
        protected override IDictionary<long, RegopProxy> GetCache()
        {
            var regopRepos = this.Container.ResolveRepository<RegOperator>();
            var config = this.Container.GetGkhConfig<AdministrationConfig>();

            using (this.Container.Using(regopRepos, config))
            {
                var paymentDay = config.FormatDataExport.FormatDataExportRegOpConfig.EpdDay;
                var epdMonth = config.FormatDataExport.FormatDataExportRegOpConfig.EpdMonth;
                var paymentMonth = epdMonth == MonthType.CurrentMonth ? 1 : epdMonth == MonthType.NextMonth ? 2 : (int?) null;

                return regopRepos.GetAll()
                    .Select(x => x.Contragent.ExportId)
                    .AsEnumerable()
                    .Select(id => new RegopProxy
                    {
                        Id = id,
                        HasPaymentInformSystem = 1,
                        HasCrInformSystem = 1,
                        PaymentModel = 1,
                        PaymentDay = paymentDay,
                        PaymentMonth = paymentMonth
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}