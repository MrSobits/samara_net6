namespace Bars.Gkh.Interceptors.Dict
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using System.Linq;
    using Bars.B4.Utils;

    /// <summary>
    /// Интерцептор для сущности <see cref="BuilderDocumentType"/>
    /// </summary>
    public class BuilderDocumentTypeInterceptor : EmptyDomainInterceptor<BuilderDocumentType>
    {
        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<BuilderDocumentType> service, BuilderDocumentType entity)
        {
            return ValidateEntity(service, entity, ServiceOperationType.Save);
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<BuilderDocumentType> service, BuilderDocumentType entity)
        {
            return ValidateEntity(service, entity, ServiceOperationType.Update);
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Протокол"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<BuilderDocumentType> service, BuilderDocumentType entity)
        {
            var builderDocumentDomain =  this.Container.ResolveDomain<BuilderDocument>();

            using (this.Container.Using(builderDocumentDomain))
            {
                if (builderDocumentDomain.GetAll().Any(x => x.BuilderDocumentType.Id == entity.Id))
                {
                    return Failure("Невозможно удалить выбранный тип, поскольку имеются документы указанного типа");
                }
            }

            return Success();
        }

        private IDataResult ValidateEntity(IDomainService<BuilderDocumentType> service, BuilderDocumentType entity, ServiceOperationType type)
        {
            var emptyProps = new List<string>();
            if (entity.Name.IsEmpty())
            {
                emptyProps.Add("Наименование");
            }

            if (entity.Code == -1)
            {
                emptyProps.Add("Код");
            }

            if (emptyProps.Any())
            {
                return Failure(string.Format("Не заполнены обязательные поля: {0}", string.Join(", ", emptyProps)));
            }

            if (entity.Name.Length > 250)
            {
                Failure("Макимальная длина поля Наименование превышает 250 символов");
            }

            var existItemsByCode = service.GetAll()
                .WhereIf(type == ServiceOperationType.Update, x => x.Id != entity.Id)
                .Any(x => x.Code == entity.Code);

            if (existItemsByCode)
            {
                return Failure("Тип документа с указанным кодом уже существует");
            }

            var existItemsByName = service.GetAll()
                .WhereIf(type == ServiceOperationType.Update, x => x.Id != entity.Id)
                .Any(x => x.Name == entity.Name);

            if (existItemsByName)
            {
                return Failure("Тип документа с указанным наименованием уже существует");
            }

            return Success();
        }
    }
}
