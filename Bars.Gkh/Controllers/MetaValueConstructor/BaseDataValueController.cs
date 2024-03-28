namespace Bars.Gkh.Controllers.MetaValueConstructor
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.MetaValueConstructor;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Контроллер для работы с объектами значениями
    /// </summary>
    public class BaseDataValueController : B4.Alt.DataController<BaseDataValue>
    {
        /// <summary>
        /// Все реализованные сервисы
        /// </summary>
        public IEnumerable<IDataValueService> Services { get; set; }

        /// <summary>
        /// Метод возвращает дерево значений с мета описанием элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>результат запроса</returns>
        public ActionResult GetMetaValues(BaseParams baseParams)
        {
            return this.GetService(baseParams).GetMetaValues(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод начинает расчёт всех элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult CalcNow(BaseParams baseParams)
        {
            return this.GetService(baseParams).CalcNow(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод начинает массовый расчёт всех элементов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult CalcMass(BaseParams baseParams)
        {
            return this.GetService(baseParams).CalcMass(baseParams).ToJsonResult();
        }

        private IDataValueService GetService(BaseParams baseParams)
        {
            var dataMetaObjectType = baseParams.Params.GetAs("dataMetaObjectType", (DataMetaObjectType)(-1), true);
            if (dataMetaObjectType > 0)
            {
                return this.Services.Single(x => x.ConstructorType == dataMetaObjectType);
            }

            throw new ValidationException("Не указан тип конструктора");
        }
    }
}