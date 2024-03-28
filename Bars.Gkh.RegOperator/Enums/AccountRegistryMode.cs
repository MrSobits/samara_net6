namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

	/// <summary>
	/// Режим отображения лс в реестре
	/// </summary>
    public enum AccountRegistryMode
	{
        /// <summary>
        /// Текущий режим
        /// </summary>
        [Display("Текущий режим")]
		Common = 0,

        /// <summary>
        /// Режим расчета
        /// </summary>
        [Display("Режим расчета")]
		Calc = 1,

        /// <summary>
        /// Формирование квитанций
        /// </summary>
        [Display("Формирование квитанций")]
		PayDoc = 2
		
	}
}
