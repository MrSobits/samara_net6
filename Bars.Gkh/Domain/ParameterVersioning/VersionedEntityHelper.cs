namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.Application;
    using B4.Utils;
    using FastMember;

    /// <summary>
    /// Вспомогательный класс для версионируемых сущностей
    /// </summary>
    public class VersionedEntityHelper
    {
        static VersionedEntityHelper()
        {
            VersionedEntityHelper.versionedEntities = ApplicationContext.Current.Container.ResolveAll<IVersionedEntity>().ToList();
            VersionedEntityHelper.set = new HashSet<ParameterEntity>(VersionedEntityHelper.versionedEntities.SelectMany(x => x.GetParameterMap()));
            VersionedEntityHelper.propertyBag = new ConcurrentDictionary<Type, HashSet<PropertyInfo>>();
            VersionedEntityHelper.SetEntityName = new HashSet<string>(VersionedEntityHelper.set.Select(x => x.ClassName));

            VersionedEntityHelper.PopulateFriendlyNamesAndRenderers();
        }

        private static void PopulateFriendlyNamesAndRenderers()
        {
            VersionedEntityHelper.set.ForEach(x =>
            {
                if (VersionedEntityHelper.friendlyNames.ContainsKey(x.ParameterName))
                {
                    if (x.FriendlyName.IsNotEmpty())
                    {
                        VersionedEntityHelper.friendlyNames[x.ParameterName] = x.FriendlyName;
                    }
                }
                else
                {
                    VersionedEntityHelper.friendlyNames.Add(x.ParameterName, x.FriendlyName);
                }

                VersionedEntityHelper.renderers[x.ParameterName] = x.Renderer;
            });
        }

        /// <summary>
        /// Получить наименование параметра
        /// </summary>
        /// <param name="className">Класс сущности</param>
        /// <param name="propertyName">Наименование свойства</param>
        /// <param name="parameter">Имя параметра версионирования</param>
        /// <returns>true, если есть параметр для свойства сущности</returns>
        public static bool TryGetParameterName(string className, string propertyName, out string parameter)
        {
            var param =
                VersionedEntityHelper.set.FirstOrDefault(
                    x => x.ClassName.EqualsIgnoreCase(className) && x.PropertyName.EqualsIgnoreCase(propertyName));

            parameter = string.Empty;

            if (param == null)
            {
                return false;
            }

            parameter = param.ParameterName;

            return true;
        }

        /// <summary>
        /// Является ли сущность версионируемой
        /// </summary>
        /// <param name="obj">Объект</param>
        public static bool IsUnderVersioning(object obj)
        {
            return VersionedEntityHelper.SetEntityName.Contains(obj.GetType().Name)
                   && VersionedEntityHelper.set.Any(x => x.ClassName.EqualsIgnoreCase(obj.GetType().Name));
        }

        /// <summary>
        /// Получить провайдер версионируемых свойств
        /// </summary>
        /// <param name="obj">Объект, свойства которого версионируются</param>
        public static ParameterCreator GetCreator(object obj)
        {
            return new ParameterCreator(obj, VersionedEntityHelper.set.Where(x => x.ClassName.EqualsIgnoreCase(obj.GetType().Name)).ToList());
        }

        /// <summary>
        /// Получить тип сущности по классу
        /// </summary>
        /// <param name="className">Наименование класса</param>
        /// <returns><see cref="System.Type"/></returns>
        public static Type GetTypeByClassName(string className)
        {
            var parameter = VersionedEntityHelper.set.FirstOrDefault(x => x.ClassName.EqualsIgnoreCase(className));

            return parameter.Return(x => x.Type);
        }

        /// <summary>
        /// Фасад валидации изменения параметра
        /// </summary>
        /// <param name="entity">Экземпляр сущности БД, чье свойство изменяется</param>
        /// <param name="value">Новое значение свойства</param>
        /// <param name="factDate">Дата фактического изменения</param>
        /// <param name="parameterName">Имя версионируемого параметра</param>
        /// <returns><see cref="IDataResult"/></returns>
        public static IDataResult Validate(object entity, object value, DateTime factDate, string parameterName)
        {
            var versioned = VersionedEntityHelper.GetMap(parameterName);

            IDataResult allResults = new BaseDataResult();
            var errors = new StringBuilder();

            foreach (var versionedEntity in versioned)
            {
                var result = versionedEntity.Validate(entity, value, factDate, parameterName);

                if (!result.Success)
                {
                    allResults.Success = false;
                    errors.Append(result.Message).Append("<br>");
                }
            }

            allResults.Message = errors.ToString();

            return allResults;
        }
        
        /// <summary>
        /// Получение понятного имени параметра
        /// </summary>
        /// <param name="parameterName">имя параметра</param>
        /// <returns>Понятное имя</returns>
        public static string GetFriendlyName(string parameterName)
        {
            string friendlyName;

            if(!VersionedEntityHelper.friendlyNames.TryGetValue(parameterName, out friendlyName))
            {
                friendlyName = parameterName;
            }

            return friendlyName;
        }

        /// <summary>
        /// Получение человекопонятного значения
        /// </summary>
        /// <param name="parameterName">имя параметра</param>
        /// <param name="value">значение</param>
        /// <returns>человекопонятное значение</returns>
        public static string RenderValue(string parameterName, string value)
        {
            Func<string, string> renderer;
            return VersionedEntityHelper.renderers.TryGetValue(parameterName, out renderer) ? renderer(value) : value;
        }

        /// <summary>
        /// Нужно ли пропускать параметр при версионировании
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static bool ShouldSkip(object entity, string parameterName)
        {
            var maps = VersionedEntityHelper.GetMap(parameterName);

            return maps.Any(x => x.SkipProperty(entity, parameterName));
        }

        private static IEnumerable<IVersionedEntity> GetMap(string parameterName)
        {
            var versioned =
                VersionedEntityHelper.versionedEntities.Where(x => x.GetParameterMap().Any(y => y.ParameterName == parameterName)).ToList();
            return versioned;
        }

        private static readonly HashSet<string> SetEntityName;
        private static readonly HashSet<ParameterEntity> set;
        private static readonly ConcurrentDictionary<Type, HashSet<PropertyInfo>> propertyBag;
        private static readonly List<IVersionedEntity> versionedEntities;
        private static readonly Dictionary<string, string> friendlyNames = new Dictionary<string, string>();
        private static readonly Dictionary<string, Func<string, string>> renderers = new Dictionary<string, Func<string, string>>();

        public class ParameterCreator
        {
            private readonly object entity;
            private readonly IEnumerable<ParameterEntity> versioned;
            private readonly ObjectAccessor accessor;

            public ParameterCreator(object entity, IEnumerable<ParameterEntity> versioned)
            {
                this.accessor = ObjectAccessor.Create(entity);
                this.entity = entity;
                this.versioned = versioned;
            }

            public IEnumerable<ParameterEntity> CreateParameters()
            {
                var type = this.entity.GetType();

                HashSet<PropertyInfo> properties;

                if (!VersionedEntityHelper.propertyBag.TryGetValue(type, out properties))
                {
                    var props = type.GetProperties();

                    properties = new HashSet<PropertyInfo>(props);

                    VersionedEntityHelper.propertyBag.TryAdd(type, properties);
                }
                else
                {
                    properties = VersionedEntityHelper.propertyBag[type];
                }

                foreach (var property in properties)
                {
                    var parameter =
                        this.versioned.FirstOrDefault(x => x.PropertyName.EqualsIgnoreCase(property.Name));

                    if (parameter != null)
                    {
                        parameter.PropertyValue = this.accessor[property.Name].ToStr();
                    }
                }

                return this.versioned;
            }
        }
    }
}