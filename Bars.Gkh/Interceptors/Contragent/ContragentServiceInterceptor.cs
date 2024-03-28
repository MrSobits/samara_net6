namespace Bars.Gkh.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    using Entities;
    using Utils;
    using Utils.AddressPattern;

    /// <summary>
    /// Интерцептор для сущности Контрагент
    /// </summary>
    public class ContragentServiceInterceptor : EmptyDomainInterceptor<Contragent>
    {
        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Контрагент"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<Contragent> service, Contragent entity)
        {
            return this.CheckContragent(entity, ServiceOperationType.Save);
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Контрагент"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<Contragent> service, Contragent entity)
        {
            return this.CheckContragent(entity, ServiceOperationType.Update);
        }

        private IDataResult CheckContragent(Contragent entity, ServiceOperationType operationType)
        {
            if (!string.IsNullOrEmpty(entity.Name) && entity.Name.Length > 300)
            {
                return this.Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            Utils.SaveFiasAddress(this.Container, entity.FiasJuridicalAddress);
            Utils.SaveFiasAddress(this.Container, entity.FiasFactAddress);
            Utils.SaveFiasAddress(this.Container, entity.FiasMailingAddress);

            try
            {
                this.ValidateOgrnInn(entity, operationType);
            }
            catch (ValidationException ex)
            {
                return this.Failure(ex, null);
            }

            Municipality mcp = null;
            Municipality mo = null;
            if (entity.FiasJuridicalAddress != null)
            {
                mcp = Utils.GetMunicipality(this.Container, entity.FiasJuridicalAddress);
                mo = Utils.GetMoSettlement(this.Container, entity.FiasJuridicalAddress);
                if (mcp == null)
                {
                    return this.Failure("По юридическому адресу не удалось определить Муниципальное образование");
                }
            }

            entity.Municipality = mcp;
            entity.MoSettlement = mo;

            entity.FactAddress = null;
            entity.JuridicalAddress = null;
            entity.MailingAddress = null;

            if (entity.FiasFactAddress != null)
            {
                entity.FactAddress = entity.FiasFactAddress.AddressName;
            }

            if (entity.FiasJuridicalAddress != null)
            {
                entity.JuridicalAddress = entity.Municipality != null
                    ? this.Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasJuridicalAddress)    
                    : entity.FiasJuridicalAddress.AddressName;
            }

            if (entity.FiasMailingAddress != null)
            {
                entity.MailingAddress = entity.FiasMailingAddress.AddressName;
            }

            if (entity.Name.IsEmpty())
            {
                return this.Failure("Не заполнены обязательные поля: Наименование");
            }

            if (entity.OrganizationForm.IsNull())
            {
                return this.Failure("Не заполнены обязательные поля: Организационно-правовая форма");
            }

            return this.Success();
        }

        private bool ValidateOgrnInn(Contragent entity, ServiceOperationType operationType)
        {
            // ИНН и ОГРН нужно проверять только у тех контрагентов у которых код Орг. прав форм != 98
            if (entity.OrganizationForm.Code == "98")
            {
                if (entity.Inn.Trim().Length != 10 && entity.Inn.Trim().Length != 12)
                {
                    throw new ValidationException("ИНН должен быть 10 или 12 символов");
                }

                if (entity.Ogrn.Trim().Length != 13)
                {
                    throw new ValidationException("ОГРН должен быть 13 символов");
                }
            }
            else
            {
                var orgFormCode = (entity.OrganizationForm.Code ?? string.Empty).Replace(" ", ""); // Замена символа 255 на пробел

                // код '91' и '50102' ИП - не юр лицо
                var isJurPerson = orgFormCode != "91" && orgFormCode != "50102";

                this.ValidateInn(entity, operationType, isJurPerson);

                this.ValidateOrgn(entity, isJurPerson);
            }

            return true;
        }

        /// <summary>
        /// Валиддация ИНН
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="operationType">Тип операции</param>
        /// <param name="isJurPerson">Признак юр.лица</param>
        /// <exception cref="ValidationException">Ошибка валидации</exception>
        protected virtual void ValidateInn(Contragent entity, ServiceOperationType operationType, bool isJurPerson)
        {
            var contragentsService = this.Container.Resolve<IRepository<Contragent>>();

            try
            {
            if (!string.IsNullOrEmpty(entity.Inn))
            {
                var contragents = contragentsService.GetAll()
                    .Where(x => x.ContragentState != ContragentState.Liquidated && x.ContragentState != ContragentState.Reorganized)
                    .Where(x => x.Inn == entity.Inn)
                    .Where(x => x.Id != entity.Id);

                if (contragents.Any())
                {
                    //Если контрагент является представительством или филиалом, то его ИНН может совпадать с ИНН головной организации и наоборот
                    if ((entity.Parent == null ||
                        entity.OrganizationForm.Code != "90" ||
                        !contragents.Any(x => entity.Parent.Id == x.Id)) &&
                        !(contragents.Any(x => entity.Id == x.Parent.Id && x.OrganizationForm.Code == "90")))
                    {
                        throw new ValidationException("Контрагент с таким ИНН уже существует");
                    }
                }

                if (!Utils.VerifyInn(entity.Inn, isJurPerson))
                {
                    throw new ValidationException("Указаный ИНН некорректен");
                }
            }          
        }
            finally
            {
                this.Container.Release(contragentsService);
            }
        }

        /// <summary>
        /// Валиддация ОРГН
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="isJurPerson">Признак юр.лица</param>
        /// <exception cref="ValidationException">Ошибка валидации</exception>
        protected virtual void ValidateOrgn(Contragent entity, bool isJurPerson)
        {
            if (!string.IsNullOrEmpty(entity.Ogrn) && !Utils.VerifyOgrn(entity.Ogrn, isJurPerson))
            {
                throw new ValidationException("Указаный ОГРН не корректен");
            }
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Контрагент"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<Contragent> service, Contragent entity)
        {
            var dependencyList = new List<string>();

            var operatorServ = this.Container.Resolve<IDomainService<Operator>>();
            var operatorContrServ = this.Container.Resolve<IDomainService<OperatorContragent>>();
            var belayOrgServ = this.Container.Resolve<IDomainService<BelayOrganization>>();
            var builderServ = this.Container.Resolve<IDomainService<Builder>>();
            var builderDocServ = this.Container.Resolve<IDomainService<BuilderDocument>>();
            var builderLoanServ = this.Container.Resolve<IDomainService<BuilderLoan>>();
            var contragentBankServ = this.Container.Resolve<IDomainService<ContragentBank>>();
            var contragentContactServ = this.Container.Resolve<IDomainService<ContragentContact>>();
            var contragentMunicServ = this.Container.Resolve<IDomainService<ContragentMunicipality>>();
            var localGovServ = this.Container.Resolve<IDomainService<LocalGovernment>>();
            var managOrgServ = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var politAuthServ = this.Container.Resolve<IDomainService<PoliticAuthority>>();
            var roServOrgServ = this.Container.Resolve<IDomainService<RealityObjectServiceOrg>>();
            var servOrgServ = this.Container.Resolve<IDomainService<ServiceOrganization>>();
            var supResOrgServ = this.Container.Resolve<IDomainService<SupplyResourceOrg>>();
            var contragentAdditionRoleServ = this.Container.Resolve<IDomainService<ContragentAdditionRole>>();

            try
            {
                if (operatorServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Оператор");
                }
                if (operatorContrServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Связь Оператор - Контрагент");
                }
                if (belayOrgServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Страховые организации");
                }
                if (builderServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Подрядчики");
                }
                if (builderDocServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Документы подрядчиков");
                }
                if (builderLoanServ.GetAll().Any(x => x.Lender.Id == entity.Id))
                {
                    dependencyList.Add("Займы подрядчиков");
                }
                if (contragentBankServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Банки контрагента");
                }
                if (contragentContactServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Контактная информация по контрагенту");
                }
                if (contragentMunicServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Муниципальные образования контрагента");
                }
                if (localGovServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Органы местного самоуправления");
                }
                if (managOrgServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Управляющие организации");
                }
                if (politAuthServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Органы государственной власти");
                }
                if (roServOrgServ.GetAll().Any(x => x.Organization.Id == entity.Id))
                {
                    dependencyList.Add("Поставщик коммунальных услуг жилового дома");
                }
                if (servOrgServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Поставщик жилищный услуг");
                }
                if (supResOrgServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Поставщик коммунальных услуг");
                }
                if (contragentAdditionRoleServ.GetAll().Any(x => x.Contragent.Id == entity.Id))
                {
                    dependencyList.Add("Дополнительные роли контрагента");
                }

                if (dependencyList.Any())
                {
                    return this.Failure(
                        $"При удалении данной записи произойдет удаление следующих связанных объектов: {string.Join(", ", dependencyList)}. Удаление невозможно.");
            }
            }
            finally
            {
                this.Container.Release(operatorServ);
                this.Container.Release(operatorContrServ);
                this.Container.Release(belayOrgServ);
                this.Container.Release(builderServ);
                this.Container.Release(builderDocServ);
                this.Container.Release(builderLoanServ);
                this.Container.Release(contragentBankServ);
                this.Container.Release(contragentContactServ);
                this.Container.Release(contragentMunicServ);
                this.Container.Release(localGovServ);
                this.Container.Release(managOrgServ);
                this.Container.Release(politAuthServ);
                this.Container.Release(roServOrgServ);
                this.Container.Release(servOrgServ);
                this.Container.Release(supResOrgServ);
                this.Container.Release(contragentAdditionRoleServ);
            }

            return this.Success();
        }
    }
}