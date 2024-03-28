namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Лифты
    /// <para>
    /// Составной первичный ключ = порядковый номер лифта в подъезде | идентификатор подъезда
    /// </para>
    /// </summary>
    public class LiftExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "LIFT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <summary>
        /// Кэш технического пасспорта
        /// </summary>
        public ITehPassportCacheService TehPassportCacheService { get; set; }

        private const string LiftForm = "Form_4_2_1";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var repository = this.Container.ResolveRepository<Entrance>();
            using (this.Container.Using(repository))
            {
                var roIds = this.FilterService.GetFiltredQuery<RealityObject>()
                    .Select(x => x.Id)
                    .ToHashSet();

                var entranceNumberCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 1)
                    .Where(x => roIds.Contains(x.RealityObjectId))
                    .ToList();

                var serialNumberCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 2);
                var carryingCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 5);
                var otherCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 7);
                var operationYearCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 9);
                var overhaulYearCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 10);
                var maxOperationTimeCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 12);
                var normativeTermYearCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 13);
                var typeCache = this.TehPassportCacheService.GetCacheByRealityObjectsAndRows(LiftExportableEntity.LiftForm, 19);

                var lifts = entranceNumberCache.Select(x => new
                    {
                        RoId = x.RealityObjectId,
                        EntranceNumber = x.Value,
                        SerialNumber = this.GetValue(serialNumberCache, x),
                        Carrying = this.GetValue(carryingCache, x),
                        Other = this.GetValue(otherCache, x),
                        OperationYear = this.GetValue(operationYearCache, x),
                        OverhaulYear = this.GetValue(overhaulYearCache, x),
                        MaxOperationTime = this.GetValue(maxOperationTimeCache, x),
                        NormativeTermYear = this.GetValue(normativeTermYearCache, x),
                        Type = this.GetValue(typeCache, x),
                    })
                    .ToList();

                entranceNumberCache.Clear();
                serialNumberCache.Clear();
                carryingCache.Clear();
                otherCache.Clear();
                operationYearCache.Clear();
                overhaulYearCache.Clear();
                maxOperationTimeCache.Clear();
                normativeTermYearCache.Clear();
                typeCache.Clear();

                var entranceDict = repository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        EntranceNumber = x.Number,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => $"{x.RoId}|{x.EntranceNumber}", x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                return lifts.Select(x => new
                    {
                        EntranceId = entranceDict.Get($"{x.RoId}|{x.EntranceNumber}"),
                        x.Type,
                        x.SerialNumber,
                        x.Carrying,
                        x.OperationYear,
                        x.NormativeTermYear,
                        x.OverhaulYear,
                        x.MaxOperationTime,
                        x.Other
                    })
                    .Where(x => x.EntranceId != 0)
                    .GroupBy(x => x.EntranceId)
                    .SelectMany(x => x.Select((v, i) => new
                    {
                        Id = UniqueIdTool.GetId(i, v.EntranceId),
                        v.EntranceId,
                        v.Type,
                        v.SerialNumber,
                        v.Carrying,
                        v.OperationYear,
                        v.OverhaulYear,
                        v.NormativeTermYear,
                        v.MaxOperationTime,
                        v.Other
                    }))
                    .Select(x => new ExportableRow(x.Id,
                        new List<string>
                        {
                            x.Id.ToStr(),
                            x.EntranceId.ToStr(),
                            this.GetType(x.Type),
                            x.SerialNumber.Cut(55),
                            string.Empty, // 5. Инвентарный номер лифта
                            this.GetDecimal(x.Carrying),
                            this.GetFirstDateYear(x.OperationYear),
                            this.GetFirstDateYear(x.NormativeTermYear),
                            this.GetFirstDateYear(x.OverhaulYear),
                            this.GetDate(x.MaxOperationTime),
                            x.Other.Cut(1000)
                        }))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2, 3 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код лифта в системе отправителя",
                "Уникальный идентификатор подъезда",
                "Тип лифта",
                "Заводской номер лифта",
                "Инвентарный номер лифта",
                "Грузоподъемность лифта",
                "Год ввода в эксплуатацию лифта",
                "Нормативный срок службы лифта",
                "Год последнего проведения капитального ремонта лифта",
                "Предельный срок эксплуатации лифта",
                "Иные характеристики лифта"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "ENTRANCE"
            };
        }

        private string GetValue(IList<TehPassportCacheCell> cache, TehPassportCacheCell x)
        {
            return cache.SingleOrDefault(y => y.RealityObjectId == x.RealityObjectId && y.RowId == x.RowId)?.Value.Trim();
        }

        private string GetType(string liftType)
        {
            // Пассажирский
            if (liftType == "1")
            {
                return "1";
            }

            // Грузопассажирский
            if (liftType == "3")
            {
                return "2";
            }

            // Грузовой
            if (liftType == "2")
            {
                return "3";
            }

            return string.Empty;
        }
    }
}