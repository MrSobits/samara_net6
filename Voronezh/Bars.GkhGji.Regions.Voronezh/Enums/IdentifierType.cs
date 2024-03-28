using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    public enum IdentifierType
    {

        /// <summary>
        /// ИНН
        /// </summary>
        [Display("ИНН")]
        INN = 10,

        /// <summary>
        /// код иностранной организации
        /// </summary>
        [Display("КИО")]
        KIO = 20
    }
}
