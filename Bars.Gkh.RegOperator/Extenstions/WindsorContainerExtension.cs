namespace Bars.Gkh.RegOperator.Extenstions
{
    using System;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Distribution.Validators;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    /// <summary>
    /// Расширения контейнера для модуля RegOperator
    /// </summary>
    public static class WindsorContainerExtension
    {
        /// <summary>
        /// Зарегистрировать распределение
        /// </summary>
        /// <typeparam name="TDistribution">Реализация распределения</typeparam>
        /// <param name="container">Контейнер</param>
        /// <param name="code">Код (опционально, иначе берётся из <see cref="DistributionCode"/>)</param>
        /// <param name="registerDefaultValidators">Зарегистрировать валидаторы по умолчанию 
        /// (напр. для <see cref="AbstractPersonalAccountDistribution"/> или <see cref="AbstractRealtyAccountDistribution"/>)</param>
        public static void RegisterDistribution<TDistribution>(this IWindsorContainer container, string code = null, bool registerDefaultValidators = true) 
            where TDistribution : IDistribution
        {
            var type = typeof(TDistribution);
            if (code.IsEmpty())
            {
                DistributionCode parsedCode;
                if (!Enum.TryParse(type.Name, out parsedCode))
                {
                    throw new InvalidOperationException("Не удалось определить код распределения по его типу");
                }

                if (parsedCode.GetAttribute<DisplayAttribute>().IsNull())
                {
                    throw new InvalidOperationException($"Отсутствует наименование распределения с кодом {parsedCode.ToString()}");
                }
                
                code = parsedCode.ToString();
            }

            if (registerDefaultValidators && typeof(AbstractPersonalAccountDistribution).IsAssignableFrom(type))
            {
                container.RegisterValidatorsForPersonalAccountDistributions<TDistribution>();
            }

            if (registerDefaultValidators && typeof(AbstractRealtyAccountDistribution).IsAssignableFrom(type))
            {
                container.RegisterValidatorsForRealityAccountDistributions<TDistribution>();
            }

            container.RegisterTransient<IDistribution, TDistribution>(code);
        }

        /// <summary>
        /// Зарегистрировать тип источника для квитанций
        /// </summary>
        /// <typeparam name="TBuilder">Источник</typeparam>
        /// <param name="container">Контейнер</param>
        /// <param name="ownerType">Тип собственника</param>
        /// <param name="code">Код</param>
        public static void RegisterDocumentSource<TBuilder>(this IWindsorContainer container, PaymentDocumentType ownerType, string code)
            where TBuilder : ISnapshotBuilder
        {
            var keyPrefix = ownerType + ".";
            Component
                .For<IBuilderInfo>()
                .Forward<ISnapshotBuilder>()
                .ImplementedBy<TBuilder>()
                .LifeStyle.Scoped()
                .Named(keyPrefix + code)
                .RegisterIn(container);
        }

        private static void RegisterValidatorsForRealityAccountDistributions<TDistribution>(this IWindsorContainer container)
            where TDistribution : IDistribution
        {
            var type = typeof(TDistribution);

            Component.For<IDistributionValidator>()
                .ImplementedBy(typeof(RealityAccountDistributionPaymentAccountValidator<>).MakeGenericType(type))
                .LifestyleTransient()
                .RegisterIn(container);
        }

        private static void RegisterValidatorsForPersonalAccountDistributions<TDistribution>(this IWindsorContainer container)
            where TDistribution : IDistribution
        {
            var type = typeof(TDistribution);

            Component.For<IDistributionValidator>()
                .ImplementedBy(typeof(PersonalAccountDistributionPaymentAccountValidator<>).MakeGenericType(type))
                .LifestyleTransient()
                .RegisterIn(container);
        }
    }
}
