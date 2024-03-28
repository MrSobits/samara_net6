namespace Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class PersonalAccountInfoOut
    {
        /// <summary>
        /// номер счета
        /// </summary>
        [DataMember]
        public string AccountNumber { get; set; }

        /// <summary>
        /// дата открытия
        /// </summary>
        [DataMember]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// дата закрытия
        /// </summary>
        [DataMember]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// итого задолженность
        /// </summary>
        [DataMember]
        public decimal TotalDebt { get; set; }

        /// <summary>
        /// задолженность по взносам всего
        /// </summary>
        [DataMember]
        public decimal FeeDebtTotal { get; set; }

        /// <summary>
        /// задолженность пени всего
        /// </summary>
        [DataMember]
        public decimal TotalPenaltyDebt { get; set; }
    }
}