namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип согласования с прокуратурой
    /// </summary>
    public enum TypeAgreementProsecutor
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 10,

        /// <summary>
        /// Требует согласования
        /// </summary>
        [Display("Требует согласования")]
        RequiresAgreement = 20,

        /// <summary>
        /// Не требует согласования
        /// </summary>
        [Display("Не требует согласования")]
        NotRequiresAgreement = 30,
            
        /// <summary>
        /// Незамедлительная проверка, ст. 66
        /// </summary>
        [Display("Незамедлительная проверка, ст. 66")]
        ImmediateInspection = 40
    }
}