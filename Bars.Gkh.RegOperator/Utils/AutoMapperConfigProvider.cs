namespace Bars.Gkh.RegOperator.Utils;

using System;
using System.Globalization;

using AutoMapper;

using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.RegOperator.Domain.ValueObjects;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.Dto;
using Bars.Gkh.RegOperator.Entities.Import.Ches;
using Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin;
using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;

public class AutoMapperConfigProvider : IAutoMapperConfigProvider
{
    /// <inheritdoc />
    public void Map(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<BasePersonalAccount, BasePersonalAccountDto>()
            .ForMember(x => x.PersonalAccountNum, y => y.MapFrom(x => x.PersonalAccountNum))
            .ForMember(x => x.PersAccNumExternalSystems, y => y.MapFrom(x => x.PersAccNumExternalSystems))
            .ForMember(x => x.OpenDate, y => y.MapFrom(x => x.OpenDate))
            .ForMember(x => x.CloseDate, y => y.MapFrom(x => x.CloseDate != DateTime.MinValue ? x.CloseDate : (DateTime?)null))
            .ForMember(x => x.Area, y => y.MapFrom(x => x.AreaShare))
            .ForMember(x => x.RoomId, y => y.MapFrom(x => x.Room.Id))
            .ForMember(x => x.OwnerId, y => y.MapFrom(x => x.AccountOwner.Id))
            .ForMember(x => x.State, y => y.MapFrom(x => x.State))
            .ForMember(x => x.AccountOwner, y => y.MapFrom(x => x.AccountOwner.Name))
            .ForMember(x => x.OwnerType, y => y.MapFrom(x => x.AccountOwner.OwnerType))
            .ForMember(
                x => x.PrivilegedCategoryId,
                y => y.MapFrom(x => x.AccountOwner.PrivilegedCategory != null ? x.AccountOwner.PrivilegedCategory.Id : (long?)null))

            .ForMember(x => x.HasOnlyOneRoomWithOpenState, y => y.MapFrom(x => x.State.StartState && x.AccountOwner.ActiveAccountsCount == 1))

            .ForMember(x => x.RoId, y => y.MapFrom(x => x.Room.RealityObject.Id))
            .ForMember(
                x => x.RoomAddress,
                y => y.MapFrom(
                    x =>
                        x.Room.RealityObject.Address + ", кв. " + x.Room.RoomNum + (x.Room.ChamberNum != string.Empty && x.Room.ChamberNum != null
                            ? ", ком. " + x.Room.ChamberNum
                            : string.Empty)))

            .ForMember(x => x.RoomNum, y => y.MapFrom(x => x.Room.RoomNum))
            .ForMember(x => x.ChamberNum, y => y.MapFrom(x => x.Room.ChamberNum))
            .ForMember(x => x.Area, y => y.MapFrom(x => x.Room.Area))
            .ForMember(x => x.Address, y => y.MapFrom(x => x.Room.RealityObject.Address))
            .ForMember(x => x.AreaMkd, y => y.MapFrom(x => x.Room.RealityObject.AreaMkd))
            .ForMember(
                x => x.AccountFormationVariant,
                y => y.MapFrom(x => (x.Room.RealityObject.AccountFormationVariant ?? CrFundFormationType.Unknown)))

            .ForMember(x => x.Municipality, y => y.MapFrom(x => x.Room.RealityObject.Municipality.Name))
            .ForMember(x => x.MuId, y => y.MapFrom(x => x.Room.RealityObject.Municipality.Id))

            .ForMember(
                x => x.Settlement,
                y => y.MapFrom(x => x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Name : null))

            .ForMember(
                x => x.SettleId,
                y => y.MapFrom(x => x.Room.RealityObject.MoSettlement != null ? x.Room.RealityObject.MoSettlement.Id : (long?)null));

        cfg.CreateMap<VtbOperation, PersonalAccountPaymentInfoIn>()
            .ForMember(dst => dst.OwnerType, opt => opt.MapFrom(src => PersonalAccountPaymentInfoIn.AccountType.Personal))

            // сумма пени
            .ForMember(dst => dst.PenaltyPaid,
                opt => opt.MapFrom(v => v.PaymentType == VtbPaymentType.Penalty ? v.Amount : Decimal.Zero))

            // сумма плaтежа
            .ForMember(dst => dst.TargetPaid,
                opt => opt.MapFrom(v => v.PaymentType == VtbPaymentType.Payment ? v.Amount : Decimal.Zero))
            .ForMember(dst => dst.PaymentDate, opt => opt.MapFrom(v => v.DateOperation))
            .ForMember(dst => dst.AccountNumber, opt => opt.MapFrom(v => v.Phone))
            .ForMember(dst => dst.DatePeriod, opt => opt.MapFrom(v => v.PaymentPeriod))
            .ForMember(dst => dst.ExternalSystemTransactionId, opt => opt.MapFrom(v => v.Uni))
            .ForMember(dst => dst.ReceiptId, opt => opt.MapFrom(v => v.ReceiptNumber.ToString(CultureInfo.InvariantCulture)));


        cfg.CreateMap<ChesMatchAccountOwner, ChesNotMatchAccountOwner>().ConvertUsing<ChesMatchAccountOwnerConverter>();
        cfg.CreateMap<ChesNotMatchAccountOwner, ChesMatchAccountOwner>().ConvertUsing<ChesMatchAccountOwnerConverter>();

        cfg.CreateMap<ChargePeriod, PeriodDto>()
            .ForMember(dst => dst.Name,
                opt => opt.MapFrom(v => v.Name))
            .ForMember(dst => dst.IsClosed,
                opt => opt.MapFrom(v => v.IsClosed))
            .ForMember(dst => dst.StartDate,
                opt => opt.MapFrom(v => v.StartDate))
            .ForMember(dst => dst.EndDate,
                opt => opt.MapFrom(v => v.EndDate))
            .ForMember(dst => dst.Id,
                opt => opt.MapFrom(v => v.Id));
    }

