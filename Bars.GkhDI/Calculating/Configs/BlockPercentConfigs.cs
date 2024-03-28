namespace Bars.GkhDi.Calculating.Configs
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;

    /// <summary>
    /// Конфигурация процентого влияния для <see cref="CoefCalcAlgoritm"/>
    /// </summary>
    public class BlockPercentConfigs
    {
        private IDictionary<Tuple<ManorgType, string>, decimal> coefDictManorg = new Dictionary<Tuple<ManorgType, string>, decimal>();
        private IDictionary<string, decimal> coefDict = new Dictionary<string, decimal>();

        /// <summary>
        /// Установить параметр коэффициента для блока
        /// </summary>
        /// <param name="type">Тип УО</param>
        /// <param name="code">Код</param>
        /// <param name="value">Значение</param>
        /// <returns><see cref="BlockPercentConfigs"/></returns>
        public BlockPercentConfigs SetPercent(ManorgType type, string code, decimal value)
        {
            this.coefDictManorg[Tuple.Create(type, code)] = value;
            return this;
        }

        /// <summary>
        /// Установить параметр коэффициента для блока
        /// </summary>
        /// <param name="code">Код</param>
        /// <param name="value">Значение</param>
        /// <returns><see cref="BlockPercentConfigs"/></returns>
        public BlockPercentConfigs SetPercent(string code, decimal value)
        {
            this.coefDict[code] = value;
            return this;
        }

        /// <summary>
        /// Вернуть код коэффициента
        /// </summary>
        /// <param name="type">Тип УО</param>
        /// <param name="code">Код</param>
        /// <returns>Процент</returns>
        public decimal GetPercent(ManorgType type, string code)
        {
            decimal value;

            if (this.coefDictManorg.TryGetValue(Tuple.Create(type, code), out value))
            {
                return value;
            }

            return this.coefDict.Get(code);
        }
    }

    /// <summary>
    /// Тип УО для различия УК и ТСЖ/ЖСК
    /// </summary>
    public enum ManorgType
    {
        /// <summary>
        /// Не является УО
        /// </summary>
        NotManorg = 0,

        /// <summary>
        /// УК
        /// </summary>
        Uk = 10,

        /// <summary>
        /// ТСЖ/ХСК
        /// </summary>
        TsjJsk = 20
    }
}