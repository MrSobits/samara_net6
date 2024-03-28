namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид распределения Для параметров распределения 
    /// </summary>
    public enum SuspenseAccountDistributionParametersView
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
        ProportionalArea = 20,

        // убрали вид распределения = "Ручное", так как есть возможность редактировать суммы в других типах
        // закомментил, чтобы быстро можно было восстановить если что (выпилить когда версия 1.7 будет в релиз)
        // Ok

        /// <summary>
        /// По лимитам
        /// </summary>
        [Display("По лимитам")]
        ByLimit = 40,

        /// <summary>
        /// По фактической сумме долга
        /// </summary>
        [Display("Пропорционально задолженности")]
        ByDebt = 50,

        /// <summary>
        /// По документу на оплату
        /// </summary>
        [Display("По документу на оплату")]
        ByPaymentDocument = 60,

		/// <summary>
		/// Пропорционально взносам собственников
		/// </summary>
		[Display("Пропорционально взносам собственников")]
		ProportionallyToOwnersContributions = 70
	}
}