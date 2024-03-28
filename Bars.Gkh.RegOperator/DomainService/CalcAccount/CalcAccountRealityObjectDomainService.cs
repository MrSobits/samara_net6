namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using B4;
    using Entities;

    public class CalcAccountRealityObjectDomainService : BaseDomainService<CalcAccountRealityObject>
    {
        /// <summary>
        /// Удалить объект
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        protected override void DeleteInternal(object id)
        {
            var value = Repository.Get(id);

            value.DateEnd = DateTime.Today;

            Repository.Update(value);
        }
    }
}
