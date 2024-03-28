namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    // Пустышка на случай если ктото наследовался
    public class WorkPriceInterceptor : WorkPriceInterceptor<WorkPrice>
    {
        // Внимание!! методы изменять и добавлять в Generiс
    }

    // Generic для того тчобы расширять в регионах 
    public class WorkPriceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : WorkPrice
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            return CheckWorkPrice(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            return CheckWorkPrice(service, entity);
        }

        public virtual IDataResult CheckWorkPrice(IDomainService<T> service, T entity)
        {
            var checkMunicipality = service.GetAll()
                .Where(x => x.Municipality.Id == entity.Municipality.Id)
                .Where(x => x.Id != entity.Id)
                .Where(x => x.Year == entity.Year)
                .Where(x => x.CapitalGroup == entity.CapitalGroup)
                .Any(x => x.Job.Id == entity.Job.Id);

            if (checkMunicipality)
            {
                return entity.CapitalGroup != null ?
                    Failure(string.Format("Стоимость работы ('{0}') за {1} год в {2} с данной группой капитальности ('{3}') уже добавлена", 
                                                                        entity.Job.Name, entity.Year, entity.Municipality.Name, entity.CapitalGroup.Name)) :
                    Failure(string.Format("Стоимость работы ('{0}') за {1} год в {2} уже добавлена", entity.Job.Name, entity.Year, entity.Municipality.Name));
            }

            return Success();
        }
    }
}