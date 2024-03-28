namespace Bars.Gkh.Gis.Enum.IncrementalImport
{
    using Bars.B4.Utils;

    public enum TypeIncLoadStatus
    {
        [Display("Добавлена")] 
        Added = 1,
        [Display("Выполняется")] 
        Execute = 2,
        [Display("Остановлено")] 
        Stopped = 3,
        [Display("Завершено")] 
        Finished = 4,
        [Display("Завершено с ошибкой(-ами)")] 
        Error = 5,
        [Display("В очереди на выполнение")] 
        sInQueue = 6
    }
}
