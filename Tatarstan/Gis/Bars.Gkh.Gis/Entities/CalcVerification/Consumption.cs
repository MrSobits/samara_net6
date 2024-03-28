namespace Bars.KP60.Protocol.Entities
{
    using System;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Протокол расчета: расходы (counters_xx)
    /// </summary>
    public class Consumption : PersistentObject
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
        /// ПУ id
        /// </summary>
        public virtual long CounterId { get; set; }

        /// <summary>
        /// ПУ наименование
        /// </summary>
        public virtual string CounterName { get; set; }

        /// <summary>
        /// ПУ короткое наименование
        /// </summary>
        public virtual string ShortCounterName { get; set; }

        /// <summary>
        /// Тип показания: 1 - ДПУ, 2 - групповой ПУ, 3 - ИПУ, 4 - общеквартирный для коммуналок
        /// </summary>
        public virtual int IntTypeCounter { get; set; }

        /// <summary>
        /// Стек расчетов
        /// </summary>
        public virtual int IntStack { get; set; }

        /// <summary>
        /// Предыдущая дата показания (dat_s)
        /// </summary>
        public virtual DateTime DateFrom { get; set; }

        /// <summary>
        /// Текущая дата показания (dat_po)
        /// </summary>
        public virtual DateTime DateTo { get; set; }

        /// <summary>
        /// Предыдущее показание (val_s)
        /// </summary>
        public virtual decimal VolumeFrom { get; set; }

        /// <summary>
        /// Текущее показание (val_po)
        /// </summary>
        public virtual decimal VolumeTo { get; set; }

        /// <summary>
        /// Общий расход (rashod)
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Норматив на 1 человека (rash_norm_one)
        /// </summary>
        public virtual decimal ValNorm { get; set; }

        /// <summary>
        /// расход на нежилые помещения (ngp_cnt)
        /// </summary>
        public virtual decimal ValNgp { get; set; }

        /// <summary>
        /// расход по счетчику или нормативные расходы в расчетном месяце (val1)
        /// </summary>
        public virtual decimal Val1 { get; set; }

        /// <summary>
        /// расход по счетчику или нормативные расходы в расчетном месяце без учета вр.выбывших
        /// </summary>
        public virtual decimal Val1G { get; set; }

        /// <summary>
        /// дом: расход КПУ (val2)
        /// </summary>
        public virtual decimal Val2 { get; set; }

        /// <summary>
        /// дом: расход нормативщики (val3)
        /// </summary>
        public virtual decimal Val3 { get; set; }

        /// <summary>
        /// общий расход по счетчику (val4)
        /// </summary>
        public virtual decimal Val4 { get; set; }

        /// <summary>
        /// Площадь лс дома (squ1)
        /// </summary>
        public virtual decimal Squ1 { get; set; }

        /// <summary>
        /// Площадь лс без КПУ дома (squ2)
        /// </summary>
        public virtual decimal Squ2 { get; set; }

        /// <summary>
        /// количество лс дома по услуге (cls1)
        /// </summary>
        public virtual int Cls1 { get; set; }

        /// <summary>
        /// количество лс без КПУ (для домовых строк)  (cls2)
        /// </summary>
        public virtual int Cls2 { get; set; }


        /// <summary>
        /// кол-во жильцов в лс без учета вр.выбывших (gil1_g)
        /// </summary>
        public virtual decimal Gil1G { get; set; }

        /// <summary>
        /// кол-во жильцов в лс без учета вр.выбывших (gil1)
        /// </summary>
        public virtual decimal Gil1 { get; set; }

        /// <summary>
        /// кол-во жильцов в лс (gil2)
        /// </summary>
        public virtual decimal Gil2 { get; set; }

        /// <summary>
        /// кол-во жильцов в лс (нормативное) без учета вр.выбывших (cnt1_g)
        /// </summary>
        public virtual decimal Cnt1G { get; set; }

        /// <summary>
        /// кол-во жильцов в лс (нормативное) (cnt1)
        /// </summary>
        public virtual int Cnt1 { get; set; }

        /// <summary>
        /// кол-во комнат в лс (cnt2)
        /// </summary>
        public virtual int Cnt2 { get; set; }

        /// <summary>
        /// тип норматива в зависимости от услуги (ссылка на resolution.nzp_res) (cnt3)
        /// </summary>
        public virtual int Cnt3 { get; set; }

        /// <summary>
        /// 1-дом не-МКД (0-МКД) (cnt4)
        /// </summary>
        public virtual int Cnt4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Cnt5 { get; set; }

        /// <summary>
        /// доп.значение 87 постановления (7кВт или добавок к нормативу  (87 П) (dop87)
        /// </summary>
        public virtual decimal Dop87 { get; set; }

        /// <summary>
        /// 7 кВт для КПУ (откорректированный множитель) (pu7kw)
        /// </summary>
        public virtual decimal Pu7Kw { get; set; }

        /// <summary>
        /// 7 кВт КПУ * gil1 (учитывая корректировку) (gl7kw)
        /// </summary>
        public virtual decimal Gl7Kw { get; set; }

        /// <summary>
        /// расход 210 для nzp_type = 6 (vl210) !!!!!!TODO
        /// </summary>
        public virtual decimal Vl210 { get; set; }

        /// <summary>
        /// коэфициент 307 для КПУ или коэфициент 87 для нормативщиков (kf307)
        /// </summary>
        public virtual decimal Kf307 { get; set; }

        /// <summary>
        /// коэфициент 307 для нормативщиков (kf307n)
        /// </summary>
        public virtual decimal Kf307N { get; set; }

        /// <summary>
        /// коэфициент 307 по формуле 9 (kf307f9)
        /// </summary>
        public virtual decimal Kf307F9 { get; set; }

        /// <summary>
        /// коэфициент ДПУ для распределения пропорционально кол-ву жильцов (kf_dpu_kg)
        /// </summary>
        public virtual decimal KfDpuKg { get; set; }

        /// <summary>
        /// коэфициент ДПУ для распределения пропорционально сумме общих площадей (kf_dpu_plob)
        /// </summary>
        public virtual decimal KfDpuPlob { get; set; }

        /// <summary>
        /// коэфициент ДПУ для распределения пропорционально сумме отапливаемых площадей (kf_dpu_plot)
        /// </summary>
        public virtual decimal KfDpuPlot { get; set; }

        /// <summary>
        /// коэфициент ДПУ для распределения пропорционально кол-ву л/с (kf_dpu_ls)
        /// </summary>
        public virtual decimal KfDpuLs { get; set; }

        /// <summary>
        /// выбранный способ учета (kod_info)
        /// </summary>
        public virtual int CodeInfo { get; set; }

        /// <summary>
        /// разрядность (cnt_stage)
        /// </summary>
        public virtual int CntStage { get; set; }

        /// <summary>
        /// масшт. множитель (mmnog)
        /// </summary>
        public virtual decimal Mmnog { get; set; }


    }
}
