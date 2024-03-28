namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using B4.DataAccess;
    using B4.Modules.States;
    using Enums;

    public class ViewPersonalAccount : PersistentObject
    {
        public virtual long RoomId { get; set; }

        public virtual long RoId { get; set; }

        public virtual long MuId { get; set; }

        public virtual long? SettleId { get; set; }

        public virtual long OwnerId { get; set; }

        public virtual long? PrivilegedCategoryId { get; set; }

        public virtual string Address { get; set; }

        public virtual string Municipality { get; set; }

        public virtual string Settlement { get; set; }

        public virtual string RoomAddress { get; set; }

        public virtual string RoomNum { get; set; }

        public virtual string PlaceName { get; set; }

        public virtual string StreetName { get; set; }

        public virtual string AccountOwner { get; set; }

        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        public virtual decimal Area { get; set; }

        public virtual string PersonalAccountNum { get; set; }

        public virtual decimal AreaShare { get; set; }

        public virtual DateTime OpenDate { get; set; }

        public virtual DateTime CloseDate { get; set; }

        public virtual State State { get; set; }

        public virtual bool HasCharges { get; set; }

        public virtual string PersAccNumExternalSystems { get; set; }
    }
}