namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;

    /// <summary>
    /// Переопределения отчёта под Татарстан
    /// </summary>
    public class RegisterMkdToBeRepaired : GkhCr.Report.RegisterMkdToBeRepaired
    {
        protected override List<RelatyObjectWorkDataProxy> GetMkdToBeRepaired()
        {
            return this.TypeWorkCrDomain.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(this.financialSourceIds.Length > 0, x => this.financialSourceIds.Contains(x.FinanceSource.Id))
                .WhereIf(this.programCrId > 0, x => this.programCrId == x.ObjectCr.ProgramCr.Id)
                .Select(x => new RelatyObjectWorkDataProxy
                {
                    Address = x.ObjectCr.RealityObject.Address,
                    MunicipalityName = x.ObjectCr.RealityObject.Municipality.Name,
                    Cost = x.Sum,
                    WorkCode = x.Work.Code,
                    TypeWork = x.Work.TypeWork,
                    Volume = x.Volume
                })
                .OrderBy(x => x.Address)
                .ToList();
        }
    }
}
