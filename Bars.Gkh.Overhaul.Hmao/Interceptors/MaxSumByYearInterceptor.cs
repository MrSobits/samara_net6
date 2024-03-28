using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    public class MaxSumByYearInterceptor : EmptyDomainInterceptor<MaxSumByYear>
    {
        public override IDataResult BeforeCreateAction(IDomainService<MaxSumByYear> service, MaxSumByYear entity)
        {
            var other = service.GetAll().ToList()
                .Where(x => x.Municipality == null || entity.Municipality == null || x.Municipality.Id == entity.Municipality.Id)
                .Where(x => x.Program == null || entity.Program == null || x.Program.Id == entity.Program.Id)
                .Where(x => x.Year == entity.Year);

            if (other.Any())
                return Failure("Запись перекрывает другую запись");

            return Success();
        }
    }
}
