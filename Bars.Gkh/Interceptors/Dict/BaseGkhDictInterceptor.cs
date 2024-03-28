namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Интерцепторы для сущностей словарей
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseGkhDictInterceptor<T> : EmptyDomainInterceptor<T> where T : BaseGkhDict
    {
        /// <summary>
        /// Имя сущности
        /// </summary>
        protected virtual string EntityName { get; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var result = this.Validate(service, entity);

            if (!result.Success)
            {
                return result;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var result = this.Validate(service, entity);

            if (!result.Success)
            {
                return result;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Проверка корректности
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность словаря</param>
        /// <returns></returns>
        protected IDataResult Validate(IDomainService<T> service, T entity)
        {
            if (entity.Code.IsEmpty())
            {
                return this.Failure("Необходимо заполнить Код");
            }

            if (entity.Name.IsEmpty())
            {
                return this.Failure("Необходимо заполнить Наименование");
            }

            if (service.GetAll().Any(x => x.Code == entity.Code && x.Id != entity.Id))
            {
                return this.Failure($"{this.EntityName} с таким кодом уже существует");
            }

            if (service.GetAll().Any(x => x.Name.Trim(' ') == entity.Name.Trim(' ') && x.Id != entity.Id))
            {
                return this.Failure($"{this.EntityName} с таким наименованием уже существует");
            }

            return this.Success();
        }
    }
}