namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Services.DataContracts;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// WCF-сервис для работы с опубликованной программой
    /// </summary>
    public class HmaoPublishProgramWcfService : IPublishProgramWcfService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для сущности Запись Опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedPrgRecDomain { get; set; }

        /// <summary>
        /// Домен-сервис для сущности Запись в версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <summary>
        /// Получить записи опубликованной программы 
        /// </summary>
        /// <param name="muId">Муниципальный район</param>
        /// <returns>Результат выполнения запроса</returns>
        public PublishProgRecWcfProxy[] GetPublishProgramRecs(long muId)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            if (groupByRoPeriod == 0)
            {
                return this.PublishedPrgRecDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIf(muId > 0, x => x.PublishedProgram.ProgramVersion.Municipality.Id == muId)
                    .Select(
                        x => new PublishProgRecWcfProxy
                        {
                            Municipality = x.RealityObject.Municipality.Name,
                            Address = x.RealityObject.Address,
                            PublishDate = x.ObjectCreateDate.ToShortDateString(),
                            Ceo = x.CommonEstateobject,
                            PublishYear = x.PublishedYear.ToString(),
                            Number = x.IndexNumber
                        })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.Number)
                    .ThenBy(x => x.PublishYear)
                    .ToArray();
            }

            var dataPublished = this.PublishedPrgRecDomain.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.Stage2 != null)
                .WhereIf(muId > 0, x => x.PublishedProgram.ProgramVersion.Municipality.Id == muId)
                .Select(x => new {x.Stage2.Stage3Version.Id, x.PublishedYear, x.ObjectCreateDate})
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(
                        x => new
                        {
                            PublishDate = x.ObjectCreateDate.ToShortDateString(),
                            x.PublishedYear
                        }).FirstOrDefault());

            var query = this.VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsMain)
                .Where(x => x.ProgramVersion.Municipality.Id == muId)
                .Where(x => this.PublishedPrgRecDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                .Select(
                    x =>
                        new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            PublishedYear = 0,
                            x.Sum,
                            CommonEstateobject = x.CommonEstateObjects,
                            x.IndexNumber
                        })
                .AsEnumerable();

            return
                query.Select(
                    x =>
                        new PublishProgRecWcfProxy
                        {
                            Municipality = x.Municipality,
                            Address = x.RealityObject,
                            PublishYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].PublishedYear.ToString() : string.Empty,
                            PublishDate = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].PublishDate : string.Empty,
                            Ceo = x.CommonEstateobject,
                            Number = x.IndexNumber
                        })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.Number)
                    .ThenBy(x => x.PublishYear)
                    .ToArray();
        }
    }
}