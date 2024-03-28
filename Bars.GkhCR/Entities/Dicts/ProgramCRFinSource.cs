namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Разрезы финансирования программы по КР
    /// </summary>
    public class ProgramCrFinSource : BaseGkhEntity
    {
        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }
    }
}
