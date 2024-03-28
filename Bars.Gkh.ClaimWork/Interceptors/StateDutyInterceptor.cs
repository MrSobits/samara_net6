namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;
    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;
    
    using Modules.ClaimWork.Entities;

    /// <summary>
    /// интерцептор справочника гос.пошлины
    /// </summary>
    public class StateDutyInterceptor : EmptyDomainInterceptor<StateDuty>
    {
        /// <summary>
        /// Метод вызывается перед удалением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<StateDuty> service, StateDuty entity)
        {
            var petitionDomain = Container.ResolveDomain<StateDutyPetition>();

            try
            {
                petitionDomain.GetAll()
                    .Where(x => x.StateDuty.Id == entity.Id)
                    .ForEach(x => petitionDomain.Delete(x.Id));
            }
            finally
            {
                Container.Release(petitionDomain);
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}
