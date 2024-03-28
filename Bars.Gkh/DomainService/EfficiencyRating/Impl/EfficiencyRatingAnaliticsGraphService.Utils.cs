namespace Bars.Gkh.DomainService.EfficiencyRating.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.PlotBuilding.Model;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Enums.EfficiencyRating;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с аналитикой Рейтинга эффективности
    /// </summary>
    public partial class EfficiencyRatingAnaliticsGraphService
    {
        private static class Helper
        {
            public static IPlotData GetPlotData(GraphParams graphParams, IEnumerable<ISerie> series)
            {
                BasePlotData data;
                switch (graphParams.DiagramType)
                {
                    case DiagramType.LineDiagram:
                        data = new SplinePlot();
                        break;
                    case DiagramType.BarDiagram:
                        data = new BarPlot();
                        break;
                    case DiagramType.LogarithmicChart:
                        data = new AreaSplinePlot();
                        break;
                    case DiagramType.PieGraph:
                        data = new PiePlot();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                data.Series = series;
                data.Title = graphParams.Name;

                return data;
            }
        }

        private class GraphParams : BaseParams
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public AnaliticsLevel AnaliticsLevel { get; set; }
            public Category Category { get; set; }
            public ViewParam ViewParam { get; set; }
            public IPlotData Data { get; set; }
            public string FactorCode { get; set; }
            public DiagramType DiagramType { get; set; }
            public long[] Periods { get; set; }
            public long[] Municipalities { get; set; }
            public long[] ManagingOrganizations { get; set; }

            public EfficiencyRatingAnaliticsGraph ToEfficiencyRatingAnaliticsGraph(IWindsorContainer container)
            {
                var municipalityDomain = container.ResolveDomain<Municipality>();
                var manorgDomain = container.ResolveDomain<ManagingOrganization>();
                var periodDomain = container.ResolveDomain<EfficiencyRatingPeriod>();

                using (container.Using(municipalityDomain, manorgDomain, periodDomain))
                {
                    return new EfficiencyRatingAnaliticsGraph
                    {
                        AnaliticsLevel = this.AnaliticsLevel,
                        Category = this.Category,
                        Data = this.Data,
                        Name = this.Name,
                        DiagramType = this.DiagramType,
                        FactorCode = this.FactorCode,
                        ViewParam = this.ViewParam,
                        ManagingOrganizations = this.ManagingOrganizations.IsNotEmpty() ? manorgDomain.GetAll().WhereContains(x => x.Id, this.ManagingOrganizations).ToHashSet() : null,
                        Municipalities = this.Municipalities.IsNotEmpty() ? municipalityDomain.GetAll().WhereContains(x => x.Id, this.Municipalities).ToHashSet() : null,
                        Periods = periodDomain.GetAll().WhereContains(x => x.Id, this.Periods).ToHashSet()
                    };
                }
            }

            public static GraphParams FromEntity(EfficiencyRatingAnaliticsGraph graph, BaseParams baseParams)
            {
                return new GraphParams
                {
                    Id = graph.Id,
                    Name = graph.Name,
                    Periods = graph.Periods.Select(x => x.Id).ToArray(),
                    Municipalities = graph.Municipalities.Select(x => x.Id).ToArray(),
                    ManagingOrganizations = graph.ManagingOrganizations.Select(x => x.Id).ToArray(),
                    DiagramType = graph.DiagramType,
                    AnaliticsLevel = graph.AnaliticsLevel,
                    Category = graph.Category,
                    ViewParam = graph.ViewParam,
                    FactorCode = graph.FactorCode,
                    Params = baseParams.Params,
                    Files = baseParams.Files,
                    Data = graph.Data
                };
            }

            public static GraphParams FromBaseParams(BaseParams baseParams)
            {
                GraphParams graphParams = null;

                if (baseParams.Params.ContainsKey("records"))
                {
                    var firstRec = baseParams.Params["records"].As<List<object>>().FirstOrDefault() as DynamicDictionary;
                    if (firstRec.IsNotNull())
                    {
                        graphParams = firstRec.ReadClass<GraphParams>(true, x => x.Data);
                        graphParams.Data = JsonNetConvert.DeserializeObject<BasePlotData>(firstRec["Data"].ToJson());
                    }
                }
                else
                {
                    graphParams = new GraphParams
                    {
                        Id = baseParams.Params.GetAsId(),
                        Name = baseParams.Params.GetAs("Name", string.Empty),
                        FactorCode = baseParams.Params.GetAs("FactorCode", string.Empty),
                        AnaliticsLevel = baseParams.Params.GetAs<AnaliticsLevel>("AnaliticsLevel"),
                        Category = baseParams.Params.GetAs<Category>("Category"),
                        ViewParam = baseParams.Params.GetAs<ViewParam>("ViewParam"),
                        DiagramType = baseParams.Params.GetAs<DiagramType>("DiagramType"),
                        Periods = baseParams.Params.GetAs<long[]>("Periods"),
                        ManagingOrganizations = baseParams.Params.GetAs<long[]>("ManagingOrganizations"),
                        Municipalities = baseParams.Params.GetAs<long[]>("Municipalities"),
                        Data = baseParams.Params.ContainsKey("Data")
                        ? JsonNetConvert.DeserializeObject<BasePlotData>(baseParams.Params["Data"].ToJson())
                        : null,
                        Params = baseParams.Params,
                        Files = baseParams.Files
                    };
                }

                return graphParams;
            }
        }

        private class GraphDataProxy
        {
            private decimal value;

            public Municipality Municipality { get; set; }
            public ManagingOrganization ManagingOrganization { get; set; }
            public EfficiencyRatingPeriod Period { get; set; }
            public ManagingOrganizationDataValue DataValue { get; set; }


            public decimal Value
            {
                get
                {
                    return this.DataValue?.Value.ToDecimal() ?? this.value;
                }
                set
                {
                    this.value = value;
                }
            }
        }

        private class GraphData
        {
            public string SerieName { get; set; }
            public string XName { get; set; }
            public decimal Value { get; set; }
        }
    }
}