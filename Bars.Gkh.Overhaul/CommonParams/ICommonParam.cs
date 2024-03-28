namespace Bars.Gkh.Overhaul.CommonParams
{
    using Enum;

    public interface ICommonParam
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Тип хранимого параметра
        /// </summary>
        CommonParamType CommonParamType { get; }

        /// <summary>
        /// Название для отображения
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Возвращает значение парметра для объекта недвижимости
        /// </summary>
        /// <param name="rObj"></param>
        /// <returns></returns>
        object GetValue(Gkh.Entities.RealityObject rObj);

        bool Validate(Gkh.Entities.RealityObject rObj, object min, object max);
    }
}
