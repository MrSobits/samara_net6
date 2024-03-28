namespace Bars.Gkh.RegOperator.Dto
{
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Dto для лицевого счета с р/с дома этого ЛС
    /// </summary>
    public class PersonalAccountWithPaymentAccountDto : PersonalAccountDto
    {
        /// <summary>
        /// Р/С дома
        /// </summary>
        public string RoPayAccountNum { get; set; }
    }
}