namespace Bars.Gkh.Gis.CommonParams
{
    using Enum;
    using Gkh.Entities;

    public interface IGisCommonParam
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
        object GetValue(RealityObject rObj);

        /// <summary>
        /// Признак что данный параметр является точным значением, а не диапазоном значений
        /// </summary>
        bool IsPrecision { get; }
    }
}
