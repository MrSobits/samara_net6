namespace Bars.Gkh.RegOperator.DomainService.PersonalAccountPrivilegedCategory
{
    using Entities.PersonalAccount;

    /// <summary>
    /// Сервис сохранение логов 
    /// </summary>
    public interface IPersonalAccountPrivilegedCategoryService
    {
        /// <summary>
        /// Сохронение лога 
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="propertyName">Измененное поле</param>
        /// <param name="propertyDescription">Комментарий</param>
        /// <param name="propertyValue">Значение изменяемого поля</param>
        void SaveLog(PersonalAccountPrivilegedCategory entity,
                                                string propertyName,
                                                string propertyDescription,
                                                string propertyValue = "");
    }
}