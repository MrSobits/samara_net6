namespace Bars.Gkh.RegOperator.DomainService.Period
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Интерфейс контрольной проверки
    /// </summary>
    public interface IPeriodCloseChecker
    {
        /// <summary>
        /// Системный код проверки
        /// </summary>
        string Impl { get; }

        /// <summary>
        /// Бессмысленный код проверки, для отображения
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="periodId">Идентификатор проверяемого периода</param>
        /// <returns>Результат проверки</returns>
        PeriodCloseCheckerResult Check(long periodId);
    }

    /// <summary>
    /// Результат проверки
    /// </summary>
    public class PeriodCloseCheckerResult
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PeriodCloseCheckerResult()
        {
            this.Log = new StringBuilder();
            this.FullLog = new StringBuilder();
        }

        /// <summary>
        /// Успех
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public StringBuilder Log { get; set; }

        /// <summary>
        /// Полный Лог для каждой записи
        /// </summary>
        public StringBuilder FullLog { get; set; }

       /// <summary>
       /// Идентификаторы ЛС с ошибками
       /// </summary>
        public IEnumerable<long> InvalidAccountIds { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Note { get; set; }
    }
}