    /// <summary>
    /// Конвертер из Сопоставленнного абонента в сопоставленного и наоборот
    /// </summary>
    internal class ChesMatchAccountOwnerConverter :
        ITypeConverter<ChesMatchAccountOwner, ChesNotMatchAccountOwner>,
        ITypeConverter<ChesNotMatchAccountOwner, ChesMatchAccountOwner>
    {
        /// <inheritdoc />
        ChesNotMatchAccountOwner ITypeConverter<ChesMatchAccountOwner, ChesNotMatchAccountOwner>.Convert(
            ChesMatchAccountOwner source,
            ChesNotMatchAccountOwner destination,
            ResolutionContext context)
        {
            var owner = source;
            var legalOwner = owner as ChesMatchLegalAccountOwner;
            if (legalOwner.IsNotNull())
            {
                return new ChesNotMatchLegalAccountOwner
                {
                    Inn = legalOwner.Inn,
                    Kpp = legalOwner.Kpp,
                    Name = legalOwner.Name,
                    OwnerType = legalOwner.OwnerType,
                    PersonalAccountNumber = legalOwner.PersonalAccountNumber
                };
            }

            var individualOwner = owner as ChesMatchIndAccountOwner;
            if (individualOwner.IsNotNull())
            {
                return new ChesNotMatchIndAccountOwner
                {
                    BirthDate = individualOwner.BirthDate,
                    Firstname = individualOwner.Firstname,
                    Surname = individualOwner.Surname,
                    Lastname = individualOwner.Lastname,
                    OwnerType = individualOwner.OwnerType,
                    PersonalAccountNumber = individualOwner.PersonalAccountNumber
                };
            }

            return null;
        }

        /// <inheritdoc />
        ChesMatchAccountOwner ITypeConverter<ChesNotMatchAccountOwner, ChesMatchAccountOwner>.Convert(
            ChesNotMatchAccountOwner source,
            ChesMatchAccountOwner destination,
            ResolutionContext context)
        {
            var owner = source;
            var legalOwner = owner as ChesNotMatchLegalAccountOwner;
            if (legalOwner.IsNotNull())
            {
                return new ChesMatchLegalAccountOwner
                {
                    Inn = legalOwner.Inn,
                    Kpp = legalOwner.Kpp,
                    Name = legalOwner.Name,
                    OwnerType = legalOwner.OwnerType,
                    PersonalAccountNumber = legalOwner.PersonalAccountNumber
                };
            }

            var individualOwner = owner as ChesNotMatchIndAccountOwner;
            if (individualOwner.IsNotNull())
            {
                return new ChesMatchIndAccountOwner
                {
                    BirthDate = individualOwner.BirthDate,
                    Firstname = individualOwner.Firstname,
                    Surname = individualOwner.Surname,
                    Lastname = individualOwner.Lastname,
                    OwnerType = individualOwner.OwnerType,
                    PersonalAccountNumber = individualOwner.PersonalAccountNumber
                };
            }

            return null;
        }
    }
}