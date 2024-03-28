namespace Bars.Gkh.Reforma.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml;
    using System.Xml.Serialization;
    using Castle.MicroKernel.Lifestyle;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Impl.DataCollectors;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Interface.DataCollectors;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.GkhDi.Entities;

    using Castle.MicroKernel;

    using Gkh.Domain;

    using Formatting = Newtonsoft.Json.Formatting;

    public class DataCollectorTestController : BaseController
    {
        public ActionResult CollectManOrg(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IManOrgDataCollector>();
            var mService = this.Container.ResolveDomain<ManagingOrganization>();
            var pService = this.Container.ResolveDomain<PeriodDi>();
            try
            {
                var moid = baseParams.Params.Get("moid").ToLong();
                var pid = baseParams.Params.Get("pid").ToLong();
                var result = service.CollectCompanyProfileData(new CompanyProfileData(), mService.Get(moid), pService.Get(pid));

                if (result.Success)
                {
                    var serializer = new XmlSerializer(typeof(CompanyProfileData));
                    var sb = new StringBuilder();
                    using (var tw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                    {
                        serializer.Serialize(tw, result.Data.ProfileData);
                    }

                    return new ContentResult { Content = sb.ToString(), ContentType = "text/xml" };
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(mService);
                this.Container.Release(pService);
            }
        }

        public ActionResult CollectManOrg988(BaseParams baseParams)
        {
            ActionResult resp = null;

            using (this.Container.BeginScope())
               {
                        var service = this.Container.Resolve<IManOrg988DataCollector>();
                        var mService = this.Container.ResolveDomain<ManagingOrganization>();
                        var pService = this.Container.ResolveDomain<PeriodDi>();
                        var argument = new Arguments { { "silentMode", true } };
                        var syncProvider = this.Container.Resolve<ISyncProvider>(argument);
                        try
                        {
                            var moid = baseParams.Params.Get("moid").ToLong();
                            var pid = baseParams.Params.Get("pid").ToLong();
                            var result = service.CollectCompanyProfile988Data(null, mService.Get(moid), pService.Get(pid));

                            if (result.Success)
                            {
                                var profileData = result.Data;

                                foreach (var collectedFile in profileData.CollectedFiles)
                                {
                                    collectedFile.Process(profileData.ProfileData, syncProvider);
                                }

                                var serializer = new XmlSerializer(typeof(CompanyProfileData988));
                                var sb = new StringBuilder();
                                using (
                                    var tw = XmlWriter.Create(
                                        sb,
                                        new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                                {
                                    serializer.Serialize(tw, profileData.ProfileData);
                                }

                                resp = new ContentResult
                                           {
                                               Content = sb.ToString(),
                                               ContentType = "text/xml"
                                           };
                                return resp;
                            }

                            resp = JsonNetResult.Failure(result.Message);
                        }
                        finally
                        {
                            this.Container.Release(service);
                            this.Container.Release(mService);
                            this.Container.Release(pService);
                            this.Container.Release(syncProvider);
                        }
                    }

            return resp;
        }

        public ActionResult CollectRobject(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRobjectDataCollector>();
            var robjectService = this.Container.ResolveDomain<RealityObject>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();
            try
            {
                var roid = baseParams.Params.Get("roid").ToLong();
                var pid = baseParams.Params.Get("pid").ToLong();
                var result = service.CollectHouseProfileData(new HouseProfileData(), robjectService.Get(roid), periodService.Get(pid));

                if (result.Success)
                {
                    var serializer = new XmlSerializer(typeof(HouseProfileData));
                    var sb = new StringBuilder();
                    using (var tw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                    {
                        serializer.Serialize(tw, result.Data);
                    }

                    return new ContentResult { Content = sb.ToString(), ContentType = "text/xml" };
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(robjectService);
                this.Container.Release(periodService);
            }
        }

        public ActionResult CollectRobject988(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IRobject988DataCollector>();
            var robjectService = this.Container.ResolveDomain<RefRealityObject>();
            var periodService = this.Container.ResolveDomain<PeriodDi>();
            try
            {
                var roid = baseParams.Params.Get("roid").ToLong();
                var pid = baseParams.Params.Get("pid").ToLong();
                var refRealObj = robjectService.GetAll().FirstOrDefault(x => x.RealityObject.Id == roid);

                var result = service.CollectHouseProfile988Data(new HouseProfileData988(), refRealObj, periodService.Get(pid), 0);

                if (result.Success)
                {
                    var serializer = new XmlSerializer(typeof(HouseProfileData988));
                    var sb = new StringBuilder();
                    using (var tw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                    {
                        serializer.Serialize(tw, result.Data.ProfileData);
                    }

                    return new ContentResult { Content = sb.ToString(), ContentType = "text/xml" };
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(robjectService);
                this.Container.Release(periodService);
            }
        }

        public ActionResult SetHouseProfile988(BaseParams baseParams)
        {
            var houseId = baseParams.Params.Get("house_id").ToInt();
            var reportingPeriodId = baseParams.Params.Get("reporting_period_id").ToInt();
            ActionResult response = null;
            using (this.Container.BeginScope())
                {
                    var service = this.Container.Resolve<IRobject988DataCollector>();
                    var refRobjectService = this.Container.ResolveDomain<RefRealityObject>();
                    var periodService = this.Container.ResolveDomain<ReportingPeriodDict>();
                    var argument = new Arguments { { "silentMode", true } };
                    var provider = this.Container.Resolve<ISyncProvider>(argument);
                    try
                    {
                        var robject = refRobjectService.GetAll().First(x => x.ExternalId == houseId);
                        var period = periodService.GetAll().First(x => x.ExternalId == reportingPeriodId);

                        var profile = provider.Client.GetHouseProfile988(houseId, reportingPeriodId);
                        var result = service.CollectHouseProfile988Data(profile.house_profile_data, robject, period.PeriodDi, 0);

                        if (result.Success)
                        {
                            var serializer = new XmlSerializer(typeof(HouseProfileData988));
                            var sb = new StringBuilder();
                            using (var tw = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                            {
                                serializer.Serialize(tw, result.Data.ProfileData);
                            }

                            response = new ContentResult { Content = sb.ToString(), ContentType = "text/xml" };
                            return response;
                        }

                        response = JsonNetResult.Failure(result.Message);
                    }
                    finally
                    {
                        provider.Close();
                        this.Container.Release(service);
                        this.Container.Release(provider);
                        this.Container.Release(refRobjectService);
                        this.Container.Release(periodService);
                    }
                }

            return response;
        }

        public ActionResult GetManagingRobjects(BaseParams baseParams)
        {
            ActionResult result = null;
            var inn = baseParams.Params.Get("inn").ToString();

            using (this.Container.BeginScope())
            {
                var service = this.Container.Resolve<IRobjectService>();
                var sw = new Stopwatch();
                sw.Start();
                var data = service.GetManagingRobjects(inn);
                sw.Stop();
                result = new ContentResult
                {
                    Content =
                        JsonNetConvert.SerializeObject(this.Container, new { time = sw.ElapsedMilliseconds, totalCount = data.Length, data }, Formatting.Indented),
                    ContentType = "application/json"
                };
            }

            return result;
        }

        public ActionResult GetRobjectsByFullAddresses(BaseParams baseParams)
        {
            ActionResult result = null;
            var inn = baseParams.Params.Get("inn").ToStr();

            using (this.Container.BeginScope())
            {
                var service = this.Container.Resolve<IRobjectService>();
                var argument = new Arguments { { "silentMode", true } };
                var provider = this.Container.Resolve<ISyncProvider>(argument);
                try
                {
                    var houses = provider.Client.GetHouseList(inn);
                    var robjects = houses.ToDictionary(x => x.house_id, x => service.FindRobjects(x.full_address).Select(y => new { y.Id, y.Address, y.ConditionHouse }).ToArray());

                    result = new ContentResult { Content = JsonNetConvert.SerializeObject(this.Container, robjects), ContentType = "application/json" };
                }
                finally
                {
                    provider.Close();
                    this.Container.Release(service);
                    this.Container.Release(provider);
                }
            }

            return result;
        }


        public ActionResult GetRobjects988ByFullAddresses(BaseParams baseParams)
        {
            ActionResult result = null;
            var houseId = baseParams.Params.GetAs<int>("houseId");
            var reportingPeriodId = baseParams.Params.GetAs<int>("reporting_period_id");

            using (this.Container.BeginScope())
            { 
                var service = this.Container.Resolve<IRobjectService>();
                var provider = this.Container.Resolve<ISyncProvider>(new Arguments
                {
                    {"remoteAddress", "https://api-beta.reformagkh.ru/api_document_literal"},
                    {"login", "tatarstan"},
                    {"password", "shK4yk7T8utv"},
                    {"silentMode", true}
                });
                    
                try
                {                       
                    var house = provider.Client.GetHouseProfile988(houseId, reportingPeriodId);

                    result = new ContentResult { Content = JsonNetConvert.SerializeObject(this.Container, house), ContentType = "application/json" };
                }
                finally
                {
                    provider.Close();
                    this.Container.Release(service);
                    this.Container.Release(provider);
                }
            }

            return result;
        }

        public ActionResult GetCompanyProfile988(BaseParams baseParams)
        {
            ActionResult result = null;
            var inn = baseParams.Params.GetAs<string>("inn");
            var period = baseParams.Params.GetAs<int>("periodId");

            using (this.Container.BeginScope())
            {
                var argument = new Arguments { { "silentMode", true } };
                var provider = this.Container.Resolve<ISyncProvider>(argument);
                try
                {
                    result = new JsonNetResult(provider.Client.GetCompanyProfile988(inn, period));
                }
                finally
                {
                    provider.Close();
                    this.Container.Release(provider);
                }
            }

            return result;
        }
    }
}
