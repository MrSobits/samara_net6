namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    /// <summary>
    /// Вью-модель для журнала судебной практики
    /// </summary>
    public interface IJurJournalType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        string Route { get; }
    }
}