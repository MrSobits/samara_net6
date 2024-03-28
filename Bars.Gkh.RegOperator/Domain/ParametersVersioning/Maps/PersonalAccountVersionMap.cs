namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using System;
    using B4;
    using Castle.Core.Internal;

    using Entities;
    using Gkh.Domain.ParameterVersioning;

    /// <summary>
    /// Маппинг версионируемых свойств для лицевого счета
    /// </summary>
    public class PersonalAccountVersionMap : VersionedEntity<BasePersonalAccount>
    {
        public PersonalAccountVersionMap()
        {
            this.Map(x => x.AreaShare, VersionedParameters.AreaShare, "Доля собственности");
            this.Map(x => x.PersAccNumExternalSystems, VersionedParameters.PersonalAccountExternalNum, "Внешний номер ЛС", 
                x => x.PersAccNumExternalSystems.IsNullOrEmpty());
            this.Map(x => x.Room, VersionedParameters.Room, "№ квартиры/помещения");
        }

        protected override bool CanValidate(string parameterName, object entity)
        {
            if (entity is BasePersonalAccount && parameterName == VersionedParameters.AreaShare)
            {
                return true;
            }

            return base.CanValidate(parameterName, entity);
        }

        protected override IDataResult ValidateInternal(object entity, object value, DateTime factDate, string parameterName)
        {
            if ((entity as BasePersonalAccount).OpenDate > factDate)
            {
                return new BaseDataResult(false, "Дата вступления в силу нового значения параметра не должна быть раньше даты открытия лицевого счета!");
            }

            return base.ValidateInternal(entity, value, factDate, parameterName);
        }
    }
}