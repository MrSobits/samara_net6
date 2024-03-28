namespace Bars.Gkh.Domain.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;

    public class BackForwardIterator<T> : IBackForwardIterator<T> where T : BaseEntity
    {
        private readonly IDomainService<T> _domainService;

        public BackForwardIterator(IDomainService<T> domainService)
        {
            _domainService = domainService;
        }

        public T Back(T current)
        {
            var previous = _domainService.GetAll().Where(x => x.Id < current.Id).OrderByDescending(x => x.Id).FirstOrDefault();
            return previous;
        }

        public T Forward(T current)
        {
            var next = _domainService.GetAll().Where(x => x.Id > current.Id).OrderBy(x => x.Id).FirstOrDefault();
            return next;
        }
    }
}
