namespace Bars.Gkh1468.DomainService
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.States;

    using Bars.Gkh.Entities;

    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    public class OkiPassportService : IOkiPassportService
    {
        public IWindsorContainer Container { get; set; }

        private IDomainService<OkiPassport> _paspServ;

        public IDomainService<OkiPassport> PaspService
        {
            get
            {
                return _paspServ ?? (_paspServ = Container.Resolve<IDomainService<OkiPassport>>());
            }
        }
        
        private IStateProvider _stateProvider;

        public IStateProvider StateProvider
        {
            get
            {
                return _stateProvider ?? (_stateProvider = Container.Resolve<IStateProvider>());
            }
        } 

        public IDataResult GetPassport(Municipality municipality, int year, int month)
        {
            var pasp =
                PaspService.GetAll()
                    .FirstOrDefault(
                        x => x.Municipality.Id == municipality.Id && x.ReportYear == year && x.ReportMonth == month);

            if (pasp == null)
            {
                pasp = new OkiPassport { ReportYear = year, ReportMonth = month, Municipality = municipality };
                PaspService.Save(pasp);
            }

            return new BaseDataResult(pasp);
        }

        public IDataResult GetCurrentPassport(Municipality municipality)
        {
            var today = DateTime.Today;
            return this.GetPassport(municipality, today.Year, today.Month);
        }

        public Dictionary<long, decimal> GetPassportsPercentage(int year, int month)
        {
            var result = PaspService.GetAll()
                .Where(x => x.ReportMonth == month && x.ReportYear == year)
                .GroupBy(x => x.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    Percent = x.Average(g => g.Percent)
                }).ToList().ToDictionary(x => x.Key, y => y.Percent);

            return result;
        }
    }
}