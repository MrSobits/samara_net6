using System;

namespace Bars.GkhGji.Regions.Nso.Entities
{
    /// <summary>
    /// Предписание ГЖИ для Нсо (расширяется дополнительными полями)
    /// </summary>
    public class NsoPrescription : Bars.GkhGji.Entities.Prescription
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