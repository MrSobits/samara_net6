namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип договора управляющей организации (для жилого дома управляющей организации)
    /// </summary>
    public enum TypeContractManOrg
    {
        /// <summary>
        /// УК с ТСЖ/ЖСК
        /// </summary>
        [Display("УК с ТСЖ/ЖСК")]
        ManagingOrgJskTsj = 10,

        /// <summary>
        /// УК с собственниками
        /// </summary>
        [Display("УК с собственниками")]
        ManagingOrgOwners = 20,

        /// <summary>
        /// ТСЖ/ЖСК
        /// </summary>
        [Display("ТСЖ/ЖСК")]
        JskTsj = 30,

        /// <summary>
        /// Непосредственное управление
        /// </summary>
        [Display("Непосредственное управление")]
        DirectManag = 40
    }
}