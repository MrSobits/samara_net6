namespace Bars.Gkh.ClaimWork.DomainService.DocumentRegister.Impl
{
    using System.Collections;
    using System.Linq;
    using B4.IoC;
    using Castle.Windsor;

    /// <summary>
    /// Реализация интерфейса реестра документов ПИР
    /// </summary>
    public class DocumentRegisterService : IDocumentRegisterService
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container"></param>
        public DocumentRegisterService(IWindsorContainer container)
        {
            _container = container;
        }

        public IList ListTypeDocument()
        {
            var viewModels = _container.ResolveAll<IDocumentRegisterType>().ToList();

            using (_container.Using(viewModels))
            {
                return viewModels
                    .Select(x => new
                    {
                        x.DisplayName,
                        x.Route
                    })
                    .ToList();
            }
        }
    }
}