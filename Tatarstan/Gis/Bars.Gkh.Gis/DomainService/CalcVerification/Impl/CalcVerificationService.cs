namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Dapper;
    using Entities.CalcVerification;
    using Gkh.DataResult;
    using Intf;
    using KP60.Protocol.Entities;
    using Npgsql;
    using ServiceStack.Common;

    public class CalcVerificationService : ICalcVerificationService
    {
        public IDataResult VerificationCalc(BaseParams baseParams)
        {
            using (var instance = new InstanceHandler(baseParams))
            {
                return instance.Run();
            }
        }

        public string GetProtocol(BaseParams baseParams)
        {
            using (var instance = new InstanceProtocolHandler(baseParams))
            {
                return instance.GetProtocol(baseParams);
            }
        }

        public TreeData GetTree(BaseParams baseParams)
        {
            using (var instance = new InstanceProtocolHandler(baseParams))
            {
                return instance.GetTree(baseParams);
            }
        }

        #region Получение списка начислений (перенесено из КП60)
        private enum ChargeType
        {
            Chd,
            Mo
        }
        public Gkh.DataResult.ListDataResult<ChargeProxyOut> Delta(ChargeProxyIn chargeIn, DeltaParams Params)
        {
            //вытащим текущий рачетный месяц

            chargeIn.Year = chargeIn.Year;
            chargeIn.Month = chargeIn.Month;
            chargeIn.YearTo = chargeIn.Year;
            chargeIn.MonthTo = chargeIn.Month;

            var inData = new ChargeProxyIn
            {
                UserId = chargeIn.UserId,
                Pref = chargeIn.Pref,
                Year = chargeIn.Year,
                Month = chargeIn.Month,
                PersonalAccountId = chargeIn.PersonalAccountId,
                ShowPrev = chargeIn.ShowPrev,
                GroupBy = chargeIn.GroupBy,
                GetFromCash = false,
                DirectUse = false, //true,
                Alias = "gis"
            };

            var listing = new List<DeltaCharge>();

            //текущий (проверочный расчет ГИС)
            inData.Year = chargeIn.Year;
            inData.Month = chargeIn.Month;
            var listCurrent = GetData(inData, ChargeType.Chd, Params);

            var summ = GetSummary(listCurrent);
            if (summ != null)
            {
                summ.service = "Итого";
                summ.dat_month = new DateTime(chargeIn.Year, chargeIn.Month, 1).ToShortDateString();
                listCurrent.Add(summ);
            }
            listing.AddRange(listCurrent.Select(x => Let(x, 1)));


            //предыдущий
            inData.Year = chargeIn.Year;
            inData.Month = chargeIn.Month;
            inData.Alias = string.Empty;
            var listBase = GetData(inData, ChargeType.Mo, Params);
            summ = GetSummary(listBase);
            if (summ != null)
            {
                summ.service = "Итого";
                listBase.Add(summ);
            }
            listing.AddRange(listBase.Select(x => Let(x, 2)));

            //вычисляем дельты
            listing.AddRange(GetDelta(listCurrent, listBase).Select(x => Let(x, 3, x.Delta)));

            int i = 1;
            var result = listing
                .Select(x => new ChargeProxyOut
                {
                    Id = i++,
                    ServiceName = x.service,
                    SupplierName = x.supplier,
                    FormulaName = x.name_frm,
                    Measure = x.measure,
                    Norm = x.norma,
                    NormConsumption = x.norma_rashod,
                    CalculationSign = x.priznak_rasch == 0 ? null : (int?)x.priznak_rasch,
                    Tariff = x.tarif,
                    TariffPrev = x.tarif_p,
                    Consumption = x.c_calc,
                    ConsumptionPrev = x.c_calc_p,
                    Recalculation = x.reval,
                    RecalculationPositive = x.reval_pol,
                    RecalculationNegative = x.reval_otr,
                    FullCalculation = x.rsum_tarif,
                    FullCalculationPrev = x.rsum_tarif_p,
                    Credited = x.sum_real,
                    CalculationTariff = x.sum_tarif,
                    CalculationTariffPrev = x.sum_tarif_p,
                    ShortDelivery = x.sum_nedop,
                    ShortDeliveryPrev = x.sum_nedop_p,
                    Benefit = x.sum_lgota,
                    BenefitPrev = x.sum_lgota_p,
                    CalculationDaily = x.sum_dlt_tarif,
                    CalculationDailyPrev = x.sum_dlt_tarif_p,
                    ChangePositive = x.real_charge_pol,
                    ChangeNegative = x.real_charge_otr,
                    Paid = x.sum_money,
                    BenefitAll = x.sum_subsidy_all,
                    IncomingSaldo = x.sum_insaldo,
                    Change = x.real_charge,
                    OutcomingSaldo = x.sum_outsaldo,
                    Payable = x.sum_charge,
                    ChargeDate = string.IsNullOrEmpty(x.dat_charge)
                                     ? (DateTime?)null
                                     : DateTime.ParseExact(x.dat_charge, "dd.MM.yyyy H:mm:ss",
                                                           System.Globalization.CultureInfo.InvariantCulture),

                    ServiceId = x.nzp_serv,
                    SupplierId = x.nzp_supp,
                    Order = x.ordering,
                    IsGis = x.IsGis == 1,

                    Delta = x.Delta,

                    CurYear = x.IsGis == 1 || x.IsGis == 2
                                  ? 9999
                                  : x.dat_month == "delta"
                                        ? 0
                                        : string.IsNullOrEmpty(x.dat_month)
                                              ? x.YM.year_
                                              : DateTime.Parse(x.dat_month)
                                                        .Year,
                    CurMonth = x.IsGis == 1 || x.IsGis == 2
                                   ? 9999
                                   : x.dat_month == "delta"
                                         ? 0
                                         : string.IsNullOrEmpty(x.dat_month)
                                               ? x.YM.month_
                                               : DateTime.Parse(x.dat_month)
                                                         .Month
                })
                .ToList()
                .OrderByDescending(x => x.IsGis)
                .ThenByDescending(x => x.CurYear)
                .ThenByDescending(x => x.CurMonth)
                .ThenBy(x => x.Order).ToList();

            foreach (var val in result)
            {
                if (val.CurMonth == 0)
                    val.TopicName = "Дельта";
                else if (val.IsGis)
                    val.TopicName = "Проверочный расчет";
                else
                    val.TopicName = "Исходные начисления";
            }

            i = 0;
            var tn = "";

            foreach (var val in result)
            {
                if (tn != val.TopicName)
                {
                    i += 1;
                    tn = val.TopicName;
                }
                val.Topic = i.ToString("000000");

                if (val.CurYear == 9999)
                {
                    val.CurYear = chargeIn.Year;
                    val.CurMonth = chargeIn.Month;
                }
            }

            if (!Params.ShowNulls)
            {
                var listResult = result.Where(val => val.Consumption != 0 || val.Tariff != 0 || val.IncomingSaldo != 0 || val.Payable != 0).ToList();
                return new Gkh.DataResult.ListDataResult<ChargeProxyOut>(listResult, 0);
            }
            return new Gkh.DataResult.ListDataResult<ChargeProxyOut>(result, 0);
        }
        private sealed class DeltaCharge : Charge
        {
            public int IsGis { get; set; }
            public DeltaValue[] Delta { get; set; }
        }

        //вычисление дельты
        //list0 - проверочный расчет, list1 - исходный расчет
        private IEnumerable<DeltaCharge> GetDelta(List<Charge> listcurrent, List<Charge> listBase)
        {
            var delta = new List<DeltaCharge>();
            var deltaValues = new List<DeltaValue>();

            //нет статьи в проверочном расчете
            foreach (var val1 in listBase)
            {
                deltaValues.Clear();
                if (listcurrent.Any(x => x.nzp_serv == val1.nzp_serv && x.nzp_supp == val1.nzp_supp && x.nzp_frm == val1.nzp_frm))
                    continue;

                deltaValues.Add(new DeltaValue { Type = 1 });

                var v = Let(val1);
                v.dat_month = "delta";
                v.Delta = deltaValues.ToArray();

                delta.Add(v);
            }

            //нет статьи в исходных начисления
            foreach (var val0 in listcurrent)
            {
                deltaValues.Clear();
                if (listBase.Any(x => x.nzp_serv == val0.nzp_serv && x.nzp_supp == val0.nzp_supp && x.nzp_frm == val0.nzp_frm))
                    continue;

                deltaValues.Add(new DeltaValue { Type = 2 });

                var v = Let(val0);
                v.dat_month = "delta";
                v.Delta = deltaValues.ToArray();

                delta.Add(v);
            }

            //дельты в значениях
            foreach (var val0 in listcurrent)
            {
                deltaValues.Clear();

                foreach (var val1 in listBase)
                {
                    if (val0.nzp_serv == val1.nzp_serv && val0.nzp_supp == val1.nzp_supp && val0.nzp_frm == val1.nzp_frm)
                    {
                        if (Math.Abs(val0.tarif - val1.tarif) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Tariff", Delta = val0.tarif - val1.tarif });
                        }
                        if (Math.Abs(val0.norma - val1.norma) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Norm", Delta = val0.norma - val1.norma });//rash_norm_one
                        }
                        if (Math.Abs(val0.norma_rashod - val1.norma_rashod) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "NormConsumption", Delta = val0.norma_rashod - val1.norma_rashod });//rash_norm_one*gil
                        }
                        if (Math.Abs(val0.c_calc - val1.c_calc) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Consumption", Delta = val0.c_calc - val1.c_calc });//
                        }
                        if (Math.Abs(val0.reval - val1.reval) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Recalculation", Delta = val0.reval - val1.reval });//0
                        }
                        if (Math.Abs(val0.reval_pol - val1.reval_pol) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "RecalculationPositive", Delta = val0.reval_pol - val1.reval_pol });//0
                        }
                        if (Math.Abs(val0.reval_otr - val1.reval_otr) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "RecalculationNegative", Delta = val0.reval_otr - val1.reval_otr });//0
                        }
                        if (Math.Abs(val0.reval_otr - val1.reval_otr) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "RecalculationNegative", Delta = val0.reval_otr - val1.reval_otr });//0
                        }
                        if (Math.Abs(val0.rsum_tarif - val1.rsum_tarif) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "FullCalculation", Delta = val0.rsum_tarif - val1.rsum_tarif });
                        }
                        if (Math.Abs(val0.sum_real - val1.sum_real) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Credited", Delta = val0.sum_real - val1.sum_real });
                        }
                        if (Math.Abs(val0.sum_tarif - val1.sum_tarif) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "CalculationTariff", Delta = val0.sum_tarif - val1.sum_tarif });//rsum_tarif-sum_nedop
                        }
                        if (Math.Abs(val0.sum_nedop - val1.sum_nedop) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "ShortDelivery", Delta = val0.sum_nedop - val1.sum_nedop });
                        }
                        if (Math.Abs(val0.sum_lgota - val1.sum_lgota) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Benefit", Delta = val0.sum_lgota - val1.sum_lgota });//0
                        }
                        if (Math.Abs(val0.sum_dlt_tarif - val1.sum_dlt_tarif) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "CalculationDaily", Delta = val0.sum_dlt_tarif - val1.sum_dlt_tarif });//0
                        }
                        if (Math.Abs(val0.real_charge_pol - val1.real_charge_pol) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "ChangePositive", Delta = val0.real_charge_pol - val1.real_charge_pol });//0
                        }
                        if (Math.Abs(val0.real_charge_otr - val1.real_charge_otr) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "ChangeNegative", Delta = val0.real_charge_otr - val1.real_charge_otr });//0
                        }
                        if (Math.Abs(val0.sum_money - val1.sum_money) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Paid", Delta = val0.sum_money - val1.sum_money });//0
                        }
                        if (Math.Abs(val0.sum_subsidy_all - val1.sum_subsidy_all) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "BenefitAll", Delta = val0.sum_subsidy_all - val1.sum_subsidy_all });//0
                        }
                        if (Math.Abs(val0.sum_insaldo - val1.sum_insaldo) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "IncomingSaldo", Delta = val0.sum_insaldo - val1.sum_insaldo });//0
                        }
                        if (Math.Abs(val0.real_charge - val1.real_charge) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Change", Delta = val0.real_charge - val1.real_charge });//0
                        }
                        if (Math.Abs(val0.sum_outsaldo - val1.sum_outsaldo) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "OutcomingSaldo", Delta = val0.sum_outsaldo - val1.sum_outsaldo });
                        }
                        if (Math.Abs(val0.sum_charge - val1.sum_charge) > (decimal)0.0001)
                        {
                            deltaValues.Add(new DeltaValue { Type = 0, Field = "Payable", Delta = val0.sum_charge - val1.sum_charge });//0
                        }

                    }
                }

                if (deltaValues.Any())
                {
                    var v = Let(val0);
                    v.dat_month = "delta";
                    v.Delta = deltaValues.ToArray();

                    delta.Add(v);
                }
            }

            return delta;
        }

        private DeltaCharge Let(Charge val, int isGis = 1, DeltaValue[] delta = null)
        {
            return new DeltaCharge
            {
                IsGis = isGis,
                ordering = val.ordering,
                dat_month = val.dat_month,
                YM = val.YM,

                norma = val.norma,
                norma_rashod = val.norma_rashod,
                priznak_rasch = val.priznak_rasch,

                service = val.service,
                nzp_serv = val.nzp_serv,
                supplier = val.supplier,
                nzp_supp = val.nzp_supp,
                measure = val.measure,
                name_frm = val.name_frm,
                nzp_frm = val.nzp_frm,
                isdel = val.isdel,
                tarif = val.tarif,
                tarif_p = val.tarif_p,
                tarif_f = val.tarif_f,
                tarif_f_p = val.tarif_f_p,
                c_calc = val.c_calc,
                reval = val.reval,
                rsum_tarif = val.rsum_tarif,
                rsum_tarif_p = val.rsum_tarif_p,
                sum_real = val.sum_real,
                sum_tarif = val.sum_tarif,
                sum_tarif_p = val.sum_tarif_p,
                sum_tarif_f = val.sum_tarif_f,
                sum_tarif_f_p = val.sum_tarif_f_p,
                sum_nedop = val.sum_nedop,
                sum_nedop_p = val.sum_nedop_p,
                sum_lgota = val.sum_lgota,
                sum_lgota_p = val.sum_lgota_p,
                sum_dlt_tarif = val.sum_dlt_tarif,
                sum_dlt_tarif_p = val.sum_dlt_tarif_p,
                real_charge = val.real_charge,
                sum_money = val.sum_money,
                money_to = val.money_to,
                money_from = val.money_from,
                money_del = val.money_del,
                sum_insaldo = val.sum_insaldo,
                sum_outsaldo = val.sum_outsaldo,
                sum_charge = val.sum_charge,
                sum_subsidy_all = val.sum_subsidy_all,
                Delta = delta
            };
        }

        private Charge GetSummary(List<Charge> list, int yy1 = 0, int mm1 = 0, int yy2 = 0, int mm2 = 0,
                                int nzp_serv = -1, long nzp_supp = -1, int nzp_frm = -1)
        {
            if (list == null || !list.Any())
                return null;

            var charge = new Charge
            {
                service = "По всем услугам",
                nzp_serv = 1,
                supplier = "По всем поставщикам",
                nzp_supp = 0,
                name_frm = "По всем формулам",
                nzp_frm = 0,
                ordering = 10000000,

                isdel = 0,
                tarif = 0,
                tarif_p = 0,
                tarif_f = 0,
                tarif_f_p = 0,
                c_calc = 0,
                reval = 0,
                rsum_tarif = 0,
                rsum_tarif_p = 0,
                sum_real = 0,
                sum_tarif = 0,
                sum_tarif_p = 0,
                sum_tarif_f = 0,
                sum_tarif_f_p = 0,
                sum_nedop = 0,
                sum_nedop_p = 0,
                sum_lgota = 0,
                sum_lgota_p = 0,
                sum_dlt_tarif = 0,
                sum_dlt_tarif_p = 0,
                real_charge = 0,
                sum_money = 0,
                money_to = 0,
                money_from = 0,
                money_del = 0,
                sum_insaldo = 0,
                sum_outsaldo = 0,
                sum_charge = 0,
                sum_subsidy_all = 0,
                sum_tarif_eot = 0,
                sum_tarif_eot_p = 0,
                sum_subsidy = 0,
                sum_subsidy_p = 0

            };

            list = list
                .Where(x => x != null)
                .Where(x => x.nzp_serv != 1) //пропускаем Итого
                .ToList();

            foreach (var val in list)
            {
                var s1 = 0;
                long s2 = 0;
                var s3 = 0;

                s1 = nzp_serv > -1 ? nzp_serv : val.nzp_serv;
                s2 = nzp_supp > -1 ? nzp_supp : val.nzp_supp;
                s3 = nzp_frm > -1 ? nzp_frm : val.nzp_frm;

                if (val.nzp_serv != s1 || val.nzp_supp != s2 || val.nzp_frm != s3)
                    continue;

                if (nzp_serv > -1)
                {
                    charge.service = val.service;
                }
                if (nzp_supp > -1)
                {
                    charge.supplier = val.supplier;
                }
                if (nzp_frm > -1)
                {
                    charge.name_frm = val.name_frm;
                }

                charge.reval += charge.reval;
                charge.rsum_tarif += val.rsum_tarif;
                charge.rsum_tarif_p += val.rsum_tarif_p;
                charge.sum_real += val.sum_real;
                charge.sum_tarif += val.sum_tarif;
                charge.sum_tarif_p += val.sum_tarif_p;
                charge.sum_tarif_f += val.sum_tarif_f;
                charge.sum_tarif_f_p += val.sum_tarif_f_p;
                charge.sum_nedop += val.sum_nedop;
                charge.sum_nedop_p += val.sum_nedop_p;
                charge.sum_lgota += val.sum_lgota;
                charge.sum_lgota_p += val.sum_lgota_p;
                charge.sum_dlt_tarif += val.sum_dlt_tarif;
                charge.sum_dlt_tarif_p += val.sum_dlt_tarif_p;
                charge.real_charge += val.real_charge;
                charge.sum_money += val.sum_money;
                charge.money_to += val.money_to;
                charge.money_from += val.money_from;
                charge.money_del += val.money_del;

                charge.sum_tarif_eot += val.sum_tarif_eot;
                charge.sum_tarif_eot_p += val.sum_tarif_eot_p;
                charge.sum_subsidy += val.sum_subsidy;
                charge.sum_subsidy_p += val.sum_subsidy_p;

                //Входящее и исходящее сальдо считается по выбранному месяцу
                if (yy1 > 0)
                {
                    charge.sum_insaldo += (val.YM.year_ == yy1 && val.YM.month_ == mm1 ? val.sum_insaldo : 0);
                    charge.sum_outsaldo += (val.YM.year_ == yy2 && val.YM.month_ == mm2 ? val.sum_outsaldo : 0);
                }
                else
                {
                    charge.sum_insaldo += val.sum_insaldo;
                    charge.sum_outsaldo += val.sum_outsaldo;
                }

                charge.sum_charge += val.sum_charge;
                charge.sum_subsidy_all += val.sum_subsidy_all;

                charge.reval_pol += val.reval > 0 ? val.reval : 0;
                charge.reval_otr += val.reval < 0 ? val.reval : 0;
                charge.real_charge_pol += val.real_charge > 0 ? val.real_charge : 0;
                charge.real_charge_otr += val.real_charge < 0 ? val.real_charge : 0;

                charge.YM = val.YM;
            }

            return charge;
        }

        private List<Charge> GetData(ChargeProxyIn chargeIn, ChargeType Type, DeltaParams Params)
        {
            List<Charge> data;
            var sql = string.Empty;
            var chargeTable = string.Format("chd_charge_{0}.chd_charge_{1}", chargeIn.Year % 1000, chargeIn.Month.ToString("00"));
            var date = new DateTime(chargeIn.Year, chargeIn.Month, 1).ToShortDateString();
            var groupBy = "";
            var select = "";
            var order = "";
            if (Params.ActGroupByFormula)
            {
                select += "min(formula) as name_frm , nzp_frm,";
                groupBy += "nzp_frm,";
                order += "name_frm,";

            }
            if (Params.ActGroupByService)
            {
                select += "max(service) as service, nzp_serv,";
                groupBy += "nzp_serv,";
                order += "service,";
            }
            if (Params.ActGroupBySupplier)
            {
                select += "max(supplier) as supplier, nzp_supp,";
                groupBy += "nzp_supp,";
                order += "supplier,";
            }
            switch (Type)
            {
                case ChargeType.Chd:
                    sql = string.Format(@"SELECT  min(measure) measure , {5}
                                nzp_kvar, nzp_dom, max(num_ls), 
                                 max(dat_charge) dat_charge, max(is_device) priznak_rasch, max(tarif_chd) as tarif,sum(c_calc_chd) as c_calc, 
                                sum(rsum_tarif_chd) as rsum_tarif, sum(sum_nedop_chd) as sum_nedop, 
                                sum(sum_real_chd) as sum_real, sum(reval_chd) as reval, sum(real_charge) real_charge, sum(sum_insaldo) sum_insaldo, sum(sum_money) sum_money, 
                                sum(sum_outsaldo_chd) as sum_outsaldo,sum(sum_charge) sum_charge, sum(rash_norm_one_chd) as norma_rashod,
                                sum(rash_norm_one_chd)*sum(gil) norma_rashod, sum(rsum_tarif_chd)-sum(sum_nedop_chd) as sum_tarif, max('{3}') dat_month
                            from {2}
                            where nzp_kvar = {0} and billing_house_code = {1} group by nzp_kvar,{4} nzp_dom order by  {6} min(measure)
                            ", Params.PersonalAccountId, Params.BillingHouseCode, chargeTable, date, groupBy, select, order);
                    break;
                case ChargeType.Mo:
                    sql = string.Format(@"SELECT  min(measure) measure , {5}
                                nzp_kvar, nzp_dom, max(num_ls), 
                                 max(dat_charge) dat_charge, max(is_device) priznak_rasch, max(tarif) as tarif,sum(c_calc) as c_calc, 
                                sum(rsum_tarif) as rsum_tarif, sum(sum_nedop) as sum_nedop, 
                                sum(sum_real) as sum_real, sum(reval) as reval, sum(real_charge) real_charge, sum(sum_insaldo) sum_insaldo, sum(sum_money) sum_money, 
                                sum(sum_outsaldo) as sum_outsaldo,sum(sum_charge) sum_charge, sum(rash_norm_one) as norma_rashod,
                                sum(rash_norm_one)*sum(gil) norma_rashod, sum(rsum_tarif)-sum(sum_nedop) as sum_tarif, max('{3}') dat_month
                            from {2}
                    where nzp_kvar = {0} and billing_house_code = {1} group by nzp_kvar,{4} nzp_dom order by  {6} min(measure)
                            ", Params.PersonalAccountId, Params.BillingHouseCode, chargeTable, date, groupBy, select, order);
                    break;
            }
            using (var connection = new NpgsqlConnection(Params.ConnectionString))
            {
                connection.Open();
                data = connection.Query<Charge>(sql).ToList();
            }
            return data;
        }
        #endregion
    }
}
