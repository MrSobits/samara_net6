namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с Жилыми домами в ДПКР
    /// </summary>
    public class RealityObjectsInPrograms : IRealityObjectsInPrograms
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для сущности Запись краткосрочной программы
        /// </summary>
        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; }

        /// <summary>
        /// Домен-сервис для сущности Запись Опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        /// <summary>
        /// Домен-сервис для сущности Запись в версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <summary>
        /// Получить Жилые дома в КПКР
        /// </summary>
        /// <param name="year">Год</param>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<RealityObject> GetInShortProgram(int year)
        {
            return this.ShortProgramRecordDomain.GetAll()
                .WhereIf(year > 0, x => x.Year == year)
                .Select(x => x.RealityObject);
        }

        /// <summary>
        /// Получить Жилые дома в Опубликованной программе
        /// </summary>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<RealityObject> GetInPublishedProgram()
        {
            return this.PublishedProgramRecordDomain.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Select(x => x.RealityObject);
        }

        /// <summary>
        /// Получить Жилые дома в Опубликованной программе 
        /// </summary>
        /// <param name="muIds">Список Муниципальных районов</param>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<RealityObject> GetInPublishedProgramByMunicipality(long[] muIds)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            if (groupByRoPeriod == 0)
            {
                return this.PublishedProgramRecordDomain.GetAll()
                    .Where(
                        x => x.PublishedProgram.ProgramVersion.IsMain
                            && muIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                    .Select(x => x.RealityObject);
            }

            return this.VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain && muIds.Contains(x.ProgramVersion.Municipality.Id))
                .Where(x => this.PublishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                .Select(x => x.RealityObject);
        }

        /// <summary>
        /// Получить Жилые дома в КПКР
        /// </summary>
        /// <returns>Результат выполнения запроса</returns>
        public IQueryable<RecordObject> GetShortProgramRecordData()
        {
            return this.ShortProgramRecordDomain.GetAll()
                .Select(
                    x => new RecordObject
                    {
                        Id = x.RealityObject.Id,
                        TotalCost = x.Stage2.Sum
                    });
        }
    }
}