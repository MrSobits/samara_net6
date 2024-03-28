namespace Bars.Gkh.DomainService.EfficiencyRating.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.PlotBuilding.Model;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Enums.EfficiencyRating;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с аналитикой Рейтинга эффективности
    /// </summary>
    public partial class EfficiencyRatingAnaliticsGraphService : IEfficiencyRatingAnaliticsGraphService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="EfficiencyRatingAnaliticsGraph"/>
        /// </summary>
        public IDomainService<EfficiencyRatingAnaliticsGraph> EfficiencyRatingAnaliticsGraphDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganizationEfficiencyRating"/>
        /// </summary>
        public IDomainService<ManagingOrganizationEfficiencyRating> ManagingOrganizationEfficiencyRatingDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganizationDataValue"/>
        /// </summary>
        public IDomainService<ManagingOrganizationDataValue> ManagingOrganizationDataValueDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="EfficiencyRatingPeriod"/>
        /// </summary>
        public IDomainService<EfficiencyRatingPeriod> EfficiencyRatingPeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganization"/>
        /// </summary>
        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        /// <inheritdoc />
        public IDataResult GetGraph(BaseParams baseParams)
        {
            var graphParams = GraphParams.FromBaseParams(baseParams);

            return new BaseDataResult(this.GetGraphInternal(graphParams));
        }

        /// <inheritdoc />
        public IDataResult SaveOrUpdateGraph(BaseParams baseParams)
        {
            var graphParams = GraphParams.FromBaseParams(baseParams);

            EfficiencyRatingAnaliticsGraph entity = null;

            this.Container.InTransaction(() =>
            {
                if (graphParams.Id > 0)
                {
                    this.EfficiencyRatingAnaliticsGraphDomain.Delete(graphParams.Id);
                    graphParams.Id = 0;
                }

                if (graphParams.Data.IsNull())
                {
                    graphParams.Data = this.GetGraphInternal(graphParams);
                }

                var plotData = graphParams.Data as BasePlotData;
                if (plotData.IsNotNull())
                {
                    plotData.Title = graphParams.Name;
                }

                entity = graphParams.ToEfficiencyRatingAnaliticsGraph(this.Container);

                this.EfficiencyRatingAnaliticsGraphDomain.Save(entity);
            });

            // возвращаем через ListDataResult, т.к. аспект работает как будто со списком
            var listResult = new List<EfficiencyRatingAnaliticsGraph> { entity };
            return new ListDataResult(listResult, 1);
        }

        private IPlotData GetGraphInternal(GraphParams graphParams)
        {
            var query = this.GetData(graphParams)
               .WhereContains(x => x.Period.Id, graphParams.Periods)
               .WhereIf(graphParams.Municipalities.IsNotEmpty(), x => graphParams.Municipalities.Contains(x.Municipality.Id))
               .WhereIf(graphParams.ManagingOrganizations.IsNotEmpty(), x => graphParams.ManagingOrganizations.Contains(x.ManagingOrganization.Id));

            var graph = this.GetGraph(query, graphParams);
            var series = this.GetSeries(graph, graphParams);

            return Helper.GetPlotData(graphParams, series);
        }

        private IQueryable<GraphDataProxy> GetData(GraphParams graphParams)
        {
            IQueryable<GraphDataProxy> query;

            switch (graphParams.Category)
            {
                case Category.RatingValue:
                    query = this.ManagingOrganizationEfficiencyRatingDomain.GetAll()
                        .Select(x => new GraphDataProxy
                        {
                            Period = x.Period,
                            ManagingOrganization = x.ManagingOrganization,
                            Municipality = x.ManagingOrganization.Contragent.Municipality,
                            Value = x.Rating
                        });
                    break;
                case Category.FactorValue:
                    query = this.ManagingOrganizationDataValueDomain.GetAll()
                        .Where(x => x.MetaInfo.Level == 0 && x.MetaInfo.Code == graphParams.FactorCode)
                        .Select(x => new GraphDataProxy
                        {
                            Period = x.EfManagingOrganization.Period,
                            ManagingOrganization = x.EfManagingOrganization.ManagingOrganization,
                            Municipality = x.EfManagingOrganization.ManagingOrganization.Contragent.Municipality,
                            DataValue = x
                        });

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return query;
        }

        private IDictionary<string, GraphData[]> GetGraph(IQueryable<GraphDataProxy> query, GraphParams graphParams)
        {
            Func<GraphData, string> groupBySerie;
            Func<GraphData, string> groupByData;

            Func<GraphDataProxy, GraphData> dataSelector = y => new GraphData
            {
                SerieName = y.ManagingOrganization.Contragent.Name,
                XName = y.Period.Name,
                Value = y.Value
            };

            if (graphParams.AnaliticsLevel == AnaliticsLevel.ByMunicipality)
            {
                dataSelector = y => new GraphData
                {
                    SerieName = y.Municipality.Name,
                    XName = y.Period.Name,
                    Value = y.Value
                };
            }

            if (graphParams.ViewParam == ViewParam.ByLevel)
            {
                groupBySerie = x => x.SerieName;
                groupByData = x => x.XName;
            }
            else
            {
                groupBySerie = x => x.XName;
                groupByData = x => x.SerieName;
            }

            return query
                .AsEnumerable()
                .Select(dataSelector)
                .GroupBy(groupBySerie)
                .ToDictionary(
                    x => x.Key, 
                    y => y
                        .GroupBy(groupByData)
                        .Select(x => new GraphData
                        {
                            XName = groupByData(x.First()),
                            Value = x.Sum(z => z.Value)
                        })
                        .ToArray());
        }

        private IEnumerable<ISerie> GetSeries(IDictionary<string, GraphData[]> data, GraphParams graphParams)
        {
            return data.Select(x => new Serie(x.Key)
            {
                Data = x.Value.Select(y => new object[] { y.XName, y.Value }).ToArray()
            });
        }
    }
}