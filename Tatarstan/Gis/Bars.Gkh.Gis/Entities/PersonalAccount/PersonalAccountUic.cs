namespace Bars.Gkh.Gis.Entities.PersonalAccount
{
    using B4.DataAccess;
    using Register.HouseRegister;

    public class PersonalAccountUic : BaseEntity
    {
        /// <summary>
        /// Уникальный идентификацонный код ЛС
        /// </summary>
        public virtual string Uic { get; set; }

        /// <summary></summary>
        public virtual HouseRegister HouseRegister { get; set; }

        /// <summary></summary>
        public virtual long PersonalAccountId { get; set; }

        /// <summary></summary>
        public virtual long AccountNumber { get; set; }

        /// <summary></summary>
        public virtual string FlatNumber { get; set; }
    }
}
