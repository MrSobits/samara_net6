namespace Bars.Gkh.Enums
{
	using Bars.B4.Utils;

	/// <summary>
	/// Использовать идентификацию домов по GUID из ФИАС
	/// </summary>
	public enum UseFiasHouseIdentification
	{
        [Display("Не использовать")]
        NotUse = 0,

        [Display("Использовать")]
        Use = 10
    }
}