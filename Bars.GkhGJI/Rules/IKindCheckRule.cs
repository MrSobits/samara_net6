namespace Bars.GkhGji.Rules
{
    using Enums;
    using Entities;

    using Castle.Windsor;

    public interface IKindCheckRule
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Приоритет правила (0 - наибольший)
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Код правила
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Наименование правила
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Дефолтное значение
        /// </summary>
        TypeCheck DefaultCode { get; }

        /// <summary>
        /// Проверка соответствия правилу
        /// </summary>
        /// <param name="entity">Распоряжение</param>
        /// <returns></returns>
        bool Validate(Disposal entity);
    }
}