namespace Bars.GkhCr.Services.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Services.DataContracts;

    public partial class Service
    {
        public GetHousesKrResponse GetHousesKr(string programId)
        {
            var idProgram = programId.ToInt();

            var programCr = Container.Resolve<IDomainService<ProgramCr>>()
                                     .GetAll()
                                     .Where(x => x.Id == idProgram)
                                     .Select(x => new ProgKr
                                                    {
                                                        Id = x.Id,
                                                        Name = x.Name,
                                                        Period = x.Period.Name
                                                    })
                                      .FirstOrDefault();

            if (programCr != null)
            {
                programCr.HousesKr =
                    Container.Resolve<IDomainService<ObjectCr>>()
                             .GetAll()
                             .Where(y => y.ProgramCr.Id == programCr.Id)
                             .Select(
                                 y =>
                                 new HouseKr
                                     {
                                         Id = y.RealityObject.Id,
                                         Mu = y.RealityObject.Municipality.Name,
                                         Address = y.RealityObject.Address
                                     })
                             .ToArray();
            }

            return programCr != null ? new GetHousesKrResponse { ProgKr = programCr, Result = Result.NoErrors } : new GetHousesKrResponse { Result = Result.DataNotFound };
        }
    }
}