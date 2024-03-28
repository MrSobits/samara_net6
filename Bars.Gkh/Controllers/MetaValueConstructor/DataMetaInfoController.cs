namespace Bars.Gkh.Controllers.MetaValueConstructor
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.MetaValueConstructor;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    /// <summary>
    /// Контроллер для <see cref="DataMetaInfo"/>
    /// </summary>
    public class DataMetaInfoController : B4.Alt.DataController<DataMetaInfo>
    {
        /// <summary>
        /// Сервис для работы с мета-информацией конструкторов
        /// </summary>
        public IDataMetaInfoService Service { get; set; }

        /// <summary>
        /// Метод возвращает дерево элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetTree(BaseParams baseParams)
        {
            return this.Service.GetTree(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод возвращает элементы верхнего уровня
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetRootElements(BaseParams baseParams)
        {
            return this.Service.GetRootElements(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод возвращает все системные источники данных
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetDataFillers(BaseParams baseParams)
        {
            return this.Service.GetDataFillers(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод копирует реализацию конструктора из одной группы в другую
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public ActionResult CopyConstructor(BaseParams baseParams)
        {
            return this.Service.CopyConstructor(baseParams).ToJsonResult();
        }
    }
}