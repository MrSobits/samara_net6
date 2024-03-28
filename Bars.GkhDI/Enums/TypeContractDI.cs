namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    public enum TypeContractDi
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Договор пользования")]
        Use = 20,

        [Display("Договор аренды")]
        Lease = 30
    }
}
