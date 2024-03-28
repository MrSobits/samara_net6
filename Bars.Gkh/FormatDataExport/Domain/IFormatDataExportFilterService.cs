namespace Bars.Gkh.FormatDataExport.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Сервис фильтрации данных по контрагентам и жилым домам
    /// </summary>
    public interface IFormatDataExportFilterService
    {
        /// <summary>
        /// Использовать фильтрацию по дате изменения
        /// </summary>
        bool UseIncremental { get; }

        /// <summary>
        /// Дата начала даипазона изменений
        /// </summary>
        DateTime StartEditDate { get; }

        /// <summary>
        /// Дата окончания даипазона изменений
        /// </summary>
        DateTime EndEditDate { get; }

        /// <summary>
        /// Период начислений <see cref="ChargePeriod.Id"/>
        /// </summary>
        long PeriodId { get; }

        /// <summary>
        /// Головная организации <see cref="Contragent.Id"/>
        /// </summary>
        long MainContragentId { get; }

        /// <summary>
        /// Поставщик информации
        /// </summary>
        FormatDataExportProviderType Provider { get; }

        /// <summary>
        /// Дата за которую производится экспорт
        /// </summary>
        DateTime ExportDate { get; }

        /// <summary>
        /// UI фильтр проверок
        /// </summary>
        LoadParam InspectionFilter { get; }

        /// <summary>
        /// UI фильтр ЛС
        /// </summary>
        LoadParam PersAccFilter { get; }

        /// <summary>
        /// UI фильтр версии ДПКР
        /// </summary>
        LoadParam ProgramVersionFilter { get; }

        /// <summary>
        /// UI фильтр объектов КР
        /// </summary>
        LoadParam ObjectCrFilter { get; }

        /// <summary>
        /// UI фильтр домов
        /// </summary>
        LoadParam RealityObjectFilter { get; }

        /// <summary>
        /// UI фильтр уставов/договоров управления
        /// </summary>
        LoadParam DuUstavFilter { get; }

        /// <summary>
        /// Коллекция идентификаторов МО
        /// </summary>
        ReadOnlyCollection<long> MunicipalityIds { get; }

        /// <summary>
        /// Коллекция идентификаторов контрагентов
        /// </summary>
        ReadOnlyCollection<long> ContragentIds { get; }

        /// <summary>
        /// Коллекция идентификаторов распоряжений
        /// </summary>
        ReadOnlyCollection<long> InspectionIds { get; }

        /// <summary>
        /// Коллекция идентификаторов ЛС
        /// </summary>
        ReadOnlyCollection<long> PersAccIds { get; }

        /// <summary>
        /// Коллекция идентификаторов версии ДПКР
        /// </summary>
        ReadOnlyCollection<long> ProgramVersionIds { get; }

        /// <summary>
        /// Коллекция идентификаторов объектов КР
        /// </summary>
        ReadOnlyCollection<long> ObjectCrIds { get; }

        /// <summary>
        /// Коллекция идентификаторов домов
        /// </summary>
        ReadOnlyCollection<long> RealityObjectIds { get; }

        /// <summary>
        /// Коллекция идентификаторов договоров управления или уставов
        /// </summary>
        ReadOnlyCollection<long> DuUstavIds { get; }

        /// <summary>
        /// Наличие фильтрации по контрагенту
        /// </summary>
        bool HasContragentFilter { get; }

        /// <summary>
        /// Наличие фильтрации по МО
        /// </summary>
        bool HasMunicipalityFilter { get; }

        /// <summary>
        /// Наличие фильтрации по ЛС
        /// </summary>
        bool HasPersAccFilter { get; }

        /// <summary>
        /// Инициализировать сервис идентификаторами контрагентов
        /// </summary>
        /// <param name="provider">Поставщик информации</param>
        /// <param name="filterParams">Параметры фильтрации</param>
        /// <param name="bulkSize">Количество идентификаторов в запросе</param>
        void Init(FormatDataExportProviderType provider, DynamicDictionary filterParams, int bulkSize = 5000);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IQueryable<Contragent> FilterByContragent(IQueryable<Contragent> query);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IQueryable<TEntity> FilterByContragent<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Contragent>> contragentSelector);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IEnumerable<TEntity> FilterByContragent<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Contragent>> contragentSelector);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках МО
        /// </summary>
        IQueryable<TEntity> FilterByMunicipality<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Municipality>> municipalitySelector);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках МО
        /// </summary>
        IEnumerable<TEntity> FilterByMunicipality<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Municipality>> municipalitySelector);

        /// <summary>
        /// Получить отфильтрованный запрос сущности
        /// </summary>
        /// <exception cref="ArgumentException">Не найдена реализация <see cref="IFormatDataExportRepository{TEntity}"/></exception>
        IQueryable<TEntity> GetFiltredQuery<TEntity>() where TEntity : BaseEntity;
    }
}