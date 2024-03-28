namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ExportFormatProviderBuilder : IExportFormatProviderBuilder
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<Contragent> ContragentDomainService { get; set; }
        public IDomainService<User> UserDomainService { get; set; }
        public IExportableEntityResolver ExportableEntityResolver { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroups { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        private BaseParams baseParams = new BaseParams();
        private LogOperation logOperation;
        private CancellationToken cancellationToken = CancellationToken.None;
        private IList<string> entityGroupCodeList;

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetLogOperation(LogOperation logOperation)
        {
            this.logOperation = logOperation;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetParams(BaseParams builderParams)
        {
            this.baseParams = builderParams;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetEntytyGroupCodeList(IList<string> entityGroupCodes)
        {
            this.entityGroupCodeList = entityGroupCodes;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProvider Build<T>() where T : BaseFormatProvider, new()
        {
            var provider = this.Container.Resolve<T>();

            provider.CancellationToken = this.cancellationToken;
            provider.DataSelectorParams.Apply(this.baseParams.Params);
            this.ApplyConfigParams(provider.DataSelectorParams);

            var userId = provider.DataSelectorParams.GetAsId("UserId");
            var contragentId = provider.DataSelectorParams.GetAsId("MainContragent");

            provider.User = this.UserDomainService.Get(userId);
            provider.Contragent = this.ContragentDomainService.Get(contragentId);
            provider.LogOperation = this.logOperation ?? new LogOperation
            {
                User = provider.User,
                StartDate = DateTime.Now,
                OperationType = LogOperationType.FormatDataExport,
                Comment = $"Ошибки при экспорте данных со сведениями ЖКХ по формату {provider.FormatVersion}"
            };

            var providerType = this.FormatDataExportRoleService.GetProviderType(provider.UserRole);
            var entityGroupCodes = this.entityGroupCodeList.IsEmpty()
                ? provider.DataSelectorParams.GetAs("EntityGroupCodes", new List<string>())
                : this.entityGroupCodeList;

            provider.SectionGroupNames = entityGroupCodes;

            provider.SelectedEntityCodeList = this.ExportableEntityGroups
                .GroupBy(g => g.Code)
                .Where(x => entityGroupCodes.Contains(x.Key))
                .SelectMany(group =>
                {
                    var inheritedEntityCodeList = group.First().InheritedEntityCodeList.ToList();
                    var trigger = true;
                    
                    foreach (var data in group)
                    {
                        if (trigger)
                        {
                            trigger = !trigger;
                            continue;
                        }
                    
                        inheritedEntityCodeList.AddRange(data.InheritedEntityCodeList);
                    }
                    
                    return inheritedEntityCodeList;
                })
                .ToHashSet();

            var entities = entityGroupCodes.IsEmpty()
                ? this.ExportableEntityResolver.GetEntityList(providerType)
                : this.ExportableEntityResolver.GetInheritedEntityList(entityGroupCodes, providerType);

            foreach (var code in provider.ServiceEntityCodes)
            {
                entities.Add(this.ExportableEntityResolver.GetEntity(code, providerType));
            }

            provider.InitExportableEntities(entities);

            return provider;
        }

        private void ApplyConfigParams(DynamicDictionary dataSelectorParams)
        {
            var config = this.Container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport;

            var paramsConfig = config.FormatDataExportParams;

            dataSelectorParams["GjiContragentId"] = paramsConfig.GjiContragent.Id;
            dataSelectorParams["LeaderPositionId"] = paramsConfig.LeaderPosition.Id;
            dataSelectorParams["AccountantPositionId"] = paramsConfig.AccountantPosition.Id;
            dataSelectorParams["TsjChairmanPosition"] = paramsConfig.TsjChairmanPosition.Id;
            dataSelectorParams["TsjMemberPositionId"] = paramsConfig.TsjMemberPosition.Id;
            dataSelectorParams["TimeZone"] = config.FormatDataExportGeneral.TimeZone;
        }
    }
}