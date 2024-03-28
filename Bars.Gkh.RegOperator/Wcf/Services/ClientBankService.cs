using System;

namespace Bars.Gkh.RegOperator.Wcf.Services
{
    using Castle.Windsor;
    using Bars.Gkh.RegOperator.Wcf.Contracts.ExchangeDocument;
    using Bars.Gkh.RegOperator.Services.DataContracts.SyncUnconfirmedPayments;
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Entities;

    // TODO wcf
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ClientBankService : IClientBankService
    {
        public IWindsorContainer Container { get; set; }
        public ExchangeDocument ExchangeDocument()
        {
            var exchangeDoc = new ExchangeDocument
            {
                FileHeader = new FileHeader
                {
                    FileInnerAttribute = "dummy string",
                    CommonInfo = new CommonInfo
                    {
                        Codepage = "dummy string",
                        CreationDate = DateTime.Now,
                        CreationTime = DateTime.Now,
                        FormatVersion = "dummy string",
                        Reciever = "dummy string",
                        Sender = "dummy string",
                    },
                    ConditionInfo = new ConditionInfo
                    {
                        BankingAccount = "dummy string",
                        BeginDate = DateTime.Now,
                        Document = "dummy string",
                        EndDate = DateTime.Now
                    },
                    Remains = new Remains
                    {
                        BankingAccount = "dummy string",
                        BeginBankingAccount = "dummy string",
                        BeginDate = DateTime.Now,
                        EndBankingAccount = "dummy string",
                        EndDate = DateTime.Now,
                        FirstRemain = 0,
                        LastRemain = 0,
                        TotalCame = 0,
                        TotalWriteOff = 0
                    }
                },
                PaymentDoc = new PaymentDoc
                {
                    Header = new PaymentHeader
                    {
                        Date = DateTime.Now,
                        Number = "dummy string",
                        Section = "dummy string",
                        Sum = 0
                    },
                    Bill = new PaymentBill
                    {
                        Content = "dummy string",
                        Date = DateTime.Now,
                        Time = DateTime.Now
                    },
                    PayerDetails = new PayerDetails
                    {
                        Account = "dummy string",
                        Date = DateTime.Now,
                        Inn = "dummy string",
                        Reciever = "dummy string",
                        PayAccount = "dummy string",
                        Bank1 = "dummy string",
                        Bank2 = "dummy string",
                        Bik = "dummy string",
                        Kor = "dummy string",
                        Payer1 = "dummy string",
                        Payer2 = "dummy string",
                        Payer3 = "dummy string",
                        Payer4 = "dummy string"
                    },
                    AdditionalDetails = new AdditionalPaymentDetails
                    {
                        BasePerformance = "dummy string",
                        DatePerformance = DateTime.Now,
                        KbkPerformance = "dummy string",
                        MakerStatus = "dummy string",
                        NumberPerformance = "dummy string",
                        Okato = "dummy string",
                        PayerKpp = "dummy string",
                        PeriodPerformance = "dummy string",
                        RecieverKpp = "dummy string",
                        TypePerformance = "dummy string"
                    },
                    AdditionalDocDetails = new AdditionalDocDetails
                    {
                        Order = "dummy string",
                        AcceptionDate = 0,
                        AccrType = "dummy string",
                        AdditionalConditions = "dummy string",
                        DocSendDate = DateTime.Now,
                        PaymenTime = DateTime.Now,
                        PaymentCondition1 = "dummy string",
                        PaymentCondition2 = "dummy string",
                        PaymentCondition3 = "dummy string",
                        PaymentRepresent = "dummy string"
                    },
                    Details = new PaymentDetails
                    {
                        Type = "dummy string",
                        Code = "dummy string",
                        Kind = 0,
                        Purpose = "dummy string",
                        Purpose1 = "dummy string",
                        Purpose2 = "dummy string",
                        Purpose3 = "dummy string",
                        Purpose4 = "dummy string",
                        Purpose5 = "dummy string",
                        Purpose6 = "dummy string"
                    },
                    RecieverDetails = new RecieverDetails
                    {
                        Account = "dummy string",
                        Date = DateTime.Now,
                        Inn = "dummy string",
                        Reciever = "dummy string",
                        RecAccount = "dummy string",
                        Bank1 = "dummy string",
                        Bank2 = "dummy string",
                        Bik = "dummy string",
                        Kor = "dummy string",
                        Reciever1 = "dummy string",
                        Reciever2 = "dummy string",
                        Reciever3 = "dummy string",
                        Reciever4 = "dummy string"
                    },
                }
            };

            return exchangeDoc;
        }


        public SyncUnconfirmedPaymentsPayResult Pay(string login, string password, string service, string accountNum, string summ, string payId)
        {
            if (!ValidateToken(password,summ,payId) )
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
                    return new SyncUnconfirmedPaymentsPayResult { Code = 201, Message = $"Оплата с payId {payId} уже присутствует в системе" };
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
                        Guid = !string.IsNullOrEmpty(payId) ? payId : Guid.NewGuid().ToString(),
                        BankName = "ПАО \"МИнБанк\"",
                        BankBik = "044525600",
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

      


        private string GetAccessToken(string sum, string payId)
        {
            var key = "D0whsHPscGT61XMi5vhEWQ==";
            var mD5 = MD5.Create();
            var hash = mD5.ComputeHash(Encoding.UTF8.GetBytes(sum + payId + key));
            var str = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            
            return str;
        }    

        private bool ValidateToken(string check_token,string sum, string payId)
        {
            return this.GetAccessToken(sum,payId) == check_token;
        }

    }
}
