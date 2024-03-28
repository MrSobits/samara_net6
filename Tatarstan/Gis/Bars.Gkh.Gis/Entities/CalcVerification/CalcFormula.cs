namespace Bars.KP60.Protocol.Entities
{
    using System;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Протокол расчета: расчетные даннные по формулам (calc_gku_xx)
    /// </summary>
    public class CalcFormula : PersistentObject
    {
        /// <summary>
        /// Расчетный год и месяц
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Расчетный год и месяц
        /// </summary>
        public virtual int Month { get; set; }

        /// <summary>
        /// Договор ЖКУ
        /// </summary>
        public virtual long SupplierId { get; set; }

        /// <summary>
        /// Договор ЖКУ наименование
        /// </summary>
        public virtual string SupplierName { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual int ServiceId { get; set; }

        /// <summary>
        /// Услуга наименование
        /// </summary>
        public virtual string ServiceName { get; set; }

        /// <summary>
        /// Формула id
        /// </summary>
        public virtual long FormulaId { get; set; }

        /// <summary>
        /// Формула наименование
        /// </summary>
        public virtual string FormulaName { get; set; }

        /// <summary>
        /// Параметр-Тариф id
        /// </summary>
        public virtual long PrmTarifId { get; set; }

        /// <summary>
        /// Параметр-Тариф наименование
        /// </summary>
        public virtual string PrmTarifName { get; set; }

        /// <summary>
        /// Параметр-Расход id
        /// </summary>
        public virtual long PrmConsumptionId { get; set; }

        /// <summary>
        /// Параметр-Расход наименование
        /// </summary>
        public virtual string PrmConsumptionName { get; set; }



        /// <summary>
        /// тариф (tarif)
        /// </summary>
        public virtual decimal Tarif { get; set; }

        /// <summary>
        /// расход (rashod)
        /// </summary>
        public virtual decimal Rashod { get; set; }

        /// <summary>
        /// расход по нормативу (rashod_norm)
        /// </summary>
        public virtual decimal RashodNorm { get; set; }

        /// <summary>
        /// расход лс без учета вр.выбывших (rashod_g)
        /// </summary>
        public virtual decimal RashodG { get; set; }

        /// <summary>
        /// кол-во жильцов в лс (gil)
        /// </summary>
        public virtual decimal Gil { get; set; }

        /// <summary>
        /// кол-во жильцов в лс без учета вр.выбывших (gil_g)
        /// </summary>
        public virtual decimal GilG { get; set; }

        /// <summary>
        /// площадь лс (squ)
        /// </summary>
        public virtual decimal Squ { get; set; }

        /// <summary>
        /// тариф в ГКал (trf1)
        /// </summary>
        public virtual decimal Trf1 { get; set; }

        /// <summary>
        /// норматив в ГКал на ед.изм. (на дом) (trf2)
        /// </summary>
        public virtual decimal Trf2 { get; set; }

        /// <summary>
        /// повышающий коэф-т для норматива в ГКал ГВС (trf3)
        /// </summary>
        public virtual decimal Trf3 { get; set; }

        /// <summary>
        /// процент тарифа для населения от ЭОТ тарифа (trf4)
        /// </summary>
        public virtual decimal Trf4 { get; set; }

        /// <summary>
        /// для отопления - норма расхода в ГКал на 1 кв.м / для ГВС - расход в м3 (rsh1)
        /// </summary>
        public virtual decimal Rsh1 { get; set; }

        /// <summary>
        /// для отопления - расход в ГКал                  / для ГВС - расход в ГКал на нагрев 1 м3 (rsh2)
        /// </summary>
        public virtual decimal Rsh2 { get; set; }

        /// <summary>
        /// для отопления - неотапливаемая площадь (rsh3)
        /// </summary>
        public virtual decimal Rsh3 { get; set; }

        /// <summary>
        /// тариф в руб. для населения при расчете дотаций (tarif_f)
        /// </summary>
        public virtual decimal TarifF { get; set; }

        /// <summary>
        /// нормативный расход на 1 ед.изм. (rash_norm_one)
        /// </summary>
        public virtual decimal RashNormOne { get; set; }

        /// <summary>
        /// расход без ОДН (valm)
        /// </summary>
        public virtual decimal Valm { get; set; }

        /// <summary>
        /// расход на ОДН (dop87)
        /// </summary>
        public virtual decimal Dop87 { get; set; }

        /// <summary>
        /// перерасчитанный в прошлых периодах расход (dlt_reval)
        /// </summary>
        public virtual decimal DltReval { get; set; }

        /// <summary>
        /// признак ИПУ: =1 - ИПУ, =9- среднее ИПУ, =0- норматив (is_device)
        /// </summary>
        public virtual int IsDevice { get; set; }

        /// <summary>
        /// Формула id (nzp_frm_typ) тип расчета тарифа
        /// </summary>
        public virtual long FormulaTypId { get; set; }

        /// <summary>
        /// Формула наименование
        /// </summary>
        public virtual string FormulaTypName { get; set; }

        /// <summary>
        /// Формула id (nzp_frm_typrs) тип расчета расхода
        /// </summary>
        public virtual long FormulaTyprsId { get; set; }

        /// <summary>
        /// Формула наименование
        /// </summary>
        public virtual string FormulaTyprsName { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual string Measure { get; set; }
        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual long? MeasureId { get; set; }
    }
}
