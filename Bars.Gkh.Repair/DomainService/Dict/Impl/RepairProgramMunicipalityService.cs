namespace Bars.Gkh.Repair.DomainService.Dict.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Entities;

    using Castle.Windsor;

    public class RepairProgramMunicipalityService : IRepairProgramMunicipalityService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RepairProgramMunicipality> RepairProgramMunicipalityDomainService { get; set; }

        public IDataResult AddMunicipality(BaseParams baseParams)
        {
            try
            {
                var repairProgramId = baseParams.Params["repairProgramId"].ToInt();

                if (!string.IsNullOrEmpty(baseParams.Params["muIds"].ToStr()))
                {
                    var muIds = baseParams.Params["muIds"].ToStr().Split(',');

                    // получаем у контроллера источники что бы не добавлять их повторно
                    var exsisting = RepairProgramMunicipalityDomainService.GetAll()
                                        .Where(x => x.RepairProgram.Id == repairProgramId)
                                        .Select(x => x.Municipality.Id)
                                        .ToList();

                    foreach (var newId in muIds.Where(id => !exsisting.Contains(id.ToInt())).Select(id => id.ToInt()))
                    {
                        var newRepairProgramMunicipality = new RepairProgramMunicipality
                        {
                            Municipality = new Municipality { Id = newId },
                            RepairProgram = new RepairProgram { Id = repairProgramId }
                        };

                        RepairProgramMunicipalityDomainService.Save(newRepairProgramMunicipality);
                    }
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