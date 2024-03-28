namespace Bars.Gkh.Interceptors.MetaValueConstructor
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Интерцептор для <see cref="BaseDataValue"/>
    /// </summary>
    public class BaseDataValueInterceptor : EmptyDomainInterceptor<BaseDataValue>
    {
        /// <summary>Метод вызывается перед удалением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<BaseDataValue> service, BaseDataValue entity)
        {
            service.GetAll().Where(x => x.Parent.Id == entity.Id).Select(x => x.Id).ForEach(x => service.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}