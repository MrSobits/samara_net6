namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Qa.Utils;

    using NHibernate;

    internal class ReflectionHelper : BindingBase
    {
        public static object GetPropertyValue(object instance, string propertyName)
        {
            Type entityType = instance.GetType();

            var property = entityType.GetProperty(propertyName);

            if (property == null)
            {
                throw new NullReferenceException(string.Format("Отсутсвует свойство с наименованием {0}", propertyName));
            }

            return property.GetValue(instance);
        }

        public static T GetPropertyValue<T>(object instance, string propertyName)
        {
            Type entityType = instance.GetType();

            var property = entityType.GetProperty(propertyName);

            if (property == null)
            {
                throw new NullReferenceException(string.Format("Отсутсвует свойство с наименованием {0}", propertyName));
            }

            var propertyValue = property.GetValue(instance);

            if (propertyValue.GetType() != typeof(T) && !(propertyValue is T))
            {
                throw new TypeMismatchException(
                    string.Format("объект типа {0} нельзя привести к типу {1}", propertyValue.GetType(), typeof(T)));
            }

            return (T)propertyValue;
        }

        public static void SetPropertyValue(object instance, string property, object value)
        {
            Type t = instance.GetType();

            PropertyInfo prop = t.GetProperty(property);

            if (prop != null)
            {
                prop.SetValue(instance, value);
            }
        }

        public static void ChangeState(object instance, string stateName, bool validate = true)
        {
            var dsState = Container.Resolve<IDomainService<State>>();
            var stateProvider = Container.Resolve<IStateProvider>();
            var instanceInfo = stateProvider.GetStatefulEntityInfo(instance.GetType());
            
            var requiredState = dsState.GetAll()
                .FirstOrDefault(x => x.TypeId == instanceInfo.TypeId && x.Name == stateName);

            if (requiredState == null)
            {
                throw new StateException(
                    string.Format("Нет статуса у сущности {0} с наименованием {1}", instanceInfo.Name, stateName));
            }

            var instanceId = (long)GetPropertyValue(instance, "Id");

            stateProvider.ChangeState(instanceId, instanceInfo.TypeId, requiredState, string.Empty, validate);
        }
    }
}
