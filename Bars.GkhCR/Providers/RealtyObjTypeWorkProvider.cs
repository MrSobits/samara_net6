namespace Bars.GkhCr.Providers
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.PassportProvider;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class RealtyObjTypeWorkProvider : IRealtyObjTypeWorkProvider
    {
        public IWindsorContainer Container { get; set; }

        public IQueryable<RealtyObjectTypeWorkCr> GetWorks(long realtyObjectId)
        {
#warning Не реализована фильтрация по состоянию программы капремонта, возвращать если ЗавершенаКР

            return Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == realtyObjectId)
                .Where(x => x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .Select(
                    x =>
                    new RealtyObjectTypeWorkCr
                        {
                            PeriodName =
                                string.Format(
                                    "{0}{1}",
                                    x.ObjectCr.ProgramCr.Period.DateStart.ToShortDateString(),
                                    x.ObjectCr.ProgramCr.Period.DateEnd.HasValue
                                        ? string.Format(
                                            "-{0}", x.ObjectCr.ProgramCr.Period.DateEnd.Value.ToShortDateString())
                                        : string.Empty),
                            WorkName = x.Work.Name
                        });

        }

        public IQueryable<RealtyObjectTypeWorkCr> GetWorks(long realtyObjectId, long periodId)
        {
            return Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.ObjectCr.RealityObject.Id == realtyObjectId
                    && x.ObjectCr.ProgramCr.Period.Id == periodId
                    && x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .Select(
                    x =>
                    new RealtyObjectTypeWorkCr
                    {
                        Work = x.Work
                    });
        }
    }
}