namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Enums;

    public class ROMCalcTask : BaseEntity
    {
        /// <summary>
        /// Инициатор задания
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Вид КНД
        /// </summary>
        public virtual KindKND KindKND { get; set; }

        /// <summary>
        ///Год расчета
        /// </summary>
        public virtual YearEnums YearEnums { get; set; }

        /// <summary>
        /// дата расчета
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// статус расчет
        /// </summary>
        public virtual string CalcState { get; set; }

        /// <summary>
        /// протокол расчета
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

    }
}
