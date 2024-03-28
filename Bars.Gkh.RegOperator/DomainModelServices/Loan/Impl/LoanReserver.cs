namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Exceptions;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// резерватор займов
    /// </summary>
    public class LoanReserver : ILoanReserver
    {
        private readonly IWindsorContainer container;
        private readonly ICalcAccountService calcAccountService;
        private readonly ILoanSourceRepository loanSourceRepo;
        private readonly IRealityObjectLoanRepository roLoanRepo;

        private readonly IDomainService<RealityObjectPaymentAccount> payaccDomain;
        private readonly IDomainService<ProgramCr> programDomain;
        private readonly IDomainService<RealityObjectLoan> loanDomain;
        private readonly IDomainService<RealityObjectLoanWallet> loanWalletDomain;
        
        /// <summary>
        /// .ctor
        /// </summary>
        public LoanReserver(
            IWindsorContainer container,
            ICalcAccountService calcAccountService,
            ILoanSourceRepository loanSourceRepo,
            IDomainService<RealityObjectPaymentAccount> payaccDomain,
            IDomainService<ProgramCr> programDomain,
            IDomainService<RealityObjectLoan> loanDomain,
            IDomainService<RealityObjectLoanWallet> loanWalletDomain,
            IRealityObjectLoanRepository roLoanRepo)
        {
            this.container = container;
            this.calcAccountService = calcAccountService;
            this.loanSourceRepo = loanSourceRepo;
            this.payaccDomain = payaccDomain;
            this.programDomain = programDomain;
            this.loanDomain = loanDomain;
            this.loanWalletDomain = loanWalletDomain;
            this.roLoanRepo = roLoanRepo;
        }

        /// <summary>
        /// Зарезервировать займ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ReserveLoan(BaseParams baseParams)
        {
            try
            {
                var roIds = baseParams.Params.GetAs<long[]>("roIds");
                var programId = baseParams.Params.GetAsId("programId");
                var takenMoney = baseParams.Params.GetAs<LoanTakenMoney[]>("takenMoney");
                var documentNum = baseParams.Params.GetAs<long>("loanCount");

                var payacc = this.payaccDomain.GetAll()
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .ToArray();

                var program = this.programDomain.Get(programId);

                this.container.InTransaction(() => this.ReserveLoan(payacc, takenMoney, program, documentNum));
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("По расчетному счету уже идет резервирование займов");
            }
            catch (GkhException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            catch (Exception)
            {
                return BaseDataResult.Error("Ошибка взятия займа");
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Зарезервировать средства для займа
        /// </summary>
        public void ReserveLoan(IEnumerable<RealityObjectPaymentAccount> payaccs, IList<LoanTakenMoney> takenMoney, ProgramCr program, long documentNum)
        {
            ArgumentChecker.NotNull(payaccs, nameof(payaccs));
            ArgumentChecker.NotNull(program, nameof(program));

            if (takenMoney.IsEmpty())
            {
                throw new ReserveLoanException("Отсутствуют источники займа");
            }

            var internalTakenMoney = takenMoney.ToArray();
            var internalPayaccs = payaccs.ToArray();

            internalTakenMoney.ForEach(x => x.TakenMoney = x.TakenMoney.RegopRoundDecimal(2));

            var roIds = internalPayaccs.Select(x => x.RealityObject.Id).ToArray();

            var calcAccount = this.GetSingleCalcAccount(roIds);

            if (!this.CheckFundMinSize(calcAccount, internalTakenMoney))
            {
                throw new ReserveLoanException("Сумма займа превышает неснижаемый размер фонда");
            }

            using (new DatabaseMutexContext(calcAccount, "Резервирование займов по расчетному счету"))
            {
                var sources = this.GetSources(internalPayaccs);

                var needLoans = this.roLoanRepo
                    .ListRealtyObjectNeedLoan(internalPayaccs.Select(x => x.RealityObject).ToArray(), program)
                    .ToDictionary(x => x.Id, x => x.NeedSum.RegopRoundDecimal(2));

                var orderedPayaccs = internalPayaccs
                    .Where(x => needLoans.ContainsKey(x.RealityObject.Id))
                    .OrderBy(x => needLoans.Get(x.RealityObject.Id));

                foreach (var account in orderedPayaccs)
                {
                    if (!needLoans.ContainsKey(account.RealityObject.Id))
                    {
                        //у дома нет потребности в займе
                        continue;
                    }

                    var needMoney = needLoans[account.RealityObject.Id];

                    var loanAmount = Math.Min(internalTakenMoney.Sum(z => z.TakenMoney), needMoney);

                    //потратили все деньги
                    if (loanAmount == 0)
                        break;

                    var loan = new RealityObjectLoan(
                        calcAccount,
                        account,
                        loanAmount,
                        program,
                        documentNum);

                    this.loanDomain.Save(loan);

                    var takenMoneyGreaterZero = internalTakenMoney.Where(x => x.TakenMoney > 0).ToArray();

                    foreach (var take in takenMoneyGreaterZero)
                    {
                        var source = sources.FirstOrDefault(x => x.TypeSource == take.TypeSource);

                        if (source == null || source.AvailableMoney < take.TakenMoney)
                        {
                            throw new ReserveLoanException("Недостаточно средств для взятия займа");
                        }

                        //не берем больше чем нужно
                        var money = Math.Min(take.TakenMoney, needMoney);

                        var w = account.ReserveLoan(loan, take.TypeSource, money);

                        this.loanWalletDomain.Save(w);

                        needMoney -= money;
                        source.AvailableMoney -= money;
                        take.TakenMoney -= money;
                    }

                    var baseParams = new BaseParams
                    {
                        Params = new DynamicDictionary
                        {
                            {"loanList", new List<RealityObjectLoan> { loan }}
                        }
                    };

                    // Плохая идея генерить здесь отчет
                    // но так как всем всё равно, то здесь
                    var document = this.CreateLoanReport(baseParams);

                    loan.Document = document;
                    this.loanDomain.Update(loan);
                }
            }
        }

        private LoanAvailableSource[] GetSources(RealityObjectPaymentAccount[] payaccs)
        {
            return this.loanSourceRepo.ListAvailableSources(payaccs.Select(x => x.RealityObject)
                .ToArray())
                .Where(x => x.AvailableMoney > 0)
                .ToArray();
        }

        /// <summary>
        /// Проверка наличия средств фонда для взятия займа
        /// </summary>
        /// <param name="account">Расчетный счёт</param>
        /// <param name="takenMoney">Средства, которые будут взяты в процессе займа</param>
        /// <returns>true - если средств достаточно и false в противном случае</returns>
        private bool CheckFundMinSize(CalcAccount account, LoanTakenMoney[] takenMoney)
        {
            var minsize = this.container.Resolve<IGkhConfigProvider>()
                .Get<RegOperatorConfig>().GeneralConfig
                .FundMinSize;

            if (!minsize.HasValue || minsize <= 0)
            {
                return true;
            }

            var accSummary = this.calcAccountService.GetAccountSummary(account)
                .Return(x => x.Get(account.Id));
            var saldo = accSummary.Return(x => x.Saldo - (x.LoanSum - x.LoanReturnedSum)).RegopRoundDecimal(2);

            if (saldo <= 0)
            {
                return false;
            }

            //вычисляем сколько максимум можно взять денег
            var diff = saldo - saldo * minsize.Value.ToDivisional() - accSummary.Return(x => x.LoanSum - x.LoanReturnedSum);

            return diff >= takenMoney.Sum(x => x.TakenMoney);
        }

        private RegopCalcAccount GetSingleCalcAccount(long[] roIds)
        {
            var calcAccounts = this.calcAccountService.GetRobjectsAccounts(roIds, DateTime.Today);

            var accountIds = calcAccounts.Select(x => x.Value.Id).ToHashSet();

            if (accountIds.Count > 1)
            {
                throw new GkhException("Жилые дома принадлежат к разным счетам");
            }

            if (accountIds.Count < 1)
            {
                throw new GkhException("Не удалось получить расчетный счет регоператора");
            }

            var calcAccount = calcAccounts.First().Value;

            if (calcAccount.TypeAccount != TypeCalcAccount.Regoperator)
            {
                throw new GkhException("Не удалось получить расчетный счет регоператора");
            }

            return (RegopCalcAccount)calcAccount;
        }

        private FileInfo CreateLoanReport(BaseParams baseParams)
        {
            var printForm = this.container.ResolveAll<IPrintForm>()
                .FirstOrDefault(x => x.Name == "LoanReport");

            if (printForm == null)
            {
                return null;
            }

            var rp = new ReportParams();
            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = this.container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            using (var result = new MemoryStream())
            {
                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                var fileInfoDomain = this.container.ResolveDomain<FileInfo>();

                var fileManager = this.container.Resolve<IFileManager>();
                var fileInfo = fileManager.SaveFile(result, "LoanReport.xlsx");
                fileInfoDomain.Save(fileInfo);

                return fileInfo;
            }
        }
    }
}