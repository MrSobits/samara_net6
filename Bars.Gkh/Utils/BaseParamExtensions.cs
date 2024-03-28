namespace Bars.Gkh.Utils
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;

    /// <summary>
    /// Расширение базовых параметров <see cref="BaseParams"/>
    /// </summary>
    public static class BaseParamExtensions
    {
        /// <summary>
        /// Получить сущность для сохранения
        /// </summary>
        /// <returns>Возвращает первую запись в массиве records</returns>
        public static T GetSaveEntity<T>(this BaseParams baseParams, bool ignoreCase = false)
            where T : class, IEntity
        {
            return baseParams.Params
                .Read<SaveParam<T>>()
                .Execute(x => Converter.ToSaveParam<T>(x, ignoreCase))
                .Records
                .Single()
                .AsObject();
        }
    }
}