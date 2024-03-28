namespace Bars.Gkh.RegOperator.Domain.ProxyEntity
{
    using System;

    public class PrivilegedCategoryInfoProxy
    {
        /// <summary>
        /// (KOD_POST) Первые три символа из нового поля "КодАдреса"
        /// </summary>
        public string KOD_POST { get; set; }

        /// <summary>
        /// (FAMIL) Карта лс. Фамилия
        /// </summary>
        public string FAMIL { get; set; }

        /// <summary>
        /// (IMJA) Карта лс. Имя
        /// </summary>
        public string IMJA { get; set; }

        /// <summary>
        /// (OTCH) Карта лс. Отчество
        /// </summary>
        public string OTCH { get; set; }

        /// <summary>
        /// (KOD_NNASP) Первые пять символов из нового поля "КодАдреса"
        /// </summary>
        public string KOD_NNASP { get; set; }

        /// <summary>
        /// (KOD_NYLIC) следующие пять символов из нового поля "КодАдреса"
        /// </summary>
        public string KOD_NYLIC { get; set; }

        /// <summary>
        /// (NDOM) следующие четыре символа из нового поля "КодАдреса"
        /// </summary>
        public string NDOM { get; set; }

        /// <summary>
        /// (NKORP) следующие три символа из нового поля "КодАдреса"
        /// </summary>
        public string NKORP { get; set; }

        /// <summary>
        /// (NKW) следующие три символа из нового поля "КодАдреса"
        /// </summary>
        public string NKW { get; set; }

        /// <summary>
        /// (NKOMN) следующие три символа из нового поля "КодАдреса"
        /// </summary>
        public string NKOMN { get; set; }

        /// <summary>
        /// (NKOD) следующие четыре символа из нового поля "КодАдреса"
        /// </summary>
        public string NKOD { get; set; }

        /// <summary>
        /// (NKOD_PU) следующие два символа из нового поля "КодАдреса"
        /// </summary>
        public string NKOD_PU { get; set; }

        /// <summary>
        /// (ROPL) Карта лс. Выводить информацию из поля "Площадь"
        /// </summary>
        public decimal ROPL { get; set; }

        /// <summary>
        /// (DATLGTS1) Карта лс/ История изменений. Выводить дату из поля Дата начала действия значения
        /// </summary>
        public string DATLGTS1 { get; set; }

        /// <summary>
        /// (DATLGTPO1) Карта лс/ История изменений. Выводить дату из поля Дата окончания действия значения
        /// </summary>
        public string DATLGTPO1 { get; set; }

        /// <summary>
        /// (SUML1) (Начислено за месяц выгрузки |кол-во дней месяца * Кол-во дней за которые предоставляется льгота) * Процент льготы | 100 (* ) Это уже реализовано в рамках требования 
        /// </summary>
        public decimal SUML1 { get; set; }

        /// <summary>
        /// (DATEPRL1) Дата должна равняться первому числу месяца, следующего за расчетным (если файл за ноябрь 2014, то в поле должна стоять дата 01.12.2014)
        /// </summary>
        public string DATEPRL1 { get; set; }

        /// <summary>
        /// (LSA) Номер лс, за который выгружаем информацию
        /// </summary>
        public string LSA { get; set; }

        /// <summary>
        /// (DOLG) Карта лс. Если поле Итого задолженность >0 то передавать 1, если = 0, то передавать 0
        /// </summary>
        public string DOLG { get; set; }
    }
}