namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    public class ShortProgramDefectListService : IShortProgramDefectListService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ShortProgramRecord> ShortTermObjectService { get; set; }

        public IDomainService<ShortProgramDefectList> DefectListService { get; set; }

        /// <summary>
        /// Данный метод получает работы по объекту 
        /// </summary>
        public IDataResult GetWorks(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("objectId");

            var works =
                ShortTermObjectService.GetAll()
                    .Where(x => x.ShortProgramObject.Id == objectId && x.Work.TypeWork == TypeWork.Work)
                    .Select(x => new {x.Work.Id, x.Work.Name})
                    .OrderBy(x => x.Name)
                    .ToList();

            return new ListDataResult(works, works.Count());
        }
    }
}