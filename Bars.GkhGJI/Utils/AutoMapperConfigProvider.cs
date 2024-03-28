namespace Bars.GkhGji.Utils;

using AutoMapper;

using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Integration.AppealCits;

public class AutoMapperConfigProvider : IAutoMapperConfigProvider
{
    /// <inheritdoc />
    public void Map(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<SedAppealCitDto, AppealCits>()
            .ForMember(x => x.NumberGji, opt => opt.MapFrom(src => src.Number))
            .ForMember(x => x.DateFrom, opt => opt.MapFrom(src => src.Date))
            .ForMember(x => x.Correspondent, opt => opt.MapFrom(src => src.Author))
            .ForMember(x => x.CorrespondentAddress, opt => opt.MapFrom(src => src.Address))
            .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Contacts.Phone))
            .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Contacts.Email))
            .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Text))
            .ForMember(x => x.ExternalId, opt => opt.MapFrom(src => src.SedId))
            .ForMember(x => x.TypeCorrespondent, opt => opt.MapFrom(src => TypeCorrespondent.CitizenHe));
    }
}