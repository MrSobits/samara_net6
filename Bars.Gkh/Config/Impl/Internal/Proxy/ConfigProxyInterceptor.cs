namespace Bars.Gkh.Config.Impl.Internal.Proxy
{
    using System;
    using System.Collections.Generic;

    using Castle.DynamicProxy;

    /// <summary>
    ///     Обработчик обращения к полям обернутых экземпляров конфигурационных секций
    /// </summary>
    internal class ConfigProxyInterceptor : IInterceptor
    {
        private readonly string currentRoute;

        private readonly IDictionary<string, ValueHolder> valueHolders;

        public string CurrentRoute
        {
            get
            {
                return currentRoute;
            }
        }

        public ConfigProxyInterceptor(IDictionary<string, ValueHolder> valueHolders, string currentRoute)
        {
            this.currentRoute = currentRoute ?? string.Empty;
            this.valueHolders = valueHolders;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            if (methodName[0] == 's')
            {
                var name = this.currentRoute + '.' + methodName.Substring(4);
                if (!this.valueHolders.ContainsKey(name))
                {
                    return;
                }

                var holder = this.valueHolders[name];
                if (invocation.Arguments[0] is IGkhConfigSection)
                {
                    throw new Exception("Нельзя перезаписывать конфигурационные секции напрямую");
                }

                holder.SetValue(invocation.Arguments[0]);
            }
            else
            {
                var name = this.currentRoute + '.' + methodName.Substring(4);
                var type = invocation.Method.ReturnType;
                ValueHolder holder;
                if (this.valueHolders.TryGetValue(name, out holder))
                {
                    invocation.ReturnValue = typeof(IGkhConfigSection).IsAssignableFrom(type)
                                                 ? ConfigProxyGenerator.Generate(type, this.valueHolders, name)
                                                 : holder.Value;
                }
            }
        }
    }
}