using Bars.B4.DataAccess;

namespace Bars.GkhGji.Contracts
{
    /// <summary>
    /// Интерфейс правила проставления номера обращения
    /// </summary>
    public interface IAppealCitsNumberRule
    {
        /// <summary>
        /// Проставить номер
        /// </summary>
        /// <param name="entity">Сущность</param>
        void SetNumber(IEntity entity);
    }
}