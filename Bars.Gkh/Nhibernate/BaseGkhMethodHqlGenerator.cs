namespace Bars.Gkh.Nhibernate
{
    using System.Collections.Generic;
    using System.Reflection;

    using NHibernate.Linq.Functions;

    /// <summary>
    /// Базовая реализация генератора методов HQL
    /// </summary>
    public abstract class BaseGkhMethodHqlGenerator : BaseHqlGeneratorForMethod, IGkhMethodHqlGenerator
    {
        private ICollection<MethodInfo> allowedMethods;

        /// <summary>
        /// Коллекция поддерживаемых методов
        /// </summary>
        public ICollection<MethodInfo> AllowedMethods
        {
            get
            {
                return this.allowedMethods;
            }
            protected set
            {
                this.allowedMethods = value;
                this.SupportedMethods = value;
            }
        }

        /// <inheritdoc />
        public bool SupportsMethod(MethodInfo method)
        {
            return this.AllowedMethods.Contains(method);
        }

        /// <inheritdoc />
        public IHqlGeneratorForMethod GetMethodGenerator(MethodInfo method)
        {
            return this;
        }
    }
}