using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.Dto;

    /// <summary>
    /// Интерфес сервиса для массовой работы с BasePersonalAccountDto
    /// <para>Использовать при массовых импортах ЛС</para> 
    /// </summary>
    /// <remarks>
    /// Этапы работы с сервисом:
    /// 1. Инициализируем кэш (InitCache)
    /// 2. Добавляем изменения ЛС (AddPersonalAccount)
    /// 3. Применяем изменения (ApplyChanges)
    /// </remarks>
    public interface IMassPersonalAccountDtoService
    {
        /// <summary>
        /// Добавить ЛС к создаваемым/изменяемым
        /// </summary>
        /// <param name="account">Лицевой счёт</param>
        void AddPersonalAccount(BasePersonalAccount account);

        /// <summary>
        /// Применить все изменения
        /// </summary>
        void ApplyChanges(bool rebuildCache = false);

        /// <summary>
        /// Инициализирвоать кэш существующих в Dto ЛС
        /// </summary>
        void InitCache();

        /// <summary>
        /// Удаляет все изменения
        /// </summary>
        void Clear();

        /// <summary>
        /// Массовое создание <see cref="BasePersonalAccountDto"/>.
        /// <para>Если отношение существует, то не пересоздаёт, если не указан параметр <paramref name="force"/></para>
        /// </summary>
        /// <param name="force">Принудительно пересоздаёт отношение, если оно уже существует</param>
        IDataResult MassCreatePersonalAccountDto(bool force);

        /// <summary>
        /// Использовать сессию без состояния
        /// </summary>
        void UseStatelessSession(bool useStateless = true);
    }
}