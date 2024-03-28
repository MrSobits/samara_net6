namespace Bars.Gkh.Gis.Entities.CalcVerification
{
    using System;

    /// <summary>
    /// Вынужденная мера, для того чтобы не писать много кода.
    /// Поля соответсвуют колонкам таблицы
    /// </summary>
    public class ChdCharge
    {
        public int chd_charge_id { get; set; }
        public long billing_house_code { get; set; }
        public string service { get; set; }
        public string supplier { get; set; }
        public string measure { get; set; }
        public string formula { get; set; }
        public int chd_point_id { get; set; }
        public int nzp_kvar { get; set; }
        public int nzp_dom { get; set; }
        public int num_ls { get; set; }
        public int nzp_serv { get; set; }
        public int nzp_supp { get; set; }
        public int nzp_frm { get; set; }
        public int nzp_measure { get; set; }
        public DateTime? dat_charge { get; set; }
        public int is_device { get; set; }
        public decimal tarif { get; set; }
        public decimal tarif_chd { get; set; }
        public decimal c_calc { get; set; }
        public decimal c_calc_chd { get; set; }
        public decimal rsum_tarif { get; set; }
        public decimal rsum_tarif_chd { get; set; }
        public decimal sum_nedop { get; set; }
        public decimal sum_nedop_chd { get; set; }
        public decimal sum_real { get; set; }
        public decimal sum_real_chd { get; set; }
        public decimal reval { get; set; }
        public decimal reval_chd { get; set; }
        public decimal real_charge { get; set; }
        public decimal sum_insaldo { get; set; }
        public decimal sum_money { get; set; }
        public decimal sum_outsaldo { get; set; }
        public decimal sum_outsaldo_chd { get; set; }
        public decimal sum_charge { get; set; }
        public decimal gil { get; set; }
        public decimal squ1 { get; set; }
        public decimal rash_norm_one { get; set; }
        public decimal rash_norm_one_chd { get; set; }
    }
}
