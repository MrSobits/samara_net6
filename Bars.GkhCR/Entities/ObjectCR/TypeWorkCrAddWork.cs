
namespace Bars.GkhCr.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.States;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид работы КР
    /// </summary>
    public class TypeWorkCrAddWork : BaseGkhEntity
    {
        /// <summary>
        /// Работа КР
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Этап работы
        /// </summary>
        public virtual AdditWork AdditWork { get; set; }

        /// <summary>
        /// Обязательная
        /// </summary>
        public virtual bool Required { get; set; }      

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime DateEndWork { get; set; }

        /// <summary>
        /// Очередность
        /// </summary>
        public virtual int? Queue { get; set; }

    }
}
