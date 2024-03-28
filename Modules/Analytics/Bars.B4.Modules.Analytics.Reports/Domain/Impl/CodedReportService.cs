namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System.Linq;
    using Castle.Windsor;

    public class CodedReportService : ICodedReportService
    {
        private readonly IWindsorContainer _container;

        public CodedReportService(IWindsorContainer container)
        {
            _container = container;
        }

        public IQueryable<ICodedReport> GetAll()
        {
            return _container.ResolveAll<ICodedReport>().AsQueryable();
        }

        public ICodedReport Get(string key)
        {
            return _container.ResolveAll<ICodedReport>().FirstOrDefault(x => x.Key == key);
        }
    }
}
