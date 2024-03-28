namespace Bars.Gkh.Regions.Perm.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Repositories;

    using Castle.Windsor;

    public class LicenseRequestStateRule : IRuleChangeStatus
    {
        private readonly string licenseEntityCode = "gkh_manorg_license";

        public IWindsorContainer Container { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> ManOrgRequestPersonDomain { get; set; }

        public IDomainService<ManOrgLicensePerson> ManOrgLicensePersonDomain { get; set; }

        public IStateRepository StateRepository { get; set; }

        public IStateProvider StateProvider { get; set; }

        public string Id => "LicenseRequestStateRule";

        public string Name => "Создание или перевод статуса лицензии в зависимости от типа заявления";

        public string TypeId => "gkh_manorg_license_request";

        public string Description => "При переводе статуса будет создана Лицензия по Заявлению либо переведен статус";

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var request = statefulEntity as ManOrgLicenseRequest;

            var licenseStates = this.StateRepository.GetAllStates<ManOrgLicense>();
            var grantStateCode = "002";
            var grantState = licenseStates.FirstOrDefault(x => x.Code == grantStateCode);

            if (request == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            if (!newState.FinalState)
            {
                return ValidateResult.No("Данное правило должно работать только на конечном статусе");
            }

            var persons = this.ManOrgRequestPersonDomain
                .GetAll()
                .Where(x => x.LicRequest.Id == request.Id)
                .ToList();

            switch (request.Type)
            {
                case LicenseRequestType.GrantingLicense:
                {
                    if (this.ManOrgLicenseDomain.GetAll().Any(x => x.Request.Id == request.Id))
                    {
                        return ValidateResult.No("По данному заявлению уже создана лицензия");
                    }

                    if (this.ManOrgLicenseDomain.GetAll().Any(x => x.Contragent.Id == request.Contragent.Id && !x.DateTermination.HasValue))
                    {
                        return ValidateResult.No("У данной организации уже есть лицензия.");
                    }

                    if (persons.Count < 1)
                    {
                        return ValidateResult.No(
                            "Не выбрано Должностное лицо с действующим Квалификационным аттестатом. Лицензия не может быть сформирована.");
                    }

                    var license = this.CreateLicense(request, grantState);

                    this.CreateLicensePerson(license, persons);

                    break;
                }
                case LicenseRequestType.RenewalLicense:
                {
                    if (persons.Count < 1)
                    {
                        return ValidateResult.No(
                            "Не выбрано Должностное лицо с действующим Квалификационным аттестатом. Лицензия не может быть сформирована.");
                    }

                    var oldLicense = request.ManOrgLicense;

                    if (oldLicense.IsNull())
                    {
                        return ValidateResult.No("Не указана лицензия");
                    }

                    oldLicense.Contragent = request.Contragent;
                    oldLicense.TypeIdentityDocument = request.TypeIdentityDocument;
                    oldLicense.IdSerial = request.IdSerial;
                    oldLicense.IdNumber = request.IdNumber;
                    oldLicense.IdIssuedBy = request.IdIssuedBy;
                    oldLicense.IdIssuedDate = request.IdIssuedDate;
                    oldLicense.DisposalNumber = request.OrderNumber;
                    oldLicense.DateDisposal = request.OrderDate;

                    this.CreateLicensePerson(oldLicense, persons);

                    break;
                }

                case LicenseRequestType.IssuingDuplicateLicense:
                {
                    var oldLicense = request.ManOrgLicense;

                    if (persons.Count < 1)
                    {
                        return ValidateResult.No(
                            "Не выбрано Должностное лицо с действующим Квалификационным аттестатом. Лицензия не может быть сформирована.");
                    }

                    if (oldLicense.IsNull())
                    {
                        return ValidateResult.No("Не указана лицензия");
                    }

                    oldLicense.DisposalNumber = request.OrderNumber;
                    oldLicense.DateDisposal = request.OrderDate;

                    this.CreateLicensePerson(oldLicense, persons);

                    break;
                }

                case LicenseRequestType.TerminationActivities:
                {
                    var terminationStateCode = "005";

                    var license = request.ManOrgLicense;

                    if (license.IsNull())
                    {
                        return ValidateResult.No("Не указана лицензия");
                    }

                    if (license.DateTermination == null || license.State.Code != terminationStateCode)
                    {
                        return ValidateResult.No("Необходимо заполнить дату прекращения действия лицензии и перевести статус лицензии на \"Деятельность прекращена\"");
                    }

                    break;
                }

                case LicenseRequestType.ProvisionCopiesLicense:
                {
                    if (persons.Count < 1)
                    {
                        return ValidateResult.No(
                            "Не выбрано Должностное лицо с действующим Квалификационным аттестатом. Лицензия не может быть сформирована.");
                    }

                    var oldLicense = request.ManOrgLicense;

                    oldLicense.DisposalNumber = request.OrderNumber;
                    oldLicense.DateDisposal = request.OrderDate;

                    this.CreateLicensePerson(oldLicense, persons);
                    break;
                }
                case LicenseRequestType.ExtractFromRegisterLicense:
                {
                    // тут сознательно ничего не делаем
                    break;
                }
                default:
                {
                    return ValidateResult.No("Не указан тип заявления");
                }
            }

            return ValidateResult.Yes();
        }

        private ManOrgLicense CreateLicense(ManOrgLicenseRequest request, State state = null, ManOrgLicense oldLicense = null)
        {
            var license = new ManOrgLicense
            {
                Contragent = new Contragent { Id = request.Contragent.Id },
                Request = new ManOrgLicenseRequest { Id = request.Id },
                TypeIdentityDocument = request.TypeIdentityDocument,
                IdSerial = request.IdSerial,
                IdNumber = request.IdNumber,
                IdIssuedBy = request.IdIssuedBy,
                IdIssuedDate = request.IdIssuedDate
            };

            if (oldLicense.IsNotNull())
            {
                license.LicNum = oldLicense.LicNum;
                license.LicNumber = $"{license.LicNum:D6}";
            }

            if (state.IsNotNull())
            {
                license.State = state;
            }

            this.ManOrgLicenseDomain.Save(license);

            return license;
        }

        private void CreateLicensePerson(ManOrgLicense license, List<ManOrgRequestPerson> persons)
        {
            var existsPersons = this.ManOrgLicensePersonDomain.GetAll()
                .Where(x => x.ManOrgLicense.Id == license.Id)
                .Select(x => x.Id)
                .ToList();

            var licPersons = persons
                .Select(
                    x => new ManOrgLicensePerson
                    {
                        ManOrgLicense = license,
                        Person = x.Person
                    });

            existsPersons.ForEach(x => this.ManOrgLicensePersonDomain.Delete(x));
            licPersons.ForEach(this.ManOrgLicensePersonDomain.Save);
        }
    }
}