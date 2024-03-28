namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Services.DataContracts.GetChargePeriods;

    public partial class Service
    {
        public PaymentDocumentResponse GetPaymentDocument(string accountNumber, string periodId, bool saveSnapshot)
        {
            var service = this.Container.Resolve<IPaymentDocumentService>();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var snapshotDomain = this.Container.ResolveDomain<PaymentDocumentSnapshot>();

            try
            {
                using (this.Container.Using(service, accountDomain, periodDomain))
                {
                    var account = accountDomain.GetAll()
                        .Where(x=> x.State.StartState).FirstOrDefault(x => x.PersonalAccountNum == accountNumber);
                    var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId.ToLong());

                    if (account == null)
                    {
                        return PaymentDocumentResponse.Fail("Не найден лицевой счет на счете регоператора");
                    }

                    if (period == null)
                    {
                        return PaymentDocumentResponse.Fail("Период не найден");
                    }

                    if (!period.IsClosed)
                    {
                        return
                            PaymentDocumentResponse.Fail(
                                "Период не закрыт. По данному периоду печать платежного документа невозможна.");
                    }

                    var snap = snapshotDomain.GetAll()
                        .Where(x => x.PaymentDocumentType == Enums.PaymentDocumentType.Individual && x.HolderType == PaymentDocumentData.AccountHolderType
                        && x.HolderId == account.Id && x.Period == period).FirstOrDefault();

                    if (snap == null)
                    {
                        snap = new PaymentDocumentSnapshot
                        {
                            DocNumber = "",
                            TotalCharge = 0
                        };
                    }

                    var stream = service.GetPaymentDocument(account, period, saveSnapshot, false);
                    stream.Seek(0, SeekOrigin.Begin);
                    var bytes = stream.ToBytes();

                    return PaymentDocumentResponse.Result(Convert.ToBase64String(bytes), snap.DocNumber, snap.TotalCharge);
                }
            }
            catch (Exception)
            {
                return PaymentDocumentResponse.Fail("Произошла непредвиденная ошибка.");
            }
        }

        public PaymentDocumentResponse GetSberDocument(string accId, string periodId, string token)
        {
            var service = this.Container.Resolve<IPaymentDocumentService>();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var snapshotDomain = this.Container.ResolveDomain<PaymentDocumentSnapshot>();
            var sberbankPayDocDomain = this.Container.ResolveDomain<SberbankPaymentDoc>();
            var fileManager = this.Container.Resolve<IFileManager>();

            if (!Validate(token))
            {
                return PaymentDocumentResponse.Fail("Ошибка авторизации");
            }

            try
            {
                using (this.Container.Using(service, accountDomain, periodDomain))
                {
                    var account = accountDomain.GetAll()
                        .Where(x => x.State.StartState)
                        .FirstOrDefault(x => x.Id == accId.ToLong());

                    var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId.ToLong());

                    if (account == null)
                    {
                        return PaymentDocumentResponse.Fail("Не найден лицевой счет на счете регоператора");
                    }

                    if (period == null)
                    {
                        return PaymentDocumentResponse.Fail("Период не найден");
                    }

                    if (!period.IsClosed)
                    {
                        return
                            PaymentDocumentResponse.Fail(
                                "Период не закрыт. По данному периоду печать платежного документа невозможна.");
                    }

                    var snap = snapshotDomain.GetAll()
                        .Where(x => x.PaymentDocumentType == Enums.PaymentDocumentType.Individual && x.HolderType == PaymentDocumentData.AccountHolderType
                        && x.HolderId == account.Id && x.Period == period).FirstOrDefault();

                    if (snap == null)
                    {
                        snap = new PaymentDocumentSnapshot
                        {
                            DocNumber = "",
                            TotalCharge = 0
                        };
                    }

                    var stream = service.GetPaymentDocument(account, period, true, false);
                    stream.Seek(0, SeekOrigin.Begin);
                    var bytes = stream.ToBytes();

                    var sberPayDoc = sberbankPayDocDomain.GetAll()
                        .Where(x => x.Account.Id == accId.ToLong() && x.Period.Id == periodId.ToLong())
                        .FirstOrDefault();

                    sberPayDoc.PaymentDocFile = fileManager.SaveFile(sberPayDoc.Account.PersonalAccountNum, "pdf", bytes);

                    sberbankPayDocDomain.Update(sberPayDoc);
                    
                    return PaymentDocumentResponse.Result(Convert.ToBase64String(bytes), snap.DocNumber, snap.TotalCharge);
                }
            }
            catch (Exception e)
            {
                return PaymentDocumentResponse.Fail("Произошла непредвиденная ошибка. " + e.ToString() + e.StackTrace);
            }
            finally
            {
                this.Container.Release(sberbankPayDocDomain);
            }
        }

        public PaymentDocumentResponse GetPayDocCheckAddress(string accountNumber, string periodId, string address, bool saveSnapshot)
        {
            var service = this.Container.Resolve<IPaymentDocumentService>();
            var accountDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var RealityObjectDomain = this.Container.ResolveDomain<RealityObject>();
            var snapshotDomain = this.Container.ResolveDomain<PaymentDocumentSnapshot>();

            try
            {
                using (this.Container.Using(service, accountDomain, periodDomain))
                {
                    var account = accountDomain.GetAll()
                       .Where(x => x.State.StartState && x.PersonalAccountNum == accountNumber).FirstOrDefault();
                    if (account == null)
                    {
                        return PaymentDocumentResponse.Fail($"По указанным параметрам лицевой счет не найден. Лс: {accountNumber}");
                    }

                    var ro = RealityObjectDomain.Get(account.Room.RealityObject.Id);
                    if (!address.Contains(account.Room.RoomNum) || !address.Contains(ro.FiasAddress.StreetName??"") || !address.Contains(ro.FiasAddress.House))
                    {
                        if (!address.Contains(account.Room.RoomNum) || !address.Contains(ro.Address) || !address.Contains(ro.FiasAddress.House))
                        {                          
                            return PaymentDocumentResponse.Fail($"По указанным параметрам лицевой счет не найден. Лс: {accountNumber}, Адрес: {address}");                            
                        }
                    }

                    var period = periodDomain.GetAll().FirstOrDefault(x => x.Id == periodId.ToLong());

                    if (period == null)
                    {
                        return PaymentDocumentResponse.Fail("Период не найден");
                    }                               

                    if (!period.IsClosed)
                    {
                        return
                            PaymentDocumentResponse.Fail(
                                "Период не закрыт. По данному периоду печать платежного документа невозможна.");
                    }

                    var snap = snapshotDomain.GetAll()
                        .Where(x => x.PaymentDocumentType == Enums.PaymentDocumentType.Individual && x.HolderType == PaymentDocumentData.AccountHolderType
                        && x.HolderId == account.Id && x.Period == period).FirstOrDefault();

                    if (snap == null)
                    {
                        snap = new PaymentDocumentSnapshot
                        {
                            DocNumber = "",
                            TotalCharge = 0
                        };
                    }

                    var stream = service.GetPaymentDocument(account, period, saveSnapshot, false);
                    stream.Seek(0, SeekOrigin.Begin);
                    var bytes = stream.ToBytes();

                    return PaymentDocumentResponse.Result(Convert.ToBase64String(bytes), snap.DocNumber, snap.TotalCharge);
                }
            }
            catch (Exception e)
            {
                return PaymentDocumentResponse.Fail("Произошла непредвиденная ошибка. " + e.StackTrace);
            }
        }

        private string GetToken()
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(hash);
        }

        private bool Validate(string check_token)
        {
            return this.GetToken() == check_token;
        }
    }
}
