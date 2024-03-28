namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;

    using Castle.Windsor;

    /// <summary>
    /// Работа с множеством хранимых файлов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectedFilesCollection<T> : CollectedFileBase, ICollectedFile<T>
    {
        private readonly Action<T, int[]> binder;

        private readonly FileInfo[] fileInfos;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="fileInfos">Файлы</param>
        /// <param name="period">Период раскрытия информации в Реформе</param>
        /// <param name="binder">Биндер</param>
        public CollectedFilesCollection(IWindsorContainer container, FileInfo[] fileInfos, ReportingPeriodDict period, Action<T, int[]> binder)
            : base(container, period)
        {
            this.fileInfos = fileInfos;
            this.binder = binder;
        }

        /// <summary>
        /// Обработать файлы
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns></returns>
        public bool Process(T entity, ISyncProvider syncProvider)
        {
            var refFileService = this.Container.ResolveDomain<RefFile>();
            try
            {
                var ids = this.fileInfos.Select(x => x.Id);
                var externalIdsDict =
                    refFileService.GetAll()
                                  .Where(x => x.ReportingPeriod.Id == this.Period.Id && ids.Contains(x.FileInfo.Id))
                                  .Select(x => new { x.FileInfo.Id, x.ExternalId })
                                  .AsEnumerable()
                                  .ToDictionary(x => x.Id, x => x.ExternalId);

                var externalIds = new List<int>();

                foreach (var fileInfo in this.fileInfos)
                {
                    var externalId = externalIdsDict.Get(fileInfo.Id);
                    if (externalId == 0)
                    {
                        externalId = this.Upload(fileInfo, refFileService, syncProvider);
                    }

                    if (externalId > 0)
                    {
                        externalIds.Add(externalId);
                    }
                }

                if (externalIds.Count > 0)
                {
                    this.binder(entity, externalIds.ToArray());
                    return true;
                }
                else
                {
                    this.binder(entity, null);
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