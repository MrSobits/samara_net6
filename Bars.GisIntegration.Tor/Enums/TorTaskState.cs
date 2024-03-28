namespace Bars.GisIntegration.Tor.Enums
{
	using Bars.B4.Utils;

	public enum TorTaskState
    {
		/// <summary>
		/// Отправка данных
		/// </summary>
		[Display("Отправка данных")]
		SendingData,

		/// <summary>
		/// Выполнена успешно
		/// </summary>
		[Display("Выполнена успешно")]
		CompleteSuccess,

		/// <summary>
		/// Выполнена c ошибками
		/// </summary>
		[Display("Выполнена c ошибками")]
		CompleteWithErrors,

        /// <summary>
        /// Не выполнена
        /// </summary>
        [Display("Не выполнена")]
        NotComplete
    }
}