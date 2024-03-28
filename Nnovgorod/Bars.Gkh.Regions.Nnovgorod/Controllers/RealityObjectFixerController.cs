namespace Bars.Gkh.Regions.Nnovgorod.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;

    using Microsoft.AspNetCore.Mvc;

    public class RealityObjectFixerController : BaseController
    {
        public IRepository<RealityObject> RoRepo { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<Municipality> MunDomain { get; set; }

        private Dictionary<string, long> _mos;

        private Dictionary<string, long> Mos
        {
            get
            {
                return _mos
                       ?? (_mos =
                           MunDomain.GetAll()
                               .GroupBy(x => x.Name)
                               .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(y => y.Id)));
            }
        }

        private List<RealityObject> _ros;

        public ActionResult FixAddresses(BaseParams baseParams)
        {
            Stream fileStream = null;

            var path = @"C:\web\gkh\address.csv";

#if DEBUG
            path = @"D:\TMP\123.csv";
#endif

            fileStream = System.IO.File.OpenRead(path);

            var dict = new Dictionary<string, List<StreetProxy>>();
            using (fileStream)
            {
                using (var sr = new StreamReader(fileStream, Encoding.GetEncoding(1251)))
                {
                    var line = string.Empty;
                    while ((line = sr.ReadLine()) != null && line.IsNotEmpty())
                    {
                        string municipality;
                        var streetProxy = ProcessLine(line, out municipality);

                        if (streetProxy != null)
                        {
                            if (dict.ContainsKey(municipality))
                            {
                                dict[municipality].Add(streetProxy);
                            }
                            else
                            {
                                dict.Add(municipality, new List<StreetProxy>() {streetProxy});
                            }
                        }
                    }
                }
            }

            var fixedCount = 0;
            var filter = new List<long>();

            var session = SessionProvider.GetCurrentSession();

            var updateBuilder = new StringBuilder();

            foreach (var kv in dict)
            {
                var key = kv.Key + " р-н";
                if (!Mos.ContainsKey(key))
                {
                    continue;
                }

                foreach (var streetProxy in kv.Value)
                {
                    StreetProxy proxy = streetProxy;
                    var ros =
                        RoDomain.GetAll()
                            .Where(x => !filter.Contains(x.Id))
                            .Where(x =>
                                x.Municipality.Name.ToUpper() == "Нижний Новгород г".ToUpper()
                                && proxy.NormalizedList.Contains(x.FiasAddress.StreetName.ToUpper()))
                            .WhereIf(proxy.Numbers.Any(),
                                x => proxy.Numbers.Contains(x.FiasAddress.House.ToUpper()))
                            .Select(x => x.Id)
                            .ToList();

                    if (ros.Any())
                    {
                        foreach (var ro in ros)
                        {
                            updateBuilder.AppendFormat(
                                "update gkh_reality_object set municipality_id = {0} where id = {1};{2}",
                                Mos[key], ro, Environment.NewLine);

                            filter.Add(ro);
                        }

                        fixedCount += ros.Count();
                    }
                }

                if (updateBuilder.Length > 0)
                {
                    session.CreateSQLQuery(updateBuilder.ToString()).ExecuteUpdate();
                    updateBuilder.Length = 0;
                }
            }

            return new JsonNetResult(new {message = string.Format("Исправлено {0} домов", fixedCount)});
        }

        private StreetProxy ProcessLine(string line, out string municipality)
        {
            var pattern = @"(?<suffix>(ул|просп|бул|мкр|пер|проспект|бульвар|проезд|наб|пл|ш|б-р|пр-кт|п|д|)\.*)(?<street>.*).*";

            var splits = line.Split(new[] {";"}, StringSplitOptions.None);
            if (splits.Count() < 3)
            {
                municipality = null;
                return null;
            }

            if (splits[0].IsEmpty() || splits[1].IsEmpty())
            {
                municipality = null;
                return null;
            }

            var street = new StreetProxy()
            {
                RawStreetName = splits[0]
            };

            var match = Regex.Match(splits[0], pattern,
                RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                municipality = null;
                return null;
            }

            var matchedStreet = match.Groups["street"];
            var matchedSuffix = match.Groups["suffix"];

            if (matchedStreet.Value.IsEmpty())
            {
                municipality = null;
                return null;
            }

            street.NormalizedList.Add(string.Format("{0} {1}", matchedStreet.Value.Trim(), matchedSuffix.Value.Trim()).Trim().ToUpper());
            street.NormalizedList.Add(string.Format("{0} {1}", matchedSuffix.Value.Trim(), matchedStreet.Value.Trim()).Trim().ToUpper());

            street.Numbers = GetNumbers(splits[2]);

            municipality = splits[1];
            return street;
        }

        private List<string> GetNumbers(string s)
        {
            var result = new List<string>();

            if (s.IsEmpty())
            {
                return result;
            }

            var splits = s.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var split in splits)
            {
                if (split.Contains("-"))
                {
                    var subSplits = split.Split(new[] {"-"}, StringSplitOptions.RemoveEmptyEntries);
                    if (subSplits.Count() < 2)
                    {
                        continue;
                    }

                    var leftEdge = Regex.Replace(subSplits[0], "[^0-9]", string.Empty).ToInt();
                    var rightEdge = Regex.Replace(subSplits[1], "[^0-9]", string.Empty).ToInt();

                    if ((leftEdge == 0 || rightEdge == 0) && leftEdge >= rightEdge)
                    {
                        continue;
                    }

                    while (leftEdge <= rightEdge)
                    {
                        result.Add(leftEdge.ToStr().ToUpper());
                        leftEdge++;
                    }
                }
                else if (split == "четные" || split == "нечетные")
                {
                    var isEven = split == "четные";
                    Enumerable.Range(1, 500).ForEach(x =>
                    {
                        if (x % 2 == 0 && isEven)
                        {
                            result.Add(x.ToString());
                        }
                        else if (x % 2 != 0 && !isEven)
                        {
                            result.Add(x.ToString());
                        }
                    });
                }
                else
                {
                    result.Add(split.ToUpper());
                }
            }

            return result;
        }

        public class MunicipalityProxy
        {
            public string Municipality { get; set; }

            public List<StreetProxy> Streets { get; set; }

            public MunicipalityProxy()
            {
                Streets = new List<StreetProxy>();
            }
        }

        public class StreetProxy
        {
            public string RawStreetName { get; set; }

            public List<string> NormalizedList { get; set; }

            public List<string> Numbers { get; set; }

            public StreetProxy()
            {
                NormalizedList = new List<string>();
                Numbers = new List<string>();
            }
        }
    }
}