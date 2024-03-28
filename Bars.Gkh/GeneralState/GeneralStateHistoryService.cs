namespace Bars.Gkh.GeneralState
{
    using System;
    using System.Linq;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для работы с обощенными состояниями
    /// </summary>
    public class GeneralStateHistoryService : IGeneralStateHistoryService
    {
        private readonly IGeneralStateProvider generalStateProvider;
        private readonly IGkhUserManager userManager;

        public GeneralStateHistoryService(IGeneralStateProvider generalStateProvider, IGkhUserManager userManager)
        {
            this.generalStateProvider = generalStateProvider;
            this.userManager = userManager;
        }

        /// <inheritdoc />
        public GeneralStatefulEntityInfo GetStateHistoryInfo(Type type, string propertyName = null)
        {
            return this.generalStateProvider
                .GetStatefulInfos()
                .Select(x => x.Value)
                .Where(x => x.EntityType.Equals(type.FullName))
                .WhereIf(propertyName != null, x => x.PropertyInfo.Name.Equals(propertyName))//если не передали имя, то первое попавшееся передаем
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public GeneralStatefulEntityInfo GetStateHistoryInfo(string code)
        {
            return this.generalStateProvider
                .GetStatefulInfos()
                .Get(code); 
        }

        /// <inheritdoc />
        public GeneralStateHistory CreateStateHistory(IHaveId entity, object oldValue, object newValue, string propertyName = null)
        {
            var info = this.GetStateHistoryInfo(entity.GetType(), propertyName);
            return this.CreateStateHistory(entity, info, oldValue, newValue);
        }

        /// <inheritdoc />
        public GeneralStateHistory CreateStateHistory(IHaveId entity, GeneralStatefulEntityInfo info, object oldValue, object newValue)
        {
            ArgumentChecker.NotNull(info, "info");

            var user = this.userManager.GetActiveUser();
            string oldValueString,
                newValueString;

            if (info.PropertyInfo.PropertyType.IsEnum)
            {
                oldValueString = this.GetEnumValue(oldValue);
                newValueString = this.GetEnumValue(newValue);
            }
            else
            {
                oldValueString = oldValue.ToStr();
                newValueString = newValue.ToStr();
            }

            return new GeneralStateHistory(entity.Id, entity.GetType().FullName, info.Code, oldValueString, newValueString, user);
        }

        /// <inheritdoc />
        public string GetDisplayValue(GeneralStatefulEntityInfo info, string value)
        {
            return info.ValueFormatter(value);
        }

        private string GetEnumValue(object value)
        {
            if (value != null)
            {
                return ((int)value).ToString();
            }

            return string.Empty;
        }
    }
}