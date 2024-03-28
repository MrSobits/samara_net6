namespace Bars.Gkh.RegOperator.Enums
{
	using Bars.B4.Utils;

	/// <summary>
	/// Типы распределений за ранее выполненные работы
	/// </summary>
	public enum PerformedWorkFundsDistributionType
	{
		/// <summary>
		/// Равномерное
		/// </summary>
		[Display("Равномерное")]
		Uniform = 10,

		/// <summary>
		/// Пропорционально площадям
		/// </summary>
		[Display("Пропорционально площадям")]
		ProportionalArea = 20
	}
}