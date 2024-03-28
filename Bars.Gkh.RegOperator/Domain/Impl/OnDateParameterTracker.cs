namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Domain.ParameterVersioning.Proxy;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для отслеживания изменений версионируемых параметров с учетом проставленной даты закрытия ЛС
    /// </summary>
    /// <remarks>Повторяет весь функционал, при этом добавляя изменение о закрытии ЛС, если проставлена дата закрытия</remarks>
    public class OnDateParameterTracker : ParameterTracker
    {
        /// <inheritdoc />
        public OnDateParameterTracker(IWindsorContainer container)
            : base(container)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date)
        {
            var result = base.GetChanges(account, date);

            if (account.CloseDate.IsValid())
            {
                result = result.Union(new[]
                {
                    new EntityLogRecord
                    {
                        DateActualChange = account.CloseDate,
                        DateApplied = DateTime.Now,
                        EntityId = account.Id,
                        ParameterName = VersionedParameters.AreaShare,
                        PropertyValue = "0",

                    },
                });
            }

            return result;
        }

        /// <inheritdoc />
        public override IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, IPeriod period)
        {
            var result = base.GetChanges(account, period);

            if (account.CloseDate.IsValid())
            {
                result = result.Union(new[]
                {
                    new EntityLogRecord
                    {
                        DateActualChange = account.CloseDate,
                        DateApplied = DateTime.Now,
                        EntityId = account.Id,
                        ParameterName = VersionedParameters.AreaShare,
                        PropertyValue = "0",

                    },
                });
            }

            return result;
        }
    }
}