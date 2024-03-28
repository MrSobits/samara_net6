namespace Bars.Gkh.Reforma.Utils.Validation
{
    using System;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Методы валидации полей интеграции с Реформой ЖКХ
    /// </summary>
    internal static class ReformaValidator
    {
        private static readonly IMetaDescriptionContainer container = new MetaDescriptionContainer();
        private const string DefaultMessage = "Не заполнено обязательное поле";

        /// <summary>
        /// Инициализировать мета-описание классов
        /// </summary>
        public static void Init()
        {
            TypeMetaBuilder.For<EmergencyObject>().WithDescription("Аварийность жилого дома").HasProperty(x => x.ReasonInexpedient).Add();
            TypeMetaBuilder.For<ReasonInexpedient>().WithDescription("Основание нецелесообразности").HasProperty(x => x.Name, "Наименование").Add();
            TypeMetaBuilder.For<TariffForConsumers>().WithDescription("Тарифы для потребителей").HasProperty(x => x.Cost, "Стоимость тарифа").Add();
            TypeMetaBuilder.For<WorkCapRepair>().WithDescription("Работа услуги капремонта").HasProperty(x => x.PlannedCost, "Запланированная сумма").Add();
            TypeMetaBuilder.For<RepairService>().WithDescription("Услуга ремонта")
                .HasProperty(x => x.SumWorkTo, "Плановая сумма")
                .HasProperty(x => x.SumFact, "Фактическая стоимость")
                .Add();

            TypeMetaBuilder.For<CostItem>().WithDescription("Статьи затрат").HasProperty(x => x.Sum, "Сумма").Add();
        }

        /// <summary>
        /// Зарегистрировать описание
        /// </summary>
        /// <typeparam name="T"> Типизация построителя </typeparam>
        /// <param name="typeBuilder"> Построитель  </param>
        /// <exception cref="ReformaValidationException">Если не будет пройдена проверка, выбрасится исключение с указанием пути ошибки</exception>
        public static void Add<T>(this TypeMetaBuilder<T> typeBuilder) where T : IEntity
        {
            typeBuilder.RegisteredIn(ReformaValidator.container);
        }

        /// <summary>
        /// Метод проверяет объект на заданность значения, по цепочке проверяя каждый объект
        /// </summary>
        /// <typeparam name="TValue">Тип объекта</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="value">Объект</param>
        /// <param name="propertySelector">Селектор до строкового свойства</param>
        /// <param name="errorMessage">Текст ошибки</param>
        /// <exception cref="ReformaValidationException">Если не будет пройдена проверка, выбрасится исключение с указанием пути ошибки</exception>
        public static void NotNull<TValue, TProperty>(this TValue value, Expression<Func<TValue, TProperty>> propertySelector, string errorMessage = ReformaValidator.DefaultMessage) 
            where TValue : class, IEntity
            where TProperty : class
        {
            value.Validate(propertySelector, x => x.IsNotNull(), errorMessage);
        }

        /// <summary>
        /// Метод проверяет строку на непустое значение, по цепочке проверяя каждый объект
        /// </summary>
        /// <typeparam name="TValue">Тип объекта</typeparam>
        /// <param name="value">Объект</param>
        /// <param name="propertySelector">Селектор до строкового свойства</param>
        /// <param name="errorMessage">Текст ошибки</param>
        /// <exception cref="ReformaValidationException">Если не будет пройдена проверка, выбрасится исключение с указанием пути ошибки</exception>
        public static void NotNullOrEmpty<TValue>(this TValue value, Expression<Func<TValue, string>> propertySelector, string errorMessage = ReformaValidator.DefaultMessage)
            where TValue : class, IEntity
        {
            value.Validate(propertySelector, x => x.IsNotEmpty(), errorMessage);
        }

        /// <summary>
        /// Метод проверяет <see cref="Nullable"/> на непустое значение, по цепочке проверяя каждый объект
        /// </summary>
        /// <typeparam name="TValue">Тип объекта</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="value">Объект</param>
        /// <param name="propertySelector">Селектор до строкового свойства</param>
        /// <param name="errorMessage">Текст ошибки</param>
        /// <exception cref="ReformaValidationException">Если не будет пройдена проверка, выбрасится исключение с указанием пути ошибки</exception>
        public static void HasValue<TValue, TProperty>(this TValue value, Expression<Func<TValue, TProperty?>> propertySelector, string errorMessage = ReformaValidator.DefaultMessage)
            where TValue : class, IEntity
            where TProperty : struct
        {
            value.Validate(propertySelector, x => x.HasValue, errorMessage);
        }

        /// <summary>
        /// Метод проверяет объект на заданность значения, по цепочке проверяя каждый объект
        /// </summary>
        /// <typeparam name="TValue">Тип объекта</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="value">Объект</param>
        /// <param name="propertySelector">Селектор до строкового свойства</param>
        /// <param name="isValidFunc">Метод валидации конечного значения</param>
        /// <param name="errorMessage">Текст ошибки</param>
        /// <exception cref="ReformaValidationException">Если не будет пройдена проверка, выбрасится исключение с указанием пути ошибки</exception>
        public static void Validate<TValue, TProperty>(
            this TValue value,
            Expression<Func<TValue, TProperty>> propertySelector,
            Func<TProperty, bool> isValidFunc,
            string errorMessage = ReformaValidator.DefaultMessage)
            where TValue : class, IEntity
        {
            ArgumentChecker.NotNull(propertySelector, nameof(propertySelector));
            ArgumentChecker.NotNull(isValidFunc, nameof(isValidFunc));

            if (errorMessage.Trim().EndsWith(":"))
            {
                errorMessage = errorMessage.Replace(":", string.Empty).Trim();
            }

            var result = value.GetResultAndPath(propertySelector);

            if (!result.Success || !isValidFunc((TProperty)result.Data))
            {
                throw new ReformaValidationException($"{errorMessage}: {result.Path}");
            }
        }

        private static ExpressionSelectorVisitor.TreeVisitResult GetResultAndPath<TValue, TProperty>(
            this TValue value, Expression<Func<TValue, TProperty>> propertySelector) where TValue : class, IEntity
        {
            return new ExpressionSelectorVisitor(ReformaValidator.container).GetResult(value, propertySelector);
        }
    }
}