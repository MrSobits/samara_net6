namespace Bars.Gkh.RegOperator.DomainService.PersonalAccountPrivilegedCategory
{
    using System;
    using Authentification;
    using B4;
    using Castle.Windsor;
    using Entities.PersonalAccount;
    using Gkh.Entities;

    /// <summary>
    /// Сервис сохранении логов о категории льготы л/с
    /// </summary>
    public class PersonalAccountPrivilegedCategoryService : IPersonalAccountPrivilegedCategoryService
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
        /// Сохранение лога 
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="propertyName">Измененное поле</param>
        /// <param name="propertyDescription">Комментарий</param>
        /// <param name="propertyValue">Значение изменяемого поля</param>
        public void SaveLog(PersonalAccountPrivilegedCategory entity, 
            string propertyName,
            string propertyDescription,
            string propertyValue = "")
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            EntityLogLightDomain.Save(new EntityLogLight
            {
                EntityId = entity.PersonalAccount.Id,
                ClassName = "BasePersonalAccount",
                PropertyName = propertyName,
                DateActualChange = entity.DateFrom,
                DateApplied = DateTime.UtcNow,
                PropertyValue = propertyValue,
                PropertyDescription = propertyDescription,
                ParameterName = "Льготная категория",
                User = userManager.GetActiveUser().Login
            });
        }
    }
}