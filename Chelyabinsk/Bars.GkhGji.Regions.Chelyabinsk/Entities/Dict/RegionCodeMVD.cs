namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
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

    public class RegionCodeMVD : BaseEntity
    {
        /// <summary>
        /// Код региона
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        ///Наименование информационного центра
        /// </summary>
        public virtual string Name { get; set; }


    }
}
