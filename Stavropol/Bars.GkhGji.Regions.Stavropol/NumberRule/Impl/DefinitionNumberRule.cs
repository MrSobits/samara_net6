namespace Bars.GkhGji.Regions.Stavropol.NumberRule.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class DefinitionNumberRule : IDefinitionNumberRule
    {
        public IDomainService<ActCheckDefinition> ActCheckDefinitionDomain { get; set; }
        public IDomainService<ProtocolDefinition> ProtocolDefinitionDomain { get; set; }
        public IDomainService<ProtocolMhcDefinition> ProtocolMhcDefinitionDomain { get; set; }
        public IDomainService<ResolutionDefinition> ResolutionDefinitionDomain { get; set; }

        /// <summary>
        /// Устанавливает значение номера определения исходя из максимального в текущем году + 1
        /// </summary>
        public int SetNumber(DateTime definitionDate)
        {
            var actCheckDefMaxNumber =
                ActCheckDefinitionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue 
                        && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .SafeMax(x => x.DocumentNumber);

            var protocolDefMaxNumber =
                ProtocolDefinitionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue
                        && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .SafeMax(x => x.DocumentNumber);

            var protocolMhcDefMaxNumber =
                ProtocolMhcDefinitionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue
                        && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .SafeMax(x => x.DocumentNumber);

            var resolutionDefMaxNumber =
                ResolutionDefinitionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue
                        && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .SafeMax(x => x.DocumentNumber);

            return Utils.Max(actCheckDefMaxNumber ?? 0, protocolDefMaxNumber ?? 0, protocolMhcDefMaxNumber ?? 0, resolutionDefMaxNumber ?? 0)
                + 1;
        }

        /// <summary>
        /// Проверяет номер определения на уникальность
        /// </summary>
        public Definition CheckNumber(DateTime definitionDate, int number, long entityId, Type entityType)
        {
            var actCheckDefNumberList =
                ActCheckDefinitionDomain.GetAll()
                    .Where(x => x.DocumentDate.HasValue 
                        && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .WhereIf(entityType == typeof(ActCheckDefinition), x => x.Id != entityId)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate
                    });

            var protocolDefMaxNumber =
                ProtocolDefinitionDomain.GetAll()
                    .Where(
                        x =>
                        x.DocumentDate.HasValue && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .WhereIf(entityType == typeof(ProtocolDefinition), x => x.Id != entityId)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate
                    });

            var protocolMhcDefMaxNumber =
                ProtocolMhcDefinitionDomain.GetAll()
                    .Where(
                        x =>
                        x.DocumentDate.HasValue && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .WhereIf(entityType == typeof(ProtocolMhcDefinition), x => x.Id != entityId)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate
                    });

            var resolutionDefMaxNumber =
                ResolutionDefinitionDomain.GetAll()
                    .Where(
                        x =>
                        x.DocumentDate.HasValue && x.DocumentDate.Value.Year == definitionDate.Year
                        && x.DocumentNumber.HasValue)
                    .WhereIf(entityType == typeof(ResolutionDefinition), x => x.Id != entityId)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate
                    });

            if (actCheckDefNumberList.Any(x => x.DocumentNumber == number))
            {
                var first = actCheckDefNumberList.FirstOrDefault(x => x.DocumentNumber == number);
                return new Definition
                {
                    IsExists = true,
                    DefinitionDate = first.DocumentDate.Value,
                    DocumentOfDefinition = DocumentOfDefinition.ActCheck
                };
            }

            if (protocolDefMaxNumber.Any(x => x.DocumentNumber == number))
            {
                var first = protocolDefMaxNumber.FirstOrDefault(x => x.DocumentNumber == number);
                return new Definition
                {
                    IsExists = true,
                    DefinitionDate = first.DocumentDate.Value,
                    DocumentOfDefinition = DocumentOfDefinition.Protocol
                };
            }

            if (protocolMhcDefMaxNumber.Any(x => x.DocumentNumber == number))
            {
                var first = protocolMhcDefMaxNumber.FirstOrDefault(x => x.DocumentNumber == number);
                return new Definition
                {
                    IsExists = true,
                    DefinitionDate = first.DocumentDate.Value,
                    DocumentOfDefinition = DocumentOfDefinition.ProtocolMhc
                };
            }

            if (resolutionDefMaxNumber.Any(x => x.DocumentNumber == number))
            {
                var first = resolutionDefMaxNumber.FirstOrDefault(x => x.DocumentNumber == number);
                return new Definition
                {
                    IsExists = true,
                    DefinitionDate = first.DocumentDate.Value,
                    DocumentOfDefinition = DocumentOfDefinition.Resolution
                };
            }

            return new Definition
            {
                IsExists = false
            };
        }

        public class Definition
        {
            public bool IsExists { get; set; }
            public DateTime DefinitionDate { get; set; }
            public DocumentOfDefinition DocumentOfDefinition { get; set; }
        }

        public enum DocumentOfDefinition
        {
            /// <summary>
            /// Акт проверки
            /// </summary>
            [Display("Акт проверки")]
            ActCheck = 0,
            
            /// <summary>
            /// Протокол
            /// </summary>
            [Display("Протокол")]
            Protocol = 1,

            /// <summary>
            /// Протокол органа муниципального жилищного контроля
            /// </summary>
            [Display("Протокол органа муниципального жилищного контроля")]
            ProtocolMhc = 2,

            /// <summary>
            /// Постановление
            /// </summary>
            [Display("Постановление")]
            Resolution = 3
        }
    }
}