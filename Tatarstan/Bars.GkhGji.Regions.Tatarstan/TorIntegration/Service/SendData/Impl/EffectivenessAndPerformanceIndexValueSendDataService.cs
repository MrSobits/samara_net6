namespace Bars.GkhGji.Regions.Tatarstan.TorIntegration.Service.SendData.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    // TODO: Расскоментировать после перевода  GisIntegration
   /* using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Tor.Enums;
    using Bars.GisIntegration.Tor.GraphQl;
    using Bars.GisIntegration.Tor.Service.SendData.Impl;*/
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Fasterflect;
    using Newtonsoft.Json;

    using ServiceStack;
    
    // TODO: Расскоментировать после перевода  GisIntegration
   // using Formatting = Bars.GisIntegration.Tor.GraphQl.Formatting;

    /*public class EffectivenessAndPerformanceIndexValueSendDataService : BaseSendDataService<EffectivenessAndPerformanceIndexValue>
    {
        private readonly IDomainService<EffectivenessAndPerformanceIndex> efpIndexDomain;
        private readonly IDomainService<EffectivenessAndPerformanceIndexValue> efpIndexValueDomain;

        public EffectivenessAndPerformanceIndexValueSendDataService(IDomainService<EffectivenessAndPerformanceIndex> efpIndexDomain,
            IDomainService<EffectivenessAndPerformanceIndexValue> efpIndexValueDomain)
        {
            this.efpIndexDomain = efpIndexDomain;
            this.efpIndexValueDomain = efpIndexValueDomain;
            this.TypeObject = TypeObject.RandEParameter;
        }

        public override IDataResult PrepareData(BaseParams baseParams)
        {
            var documentIdsHash = Array.ConvertAll(baseParams.Params.GetAs<string>("ids")?.Split(","),
                input => long.TryParse(input, out var id) ? id : default(long)).Where(x => x != 0).Distinct().ToHashSet();

            using (this.Container.Using(this.efpIndexDomain, this.efpIndexValueDomain))
            {
                var effectivenessAndPerformanceIndexValuesDict = this.efpIndexValueDomain.GetAll()
                    .Where(x => documentIdsHash.Contains(x.Id))
                    .ToDictionary(x => x.Id);

                var selectedEfpIndex = effectivenessAndPerformanceIndexValuesDict.Values
                    .Select(x => x.EffectivenessAndPerformanceIndex.Id)
                    .Distinct().ToList();

                var efpIndexList = this.efpIndexDomain.GetAll()
                    .Where(x => x.TorId == null && selectedEfpIndex.Contains(x.Id))
                    .ToList();

                if (efpIndexList.Any())
                {
                    foreach (var entity in efpIndexList)
                    {
                        this.TypeRequest = TypeRequest.Initialization;
                        this.CreateRandEParameter(entity);
                    }

                    this.SendRequest(false);
                }

                foreach (var id in documentIdsHash)
                {
                    if (!effectivenessAndPerformanceIndexValuesDict.TryGetValue(id, out var efpValue))
                    {
                        continue;
                    }

                    this.SendObject = efpValue;
                    this.TypeRequest = this.SendObject?.TorId == null ? TypeRequest.Initialization : TypeRequest.Correction;

                    var parameterName = string.IsNullOrWhiteSpace(this.SendObject.EffectivenessAndPerformanceIndex.ParameterName)
                        ? this.SendObject.EffectivenessAndPerformanceIndex?.Name
                        : this.SendObject.EffectivenessAndPerformanceIndex?.ParameterName;

                    if (this.TypeRequest == TypeRequest.Initialization)
                    {
                        this.CreateRandEParameterValue(parameterName);
                        continue;
                    }

                    this.UpdateRandEParameterValue(parameterName);
                }

                return this.SendRequest();
            }
        }

        private void CreateRandEParameter(EffectivenessAndPerformanceIndex entity)
        { 
            var createRandEParameter = new MutationQueryBuilder()
                .WithCreateRandEParameter(
                    new RandEParameterQueryBuilder().WithId(),
                    new RandEParameterCreateInput
                    {
                        Name = entity.Name,
                        ControlOrganizationParameter = entity.ControlOrganization?.TorId != null
                            ? new Guid[1] { (Guid)entity.ControlOrganization.TorId }
                            : new Guid[0],
                        MeasureUnitTypeId = null
                    }).Build(Formatting.Indented, 2);

            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("CreateRandEParameter", createRandEParameter, entity));
        }

        private void CreateRandEParameterValue(string parameterName)
        {
            var createControlItemRandEParameter = new MutationQueryBuilder()
                .WithCreateRandEParameterValue(
                    new RandEParameterValueQueryBuilder().WithId(),
                    new RandEParameterValueCreateInput
                    {
                        ParameterName = parameterName,
                        ParameterValue = this.SendObject.Value,
                        RandEParameterId = this.SendObject.EffectivenessAndPerformanceIndex.TorId
                    }).Build(Formatting.Indented, 2);

            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("CreateRandEParameterValue", createControlItemRandEParameter, this.SendObject));
        }

        private void UpdateRandEParameterValue(string parameterName)
        {
            var updateControlItemRandEParameter = new MutationQueryBuilder()
                .WithUpdateRandEParameterValue(
                    new RandEParameterValueQueryBuilder().WithId(),
                    new RandEParameterValueUpdateInput
                    {
                        Id = this.SendObject.TorId,
                        ParameterName = parameterName,
                        ParameterValue = this.SendObject.Value,
                        RandEParameterId = this.SendObject.EffectivenessAndPerformanceIndex.TorId
                    }).Build(Formatting.Indented, 2);

            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("UpdateRandEParameterValue", updateControlItemRandEParameter, this.SendObject));
        }

        /// <inheritdoc />
        public override IDataResult GetData(BaseParams baseParams)
        {
            try
            {
                this.GetRandEParameter();
                this.GetRandEParameterValue();

                this.TypeRequest = TypeRequest.Getting;
                this.SendRequest();

                if (!this.ListResponses.Any())
                {
                    return new BaseDataResult { Success = false };
                }

                var resultList = this.ListResponses.Select(x => new { x.Item1, x.Item2 });

                foreach (var result in resultList)
                {
                    switch (this.TryGetCollection(result.Item1, result.Item2, out var collection))
                    {
                        case "FindAllRandEParameter":
                            this.AddEffectivenessAndPerformanceIndex((ICollection<RandEParameter>)collection);
                            break;
                        case "FindAllRandEParameterValue":
                            this.UpdateOrCreateEffectivenessAndPerformanceIndexValue((ICollection<RandEParameterValue>)collection);
                            break;
                        default:
                            return new BaseDataResult { Success = false };
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (Exception e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(this.efpIndexDomain);
                this.Container.Release(this.efpIndexValueDomain);
            }
        }
        private void GetRandEParameter()
        {
            var getRandEParameter = new QueryQueryBuilder()
                .WithFindAllRandEParameter(
                    new RandEParameterQueryBuilder()
                            .WithAllScalarFields()
                            .WithMeasureUnitType(new DicMeasureUnitsTypeQueryBuilder()
                                .WithAllScalarFields()),
                    limit: 10000)
                .Build(Formatting.Indented);

            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("FindAllRandEParameter", getRandEParameter, this.SendObject));
        }

        private void GetRandEParameterValue()
        {
            var getRandEParameterValue = new QueryQueryBuilder()
                .WithFindAllRandEParameterValue(
                    new RandEParameterValueQueryBuilder()
                        .WithAllScalarFields()
                        .WithRandEParameter(new RandEParameterQueryBuilder()
                            .WithAllScalarFields()
                            .WithMeasureUnitType(new DicMeasureUnitsTypeQueryBuilder()
                                .WithAllScalarFields())),
                    limit: 10000)
                .Build(Formatting.Indented);

            this.ListRequests.Add(new Tuple<string, string, IUsedInTorIntegration>("FindAllRandEParameterValue", getRandEParameterValue, this.SendObject));
        }        

        /// <summary>
        /// Попытка получить коллекцию из JSON по указанному имени свойства
        /// </summary>
        private string TryGetCollection(string typeClass, string response, out object collection)
        {
            var deserializeObject = JsonConvert.DeserializeObject<GraphQlResponse<Query>>(response);
            var data = deserializeObject.TryGetPropertyValue("Data");

            collection = data.TryGetPropertyValue(typeClass);
                
            return data is null && collection is null ? string.Empty : typeClass;
        }

        /// <summary>
        /// Добавление отсутствующих EffectivenessAndPerformanceIndex
        /// </summary>
        private void AddEffectivenessAndPerformanceIndex(ICollection<RandEParameter> allRandEParameter)
        {
            var efpIndexHash = this.efpIndexDomain.GetAll()
                .Where(x => x.TorId != null)
                .Select(x => x.TorId)
                .Distinct()
                .ToHashSet();

            foreach (var entity in allRandEParameter)
            {
                if (!entity.IsNotNull() || efpIndexHash.Contains(entity.Id)) continue;

                this.efpIndexDomain.Save(new EffectivenessAndPerformanceIndex
                {
                    TorId = entity.Id,
                    Name = entity.Name,
                    ParameterName = string.Empty,
                    UnitMeasure = entity.MeasureUnitType?.MeasureUnitTypeName,
                    Code = string.Empty,
                    //ToDo - ControlOrganization
                }); 
            }
        }

        /// <summary>
        /// Обновление существующего EffectivenessAndPerformanceIndex
        /// </summary>
        private EffectivenessAndPerformanceIndex UpdateEffectivenessAndPerformanceIndex(Dictionary<Guid?, EffectivenessAndPerformanceIndex> efpIndexDict, RandEParameterValue entity)
        {
            if (!efpIndexDict.ContainsKey(entity.RandEParameter?.Id)) return null;            

            var efpIndex = efpIndexDict[entity.RandEParameter.Id];

            efpIndex.Name = entity.RandEParameter.Name;
            efpIndex.ParameterName = entity.ParameterName;
            efpIndex.UnitMeasure = entity.RandEParameter.MeasureUnitType?.MeasureUnitTypeName;

            this.efpIndexDomain.Update(efpIndex);

            return efpIndex; 
        }

        /// <summary>
        /// Обновление или добавление EffectivenessAndPerformanceIndexValue
        /// </summary>
        private void UpdateOrCreateEffectivenessAndPerformanceIndexValue(IEnumerable<RandEParameterValue> allRandEParameterValue)
        {
            var efpIndexDict = this.efpIndexDomain.GetAll()
                .Where(x => x.TorId != null)
                .GroupBy(x => x.TorId)
                .ToDictionary(x => x.Key, x => x.First());

            var efpIndexValueDict = this.efpIndexValueDomain.GetAll()
                .Where(x => x.TorId != null)
                .ToDictionary(x => x.TorId, x => x.Id);

            foreach (var entity in allRandEParameterValue)
            {
                if (entity?.Id == null)
                {
                    continue;
                }

                if (efpIndexValueDict.ContainsKey(entity.Id))
                {
                    this.efpIndexValueDomain.Delete(efpIndexValueDict[entity.Id]);
                }

                this.efpIndexValueDomain.Save(new EffectivenessAndPerformanceIndexValue
                {
                    EffectivenessAndPerformanceIndex = this.UpdateEffectivenessAndPerformanceIndex(efpIndexDict, entity),
                    CalculationStartDate = DateTime.Now,
                    CalculationEndDate = DateTime.Now,
                    Value = entity.ParameterValue,
                    TorId = entity.Id
                });
            }
        }
    }*/
}