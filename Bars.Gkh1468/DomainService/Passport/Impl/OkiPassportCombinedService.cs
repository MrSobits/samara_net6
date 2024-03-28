using Bars.Gkh.Utils;

namespace Bars.Gkh1468.DomainService.Impl
{
    using DomainService;
    using Castle.Windsor;
    using B4;
    using Entities;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4.Utils;
    using Enums;
    using Gkh.Entities;

    public class OkiPassportCombinedService : IOkiPassportCombinedService
    {
        public IWindsorContainer Container { get; set; }

        public class CombinedPassport
        {
            public string PpNumber;
            public string Name;
            public string Value;
            public string InfoSupplier;
            public bool IsMultiple;
        }

        public class Attribute
        {
            public string Code;
            public string Name;
            public IEnumerable<AttrValue> Values;
        }

        public class AttrValue
        {
            public string Value;
            public ValueType ValueType;
            public int Exp;
            public Contragent Contragent;
        }

        public IDataResult GetList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var passportId = baseParams.Params.GetAs<long>("passport");
            var stateId = baseParams.Params.GetAs<long>("stateId");

            var providerPassport = Container.Resolve<IDomainService<OkiProviderPassport>>().GetAll()
                .Where(x => x.OkiPassport.Id == passportId)
                .WhereIf(stateId != 0, x => x.State.Id == stateId)
                .Select(x => x)
                .Filter(loadParam, Container);

            var provPasppIds = providerPassport.Select(x => x.Id).ToList();

            var list = new List<CombinedPassport>();

            var data = providerPassport.AsEnumerable().FirstOrDefault();
            var paspStruct = data.Return(x => x.PassportStruct);

            if (paspStruct == null)
            {
                var housePassport = Container.Resolve<IDomainService<OkiPassport>>()
                    .GetAll()
                    .Where(x => x.Id == passportId)
                    .Select(x => x)
                    .FirstOrDefault();

                if (housePassport == null)
                {
                    list.Add(new CombinedPassport { Name = "Нет паспорта!" });
                    return new ListDataResult(list, list.Count);
                }

                paspStruct = Container.Resolve<IDomainService<PassportStruct>>()
                    .GetAll()
                    .Where(x => (housePassport.ReportYear - x.ValidFromYear) * 12 + housePassport.ReportMonth - x.ValidFromMonth >= 0)
                    .Where(x => x.PassportType == PassportType.Nets)
                    .OrderByDescending(x => x.ValidFromYear)
                    .ThenByDescending(x => x.ValidFromMonth)
                    .Select(x => x)
                    .FirstOrDefault();
            }

            if (paspStruct == null)
            {
                list.Add(new CombinedPassport { Name = "Нет структуры паспорта на отчетный период!" });
                return new ListDataResult(list, list.Count);
            }

            var maService = Container.Resolve<IDomainService<MetaAttribute>>();
            var hprService = Container.Resolve<IDomainService<OkiProviderPassportRow>>();

            var partsService = Container.Resolve<IDomainService<Part>>();
            var parts = partsService.GetAll()
                .Where(x => x.Struct.Id == paspStruct.Id)
                .Where(x => x.Parent == null)
                .OrderBy(x => x.Code)
                .ToList()
                .Select(x => new
                {
                    x.Code,
                    x.Name,
                    MetaAttributes = maService.GetAll()
                        .OrderBy(y => y.Code)
                        .Where(y => y.ParentPart.Id == x.Id)
                        .ToList()
                        .Select(z => new Attribute
                        {
                            Code = string.Format("{0}", z.Code),
                            Name = z.Name,
                            Values = hprService.GetAll()
                            .Where(a => a.MetaAttribute.Id == z.Id && provPasppIds.Contains(a.ProviderPassport.Id))
                            .Select(b => new AttrValue
                            {
                                Value = b.Value,
                                ValueType = b.MetaAttribute.ValueType,
                                Exp = b.MetaAttribute.Exp,
                                Contragent = b.ProviderPassport.Contragent
                            })
                        }),

                    SubParts = partsService.GetAll()
                        .OrderBy(y => y.Code)
                        .Where(y => y.Parent.Id == x.Id)
                        .ToList()
                        .Select(y => new
                        {
                            Code = string.Format("{0}", y.Code),
                            y.Name,
                            MetaAttributes = maService.GetAll()
                                .OrderBy(z => z.Code)
                                .Where(z => z.ParentPart.Id == y.Id)
                                .ToList()
                                .Select(a => new Attribute
                                {
                                    Code = string.Format("{0}", a.Code),
                                    Name = a.Name,
                                    Values = hprService.GetAll()
                                    .Where(b => b.MetaAttribute.Id == a.Id && provPasppIds.Contains(b.ProviderPassport.Id))
                                    .Select(c => new AttrValue
                                    {
                                        Value = c.Value,
                                        ValueType = c.MetaAttribute.ValueType,
                                        Exp = c.MetaAttribute.Exp,
                                        Contragent = c.ProviderPassport.Contragent
                                    })
                                })
                        })
                });

            foreach (var part in parts)
            {
                list.Add(new CombinedPassport { PpNumber = part.Code, Name = part.Name });
                list.AddRange(AddMetaAttributes(part.MetaAttributes));

                foreach (var subPart in part.SubParts)
                {
                    list.Add(new CombinedPassport { PpNumber = subPart.Code, Name = subPart.Name });
                    list.AddRange(AddMetaAttributes(subPart.MetaAttributes));
                }
            }

            return new ListDataResult(list, list.Count);
        }

        private IEnumerable<CombinedPassport> AddMetaAttributes(IEnumerable<Attribute> attributes)
        {
            var list = new List<CombinedPassport>();

            foreach (var attr in attributes)
            {
                var count = attr.Values.Count();
                if (count == 0)
                {
                    list.Add(new CombinedPassport { PpNumber = attr.Code, Name = attr.Name });
                }
                else if (count == 1)
                {
                    var val = attr.Values.FirstOrDefault();
                    list.Add(new CombinedPassport { PpNumber = attr.Code, Name = attr.Name, Value = val.Return(r => r.Value) });
                }
                else
                {
                    var valGroups = attr.Values.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Value).Distinct().ToArray();

                    if (valGroups.Count() > 1)
                    {
                        // ReSharper disable once LoopCanBeConvertedToQuery
                        foreach (var value in attr.Values.Where(x => !string.IsNullOrEmpty(x.Value)))
                        {
                            list.Add(new CombinedPassport
                            {
                                PpNumber = attr.Code,
                                Name = attr.Name,
                                Value = value.Value,
                                InfoSupplier = string.Format("Значение из паспорта контрагента {0}", value.Contragent.Name),
                                IsMultiple = true
                            });
                        }
                    }
                    else
                    {
                        var contragents = attr.Values.Where(x => !string.IsNullOrEmpty(x.Value)).Select(x => x.Contragent.Name).Distinct().ToArray();

                        list.Add(new CombinedPassport
                        {
                            PpNumber = attr.Code,
                            Name = attr.Name,
                            Value = valGroups.FirstOrDefault(),
                            InfoSupplier = contragents.Any() ? contragents.Count() > 1 ? string.Format("Значение из паспортов контрагентов {0}", contragents.AggregateWithSeparator(", ")) :
                            string.Format("Значение из паспорта контрагента {0}", contragents.FirstOrDefault()) : string.Empty,
                            IsMultiple = false
                        });
                    }

                }
            }

            return list;
        }
    }
}
