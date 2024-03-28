namespace Bars.Gkh.FormatDataExport.Domain
{
    using System.ComponentModel;

    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;

    public interface IFormatDataExportRoleService
    {
        /// <summary>
        /// Получить результирующий флаг доступа по ролям оператора
        /// </summary>
        /// <param name="user">Пользователь в системе</param>
        /// <exception cref="InvalidEnumArgumentException">Для роли не определен тип поставщика информации</exception>
        FormatDataExportProviderFlags GetCustomProviderFlags(User user);

        /// <summary>
        /// Получить тип поставщика информации по роли
        /// </summary>
        /// <param name="operatorRole">Роль оператора в системе</param>
        /// <exception cref="InvalidEnumArgumentException">Для роли не определен тип поставщика информации</exception>
        FormatDataExportProviderType GetProviderType(Role operatorRole);

        /// <summary>
        /// Получить флаг поставщика информации по роли
        /// </summary>
        /// <param name="providerType">Тип поставщика информации</param>
        /// <exception cref="InvalidEnumArgumentException">Для роли не определен тип поставщика информации</exception>
        FormatDataExportProviderFlags GetProviderFlag(FormatDataExportProviderType providerType);
    }
}