namespace Bars.GkhGjiCr.Report
{
    using System;
    using B4.Modules.Reports;
    using Bars.B4;

    using Castle.Windsor;

    public class TestReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        //идентификатор муниципального образования
        private long municipalityId;

        // Конструктор отчета где в качестве параметра присваиивается Добавленный в Resource.resx файл шаблона
        public TestReport()
            : base(new ReportTemplateBinary(Properties.Resources.TestReport))
        {
        }

        // Имя отчета
        public override string Name
        {
            get { return "Тестовый отчет"; }
        }

        // группа отчета
        public override string GroupName
        {
            get { return "Тестовая группа"; }
        }

        // Описание отчета
        public override string Desciption
        {
            get { return "Описание тестового отчета"; }
        }

        // js контроллер для обработки пользовательских параметров
        public override string ParamsController
        {
            get { return "B4.controller.report.TestReport"; }
        }

        // Идентификатор права доступа к отчету.
        public override string RequiredPermission
        {
            get { return "Reports.TestReport"; }
        }

        // Метод получения введенных пользователем параметров 
        public override void SetUserParams(BaseParams baseParams)
        {
            //Тут мы получаем все пользовательские значения,
            //которые пользователь введет на форме ввода параметров
            municipalityId = baseParams.Params.GetAs<long>("municipalityId");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            //В этом методе выполняются все выборки, необходимые для отчета 
            //и затем параметры передаются в Шаблон.

            reportParams.SimpleReportParams["Параметр1"] = 4567;
            reportParams.SimpleReportParams["Параметр2"] = 384.45;
            reportParams.SimpleReportParams["Параметр3"] = DateTime.Today;
            reportParams.SimpleReportParams["Параметр4"] = DateTime.Today;
            reportParams.SimpleReportParams["Параметр5"] = DateTime.Today;
            reportParams.SimpleReportParams["Параметр6"] = DateTime.Today;
        }
        /*
        //Горизонтальная секция
        public override void PrepareReport(ReportParams reportParams)
        {
            //В этом методе выполняются все выборки, необходимые для отчета 
            //и затем параметры передаются в Шаблон.

            //сервис для работы с муниципальными образованиями
            var domainMunicipality = Container.Resolve<IDomainService<Municipality>>();

            //сервис для работы с домами
            var domainRO = Container.Resolve<IDomainService<RealityObject>>();

            //Загружаем Муниципальное образование которое выбрал пользователь
            var mu = domainMunicipality.Load(municipalityId);

            //Простой параметр Муниципальное Образование в которое передается Name
            reportParams.SimpleReportParams["МуниципальноеОбразование"] = mu.Name;


            var data = domainRO.GetAll()
                        .Where(x => x.Municipality.Id == municipalityId)
                        .Select(
                            x => new
                            {
                                x.Id, //идентификатор дома
                                x.Address, //адрес дома
                                x.AreaLiving //жилая площадь
                            })
                            .OrderBy(x => x.Address)
                            .ToList();

            //Добавляем горизонтальную секцию
            var horizontalSection = reportParams.ComplexReportParams.ДобавитьСекцию("СписокДомов");

            foreach (var item in data)
            {
                //Добавляем строки
                horizontalSection.ДобавитьСтроку();
                //Добавляем секционные параметры Адрес и Площадь
                horizontalSection["Адрес"] = item.Address;
                horizontalSection["Площадь"] = item.AreaLiving;
            }
        }
        */
        /*
       
        //Вертикальная секция
        public override void PrepareReport(ReportParams reportParams)
        {
            //В этом методе выполняются все выборки, необходимые для отчета 
            //и затем параметры передаются в Шаблон.

            //домен сервис для работы с Муниципальными образованиями
            var domainMO = Container.Resolve<IDomainService<Municipality>>();

            //домен сервис для работы с Домами
            var domainRO = Container.Resolve<IDomainService<RealityObject>>();

            //Получаем список Муниципальных образований
            var dataMO = domainMO.GetAll().Select(x => new { x.Id, x.Name }).ToList();

            //получаем список площадей домов 
            var dataRO = domainRO.GetAll()
                        .Select(x => new { x.Id, MunicipalityId = x.Municipality.Id, x.AreaMkd, x.AreaLiving })
                        .AsEnumerable();

            //получаем словарь Общих Площадей по МО 
            var dictAreaMKD = dataRO
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Sum(x => x.AreaMkd));

            //получаем словарь Жилых площадей по МО
            var dictAreaLiving = dataRO
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Sum(x => x.AreaLiving));
                        
            //Добавляем вертикальную секцию
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("СписокМО");

            foreach (var item in dataMO)
            {
                //Добавляем строки
                verticalSection.ДобавитьСтроку();

                //Добавляем секционные параметры МО, ПлощадьОбщая и ПлощадьЖилая
                verticalSection["МО"] = item.Name;

                if (dictAreaMKD.ContainsKey(item.Id))
                    verticalSection["ОбщаяПлощадь"] = dictAreaMKD[item.Id];
                
                if (dictAreaLiving.ContainsKey(item.Id))
                    verticalSection["ЖилаяПлощадь"] = dictAreaLiving[item.Id];
            }
        }
        */

