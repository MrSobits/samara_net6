namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using Castle.Windsor;

    using Newtonsoft.Json;
    using Overhaul.DomainService;

    public class DbDpkrParamsService : IDpkrParamsService
    {
        private readonly List<string> keys = new List<string>
                                                 {
                                                     "ActualizePeriodStart", 
                                                     "ActualizePeriodEnd",
                                                     "ProgrammPeriodStart",
                                                     "ProgrammPeriodEnd", 
                                                     "GroupByCeoPeriod", 
                                                     "GroupByRoPeriod", 
                                                     "ServiceCost", 
                                                     "YearPercent", 
                                                     "ShortTermProgPeriod", 
                                                     "PublicationPeriod"
                                                 };

        public IWindsorContainer Container { get; set; }

        public List<string> Keys
        {
            get
            {
                return keys;
            }
        }

        public Dictionary<string, string> SaveParams(BaseParams baseParams)
        {
            var dpkrParamsService = Container.Resolve<IDomainService<DpkrParams>>();

            var oldDpkrParams = dpkrParamsService.GetAll().FirstOrDefault();
            var oldParams = oldDpkrParams != null ? this.DeserializeJsonToDict(oldDpkrParams.Params) : new Dictionary<string, string>();


            var jsonString = JsonConvert.SerializeObject(baseParams.Params.Where(x => keys.Contains(x.Key)).ToDictionary(x => x.Key, y => y.Value));

            if (oldDpkrParams == null)
            {
                dpkrParamsService.Save(new DpkrParams { Params = jsonString });
            }
            else
            {
                oldDpkrParams.Params = jsonString;
                dpkrParamsService.Update(oldDpkrParams);
            }

            return oldParams;
        }

        public Dictionary<string, string> GetParams()
        {
            var dpkrParamsService = Container.Resolve<IDomainService<DpkrParams>>();

            var result = dpkrParamsService.GetAll().FirstOrDefault();

            return result != null ? DeserializeJsonToDict(result.Params) : new Dictionary<string, string>();
        }

        private Dictionary<string, string> DeserializeJsonToDict(string jsonString)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
        }
    }
}