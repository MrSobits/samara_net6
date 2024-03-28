namespace Bars.Gkh.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Класс-интерцептор для организационно-правовой формы 
    /// </summary>
    public class OrganizationFormServiceInterceptor : EmptyDomainInterceptor<OrganizationForm>
    {

        /// <summary>
        /// Метод, позволяющий выполнять какие-либо действия перед созданием сущности организационно-правовой формы
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность организационно-правовой формы</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult BeforeCreateAction(IDomainService<OrganizationForm> service, OrganizationForm entity)
        {
            return CheckOrganizationForm(entity, ServiceOperationType.Save);
        }

        /// <summary>
        /// Метод, позволяющий выполнять какие-либо действия перед обновлением сущности организационно-правовой формы
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность организационно-правовой формы</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<OrganizationForm> service, OrganizationForm entity)
        {
            return CheckOrganizationForm(entity, ServiceOperationType.Update);
        }


        /// <summary>
        /// Метод, позволяющий выполнять какие-либо действия перед удалением сущности организационно-правовой формы
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность организационно-правовой формы</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<OrganizationForm> service, OrganizationForm entity)
        {
            if (Container.Resolve<IDomainService<Contragent>>().GetAll().Any(x => x.OrganizationForm.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Контрагенты;");
            }

            return Success();
        }

        /// <summary>
        /// Метод, выполняющий валидацию свойств Name, OkopfCode, Code сущности организационно-правовой формы
        /// </summary>
        /// <param name="entity">Сущность организационно-правовой формы</param>
        /// <param name="operationType">Тип операции</param>
        /// <returns>Результат выполнения запроса</returns>
        private IDataResult CheckOrganizationForm(OrganizationForm entity, ServiceOperationType operationType)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Поле Наименование обязательно для заполнения");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.OkopfCode.IsNotEmpty() && entity.OkopfCode.Length > 50)
            {
                return Failure("Количество знаков в поле Код ОКОПФ не должно превышать 50 символов");
            }

            if (entity.Code.IsNotEmpty() && entity.Code.Length > 50)
            {
                return Failure("Количество знаков в поле Код не должно превышать 50 символов");
            }

            return Success();
        }
    }
}
