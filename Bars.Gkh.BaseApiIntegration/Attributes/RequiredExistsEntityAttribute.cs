namespace Bars.Gkh.BaseApiIntegration.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    /// <summary>
    /// Required-атрибут с проверкой наличия
    /// сущности(-ей) с переданным(-и) идентификатором(-ами)
    /// </summary>
    /// <remarks>
    /// Required + проверка переданного(-ых) идентификатора(-ов)
    /// </remarks>
    public class RequiredExistsEntityAttribute : RequiredAttribute
    {
        /// <summary>
        /// Тип сервиса сущности
        /// </summary>
        private Type ServiceType { get; }

        public RequiredExistsEntityAttribute(Type entityType)
        {
            this.ServiceType = typeof(IDomainService<>).MakeGenericType(entityType);
        }

        /// <inheritdoc />
        public override bool IsValid(object objValue)
        {
            // Корректность заполнения свойства
            var result = this.RequiredCheck(objValue);

            // Доплнительно наложить проверку наличия сущности при правильном заполнении
            return result ? this.ConvertValueAndExistsCheck(objValue) : result;
        }

        /// <summary>
        /// Проверить заполненность свойства
        /// </summary>
        /// <param name="objValue">Значение свойства</param>
        protected bool RequiredCheck(object objValue) => base.IsValid(objValue);

        /// <summary>
        /// Привести значение и проверить наличие сущности
        /// </summary>
        /// <param name="objValue">Значение свойства</param>
        protected bool ConvertValueAndExistsCheck(object objValue)
        {
            var values = objValue as List<long>;
            var value = objValue as long? ?? 0;

            values = values ?? new List<long> { value };

            if (values.Any(x => x == 0))
                return false;

            return this.AllEntitiesExists(values);
        }

        /// <summary>
        /// Проверить - существуют ли все сущности с переданными идентификаторами
        /// </summary>
        /// <param name="ids">Идентификаторы сущностей для проверки</param>
        private bool AllEntitiesExists(IEnumerable<long> ids)
        {
            var container = ApplicationContext.Current.Container;
            var domainService = container.Resolve(this.ServiceType) as IDomainService;

            using (container.Using(domainService))
            {
                return ((IQueryable<PersistentObject>)domainService.GetAll())
                    .Count(x => ids.Contains(x.Id)) == ids.Count();
            }
        }
    }
}