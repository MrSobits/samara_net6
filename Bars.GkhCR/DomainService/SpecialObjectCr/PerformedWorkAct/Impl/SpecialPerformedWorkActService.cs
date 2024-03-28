namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.IoC;

    using Enums;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Entities;

    using Castle.Windsor;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    /// <summary>
    /// Сервиса для Акт выполненных работ
    /// </summary>
    public class SpecialPerformedWorkActService : ISpecialPerformedWorkActService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список актов по новым активным программам
        /// </summary>
        public IDataResult ListByActiveNewOpenPrograms(BaseParams baseParams)
        {
            var actDomain = this.Container.Resolve<IDomainService<SpecialPerformedWorkAct>>();

            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            using (this.Container.Using(actDomain))
            {
                return actDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                        || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Open
                        || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.New)
                    .Select(x => new
                    {
                        x.Id,
                        TypeWorkCr = x.TypeWorkCr.Work.Name,
                        x.ObjectCr.RealityObject.Address,
                        x.Volume,
                        x.Sum,
                        State = x.State.Name
                    })
                    .ToListDataResult(loadParams, this.Container);
            }
        }

        /// <summary>
        /// Получить информацию по акту
        /// </summary>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var specialObjectCrDomain = this.Container.Resolve<IDomainService<Entities.SpecialObjectCr>>();

            using (this.Container.Using(specialObjectCrDomain))
            {
                if (objectCrId > 0)
                {
                    var objCr = specialObjectCrDomain.Get(objectCrId);

                    var objCrProgram = objCr.RealityObject.Address + " (" + objCr.ProgramCr.Name + ")";

                    return new BaseDataResult(new { objCrProgram });
                }

                return BaseDataResult.Error("Не удалось получить объект кап.ремонта");
            }
        }
    }
}
