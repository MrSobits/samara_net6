namespace Bars.Gkh.RegOperator.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Services.DataContracts.SyncUnconfirmedPayments;
    using Bars.Gkh.Services.DataContracts;

    public partial class Service
    {
        public SyncUnconfirmedPaymentsCheckResult Check(string login, string password, string service, string accountNum, string summ)
        {
            if (!ValidateToken(password))
            {
                return new SyncUnconfirmedPaymentsCheckResult { Code = 5, Message = "Ошибка авторизации" };
            }

            var result = new SyncUnconfirmedPaymentsCheckResult { Code = 100, Message = "Неизвестная ошибка" };

            var accountDomain = Container.ResolveDomain<BasePersonalAccount>();
            var account = accountDomain.GetAll()
                .Where(x => x.PersonalAccountNum == accountNum)
                .FirstOrDefault();
            if (account != null)
            {
                result.Code = 0;
                result.Message = String.Empty;
                result.Description = account.AccountOwner.Name;
            }
            else
            {
                result.Code = 1;
                result.Message = "Лицевой счет не найден";
            }

            Container.Release(accountDomain);

            return result;
        }

        public SyncUnconfirmedPaymentsPayResult Pay(string login, string password, string service, string accountNum, string summ, string payId)
        {
            if (!ValidateToken(password))
            {
                return new SyncUnconfirmedPaymentsPayResult { Code = 5, Message = "Ошибка авторизации" };
            }

            var result = new SyncUnconfirmedPaymentsPayResult { Code = 100, Message = "Неизвестная ошибка" };

            var accountDomain = Container.ResolveDomain<BasePersonalAccount>();
            var unconPaymDomain = Container.ResolveDomain<UnconfirmedPayments>();

            if (!string.IsNullOrEmpty(payId))
            {
                var existsPay = unconPaymDomain.GetAll().FirstOrDefault(x => x.Guid == payId);
                if (existsPay != null)
                {
                    return new SyncUnconfirmedPaymentsPayResult { Code = 201, Message = $"Оплата с payId {payId} уже присутствует в системе"};
                }
            }

            try
            {
                var account = accountDomain.GetAll()
                    .Where(x => x.PersonalAccountNum == accountNum)
                    .FirstOrDefault();

                if (account != null)
                {
                    var newPayment = new UnconfirmedPayments
                    {
                        PersonalAccount = account,
                        Sum = decimal.Parse(summ) / 100,
                        Guid = !string.IsNullOrEmpty(payId)? payId:Guid.NewGuid().ToString(),
                        BankName = "ОАО \"Челябинвестбанк\"",
                        BankBik = "047501779",
                        PaymentDate = DateTime.Now,
                        Description = "",
                        IsConfirmed = Gkh.Enums.YesNo.No,
                        File = null
                    };

                    unconPaymDomain.Save(newPayment);
                    result.Code = 0;
                    result.Message = String.Empty;
                    result.Guid = newPayment.Guid;
                    result.PlatID = newPayment.Guid;
                    result.Date = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    result.Code = 1;
                    result.Message = "Лицевой счет не найден";
                }

                return result;
            }
            catch (Exception e)
            {
                return new SyncUnconfirmedPaymentsPayResult { Code = 2, Message = "Не удалось сохранить запись. Ошибка: " + e };
            }
            finally
            {
                Container.Release(accountDomain);
                Container.Release(unconPaymDomain);
            }
        }

        private string GetAccessToken()
        {
            var token = $"{DateTime.Now.ToShortDateString()}_ANV_6966644";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            string str = Convert.ToBase64String(hash);
            return str;
        }

        private bool ValidateToken(string check_token)
        {
            return this.GetAccessToken() == check_token;
        }
    }
}