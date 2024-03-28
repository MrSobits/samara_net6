namespace Bars.Gkh.Overhaul.Interceptors
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Интерцептор Кредитная Организация
    /// </summary>
    public class CreditOrgInterceptor : EmptyDomainInterceptor<CreditOrg>
    {
        /// <summary>
        /// Фактические адреса ФИАС
        /// </summary>
        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }
        /// <summary>
        /// Описание действий перед созданием записи
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns> Возвращается результат выполнения запроса </returns>
        public override IDataResult BeforeCreateAction(IDomainService<CreditOrg> service, CreditOrg entity)
        {
            var result = ValidateEntity(entity);

            if (!result.Success)
            {
                return result;
            }

            entity.Address = null;

            if (entity.IsAddressOut)
            {
                entity.Address = entity.AddressOutSubject;
            }
            else if (entity.FiasAddress != null)
            {
                Utils.SaveFiasAddress(Container, entity.FiasAddress);
                entity.Address = entity.FiasAddress.AddressName;
            }

            if (entity.Address.IsEmpty())
            {
                return this.Failure("Не заполнены обязательные поля: Адрес");
            }

            entity.MailingAddress = null;

            if (entity.IsMailingAddressOut)
            {
                entity.MailingAddress = entity.MailingAddressOutSubject;
            }
            else if (entity.FiasMailingAddress != null)
            {
                Utils.SaveFiasAddress(Container, entity.FiasMailingAddress);
                entity.MailingAddress = entity.FiasMailingAddress.AddressName;
            }

            return ValidateInnOgrn(service, entity);
        }
        /// <summary>
        /// Описание действий перед обновлением записи
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns> Возвращается результат выполнения запроса </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<CreditOrg> service, CreditOrg entity)
        {
            var result = ValidateEntity(entity);

            if (!result.Success)
            {
                return result;
            }

            if (entity.IsAddressOut)
            {
                entity.Address = entity.AddressOutSubject;

                if (entity.FiasAddress != null)
                {
                    var fiasId = entity.FiasAddress.Id;
                    entity.FiasAddress = null;
                    FiasAddressDomain.Delete(fiasId);
                }
            }
            else
            {
                entity.AddressOutSubject = null;
                if (entity.FiasAddress != null)
                {
                    Utils.SaveFiasAddress(Container, entity.FiasAddress);
                    entity.Address = entity.FiasAddress.AddressName;    
                }
            }

            if (entity.Address.IsEmpty())
            {
                return this.Failure("Не заполнены обязательные поля: Адрес");
            }

            if (entity.IsMailingAddressOut)
            {
                entity.MailingAddress = entity.MailingAddressOutSubject;

                if (entity.FiasMailingAddress != null)
                {
                    var fiasId = entity.FiasMailingAddress.Id;
                    entity.FiasMailingAddress = null;
                    FiasAddressDomain.Delete(fiasId);
                }
            }
            else
            {
                entity.MailingAddressOutSubject = null;
                if (entity.FiasMailingAddress != null)
                {
                    Utils.SaveFiasAddress(Container, entity.FiasMailingAddress);
                    entity.MailingAddress = entity.FiasMailingAddress.AddressName;    
                }
            }

            return ValidateInnOgrn(service, entity);
        }

        private bool IsUniqueInn(IDomainService<CreditOrg> service, CreditOrg entity)
        {
            return entity.Parent != null && entity.Inn == entity.Parent.Inn //можно создавать филиалы с тем же ИНН, 
                    //что и у головной организации, в этом случае ИНН априори валидный
                || !service.GetAll().Any(x => x.Id != entity.Id && (x.Parent == null || x.Parent.Id != entity.Id) && x.Inn == entity.Inn);
        }

        private IDataResult ValidateEntity(CreditOrg entity)
        {
            if (entity.Name.IsEmpty())
            {
                return this.Failure(" Не заполнены обязательные поля: Наименование");
            }

            if (entity.Name.Length > 300)
            {
                return this.Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Inn.IsEmpty())
            {
                return this.Failure("Не заполнены обязательные поля: Инн");
            }

            if (entity.Bik.IsNotEmpty() && entity.Bik.Length != 9)
            {
                return this.Failure("Введен некорректный БИК");
            }

            return Success();
        }

        private IDataResult ValidateInnOgrn(IDomainService<CreditOrg> service, CreditOrg entity)
        {
            if (!Container.Resolve<IAppContext>().IsDebug)
            {
                if (!Utils.VerifyInn(entity.Inn, true))
                {
                    return this.Failure("Введен некорректный ИНН");
                }

                if (!string.IsNullOrEmpty(entity.Ogrn))
                {
                    if (!Utils.VerifyOgrn(entity.Ogrn, true))
                    {
                        return this.Failure("Указаный ОГРН не корректен");
                    }
                }
            }

            if (!IsUniqueInn(service, entity))
            {
                return this.Failure("Кредитная организация с таким ИНН уже существует");
            }

            return this.Success();
        }

        /// <summary>
        /// Описание действий перед удалением записи
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns> Возвращается результат выполнения запроса </returns>
        public override IDataResult BeforeDeleteAction(IDomainService<CreditOrg> service, CreditOrg entity)
        {
            var contrAgentBankCreditOrgServ = Container.Resolve<IDomainService<ContragentBankCreditOrg>>();

            try
            {
                contrAgentBankCreditOrgServ.GetAll().Where(x => x.CreditOrg.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => contrAgentBankCreditOrgServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return this.Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(contrAgentBankCreditOrgServ);
            }
        }
    }
}