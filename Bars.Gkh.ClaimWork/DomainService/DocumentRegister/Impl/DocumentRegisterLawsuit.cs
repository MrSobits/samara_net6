namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentRegisterLawsuit : IDocumentRegisterType
    {
        /// <summary>
        /// отображаемое имя
        /// </summary>
        public string DisplayName
        {
            get { return "Исковое заявление"; }
        }

        /// <summary>
        /// роут клиенского контроллера
        /// </summary>
        public string Route
        {
            get { return "documentregister/lawsuit"; }
        }
    }
}