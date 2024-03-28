namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Привязка типа заявления к госпошлине
    /// </summary>
    public class StateDutyPetition : BaseEntity
    {
        /// <summary>
        /// Госпошлина
        /// </summary>
        public virtual StateDuty StateDuty { get; set; }

        /// <summary>
        /// Тип заявления
        /// </summary>
        public virtual PetitionToCourtType PetitionToCourtType { get; set; }
    }
}