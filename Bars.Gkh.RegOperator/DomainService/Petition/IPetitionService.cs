namespace Bars.Gkh.RegOperator.DomainService.Petition
{
	using System;
	using Bars.Gkh.RegOperator.Entities;

	/// <summary>
	/// Сервис для работы с исковым заявлением
	/// </summary>
	public interface IPetitionService
	{
        /// <summary>
        /// Получить задолженность на дату
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="documentDate"></param>
        decimal GetChargeDebt(PersonalAccountOwner owner, DateTime documentDate);
	}
}