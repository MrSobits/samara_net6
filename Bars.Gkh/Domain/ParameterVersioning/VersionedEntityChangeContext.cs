namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;

    /// <summary>
    /// Контекст изменения сущности
    /// </summary>
    public class VersionedEntityChangeContext : IDisposable
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; private set; }

        /// <summary>
        /// Класс сущности
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// Имя свойства
        /// </summary>
        public string PropName { get; private set; }

        /// <summary>
        /// Значение
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime FactDate { get; private set; }

        /// <summary>
        /// Имя параметра
        /// </summary>
        public string ParamName { get; private set; }

        /// <summary>
        /// Текущий контекст
        /// </summary>
        [ThreadStatic]
        public static VersionedEntityChangeContext Current;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="className">Имя класса</param>
        /// <param name="propName">Имя свойства</param>
        /// <param name="value">Значение</param>
        /// <param name="factDate">Дата</param>
        /// <param name="paramName">Имя параметра</param>
        public VersionedEntityChangeContext(long id, string className, string propName, object value, DateTime factDate, string paramName)
        {
            this.Id = id;
            this.ClassName = className;
            this.PropName = propName;
            this.Value = value;
            this.FactDate = factDate;
            this.ParamName = paramName;

            Current = this;
        }

        public void Dispose()
        {
            Current = null;
        }
    }
}
