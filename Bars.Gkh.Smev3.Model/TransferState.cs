namespace Bars.Gkh.Smev3
{
    /// <summary>
    /// Статус передачи
    /// </summary>
    public enum TransferState
    {
        /// <summary>
        /// Успешно
        /// </summary>
        Success = 1,

        /// <summary>
        /// С ошибкой
        /// </summary>
        Failure = 2,

        /// <summary>
        /// В процессе
        /// </summary>
        InProcess = 3
    }
}