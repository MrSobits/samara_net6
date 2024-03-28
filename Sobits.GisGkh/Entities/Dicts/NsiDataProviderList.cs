using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using GisGkhLibrary.NsiCommonAsync;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Частный справочник
    /// </summary>
    public class NsiDataProviderList : NsiList
    {
        /// <summary>
        /// Контрагент (владелец справочника)
        /// </summary>
        public virtual Contragent Contragent { get; set; }
    }
}