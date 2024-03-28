namespace Bars.GisIntegration.Base.Enums
{
    /// <summary>
    /// Операция, которую необходимо выполнить с записью в ГИС
    /// </summary>
    public enum RisEntityOperation
    {
        /// <summary>
        /// Создать новую запись
        /// </summary>
        Create = 0,

        /// <summary>
        /// Обновить существующую запись
        /// </summary>
        Update = 1,

        /// <summary>
        /// Удалить запись
        /// </summary>
        Delete = 2
    }
}
