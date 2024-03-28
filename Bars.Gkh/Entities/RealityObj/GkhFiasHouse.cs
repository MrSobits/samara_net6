namespace Bars.Gkh.Entities.RealityObj
{
	using B4.DataAccess;

	/// <summary>
	/// Запись фиас-адреса c HouseGuid
	/// </summary>
	public class GkhFiasHouse : PersistentObject
	{
		/// <summary>
		/// POSTALCODE
		/// </summary>
		public virtual string PostalCode { get; set; }

		/// <summary>
		/// ОКАТО
		/// </summary>
		public virtual string Okato { get; set; }

		/// <summary>
		/// ОКТМО
		/// </summary>
		public virtual string Oktmo { get; set; }

		/// <summary>
		/// BUILDNUM
		/// </summary>
		public virtual string BuildNum { get; set; }

		/// <summary>
		/// STRUCNUM
		/// </summary>
		public virtual string StrucNum { get; set; }

		/// <summary>
		/// HOUSENUM
		/// </summary>
		public virtual string HouseNum { get; set; }

		/// <summary>
		/// HOUSEID
		/// </summary>
		public virtual string HouseId { get; set; }

		/// <summary>
		/// HOUSEGUID
		/// </summary>
		public virtual string HouseGuid { get; set; }

		/// <summary>
		/// AOGUID
		/// </summary>
		public virtual string AoGuid { get; set; }
	}
}