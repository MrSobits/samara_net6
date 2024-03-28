namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерцептор для сущности <see cref="PersAccGroup"/>
    /// </summary>
    public class PersAccGroupInterceptor :EmptyDomainInterceptor<PersAccGroup>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<PersAccGroup> service, PersAccGroup entity)
        {
            return this.ValidateEntity(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PersAccGroup> service, PersAccGroup entity)
        {
            return this.ValidateEntity(service, entity);
        }

        private IDataResult ValidateEntity(IDomainService<PersAccGroup> service, PersAccGroup entity)
        {
            if (entity.Name.IsEmpty())
            {
                return this.Failure("Не заполнено обязательное поле: Наименование группы");
            }

            if (entity.Name.Length > 100)
            {
                return this.Failure("Количество знаков в поле Наименование не должно превышать 100 символов");
            }

            var anyGroupWithSameName = service.GetAll().Any(x => x.Id != entity.Id && x.Name == entity.Name);
            if (anyGroupWithSameName)
            {
                return this.Failure("Группа с указанным наименованием уже существует - " + entity.Name);
            }

            if (entity.IsSystem == 0)
            {
                entity.IsSystem = YesNo.No;
            }

            return this.Success();
        }
    }
}
