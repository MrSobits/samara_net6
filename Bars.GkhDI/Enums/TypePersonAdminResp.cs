namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип лица, привлеченного к административной ответственности
    /// </summary>
    public enum TypePersonAdminResp
    {
        [Display("Не заполнено")]
        NotSet = 0,

        [Display("Юридическое лицо")]
        JurPerson = 10,

        [Display("Физическое лицо")]
        PhisicalPerson = 20
    }
}
