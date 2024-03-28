namespace Bars.Gkh.Reforma.Interface
{
    /// <summary>
    ///     Дурацкий склеиватель инстансов
    /// </summary>
    public interface IClassMerger
    {
        bool AggressiveMode { get; set; }

        /// <summary>
        ///     Заполнить значения в dst непустыми значениями из src
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        void Apply<T>(T dst, T src);
    }
}