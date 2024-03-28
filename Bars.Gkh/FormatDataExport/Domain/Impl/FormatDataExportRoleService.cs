namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Dto;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class FormatDataExportRoleService : IFormatDataExportRoleService
    {
        private readonly IDictionary<long, FormatDataExportProviderType> roleDict
            = new Dictionary<long, FormatDataExportProviderType>();

        public FormatDataExportRoleService(IWindsorContainer container)
        {
            var roleConfig = container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportRole;

            foreach (var property in roleConfig.GetType().GetProperties())
            {
                var value = property.GetValue(roleConfig) as EntityDto<Role>;
                if (value == null || value.Id == 0)
                {
                    continue;
                }
                FormatDataExportProviderType providerType = 0;
                if (Enum.TryParse(property.Name, out providerType))
                {
                    this.roleDict.Add(value.Id, providerType);
                }
            }
        }

        /// <inheritdoc />
        public FormatDataExportProviderType GetProviderType(Role operatorRole)
        {
            if (operatorRole == null)
            {
                throw new ArgumentNullException(nameof(operatorRole), @"Не удалось определить роль оператора");
            }

            if (!this.roleDict.ContainsKey(operatorRole.Id))
            {
                throw new InvalidEnumArgumentException($@"Не удалось определить тип поставщика информации для роли '{operatorRole.Name}'");
            }

            return this.roleDict.Get(operatorRole.Id);
        }

        /// <inheritdoc />
        public FormatDataExportProviderFlags GetProviderFlag(FormatDataExportProviderType providerType)
        {
            switch (providerType)
            {
                case FormatDataExportProviderType.Uo:
                    return FormatDataExportProviderFlags.Uo;

                case FormatDataExportProviderType.Rso:
                    return FormatDataExportProviderFlags.Rso;

                case FormatDataExportProviderType.Gji:
                    return FormatDataExportProviderFlags.Gji;

                case FormatDataExportProviderType.Omjk:
                    return FormatDataExportProviderFlags.Omjk;

                case FormatDataExportProviderType.Fst:
                    return FormatDataExportProviderFlags.Fst;

                case FormatDataExportProviderType.Ogv:
                    return FormatDataExportProviderFlags.Ogv;

                case FormatDataExportProviderType.Oms:
                    return FormatDataExportProviderFlags.Oms;

                case FormatDataExportProviderType.Oiv:
                    return FormatDataExportProviderFlags.Oiv;

                case FormatDataExportProviderType.AdminOss:
                    return FormatDataExportProviderFlags.AdminOss;

                case FormatDataExportProviderType.OgvEnergo:
                    return FormatDataExportProviderFlags.OgvEnergo;

                case FormatDataExportProviderType.GjiAccounting:
                    return FormatDataExportProviderFlags.GjiAccounting;

                case FormatDataExportProviderType.RegOpCr:
                    return FormatDataExportProviderFlags.RegOpCr;

                case FormatDataExportProviderType.GkhFound:
                    return FormatDataExportProviderFlags.GkhFound;

                case FormatDataExportProviderType.Uos:
                    return FormatDataExportProviderFlags.Uos;

                case FormatDataExportProviderType.MinStroy:
                    return FormatDataExportProviderFlags.MinStroy;

                case FormatDataExportProviderType.RegOpWaste:
                    return FormatDataExportProviderFlags.RegOpWaste;

                case FormatDataExportProviderType.Rc:
                    return FormatDataExportProviderFlags.Rc;

                case FormatDataExportProviderType.Administrator:
                    return FormatDataExportProviderFlags.All;
            }

            throw new InvalidEnumArgumentException(nameof(providerType), (int)providerType, providerType.GetType());
        }

        /// <inheritdoc />
        public FormatDataExportProviderFlags GetCustomProviderFlags(User user)
        {
            var providerFlag = FormatDataExportProviderFlags.None;

            if (user != null && user.Roles.IsNotEmpty())
            {
                foreach (var userRole in user.Roles)
                {
                    var provider = this.GetProviderType(userRole.Role);
                    providerFlag |= this.GetProviderFlag(provider);
                }
            }

            return providerFlag;
        }
    }
}