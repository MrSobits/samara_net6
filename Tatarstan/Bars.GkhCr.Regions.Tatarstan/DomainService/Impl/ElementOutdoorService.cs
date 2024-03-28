namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict;
    using Castle.Windsor;
    using System.Linq;

    using Bars.Gkh.Regions.Tatarstan.Entities.Dicts;

    public class ElementOutdoorService : IElementOutdoorService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddWorks(BaseParams baseParams)
        {
            try
            {
                var elementOutdoorId = baseParams.Params.GetAsId("elementOutdoorId");
                var workIds = baseParams.Params["workIds"].ToStr().ToLongArray();

                var elementOutdoorDomain = this.Container.ResolveDomain<ElementOutdoor>();
                var workOutdoorDomain = this.Container.ResolveDomain<WorkRealityObjectOutdoor>();
                var workElementOutdoorDomain = this.Container.ResolveDomain<WorksElementOutdoor>();

                using (this.Container.Using(elementOutdoorDomain, workOutdoorDomain, workElementOutdoorDomain))
                {
                    var elementOutdoor = elementOutdoorDomain.Get(elementOutdoorId);

                    if (elementOutdoor == null)
                    {
                        return new BaseDataResult
                        {
                            Message = "Элемент двора не найден",
                            Success = false
                        };
                    }

                    var elementWorksIds = workElementOutdoorDomain.GetAll()
                        .Where(x => x.ElementOutdoor.Id == elementOutdoorId)
                        .Select(x => x.Work.Id)
                        .ToList();

                    workOutdoorDomain.GetAll()
                        .Where(x => workIds.Contains(x.Id) && !elementWorksIds.Contains(x.Id))
                        .ForEach(x => workElementOutdoorDomain.Save(new WorksElementOutdoor
                        {
                            ElementOutdoor = elementOutdoor,
                            Work = x
                        }));
                }

                return new BaseDataResult
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {

                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }

        public IDataResult DeleteWork(BaseParams baseParams)
        {
            try
            {
                var workId = baseParams.Params.GetAsId("workId");

                var workElementOutdoorDomain = this.Container.ResolveDomain<WorksElementOutdoor>();

                using (this.Container.Using(workElementOutdoorDomain))
                {
                    workElementOutdoorDomain.Delete(workId);
                }

                return new BaseDataResult
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }
        }
    }
}