        /*
        //Вложенная секция
        public override void PrepareReport(ReportParams reportParams)
        {
            //В этом методе выполняются все выборки, необходимые для отчета 
            //и затем параметры передаются в Шаблон.

            //домен сервис для работы с Муниципальными образованиями
            var domainMO = Container.Resolve<IDomainService<Municipality>>();

            //домен сервис для работы с Домами
            var domainRO = Container.Resolve<IDomainService<RealityObject>>();

            //Получаем список Муниципальных образований
            var dataMO = domainMO.GetAll().Select(x => new { x.Id, x.Name }).ToList();

            //получаем список площадей домов 
            var dataRO = domainRO.GetAll()
                        .Select(x => new { x.Id, MunicipalityId = x.Municipality.Id, x.AreaMkd, x.AreaLiving, x.TypeHouse})
                        .AsEnumerable();

            //получаем словарь Общих Площадей по МО 
            var dictAreaMKD = dataRO
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Sum(x => x.AreaMkd));

            //получаем словарь Жилых площадей по МО
            var dictAreaLiving = dataRO
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Sum(x => x.AreaLiving));

            //получаем словарь количества многоквартирных домов
            var dictCntManyApart = dataRO
                            .Where( x => x.TypeHouse == TypeHouse.ManyApartments)
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Count());

            //получаем словарь количества индивидуальных домов
            var dictCntIndividual = dataRO
                            .Where(x => x.TypeHouse == TypeHouse.Individual)
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Count());

            //получаем словарь количества домов с блокированной застройкой
            var dictCntBlocked = dataRO
                            .Where(x => x.TypeHouse == TypeHouse.BlockedBuilding)
                            .GroupBy(x => x.MunicipalityId)
                            .ToDictionary(x => x.Key, y => y.Count());

            //Добавляем вертикальную секцию
            var mainSection = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияОсновная");

            foreach (var item in dataMO)
            {
                //Добавляем строки в основную секцию
                mainSection.ДобавитьСтроку();

                //Добавляем секционные параметры МО, ПлощадьОбщая и ПлощадьЖилая
                mainSection["Наименование"] = item.Name;

                //Далее внутри цикла открываем вложенную секцию и заполняем ее значениями
                var secondSection = mainSection.ДобавитьСекцию("СекцияВложенная");

                if (dictAreaMKD.ContainsKey(item.Id))
                {
                    secondSection.ДобавитьСтроку();
                    secondSection["Инфо"] = "Общая площадь, кв.м.";
                    secondSection["Значение"] = dictAreaMKD[item.Id];
                }

                if (dictAreaLiving.ContainsKey(item.Id))
                {
                    secondSection.ДобавитьСтроку();
                    secondSection["Инфо"] = "Жилая площадь, кв.м.";
                    secondSection["Значение"] = dictAreaLiving[item.Id];
                }

                if (dictCntManyApart.ContainsKey(item.Id))
                {
                    secondSection.ДобавитьСтроку();
                    secondSection["Инфо"] = "Кол-во многоквартирных домов";
                    secondSection["Значение"] = dictCntManyApart[item.Id];
                }

                if (dictCntIndividual.ContainsKey(item.Id))
                {
                    secondSection.ДобавитьСтроку();
                    secondSection["Инфо"] = "Кол-во индивидуальных домов";
                    secondSection["Значение"] = dictCntIndividual[item.Id];
                }

                if (dictCntBlocked.ContainsKey(item.Id))
                {
                    secondSection.ДобавитьСтроку();
                    secondSection["Инфо"] = "Кол-во домов блокированной застройки";
                    secondSection["Значение"] = dictCntBlocked[item.Id];
                }
            }
        }
         */
    }
}
