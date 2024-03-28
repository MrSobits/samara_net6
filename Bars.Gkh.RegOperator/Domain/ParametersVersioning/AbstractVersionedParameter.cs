namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.ParameterVersioning.Proxy;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public abstract class AbstractVersionedParameter : IVersionedParameter
    {
        protected readonly EntityLogHelper LogHelper;

        protected AbstractVersionedParameter(IWindsorContainer container)
        {
            this.Container = container;
            this.LogHelper = new EntityLogHelper(container);
        }

        protected IWindsorContainer Container { get; private set; }

        public abstract string ParameterName { get; set; }

        public IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, IPeriod period)
        {
            if (this.GetPersistentObject() != null)
            {
                return this.LogHelper.GetUncalcAppliedInPeriod(this.ParameterName, this.GetPersistentObject(), period, account);
            }

            return new EnumerableQuery<EntityLogRecord>(new List<EntityLogRecord>());
        }

        public virtual IEnumerable<EntityLogRecord> GetChanges(BasePersonalAccount account, DateTime date)
        {
            if (this.GetPersistentObject() != null)
            {
                return this.LogHelper.GetUncalcAppliedInRange(this.ParameterName, this.GetPersistentObject(), date, account);
            }

            return new EnumerableQuery<EntityLogRecord>(new List<EntityLogRecord>());
        }

        public virtual KeyValuePair<object, T> GetLastChangedByDate<T>(BasePersonalAccount account, IPeriod period, DateTime date)
        {
            if (this.GetPersistentObject() == null)
            {
                return default(KeyValuePair<object, T>);
            }

            var lastValue = this.LogHelper.GetLastCalculatedOrLastAppliedByDate(this.ParameterName, this.GetPersistentObject(), date, account);

            var actualInChanges = this.LogHelper
                .GetUncalcAppliedInPeriod(this.ParameterName, this.GetPersistentObject(), period, account)
                .OrderByDescending(x => x.DateApplied)
                .ThenByDescending(x => x.DateActualChange)
                .Where(x => x.ParameterName == this.ParameterName)
                .FirstOrDefault(x => x.DateActualChange.Date <= date.Date);

            var value = (actualInChanges ?? lastValue).Return(x => x.PropertyValue, string.Empty).Replace('.', ',');
            var actualDate = (actualInChanges ?? lastValue).Return(x => x.DateActualChange);
            return value.IsEmpty() ? default(KeyValuePair<object, T>) : new KeyValuePair<object, T>(actualDate, value.To<T>());
        }

        public KeyValuePair<object, T> GetActualByDate<T>(BasePersonalAccount account, DateTime date, bool useCache, bool limitDateApplied = false)
        {
            if (useCache)
            {
                return this.GetActualByDate<T>(account, date, limitDateApplied);
            }

            return this.GetActualByDateNonCached<T>(account, date, limitDateApplied);
        }

        protected virtual KeyValuePair<object, T> GetActualByDateNonCached<T>(BasePersonalAccount account, DateTime date, bool limitDateApplied = false)
        {
            throw new NotImplementedException();
        }

        public virtual KeyValuePair<object, T> GetActualByDate<T>(BasePersonalAccount account, DateTime date, bool limitDateApplied = false)
        {
            if (this.GetPersistentObject() == null)
            {
                return default(KeyValuePair<object, T>);
            }

            var a = this.LogHelper
                .GetActualChangesInRange(this.ParameterName, new[] {this.GetPersistentObject()}, date, account)
                .WhereIf(limitDateApplied, x => x.DateApplied.Value.Date <= date.Date)
                .OrderByDescending(x => x.DateApplied)
                .ThenByDescending(x => x.DateActualChange)
                .ToList();

            EntityLogRecord result = null;

            for (int i = 0; i < a.Count; i++)
            {
                if (date.Date >= a[i].DateActualChange.Date)
                {
                    result = a[i];
                    break;
                }
            }

            var b = result;

            var value = b.Return(x => x.PropertyValue, string.Empty).Replace('.', ',');

            var dateActualChange = b.Return(x => x.DateActualChange);

            //оптимизация для decimal
            if (typeof(T) == typeof(decimal))
            {
                decimal res = 0;
                decimal.TryParse(value, out res);
                return new KeyValuePair<object, T>(dateActualChange, res.As<T>());
            }
            return value.IsEmpty() ? default(KeyValuePair<object, T>) : new KeyValuePair<object, T>(dateActualChange, value.To<T>());
        }

        protected internal abstract PersistentObject GetPersistentObject();
    }
}