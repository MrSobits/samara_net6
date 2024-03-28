namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using Bars.Gkh.RegOperator.Domain.ImportExport.Enums;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;

    public class PrivilegedCategoryProxyMap : AbstractImportMap<PrivilegedCategoryInfoProxy>
    {
        public override string ProviderCode
        {
            get
            {
                return "Privileges";
            }
        }

        public override string ProviderName
        {
            get
            {
                return "Сведения по льготным категориям граждан 2";
            }
        }

        public override string Format
        {
            get
            {
                return "dbf";
            }
        }

        public override ImportExportType Direction
        {
            get
            {
                return ImportExportType.Export;
            }
        }

        public PrivilegedCategoryProxyMap()
        {
            // Первые три символа из нового поля "КодАдреса"
            Map(x => x.KOD_POST, x => x.SetLookuper(new Lookuper("KOD_POST")), 3);
            // Карта лс. Фамилия
            Map(x => x.FAMIL, x => x.SetLookuper(new Lookuper("FAMIL")), 50);
            // Карта лс. Имя
            Map(x => x.IMJA, x => x.SetLookuper(new Lookuper("IMJA")), 50);
            // Карта лс. Отчество
            Map(x => x.OTCH, x => x.SetLookuper(new Lookuper("OTCH")), 50);
            // Первые пять символов из нового поля "КодАдреса"
            Map(x => x.KOD_NNASP, x => x.SetLookuper(new Lookuper("KOD_NNASP")), 5);
            // следующие пять символов из нового поля "КодАдреса"
            Map(x => x.KOD_NYLIC, x => x.SetLookuper(new Lookuper("KOD_NYLIC")), 5);
            // следующие четыре символа из нового поля "КодАдреса"
            Map(x => x.NDOM, x => x.SetLookuper(new Lookuper("NDOM")), 4);
            // следующие три символа из нового поля "КодАдреса"
            Map(x => x.NKORP, x => x.SetLookuper(new Lookuper("NKORP")), 3);
            // следующие три символа из нового поля "КодАдреса"
            Map(x => x.NKW, x => x.SetLookuper(new Lookuper("NKW")), 3);
            // следующие три символа из нового поля "КодАдреса"
            Map(x => x.NKOMN, x => x.SetLookuper(new Lookuper("NKOMN")), 3);
            // следующие четыре символа из нового поля "КодАдреса"
            Map(x => x.NKOD, x => x.SetLookuper(new Lookuper("NKOD")), 4);
            // следующие два символа из нового поля "КодАдреса"
            Map(x => x.NKOD_PU, x => x.SetLookuper(new Lookuper("NKOD_PU")), 2);
            // Карта лс. Выводить информацию из поля "Площадь"
            Map(x => x.ROPL, x => x.SetLookuper(new Lookuper("ROPL")), 12, 2);
            // Карта лс/ История изменений. Выводить дату из поля Дата начала действия значения
            Map(x => x.DATLGTS1, x => x.SetLookuper(new Lookuper("DATLGTS1")), 10);
            // Карта лс/ История изменений. Выводить дату из поля Дата окончания действия значения
            Map(x => x.DATLGTPO1, x => x.SetLookuper(new Lookuper("DATLGTPO1")), 10);
            // (Начислено за месяц выгрузки |кол-во дней месяца * Кол-во дней за которые предоставляется льгота) * Процент льготы | 100 (* ) Это уже реализовано в рамках требования 
            Map(x => x.SUML1, x => x.SetLookuper(new Lookuper("SUML1")), 12, 2);
            // Дата должна равняться первому числу месяца, следующего за расчетным (если файл за ноябрь 2014, то в поле должна стоять дата 01.12.2014)
            Map(x => x.DATEPRL1, x => x.SetLookuper(new Lookuper("DATEPRL1")), 10);
            // Номер лс, за который выгружаем информацию
            Map(x => x.LSA, x => x.SetLookuper(new Lookuper("LSA")), 9);
            // Карта лс. Если поле Итого задолженность >0 то передавать 1, если = 0, то передавать 0
            Map(x => x.DOLG, x => x.SetLookuper(new Lookuper("DOLG")), 1);
        }
    }
}