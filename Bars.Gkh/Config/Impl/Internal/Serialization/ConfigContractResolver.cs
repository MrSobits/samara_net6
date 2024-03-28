namespace Bars.Gkh.Config.Impl.Internal.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Castle.DynamicProxy;

    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// При сериализации в JSON приводит конфигурационные секции к базовому типу,
    /// исключая из сохранения служебные поля
    /// </summary>
    internal class ConfigContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return
                base.GetSerializableMembers(
                    typeof(IProxyTargetAccessor).IsAssignableFrom(objectType) ? objectType.BaseType : objectType);
        }
    }
}