namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using System;
    using System.Collections.Generic;
    using Entities.PersonalAccount;
    using FastMember;

    /// <summary>
    /// Обертка для результата расчета
    /// </summary>
    /// <typeparam name="T">Тип результата реасчета</typeparam>
    public class CalculationResult<T>
    {
        private List<CalculationParameterTrace> _traces;
        private List<PersonalAccountCalcParam_tmp> _params;

        /// <summary>
        /// Результат вычислений
        /// </summary>
        public T Result { get; set; }

        /// <summary>
        /// Список трассировки параметров расчета
        /// </summary>
        public IEnumerable<CalculationParameterTrace> Traces
        {
            get { return _traces; }
        }

        /// <summary>
        /// Список параметров, использованных при перерасчете, для последующей фиксации использования
        /// </summary>
        public IEnumerable<PersonalAccountCalcParam_tmp> CalculatedParameters { get { return _params; } }

        public CalculationResult()
        {
            _traces = new List<CalculationParameterTrace>();
            _params = new List<PersonalAccountCalcParam_tmp>();

            var type = Nullable.GetUnderlyingType(typeof (T)) ?? typeof (T);
            TypeAccessor typeAccessor = TypeAccessor.Create(type);

            Result = (type.IsValueType || type == typeof (string))
                ? default(T)
                : (T)(typeAccessor.CreateNewSupported ? typeAccessor.CreateNew() : default(T));
        }

        /// <summary>
        /// Добавить трассировку
        /// </summary>
        /// <param name="trace">Трассировка</param>
        public void AddTrace(CalculationParameterTrace trace)
        {
            _traces.Add(trace);
        }

        /// <summary>
        /// Добавить использованный параметр
        /// </summary>
        /// <param name="param">Параметр</param>
        public void AddParam(PersonalAccountCalcParam_tmp param)
        {
            _params.Add(param);
        }
    }
}