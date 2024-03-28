namespace Bars.Gkh.Modules.Gkh1468.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>
    /// Интерцептор для сущностей сторона договора
    /// </summary>
    /// <typeparam name="T">Вид договора</typeparam>
    public class ContractPartInterceptor<T> : EmptyDomainInterceptor<T> where T : BaseContractPart
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var baseDomain = this.Container.ResolveDomain<BaseContractPart>();
            try
            {
                baseDomain.GetAll()
                    .Where(x => x.PublicServiceOrgContract.Id == entity.PublicServiceOrgContract.Id)
                    .ForEach(x => baseDomain.Delete(x.Id));
            }
            finally
            {
                this.Container.Release(baseDomain);
            }

            return base.BeforeCreateAction(service, entity);
        }
    }
}
