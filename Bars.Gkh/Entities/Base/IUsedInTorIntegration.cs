namespace Bars.Gkh.Entities.Base
{
	using System;

	using Bars.B4.DataAccess;

	/// <summary>
	/// Наличие поля для размещения гуида из ТОР 
	/// </summary>
	public interface IUsedInTorIntegration : IEntity
	{
		/// <summary>
		/// Идентификатор для интеграции
		/// </summary>
		Guid? TorId { get; set; }
	}
}