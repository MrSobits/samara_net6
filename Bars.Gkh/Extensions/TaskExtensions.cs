namespace Bars.Gkh.Extensions;

using System.Threading.Tasks;

public static class TaskExtensions
{
    /// <summary>
    /// Получение результата Task с блокировкой потока, но без захвата контекста
    /// </summary>
    public static void GetResultWithoutContext(this Task task) =>
        task.ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Получение результата задачи с блокировкой потока, но без захвата контекста
    /// </summary>
    public static TResult GetResultWithoutContext<TResult>(this Task<TResult> task) =>
        task.ConfigureAwait(false).GetAwaiter().GetResult();
}