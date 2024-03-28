namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Linq;

    using B4.DataAccess;

    using Bars.Gkh.Enums.ClaimWork;

    using Castle.Windsor;
    using Gkh.Modules.ClaimWork.DomainService;
    using Gkh.Modules.ClaimWork.Entities;
    using Gkh.Modules.ClaimWork.Enums;
    using Gkh.RegOperator.Entities;

    /// <summary>
    /// Реализация IClaimWorkService для модуля RegOperator
    /// </summary>
    public class RegopClaimWorkService : IClaimWorkService
    {
        public IWindsorContainer Container { get; set; }

        public ClaimWorkTypeBase TypeBase => ClaimWorkTypeBase.Debtor;

        public ClaimWorkReportInfo ReportInfoByClaimwork(long id)
        {
            var debtorCwDomain = this.Container.ResolveDomain<DebtorClaimWork>();
            var individualOwnerDomain = this.Container.ResolveDomain<IndividualAccountOwner>();
            var legalOwnerDomain = this.Container.ResolveDomain<LegalAccountOwner>();
            var accountDetailDetail = this.Container.ResolveDomain<ClaimWorkAccountDetail>();

            try
            {
                var debtorClaimWork = debtorCwDomain.FirstOrDefault(x => x.Id == id);

                if (debtorClaimWork == null)
                {
                    throw new Exception("Не найдено основание проверки");
                }

                string muName;
                string address;

                if (debtorClaimWork.DebtorType == DebtorType.Legal)
                {
                    var legalOwner = legalOwnerDomain.FirstOrDefault(x => x.Id == debtorClaimWork.AccountOwner.Id);

                    muName = legalOwner.Contragent.Municipality?.Name;
                    address = legalOwner.Contragent.FiasJuridicalAddress?.AddressName ?? string.Empty;
                }
                else
                {
                    var individualOwner = individualOwnerDomain.FirstOrDefault(x => x.Id == debtorClaimWork.AccountOwner.Id);

                    var individualAddress = individualOwner.RegistrationAddress
                        ?? accountDetailDetail.FirstOrDefault(x => x.ClaimWork.Id == debtorClaimWork.Id)?.PersonalAccount.Room.RealityObject;

                    var individualAddressRoom = accountDetailDetail.FirstOrDefault(x => x.ClaimWork.Id == debtorClaimWork.Id)?.PersonalAccount.Room.RoomNum;

                    muName = individualAddress?.Municipality?.Name;
                    address = individualAddress?.FiasAddress.AddressName + ", кв." + individualAddressRoom ?? string.Empty;
                }

                return new ClaimWorkReportInfo
                {
                    Info = debtorClaimWork.AccountOwner.Name,
                    MunicipalityName = muName ?? string.Empty,
                    Address = address
                };
            }
            finally
            {
                this.Container.Release(debtorCwDomain);
                this.Container.Release(legalOwnerDomain);
                this.Container.Release(individualOwnerDomain);
                this.Container.Release(accountDetailDetail);
            }
        }

        /// <inheritdoc />
        public ClaimWorkReportInfo ReportInfoByClaimworkDetail(long id)
        {
            var accountDetailDetail = this.Container.ResolveDomain<ClaimWorkAccountDetail>();

            try
            {
                var debtorClaimWork = accountDetailDetail.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        AccountOwnerId = (long?)x.PersonalAccount.AccountOwner.Id,
                        x.PersonalAccount.AccountOwner.Name,
                        x.PersonalAccount.Room.RoomNum,

                        RegMuName = (x.PersonalAccount.AccountOwner as IndividualAccountOwner).RegistrationAddress.Municipality.Name,
                        RegAddress = (x.PersonalAccount.AccountOwner as IndividualAccountOwner).RegistrationAddress.FiasAddress.AddressName,

                        MuName = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                        Address = x.PersonalAccount.Room.RealityObject.FiasAddress.AddressName
                    })
                    .FirstOrDefault();

                if (debtorClaimWork == null)
                {
                    throw new Exception("Не найдено основание проверки");
                }

                string muName;
                string address;
                if (!string.IsNullOrEmpty(debtorClaimWork.RegMuName) && !string.IsNullOrEmpty(debtorClaimWork.RegAddress))
                {
                    muName = debtorClaimWork.RegMuName;
                    address = debtorClaimWork.RegAddress;
                }
                else
                {
                    muName = debtorClaimWork.MuName;
                    address = debtorClaimWork.Address;
                    if (!string.IsNullOrWhiteSpace(debtorClaimWork.RoomNum))
                    {
                        address += $", кв. {debtorClaimWork.RoomNum}";
                    }
                }

                return new ClaimWorkReportInfo
                {
                    Info = debtorClaimWork.Name,
                    MunicipalityName = muName ?? string.Empty,
                    Address = address ?? string.Empty
                };
            }
            finally
            {
                this.Container.Release(accountDetailDetail);
            }
        }
    }
}