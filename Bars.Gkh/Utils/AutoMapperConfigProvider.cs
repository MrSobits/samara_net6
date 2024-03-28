namespace Bars.Gkh.Utils;

using AutoMapper;

using Bars.B4.Modules.FIAS;
using Bars.B4.Utils;

/// <summary>
/// Провайдер для настроек автомаппера
/// </summary>
public class AutoMapperConfigProvider : IAutoMapperConfigProvider
{
    /// <inheritdoc />
    public void Map(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<FiasAddress, Utils.FiassHouseProxy>()
            .ForMember(x => x.HouseGuid,
                x => x.MapFrom(y => y.HouseGuid.ToStr()));
    }
}