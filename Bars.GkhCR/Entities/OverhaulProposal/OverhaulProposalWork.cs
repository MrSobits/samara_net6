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
    using Bars.Gkh.Enums;
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Newtonsoft.Json;

    /// <summary>
    /// Вид работы КР в предложении
    /// </summary>
    public class OverhaulProposalWork : BaseEntity
    {        
        /// <summary>
        /// Предложение капитального ремонта
        /// </summary>
        public virtual OverhaulProposal OverhaulProposal { get; set; }             

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }       

        /// <summary>
        /// Объем (плановый)
        /// </summary>
        public virtual decimal? Volume { get; set; }       

        /// <summary>
        /// Сумма (плановая)
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Расчет по площади
        /// </summary>
        public virtual bool ByAreaMkd { get; set; }

    }
}
