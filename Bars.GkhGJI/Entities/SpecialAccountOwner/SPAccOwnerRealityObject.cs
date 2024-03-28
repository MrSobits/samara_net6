namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Gkh.Entities;
    using Gkh.Overhaul.Entities;

    /// <summary>
    /// МКД на специальном счете
    /// </summary>
    public class SPAccOwnerRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Владелец спецсчета
        /// </summary>
        public virtual SpecialAccountOwner SpecialAccountOwner { get; set; }

        /// <summary>
        /// МКД на спецсчете
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер Спецсчета
        /// </summary>
        public virtual string SpacAccNumber { get; set; }

        //Деятельность
        /// <summary>
        /// Дата открытия спецсчета
        /// </summary>
        public virtual DateTime DateStart { get; set; }


        //Деятельность
        /// <summary>
        /// Дата окончания деятельности
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Банк
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }

    }
}
