namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.GkhGji.Enums;

    public class ViewStatusPaymentDocumentHouses : PersistentObject
    {
        public virtual long Period { get; set; }
        public virtual string Name { get; set; }

        public virtual string Address { get; set; }

        public virtual int Account { get; set; }

        public virtual StatusPaymentDocumentHousesType State { get; set; }
    }
}