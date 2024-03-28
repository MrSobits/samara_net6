namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentRegisterPretension : IDocumentRegisterType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return "Претензия"; }
        }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route
        {
            get { return "documentregister/pretension"; }
        }
    }
}