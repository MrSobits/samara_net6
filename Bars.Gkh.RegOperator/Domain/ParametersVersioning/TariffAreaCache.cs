namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    using Castle.Windsor;

    /// <summary>
    /// Кэш для получения актуальных тарифа и площади 
    /// </summary>
    public class TariffAreaCache : ITariffAreaCache
    {
        private readonly IWindsorContainer container;

        private IParameterTracker paramTracker;
        private ITariffCache tariffCache;        
        private IEntityLogCache entityLogCache;      
        private ICalculatedParameterCache traceCache;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        public TariffAreaCache(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void Init(PersonalAccountRecord[] mainInfo, IPeriod period)
        {
            this.Init(mainInfo, new[] {period});
        }

        /// <inheritdoc />
        public void Init(PersonalAccountRecord mainInfo, IPeriod[] periods)
        {
            this.Init(new[] {mainInfo}, periods);
        }

        private void Init(PersonalAccountRecord[] mainInfo, IPeriod[] periods)
        {
            this.paramTracker = this.container.Resolve<IParameterTracker>();

            this.tariffCache = this.container.Resolve<ITariffCache>();
            this.entityLogCache = this.container.Resolve<IEntityLogCache>();
            this.traceCache = this.container.Resolve<ICalculatedParameterCache>();

            this.InitCache(mainInfo, periods);
        }

        private void InitCache(PersonalAccountRecord[] mainInfo, IPeriod[] periods)
        {
            var roIds = mainInfo.Select(x => x.RealityObjectId)
               .Distinct()
               .ToArray();

            var accountIds = mainInfo.Select(x => x.AccountId)
                .Distinct()
                .ToArray();

            var logIds = mainInfo.Select(x => x.RoomId)
                .Distinct()
                .ToArray()
                .Union(accountIds)
                .Distinct()
                .ToArray();

            this.tariffCache.Init(roIds);

            this.entityLogCache.Init(logIds);

            this.traceCache.Init(accountIds, periods);
        }

        /// <inheritdoc />
        public TariffAreaRecord GetTariffArea(BasePersonalAccount account, IPeriod period)
        {
            var record = new TariffAreaRecord();

            var tarifParam = this.paramTracker.GetParameter(
                    VersionedParameters.BaseTariff,
                    account,
                    period);

            var periodEnd = period.StartDate.AddMonths(1).AddDays(-1);

            record.BaseTariff = tarifParam.GetActualByDate<decimal>(
                   account,
                   period.EndDate ?? periodEnd,
                   true).Value;
            
            var calcParamTraceData = this.traceCache.GetParameters(account, period);
            if (calcParamTraceData != null)
            {
                record.AreaShare = calcParamTraceData.Get(VersionedParameters.AreaShare).ToDecimal();
                record.Tariff = calcParamTraceData.Get(VersionedParameters.BaseTariff).ToDecimal();
                record.RoomArea = calcParamTraceData.Get(VersionedParameters.RoomArea).ToDecimal();
            }
            else
            {
                record.Tariff = tarifParam.GetActualByDate<decimal>(
                    account,
                    period.EndDate ?? periodEnd,
                    false).Value;

                var areaParam = this.paramTracker.GetParameter(VersionedParameters.RoomArea, account, period);
                record.RoomArea = areaParam.GetActualByDate<decimal?>(
                    account,
                    period.EndDate ?? period.StartDate,
                    true,
                    true).Value ?? account.Room.Area;

                var areaShareParam = this.paramTracker.GetParameter(
                    VersionedParameters.AreaShare,
                    account,
                    period);

                record.AreaShare = areaShareParam.GetActualByDate<decimal?>(
                    account,
                    period.EndDate ?? period.StartDate,
                    true).Value ?? account.AreaShare;     
            }

            record.Account = account;
            return record;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.entityLogCache != null)
            {
                this.entityLogCache.Dispose();
            }

            if (this.tariffCache != null)
            {
                this.tariffCache.Dispose();
            }

            if (this.traceCache != null)
            {
                this.traceCache.Dispose();
            }

            this.container.Release(this.tariffCache);
            this.container.Release(this.entityLogCache);
            this.container.Release(this.traceCache);
        }
    }
}