namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Базовый контроллер с переопределением Json сериализатора
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseGkhDataController<T> : B4.Alt.DataController<T>
        where T : class, IEntity
    {
        /// <summary>Получить список объектов.</summary>
        /// <param name="baseParams">Базовые параметры.</param>
        /// <returns>JsonNetResult.</returns>
        public override ActionResult List(BaseParams baseParams)
        {
            return this.Js(this.ViewModel.List(this.DomainService, baseParams));
        }

        /// <summary>Создает JsonNetResult</summary>
        /// <param name="data">Данные</param>
        /// <returns>JsonNetResult</returns>
        protected new ActionResult Js(object data)
        {
            return BaseGkhControllerHelper.GetJsonNetResult(data);
        }

        /// <summary>Создает JsonListResult</summary>
        /// <param name="data">Данные (коллекция элементов)</param>
        /// <param name="count">Количество элементов</param>
        /// <typeparam name="TData">Тип элементов</typeparam>
        /// <returns>JsonListResult</returns>
        protected new ActionResult JsList<TData>(IEnumerable<TData> data, int count = 0)
        {
            return BaseGkhControllerHelper.GetJsonListResult(data, count);
        }
    }
}