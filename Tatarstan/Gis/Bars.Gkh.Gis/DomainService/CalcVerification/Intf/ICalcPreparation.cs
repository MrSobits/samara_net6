using System;

namespace Bars.Gkh.Gis.DomainService.CalcVerification.Intf
{
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    /// <summary>
    /// Методы подготовки проверочного расчета
    /// </summary>
    public interface ICalcPreparation
    {
        /// <summary>
        /// Подготовка расчетных таблиц
        /// </summary>
        /// <returns></returns>
        IDataResult PrepareCalc(ref CalcTypes.ParamCalc param);
        /// <summary>
        /// Получить начисления УК во временную таблицу
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        string GetMOCharges(ref CalcTypes.ParamCalc param);

        /// <summary>
        /// Темповые таблицы (месячные)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IDataResult LoadTempTablesForMonth(ref CalcTypes.ParamCalc param);

        /// <summary>
        /// Очистка старых расчетных данных по текущей выборке
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        IDataResult ClearData(ref CalcTypes.ParamCalc param);

        /// <summary>
        /// Создание таблицы Charge
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="paramCalc"></param>
        void CreateChargesTable(IDbConnection connection, CalcTypes.ParamCalc paramCalc, bool isChd = false);
    }
}