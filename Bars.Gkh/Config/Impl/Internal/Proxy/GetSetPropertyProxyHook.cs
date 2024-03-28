namespace Bars.Gkh.Config.Impl.Internal.Proxy
{
    using System;
    using System.Reflection;

    using Castle.DynamicProxy;

    /// <summary>
    /// Хук для генерации оберток.
    /// Оборачиваются только геттеры/сеттеры virtual properties
    /// </summary>
    public class GetSetPropertyProxyHook : IProxyGenerationHook
    {
        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == this.GetType();
        }

        public override int GetHashCode()
        {
            return this.GetType().GetHashCode();
        }

        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return methodInfo.IsSpecialName && (methodInfo.Name.StartsWith("get_", StringComparison.Ordinal) || methodInfo.Name.StartsWith("set_", StringComparison.Ordinal));
        }
    }
}