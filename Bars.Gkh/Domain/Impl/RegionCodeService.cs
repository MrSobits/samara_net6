namespace Bars.Gkh.Domain.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    /// <summary>
    /// Сервис для работы с кодами субъекта РФ из универсального справочника
    /// </summary>
    public class RegionCodeService : IRegionCodeService
    {
        /// <summary>
        /// Код справочника
        /// </summary>
        public static string Code => nameof(RegionCodeService);

        /// <inheritdoc />
        string IRegionCodeService.Code => RegionCodeService.Code;

        /// <inheritdoc />
        public string Name => "Справочник субъектов РФ";

        /// <summary>
        /// Универсальный справочник
        /// </summary>
        public IRepository<MultipurposeGlossary> MultipurposeGlossaryRepository { get; set; }

        /// <summary>
        /// Справочник ФИАС
        /// </summary>
        public IRepository<Fias> FiasRepository { get; set; }

        /// <inheritdoc />
        public string GetRegionName(string code)
        {
            return this.MultipurposeGlossaryRepository.GetAll()
                .Single(x => x.Code == RegionCodeService.Code).Items
                .SingleOrDefault(x => x.Key == code)?.Value;
        }

        /// <inheritdoc />
        public string GetRegionCode()
        {
            return this.FiasRepository.GetAll()
                .Where(x => x.TypeRecord == FiasTypeRecordEnum.Fias)
                .Where(x => x.AOLevel == FiasLevelEnum.Region)
                .Select(x => x.CodeRegion)
                .Single(x => x.Length == 2);
        }

        /// <inheritdoc />
        public IDictionary<string, string> GetAll()
        {
            return this.MultipurposeGlossaryRepository.GetAll()
                .Single(x => x.Code == RegionCodeService.Code)
                .Items
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}