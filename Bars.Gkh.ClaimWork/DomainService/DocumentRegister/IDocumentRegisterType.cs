namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister
{
    /// <summary>
    /// Вью-модель для реестра документов ПИР
    /// </summary>
    public interface IDocumentRegisterType
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