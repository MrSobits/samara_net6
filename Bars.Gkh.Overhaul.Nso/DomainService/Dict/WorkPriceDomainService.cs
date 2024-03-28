namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;

    // Поскольку сущность WorkPrice заменена Через sublass сущностью HmaoWorkPrice то необходимо делать такую херню
    // С подменой домен сервиса и интерцепторов
    public class WorkPriceDomainService : Bars.Gkh.Overhaul.DomainService.WorkPriceDomainService
    {
        private NsoWorkPrice ToDerived(WorkPrice tBase)
        {
            var tDerived = new NsoWorkPrice();

            foreach (PropertyInfo propBase in typeof(WorkPrice).GetProperties())
            {
                PropertyInfo propDerived = typeof(NsoWorkPrice).GetProperty(propBase.Name);
                propDerived.SetValue(tDerived, propBase.GetValue(tBase, null), null);
            }
            return tDerived;
        }

        protected override void SaveInternal(WorkPrice value)
        {
            var newValue = this.ToDerived(value);
            var newDomain = Container.Resolve<IDomainService<NsoWorkPrice>>();
            var newInterceptors = Container.ResolveAll<IDomainServiceInterceptor<NsoWorkPrice>>();
            var newRepository = Container.Resolve<IRepository<NsoWorkPrice>>();

            try
            {
                IDataResult result;
                foreach (var interceptor in newInterceptors)
                {
                    result = interceptor.BeforeCreateAction(newDomain, newValue);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }
                }

                newRepository.Save(newValue);

                value.Id = newValue.Id;

                foreach (var interceptor in newInterceptors)
                {
                    result = interceptor.AfterCreateAction(newDomain, newValue);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }
                }
            }
            finally
            {
                Container.Release(newDomain);
                Container.Release(newInterceptors);
                Container.Release(newRepository);
            }
        }

        /// <summary>
        /// тут удаление тоже всвязи с этим делаем
        /// </summary>
        protected override void DeleteInternal(object id)
        {
            var newDomain = Container.Resolve<IDomainService<NsoWorkPrice>>();
            var newInterceptors = Container.ResolveAll<IDomainServiceInterceptor<NsoWorkPrice>>();
            var newRepository = Container.Resolve<IRepository<NsoWorkPrice>>();

            var newValue = newRepository.Get(id);

            try
            {
                IDataResult result;
                foreach (var interceptor in newInterceptors)
                {
                    result = interceptor.BeforeDeleteAction(newDomain, newValue);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }
                }

                newRepository.Delete(id);

                foreach (var interceptor in newInterceptors)
                {
                    result = interceptor.AfterDeleteAction(newDomain, newValue);
                    if (!result.Success)
                    {
                        throw new ValidationException(result.Message);
                    }
                }
            }
            finally
            {
                Container.Release(newDomain);
                Container.Release(newInterceptors);
                Container.Release(newRepository);
            }
        }
    }
}
