namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{

    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс сервиса абонентов
    /// </summary>
    public interface IPersonalAccountOwnerService
    {
        /// <summary>
        /// При обновлении абонента
        /// </summary>
        /// <param name="owner">абонент</param>
        /// <returns>Изменился ли абонент</returns>
        bool OnUpdateOwner(PersonalAccountOwner owner);

        /// <summary>
        /// Обновление наименования абонента
        /// </summary>
        /// <param name="owner">Абонент</param>
        /// <returns>Изменился ли абонент</returns>
        bool UpdateName(PersonalAccountOwner owner);
    }
}
