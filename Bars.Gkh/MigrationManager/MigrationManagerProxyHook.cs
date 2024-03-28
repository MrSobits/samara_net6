namespace Bars.Gkh.MigrationManager
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Castle.DynamicProxy;

    /// <summary>
    /// Обработчик вызова метода
    /// </summary>
    public class MigrationManagerProxyHook : IProxyGenerationHook
    {
        private readonly string[] methods =
        {
            "MigrateToLastVersion",
            "MigrateToVersion",
            "ApplySingleMigration"
        };

        /// <inheritdoc />
        public void MethodsInspected()
        {
        }

        /// <inheritdoc />
        public void NonProxyableMemberNotification(Type type, MemberInfo memberInfo)
        {
        }

        /// <inheritdoc />
        public bool ShouldInterceptMethod(Type type, MethodInfo methodInfo)
        {
            return this.methods.Contains(methodInfo.Name);
        }
    }
}