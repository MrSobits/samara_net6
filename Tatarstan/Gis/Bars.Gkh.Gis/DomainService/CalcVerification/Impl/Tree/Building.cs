using System.Data;
using System.Linq;
using Bars.B4.Utils;
using Bars.Gkh.Gis.DomainService.CalcVerification.Impl;
using Bars.Gkh.Gis.Entities.CalcVerification;
using Dapper;

namespace Bars.KP60.Protocol.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using B4;
    using Castle.Windsor;
    using Bars.KP60.Protocol.Entities;


    /// <summary>
    /// Сервис построения дерева протокола
    /// </summary>
    public abstract class Building : IBuildTree
    {
        public IWindsorContainer Container { get; set; }

        protected AccountDataChd Account { get; set; } //входные данные
        protected TreeData RootData { get; set; } //выходные данные

        protected CalcFormula CalcFormula { get; set; } //данные по начислениям

        //Получить построенное дерево
        public TreeData GetTree(BaseParams baseParams)
        {
            var nzp_kvar = (int)baseParams.Params.GetAs<long>("personalAccountId");
            var nzp_serv = (int)baseParams.Params.GetAs<long>("serviceId");
            var nzp_supp = (int)baseParams.Params.GetAs<long>("supplierId");
            var billing_house_id = baseParams.Params.GetAs<long>("billingHouseId");
            var isChd = baseParams.Params.GetAs<bool>("isGis");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");


            if (!(nzp_kvar > 0))
            {
                return new TreeData
                    {
                        Value = "********",
                        Hint = "Не обнаружен лицевой счет",
                        Operation = "",
                        children = null
                    };
            }
            RootData = new TreeData();
            Account = new AccountDataChd()
            {
                HouseId = billing_house_id,
                PersonalAccountId = nzp_kvar,
                Month = month,
                Year = year,
                ServiceId = nzp_serv,
                SupplierId = nzp_supp,
                IsGis = isChd
            };

            //получаем начисления
            GetCharges();

            //"простая" формула - Тариф * Расход, покрывает все жилищные услуги, по идее, и другие аналогичные услуги
            BuildTreeSimple();

            return RootData;
        }

        //формируем протокол по жилищным услугам
        private void BuildTreeSimple()
        {
            //----------------------------------------------------------------------
            //начинаем строить ДРЕВО ЖИЗНИ (дерево протокола)
            //----------------------------------------------------------------------

            //сколько всего начислено
            RootData.Value = decimal.Round(CalcFormula.Rashod * CalcFormula.Tarif, 2).ToString();
            RootData.Hint = "Начисление по услуге с тарифом на 1 " + CalcFormula.Measure;

            RootData.children = new List<TreeData>
                {
                    //Тариф
                    new TreeData
                        {
                            children = null,
                            Value = Math.Round(CalcFormula.Tarif, 2).ToString(),
                            Hint = "Тариф "
                        },
                    //Расход
                    new TreeData
                        {
                            children = null,
                            Operation = "(*)",
                            Value = Math.Round(CalcFormula.Rashod, 3).ToString(),
                            Hint = GetHint(CalcFormula.IsDevice,CalcFormula.MeasureId)
                        }
                };
        }

        //костыль
        private string GetHint(int typeDevice, long? measureId)
        {
            switch (measureId)
            {
                case 1: return "Площадь (" + CalcFormula.Measure + ")";

                default:
                    switch (typeDevice)
                    {
                        case 1: return "Расход по нормативу (" + CalcFormula.Measure + ")";
                        case 2: return "Расход по показаниям ИПУ (" + CalcFormula.Measure + ")";
                        case 3: return "Расход по показаниям ОДПУ (" + CalcFormula.Measure + ")";
                    } break;

            }

            return "";
        }

        //"Вырастить" дерево протокола расчетов - переопределяется в наследниках
        public virtual void BuildTree()
        {
            Empty();
        }

        private void Empty()
        {
            RootData = new TreeData
            {
                Value = "********",
                Hint = "Протокол не реализован",
                Operation = "",
                children = null
            };
        }


        //вытащить список узлов норматива по услуге - переопределяется в наследниках
        public virtual List<TreeData> GetNorma(Consumption consumption)
        {
            return null;
        }


        private void GetCharges()
        {
            var TableCharge = "chd_charge_" + (Account.Year % 2000) + ".chd_charge_" + Account.Month.ToString("00");
            var charge =
                Container.Resolve<IDbConnection>()
                    .Query<ChdCharge>(string.Format(" SELECT service, measure, nzp_measure, supplier, " +
                                         " is_device,rsum_tarif,rsum_tarif_chd,tarif,tarif_chd," +
                                         " c_calc,c_calc_chd, formula, gil" +
                                         " FROM {0}" +
                                         " WHERE billing_house_code={1} " +
                                         " AND nzp_kvar={2} AND nzp_serv={3} AND nzp_supp={4}",
                TableCharge, Account.HouseId, Account.PersonalAccountId, Account.ServiceId, Account.SupplierId)).First();

            CalcFormula = new CalcFormula
            {
                ServiceName = charge.service,
                Measure = charge.measure,
                SupplierName = charge.supplier,
                Gil = charge.gil,
                IsDevice = charge.is_device,
                FormulaName = charge.formula,
                MeasureId = charge.nzp_measure

            };

            if (Account.IsGis)
            {
                CalcFormula.Rashod = charge.c_calc_chd;
                CalcFormula.Tarif = charge.tarif_chd;
            }
            else
            {
                CalcFormula.Rashod = charge.c_calc;
                CalcFormula.Tarif = charge.tarif;
            }




        }
    }
}


