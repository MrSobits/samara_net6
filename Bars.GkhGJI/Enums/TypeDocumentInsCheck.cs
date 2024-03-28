namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа в инспекционных проверках
    /// </summary>
    public enum TypeDocumentInsCheck
    {
        [Display("Приказ начальника ГЖИ")]
        BossOrder = 10,

        [Display("Требование прокуратуры")]
        ProsecutionDemand = 20
    }
}