namespace Bars.Gkh.Decisions.Nso.Enums
{
	using Bars.B4.Utils;

	/// <summary>
    /// Способ формирования фонда
    /// </summary>
    public enum MethodFormFund
    {
		[Display("На специальном счете")]
		SpecialAccount = 1,

		[Display("На счете регионального оператора")]
		RegOperAccount = 2,

		[Display("На специальном счете, владелец региональный оператор")]
		SpecialAccountWithRegOperOwner = 3,

        [Display("Не выбрано")]
        NotSelected = 4

    }
}
