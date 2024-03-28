namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentRegisterActViolIdentification : IDocumentRegisterType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return "Акт выявления нарушений"; }
        }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route
        {
            get { return "documentregister/actviolidentification"; }
        }
    }
}