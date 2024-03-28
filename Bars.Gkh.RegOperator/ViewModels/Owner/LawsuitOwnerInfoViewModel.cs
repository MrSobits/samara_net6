namespace Bars.Gkh.RegOperator.ViewModels.Owner
{
    using System;
    using System.Linq;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Enums;

    using Gkh.Domain;
    using Entities.Owner;

    using Bars.Gkh.Utils;
    using System.Collections.Generic;

    /// <summary>
    /// ViewModel для Собственник в исковом заявлении
    /// </summary>
    public class LawsuitOwnerInfoViewModel : BaseViewModel<LawsuitOwnerInfo>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<LawsuitOwnerInfo> domainService, BaseParams baseParams)
        {
            var lawsuitId = baseParams.Params.GetAs("Lawsuit", 0L);
            var ownerType = baseParams.Params.GetAs<PersonalAccountOwnerType?>("OwnerType");
            
            var indLawSwOwnerDomain = this.Container.Resolve<IDomainService<LawsuitIndividualOwnerInfo>>();
            var legalOwnerDomain = this.Container.Resolve<IDomainService<LawsuitLegalOwnerInfo>>();
            
            var indInfo = indLawSwOwnerDomain.GetAll();
            var legalInfo = legalOwnerDomain.GetAll();

            var loadParam = this.GetLoadParam(baseParams);
            
            List<LawsuitOwnerInfoDTO> res = domainService.GetAll()
                .Where(x => x.Lawsuit.Id == lawsuitId)
                .WhereIf(ownerType.HasValue, x => x.OwnerType == ownerType)
                .Join(indInfo, x => x.Id, y => y.Id, (x, y) => new LawsuitOwnerInfoDTO {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerType = x.OwnerType,
                    Address = $"{x.PersonalAccount.Room.RealityObject.Address}, к. {x.PersonalAccount.Room.RoomNum}",
                    PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                    AreaShare = x.AreaShareNumerator == 0
                        ? "0"
                        : x.AreaShareNumerator + " / " + x.AreaShareDenominator,
                    DebtBaseTariffSum = x.DebtBaseTariffSum,
                    DebtDecisionTariffSum = x.DebtDecisionTariffSum,
                    PenaltyDebt = x.PenaltyDebt,
                    SharedOwnership = x.SharedOwnership,
                    Underage = x.Underage,
                    JurInstitution = x.JurInstitution != null? x.JurInstitution.Name: "",
                    ClaimNumber = x.ClaimNumber,
                    SNILS = x.SNILS,
                    CalcPeriod = x.StartPeriod.Name + " - " + x.EndPeriod.Name,
                    BirthDate = y.BirthDate,
                    LivePlace = y.LivePlace,
                    DocIndName = y.DocIndName,
                    DocIndSerial = y.DocIndSerial,
                    DocIndNumber = y.DocIndNumber,
                    DocIndDate = y.DocIndDate
                }
                ).ToList();
            
            res.AddRange(domainService.GetAll()
                .Where(x => x.Lawsuit.Id == lawsuitId)
                .WhereIf(ownerType.HasValue, x => x.OwnerType == ownerType)
                .Join(legalInfo, x => x.Id, y => y.Id, (x, y) => new LawsuitOwnerInfoDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    OwnerType = x.OwnerType,
                    Address = $"{x.PersonalAccount.Room.RealityObject.Address}, к. {x.PersonalAccount.Room.RoomNum}",
                    PersonalAccount = x.PersonalAccount.PersonalAccountNum,
                    AreaShare = x.AreaShareNumerator == 0
                        ? "0"
                        : x.AreaShareNumerator + " / " + x.AreaShareDenominator,
                    DebtBaseTariffSum = x.DebtBaseTariffSum,
                    DebtDecisionTariffSum = x.DebtDecisionTariffSum,
                    PenaltyDebt = x.PenaltyDebt,
                    SharedOwnership = x.SharedOwnership,
                    Underage = x.Underage,
                    JurInstitution = x.JurInstitution != null? x.JurInstitution.Name: "",
                    ClaimNumber = x.ClaimNumber,
                    SNILS = x.SNILS,
                    CalcPeriod = x.StartPeriod.Name + " - " + x.EndPeriod.Name,
                    }).ToList()
            );
            return res.ToListDataResult(loadParam);
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<LawsuitOwnerInfo> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            return new BaseDataResult(domainService.Get(id));
        }
        
        
        
    }

    public class LawsuitOwnerInfoDTO 
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public PersonalAccountOwnerType OwnerType { get; set; }
        public string PersonalAccount { get; set; }

        public string BirthDate { get; set; }
        public string LivePlace { get; set; }
        public string DocIndName { get; set; }
        public string DocIndSerial { get; set; }
        public string DocIndNumber { get; set; }
        public DateTime? DocIndDate { get; set; }
        public string Address { get; set; }
        public string AreaShare { get; set; }
        public string JurInstitution { get; set; }
        public string CalcPeriod { get; set; }

        public decimal DebtBaseTariffSum { get; set; }

        public decimal DebtDecisionTariffSum { get; set; }

        public decimal PenaltyDebt { get; set; }

        public bool SharedOwnership { get; set; }

        public bool Underage { get; set; }

        public string ClaimNumber { get; set; }

        public string SNILS { get; set; }
    }
}