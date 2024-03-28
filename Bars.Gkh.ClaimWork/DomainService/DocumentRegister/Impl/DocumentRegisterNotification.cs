namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentRegisterNotification : IDocumentRegisterType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return "Уведомление"; }
        }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route
        {
            get { return "documentregister/notification"; }
        }
    }
}