namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class NsoRealObjOverhaulDataService : IRealObjOverhaulDataService
    {
        /// <summary>
        /// Домен-сервис для сущности Запись Опубликованной программы
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishProgramRecordDomain { get; set; }

        /// <inheritdoc />
        public Dictionary<long, DateTime?> GetPublishDatesByRo(IEnumerable<long> roIds) =>
            PublishProgramRecordDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => roIds.Contains(x.Stage2.Stage3Version.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.Stage2.Stage3Version.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.PublishedProgram.ObjectEditDate)
                                                .FirstOrDefault()?.PublishedProgram.PublishDate);
        
        public DateTime? GetPublishDateByRo(RealityObject ro)
        {
            return PublishProgramRecordDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == ro.Id)
                .OrderByDescending(x => x.PublishedProgram.ObjectEditDate)
                .Select(x => x.PublishedProgram.PublishDate)
                .FirstOrDefault();
        }
    }
}