namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using B4.Utils.Annotations;

    /// <summary>
    ///     Провайдер атрибутов
    /// </summary>
    public class AttributeProvider : ICustomAttributeProvider
    {
        private readonly List<ICustomAttributeProvider> providers;

        /// <summary>
        /// Конструктор
        /// </summary>
        public AttributeProvider()
        {
            providers = new List<ICustomAttributeProvider>();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="p"></param>
        public AttributeProvider(ICustomAttributeProvider[] p)
        {
            providers = new List<ICustomAttributeProvider>(p);
        }

        /// <summary>
        /// Добавление провайдера в коллекцию
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public AttributeProvider Append(ICustomAttributeProvider provider)
        {
            ArgumentChecker.NotNull(provider, "provider");
            ArgumentChecker.NotInCollection(providers, provider, "provider");

            providers.Add(provider);

            return this;
        }

        /// <summary>
        /// Удаление провайдера из коллекции
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public AttributeProvider Remove(ICustomAttributeProvider provider)
        {
            ArgumentChecker.NotNull(provider, "provider");

            providers.Remove(provider);

            return this;
        }

        #region ICustomAttributeProvider implementation

        object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit)
        {
            return providers.SelectMany(x => x.GetCustomAttributes(attributeType, inherit)).ToArray();
        }

        object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit)
        {
            return providers.SelectMany(x => x.GetCustomAttributes(inherit)).ToArray();
        }

        bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit)
        {
            return providers.Any(x => x.IsDefined(attributeType, inherit));
        }

        #endregion
    }
}