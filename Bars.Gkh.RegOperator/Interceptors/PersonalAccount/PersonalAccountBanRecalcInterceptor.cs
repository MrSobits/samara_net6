namespace Bars.Gkh.RegOperator.Interceptors.PersonalAccount
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Интерцептор для <see cref="PersonalAccountBanRecalc"/>
    /// </summary>
    public class PersonalAccountBanRecalcInterceptor : EmptyDomainInterceptor<PersonalAccountBanRecalc>
    {
        /// <summary>
        /// Домен-сервис <see cref="EntityLogLight"/>
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>Метод вызывается после удаления объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterDeleteAction(IDomainService<PersonalAccountBanRecalc> service, PersonalAccountBanRecalc entity)
        {
            this.EntityLogLightDomain.Save(
                new EntityLogLight
                {
                    ClassName = "BasePersonalAccount",
                    EntityId = entity.PersonalAccount.Id,
                    ParameterName = "Запрет перерасчета",
                    PropertyName = "Запрет перерасчета",
                    DateActualChange = DateTime.Now,
                    DateApplied = DateTime.Now,
                    PropertyValue = "",
                    PropertyDescription = $"Снят {entity.Type.GetDisplayName()} за период с {entity.DateStart:MM.yyyy} по {entity.DateEnd:MM.yyyy}",
                    User = this.UserManager.GetActiveUser()?.Login ?? "anonymous"
                });

            return base.AfterDeleteAction(service, entity);
        }
    }
}