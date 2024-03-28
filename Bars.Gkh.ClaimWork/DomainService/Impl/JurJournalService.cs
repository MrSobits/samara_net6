namespace Bars.Gkh.ClaimWork.DomainService.Impl
{
    using System.Collections;
    using System.Linq;
    using B4.IoC;
    using Castle.Windsor;
    using Modules.ClaimWork.DomainService;

    /// <summary>
    /// 
    /// </summary>
    public class JurJournalService : IJurJournalService
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container"></param>
        public JurJournalService(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList ListTypeBase()
        {
            var viewModels = _container.ResolveAll<IJurJournalType>();

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