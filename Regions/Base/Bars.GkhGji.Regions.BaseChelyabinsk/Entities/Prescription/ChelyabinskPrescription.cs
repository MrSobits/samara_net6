namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription
{
    using System;

    /// <summary>
    /// Предписание ГЖИ для Челябинска (расширяется дополнительными полями)
    /// </summary>
    public class ChelyabinskPrescription : Bars.GkhGji.Entities.Prescription
    {

		/// <summary>
		/// Место составления
		/// </summary>
		public virtual string DocumentPlace { get; set; }

		/// <summary>
		/// Время составления
		/// </summary>
		public virtual DateTime? DocumentTime { get; set; }
    }
}