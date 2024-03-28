namespace Bars.Gkh.RegOperator.Interceptors.PersonalAccount
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using Authentification;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Castle.Windsor;
    using Gkh.Entities;

    /// <summary>
    /// Интерцептор для категорий льгот абонента
    /// </summary>
    public class PersonalAccountPrivilegedCategoryInterceptor : EmptyDomainInterceptor<PersonalAccountPrivilegedCategory>
    {
        /// <summary>
        /// Домен сервис для легковесной сущности для хранения изменения сущности
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сессия
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Действие, выполняемое до создания записи
        /// </summary>
        /// <param name="service">Домен сервис <see cref="PersonalAccountPrivilegedCategory"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<PersonalAccountPrivilegedCategory> service, PersonalAccountPrivilegedCategory entity)
        {
            return this.Check(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое до изменения записи
        /// </summary>
        /// <param name="service">Домен сервис <see cref="PersonalAccountPrivilegedCategory"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PersonalAccountPrivilegedCategory> service, PersonalAccountPrivilegedCategory entity)
        {
           return this.Check(service, entity);
        }

        private IDataResult Check(IDomainService<PersonalAccountPrivilegedCategory> service, PersonalAccountPrivilegedCategory entity)
        {
            var emptyFields = this.GetEmptyFields(ref entity);
            if (!string.IsNullOrEmpty(emptyFields))
            {
                return this.Failure(string.Format("Не заполнены обязательные поля: {0}", emptyFields));
            }

            if (entity.PrivilegedCategory.DateFrom > DateTime.Now)
            {
                return this.Failure("Невозможно сохранить категорию будущего периода");
            }

            if (
                service.GetAll()
                    .Any(
                        x =>
                        x.Id != entity.Id && x.PersonalAccount.Id == entity.PersonalAccount.Id
                        && ((!x.DateTo.HasValue && (entity.DateFrom >= x.DateFrom || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom)))
                            || (x.DateTo.HasValue
                                && ((entity.DateFrom >= x.DateFrom && entity.DateFrom <= x.DateTo) || (entity.DateTo.HasValue && entity.DateTo >= x.DateFrom && entity.DateTo <= x.DateTo)))
                            || (!entity.DateTo.HasValue && x.DateFrom >= entity.DateFrom))))
            {
                return this.Failure("Уже добавлена льготная категория с пересекающимися датами действия");
            }

            return this.Success();
        }

        private string GetEmptyFields(ref PersonalAccountPrivilegedCategory entity)
        {
            List<string> fieldList = new List<string>();
            if (entity.PrivilegedCategory == null)
            {
                fieldList.Add("Льготная категория");
            }
            if (entity.PersonalAccount == null)
            {
                fieldList.Add("Лицевой счёт");
            }

            return string.Join(", ", fieldList.ToArray());
        }

        /// <summary>
        /// Действие, выполняемое после изменения записи
        /// </summary>
        /// <param name="service">Домен сервис <see cref="PersonalAccountPrivilegedCategory"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterCreateAction(IDomainService<PersonalAccountPrivilegedCategory> service, PersonalAccountPrivilegedCategory entity)
        {
            this.SaveLog(entity, "PersonalAccount", "Добавление льготы");

            return base.AfterCreateAction(service, entity);
        }

        /// <summary>
        /// Действие, выполняемое после удаления записи
        /// </summary>
        /// <param name="service">Домен сервис <see cref="PersonalAccountPrivilegedCategory"/></param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult AfterDeleteAction(IDomainService<PersonalAccountPrivilegedCategory> service, PersonalAccountPrivilegedCategory entity)
        {
            this.SaveLog(entity, "PersonalAccount", "Удаление льготы");

            return base.AfterDeleteAction(service, entity);
        }
        private void SaveLog(PersonalAccountPrivilegedCategory entity, string propertyName, string propertyDescription, string propertyValue="")
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            this.EntityLogLightDomain.Save(new EntityLogLight
            {
                EntityId = entity.PersonalAccount.Id,
                ClassName = "BasePersonalAccount",
                PropertyName = propertyName,
                DateActualChange = entity.DateFrom,
                DateEnd = entity.DateTo,
                DateApplied = DateTime.UtcNow,
                PropertyValue = propertyValue,
                PropertyDescription = propertyDescription,
                ParameterName = "Льготная категория",
                User = userManager.GetActiveUser().Login
            });

        }


    }


}