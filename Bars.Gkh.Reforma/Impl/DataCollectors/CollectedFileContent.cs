namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Text;

    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;

    using Castle.Windsor;

    /// <summary>
    /// Работа с хранимым файлом
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class CollectedFileContent<T> : CollectedFileBase, ICollectedFile<T>
    {
        private readonly string name;

        private readonly string content;

        private readonly Action<T, int> binder;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="name">Имя файла</param>
        /// <param name="content">Данные</param>
        /// <param name="binder">Биндер</param>
        /// <param name="period">Период раскрытия информации в Реформе</param>
        public CollectedFileContent(IWindsorContainer container, string name, string content, Action<T, int> binder, ReportingPeriodDict period)
            : base(container, period)
        {
            this.name = name;
            this.content = content;
            this.binder = binder;
        }

        /// <summary>
        /// Обработать файл
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns>Флаг успешности</returns>
        public bool Process(T entity, ISyncProvider syncProvider)
        {
            if (string.IsNullOrEmpty(this.content))
            {
                return true;
            }

            var externalId = this.UploadContent(string.IsNullOrEmpty(this.name) ? Guid.NewGuid().ToString() : this.name, Convert.ToBase64String(Encoding.UTF8.GetBytes(this.content)), syncProvider);

            if (externalId > 0)
            {
                this.binder(entity, externalId);
                return true;
            }

            return false;
        }
    }
}