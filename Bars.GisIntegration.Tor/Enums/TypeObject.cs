namespace Bars.GisIntegration.Tor.Enums
{
	using Bars.B4.Utils;

	public enum TypeObject
	{
		/// <summary>
		/// Распоряжение
		/// </summary>
		[Display("КНМ")]
		Disposal,

        /// <summary>
        /// Субъекты
        /// </summary>
        [Display("Субъект")]
	    Subject,

	    /// <summary>
	    /// Объект
	    /// </summary>
	    [Display("Объект")]
	    Object,

	    /// <summary>
	    /// Обязательные требования
	    /// </summary>
	    [Display("Обязательные требования")]
	    MandatoryReq,
        
	    /// <summary>
	    /// Показатели эффективности
	    /// </summary>
	    [Display("Показатели эффективности")]
	    RandEParameter
    }
}