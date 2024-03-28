namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;

    using Castle.Windsor;

    /// <summary>
    /// Работа с хранимыми файлами
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public class CollectedFile<T> : CollectedFileBase, ICollectedFile<T>
    {
        private readonly Action<T, int> binder;

        private readonly FileInfo fileInfo;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="fileInfo">Файл</param>
        /// <param name="period">Период раскрытия информации в Реформе</param>
        /// <param name="binder">Биндер свойства</param>
        public CollectedFile(IWindsorContainer container, FileInfo fileInfo, ReportingPeriodDict period, Action<T, int> binder)
            : base(container, period)
        {
            this.fileInfo = fileInfo;
            this.binder = binder;
        }

        /// <summary>
        /// Обработать файл
        /// </summary>
        /// <param name="entity">Объект</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns>Успешность операции</returns>
        public bool Process(T entity, ISyncProvider syncProvider)
        {
            if (this.fileInfo == null)
            {
                return true;
            }

            var refFileService = this.Container.ResolveDomain<RefFile>();
            try
            {
                var externalId =
                    refFileService.GetAll()
                                  .Where(x => x.FileInfo.Id == this.fileInfo.Id && x.ReportingPeriod != null && x.ReportingPeriod.Id == this.Period.Id)
                                  .Select(x => x.ExternalId)
                                  .FirstOrDefault();

                if (externalId == 0)
                {
                    externalId = this.Upload(this.fileInfo, refFileService, syncProvider);
                }

                if (externalId > 0)
                {
                    this.binder(entity, externalId);
                    return true;
                }

                return false;
            }
            finally
            {
                this.Container.Release(refFileService);
            }
        }
    }
}