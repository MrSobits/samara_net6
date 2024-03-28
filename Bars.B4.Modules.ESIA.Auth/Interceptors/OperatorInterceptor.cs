namespace Bars.B4.Modules.ESIA.Auth.Interceptors
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерсептор оператора
    /// </summary>
    public class OperatorInterceptor : EmptyDomainInterceptor<Operator>
    {
        /// <summary>Метод вызывается перед удалением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<Operator> service, Operator entity)
        {
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();

            using (this.Container.Using(esiaOperatorDomain))
            {
                //Удаляем связанных операторов есиа
                esiaOperatorDomain.GetAll()
                    .Where(x => x.Operator != null && x.Operator.Id == entity.Id)
                    .ForEach(x => esiaOperatorDomain.Delete(x.Id));
            }

            return this.Success();
        }
    }
}
