namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain.ParameterVersioning.Proxy;
    using Interfaces;
    using ParametersVersioning;
    using Repository;

    /// <summary>
    /// Сервис для отслеживания изменений версионируемых параметров
    /// </summary>
    public class ParameterTracker : IParameterTracker
    {
        private readonly IWindsorContainer container;
        private readonly HashSet<IVersionedParameter> parameters;
        private readonly IChargePeriodRepository periodRepo;
        private readonly ITariffCache tariffCache;

        public ParameterTracker(IWindsorContainer container)
        {
            this.container = container;
            this.parameters = new HashSet<IVersionedParameter>();
            this.periodRepo = container.Resolve<IChargePeriodRepository>();
            this.tariffCache = container.Resolve<ITariffCache>();
        }

        /// <inheritdoc />
        public virtual IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, IPeriod period)
        {
            this.CheckParameters(account, period);

            return this.parameters.SelectMany(x => x.GetChanges(account, period));
        }

        /// <inheritdoc />
        public virtual IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date)
        {
            this.CheckParameters(account, this.periodRepo.GetPeriodByDate(date));

            return this.parameters.SelectMany(x => x.GetChanges(account, date));
        }

        /// <inheritdoc />
        public virtual IVersionedParameter GetParameter(string parameterName, BasePersonalAccount account, IPeriod period)
        {
            this.CheckParameters(account, period);

            if (this.parameters.All(x => x.ParameterName != parameterName))
            {
                throw new NotImplementedException("Не удалось получить данные для данного параметра");
            }

            return this.parameters.First(x => x.ParameterName == parameterName);
        }

        private void CheckParameters(BasePersonalAccount account, IPeriod period)
        {
            if (this.prevAcc.Return(x => x.Id) != account.Return(x => x.Id) || this.prevPer.Return(x => x.Id) != period.Return(x => x.Id))
            {
                this.prevAcc = account;
                this.prevPer = period;
                this.Clear();
            }

            if (!this.parameters.Any())
            {
                this.parameters.Add(new AreaShareVersionedParameter(this.container, account));
                this.parameters.Add(new RoomAreaVersionedParameter(this.container, account.Room));
                this.parameters.Add(new TariffVersionedParameter(this.container, account, this.tariffCache));
                this.parameters.Add(new PersonalAccountOpenDateVersionedParameter(this.container, account));
            }
        }

        private void Clear()
        {
            this.parameters.Clear();
            this.parameters.TrimExcess();
        }

        private BasePersonalAccount prevAcc;
        private IPeriod prevPer;
    }
}