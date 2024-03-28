namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис для "Инспектируемая часть в акте без взаимодействия"
    /// </summary>
    public interface IActIsolatedInspectedPartService
    {
        /// <summary>
        /// Добавить инспектируемые части
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult AddInspectedParts(BaseParams baseParams);
    }
}
