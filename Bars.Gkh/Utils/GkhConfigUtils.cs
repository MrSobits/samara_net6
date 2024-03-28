namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Impl.Internal.Proxy;

    using Castle.DynamicProxy;

    public static class GkhConfigUtils
    {
        /// <summary>
        ///     Позволяет узнать полный ключ объекта настроек.<br />
        ///     ВАЖНО: объект настроек должен быть получен от провайдера
        ///     конфигурации, а не создан вручную
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Объект настроек</param>
        /// <returns>Полный ключ параметра или null если что-то пошло не так</returns>
        public static string GetConfigKey<T>(this T obj) where T : IGkhConfigSection
        {
            // если решарпер говорит что все с этим кодом плохо и он не работает -
            // не верьте. он глупый, а я умный
            var targetAccessor = obj as IProxyTargetAccessor;
            if (targetAccessor == null)
            {
                return null;
            }

            var interceptor = targetAccessor.GetInterceptors().OfType<ConfigProxyInterceptor>().FirstOrDefault();
            if (interceptor == null)
            {
                return null;
            }

            return interceptor.CurrentRoute;
        }

        /// <summary>
        ///     Позволяет узнать полный ключ параметра конфигурации
        ///     по заполненному объекту настроек.<br />
        ///     ВАЖНО: объект настроек должен быть получен от провайдера
        ///     конфигурации, а не создан вручную
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProp"></typeparam>
        /// <param name="obj">Объект настроек</param>
        /// <param name="selector">Селектор интересующего параметра</param>
        /// <returns>Полный ключ параметра или null если что-то пошло не так</returns>
        public static string GetConfigPropertyKey<T, TProp>(this T obj, Expression<Func<T, TProp>> selector)
            where T : IGkhConfigSection
        {
            var currentRoute = obj.GetConfigKey();
            if (currentRoute.IsEmpty())
            {
                return null;
            }

            var memberName = selector.MemberName();
            if (memberName.IsEmpty())
            {
                return null;
            }

            return string.Format("{0}.{1}", currentRoute, memberName);
        }

        /// <summary>Получить атрибут секции</summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        public static T GetAttribute<T>(this IGkhConfigSection value) where T : Attribute
        {
            return value.GetType().BaseType.GetCustomAttributes(typeof(T), false)
                .FirstOrDefault() as T;
        }

        /// <summary>
        /// Получить значение атрибута <see cref="DisplayNameAttribute"/>
        /// </summary>
        public static string GetDisplayName(this IGkhConfigSection value)
        {
            var attribute = value.GetAttribute<DisplayNameAttribute>();
            return attribute == null ? (value.GetConfigKey() ?? value.ToString()) : attribute.DisplayName;
        }
    }
}