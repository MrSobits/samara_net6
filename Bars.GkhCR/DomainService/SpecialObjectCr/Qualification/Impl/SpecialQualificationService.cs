namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class SpecialQualificationService : ISpecialQualificationService
    {
        public IWindsorContainer Container { get; set; }

        public IEnumerable<string> GetActiveColumns(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId", 0);

            var periodId = this.Container.Resolve<IDomainService<Entities.SpecialObjectCr>>()
                .GetAll()
                .Where(x => x.Id == objectCrId)
                .Select(x => x.ProgramCr.Period.Id)
                .FirstOrDefault();

            return this.Container.Resolve<IDomainService<QualificationMember>>()
                .GetAll()
                .Where(x => x.Period.Id == periodId)
                .Select(x => x.Name)
                .ToList();
        }
    }
}
