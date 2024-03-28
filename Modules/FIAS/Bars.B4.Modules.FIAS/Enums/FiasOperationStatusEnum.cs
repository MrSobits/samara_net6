namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус действия (Причина появления записи)
    /// </summary>
    public enum FiasOperationStatusEnum : byte
    {
        [Display("Инициация")]
        Initiation = 1,

        [Display("Добавление")]
        Added = 10,

        [Display("Изменение")]
        Change = 20,

        [Display("Групповое изменение")]
        GroupChange = 21,

        [Display("Удаление")]
        Remove = 30,

        [Display("Удаление в следствии удаления вышестоящего объекта")]
        RemoveParent = 31,

        [Display("Присоединение адресного объекта (слияние)")]
        Merge = 40,

        [Display("Переподчинение вследствие слияния вышестоящего объекта")]
        MergeParentSubordination = 41,

        [Display("Прекращение существования вследствии присоединения к другому адресному объекту")]
        MergeStopJoining = 42,

        [Display("Создание нового адресного объекта в результате слияния адресных объектов")]
        MergeNewObject = 43,

        [Display("Переподчинение")]
        Subordination = 50,

        [Display("Переподчинение в следствии переподчинения вышестоящего объекта")]
        SubordinationParent = 51,

        [Display("Прекращение существования вследствие дробления")]
        CrushStop = 60,

        [Display("Создание нового адресного объекта в результате дробления")]
        CrushNewObject = 61,

        [Display("Восстановление прекратившего существование объекта")]
        RecoveryStopObject = 70
    }
}