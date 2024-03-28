namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип записи
    /// Означает запись загруженная 
    /// </summary>
    public enum FiasTypeRecordEnum : byte
    {
        [Display("Загруженная из ФИАС")]
        Fias = 10,

        [Display("Пользовательская")]
        User = 20
    }
}