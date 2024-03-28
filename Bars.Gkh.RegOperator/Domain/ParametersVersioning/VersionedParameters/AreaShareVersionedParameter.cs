namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;

    using B4.DataAccess;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;

    public class AreaShareVersionedParameter : AbstractVersionedParameter
    {
        public AreaShareVersionedParameter(IWindsorContainer container, BasePersonalAccount account) : base(container)
        {
            this._object = account;
        }

        public override string ParameterName
        {
            get { return VersionedParameters.AreaShare; }
            set { }
        }

        protected internal override PersistentObject GetPersistentObject()
        {
            return this._object;
        }

        private readonly PersistentObject _object;

        /// <inheritdoc />
        public override KeyValuePair<object, T> GetActualByDate<T>(BasePersonalAccount account, DateTime date, bool limitDateApplied = false)
        {
            // для расчетов до указанной даты нам необходима доля собственности 0 после даты закрытия
            // но при этом я не хочу сохранять EntityLogLight
            if (date >= account.CloseDate && account.CloseDate.IsValid())
            {
                return default(KeyValuePair<object, T>);
            }

            return base.GetActualByDate<T>(account, date, limitDateApplied);
        }
    }
}