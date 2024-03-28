namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class WorkWinterMarkFillAction : BaseExecutionAction
    {
        private readonly IDomainService<WorkInWinterMark> _workWinterMarkService;

        public WorkWinterMarkFillAction(
            IDomainService<WorkInWinterMark> workWinterMarkService)
        {
            this._workWinterMarkService = workWinterMarkService;
        }

        public override string Description => "Заполнение таблицы с данными полей для формы Подготовки к работе в зимних условиях";

        public override string Name => "Форма подготовки к работе в зимних условиях";

        public override Func<IDataResult> Action => this.WorkWinterMarkFill;

        public BaseDataResult WorkWinterMarkFill()
        {
            string errorMsg = null;

            try
            {
                var marks = new List<WorkInWinterMark>();
                marks.Add(new WorkInWinterMark {Name = "Жилищный фонд", RowNumber = 1, Measure = "тыс.ед", Okei = "643"});
                marks.Add(new WorkInWinterMark {Name = "Жилищный фонд", RowNumber = 2, Measure = "тыс м2", Okei = "58"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. муниципальный", RowNumber = 3, Measure = "тыс.ед", Okei = "643"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. Муниципальный", RowNumber = 4, Measure = "тыс м2", Okei = "58"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. государственный", RowNumber = 5, Measure = "тыс.ед", Okei = "643"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. государственный", RowNumber = 6, Measure = "тыс м2", Okei = "58"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. частный", RowNumber = 7, Measure = "тыс.ед", Okei = "643"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. частный", RowNumber = 8, Measure = "тыс м2", Okei = "58"});
                marks.Add(new WorkInWinterMark {Name = "Котельные, по всем видам собственности", RowNumber = 9, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "мощность", RowNumber = 10, Measure = "Гкал", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 11, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "мощность", RowNumber = 12, Measure = "Гкал", Okei = "---"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Тепловые сети (в двухтрубном исчислении), по всем видам собственности",
                        RowNumber = 13,
                        Measure = "км",
                        Okei = "8"
                    });
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 14, Measure = "км", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Ветхие тепловые сети (в двухтрубном исчислении), по всем видам собственности",
                        RowNumber = 15,
                        Measure = "км",
                        Okei = "8"
                    });
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 16, Measure = "км", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark {Name = "Тепловые насосные станции, по всем видам собственности", RowNumber = 17, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 18, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Центральные тепловые пункты (ЦТП), по всем видам собственности",
                        RowNumber = 19,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 20, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "Водозаборы, по всем видам собственности", RowNumber = 21, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 22, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark {Name = "Насосные станции водопровода, по всем видам собственности", RowNumber = 23, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 24, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Очистные сооружения водопровода, по всем видам собственности",
                        RowNumber = 25,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "пропускная способность", RowNumber = 26, Measure = "тыс м3/сутки", Okei = "599"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 27, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "пропускная способность", RowNumber = 28, Measure = "тыс м3/сутки", Okei = "599"});
                marks.Add(new WorkInWinterMark {Name = "Водопроводные сети, по всем видам собственности", RowNumber = 29, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 30, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "Ветхие сети водопровода, по всем видам собственности", RowNumber = 31, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 32, Measure = "км", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Канализационные насосные станции, по всем видам собственности",
                        RowNumber = 33,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 34, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Очистные сооружения канализации, по всем видам собственности",
                        RowNumber = 35,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "пропускная способность", RowNumber = 36, Measure = "тыс м3/сутки", Okei = "599"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 37, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "пропускная способность", RowNumber = 38, Measure = "тыс м3/сутки", Okei = "599"});
                marks.Add(new WorkInWinterMark {Name = "Канализационные сети, по всем видам собственности", RowNumber = 39, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 40, Measure = "км", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark {Name = "Ветхие канализационные сети, по всем видам собственности", RowNumber = 41, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 42, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "Электрические сети, по всем видам собственности", RowNumber = 43, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 44, Measure = "км", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark {Name = "Ветхие электрические сети, по всем видам собственности", RowNumber = 45, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 46, Measure = "кмд", Okei = "8"});
                marks.Add(
                    new WorkInWinterMark {Name = "Трансформаторные подстанции, по всем видам собственности", RowNumber = 47, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "в том числе ЖКХ муниципальных образований", RowNumber = 48, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Специальные машины для механизированной уборки, независимо от формы собственности",
                        RowNumber = 49,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 50, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark {Name = "Улично-дорожная сеть, по всем видам собственности", RowNumber = 51, Measure = "тыс м 2", Okei = "58"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 52, Measure = "тонн", Okei = "58"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Подготовка мостовых сооружений (транспортных и пешеходных мостов и путепроводов), труб, независимо от формы собственности",
                        RowNumber = 53,
                        Measure = "тыс.ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 54, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Подготовка транспортных и пешеходных тоннелей, независимо от формы собственности",
                        RowNumber = 55,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований", RowNumber = 56, Measure = "ед", Okei = "642"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Подготовка гидротехнических сооружений,  независимо от формы собственности",
                        RowNumber = 57,
                        Measure = "ед",
                        Okei = "642"
                    });
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ЖКХ муниципальных образований ", RowNumber = 58, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "Газопроводы", RowNumber = 59, Measure = "км", Okei = "8"});
                marks.Add(new WorkInWinterMark {Name = "Создание запасов угля", RowNumber = 60, Measure = "тонн", Okei = "168"});
                marks.Add(new WorkInWinterMark {Name = "Создание запасов другого твердого топлива", RowNumber = 61, Measure = "тонн", Okei = "168"});
                marks.Add(new WorkInWinterMark {Name = "Создание запасов жидкого топлива", RowNumber = 62, Measure = "тонн", Okei = "168"});
                marks.Add(new WorkInWinterMark {Name = "Создание запасов газа в подземных газохранилищах", RowNumber = 63, Measure = "тонн", Okei = "114"});
                marks.Add(new WorkInWinterMark {Name = "Нетрадиционные источники энергии", RowNumber = 64, Measure = "ед", Okei = "642"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. приливные", RowNumber = 65, Measure = "кВт", Okei = "214"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. солнечные", RowNumber = 66, Measure = "кВт", Okei = "214"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. ветровые", RowNumber = 67, Measure = "кВт", Okei = "214"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. термальные", RowNumber = 68, Measure = "кВт", Okei = "214"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name =
                            "Финансовые средства субъекта Российской Федерации, выделяемые для подготовки ЖКХ муниципальных образований  к зиме, из них:",
                        RowNumber = 69,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "на приобретение топлива для предприятий и образований ЖКХ",
                        RowNumber = 70,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "для формирования аварийного запаса материально-технических ресурсов",
                        RowNumber = 71,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(new WorkInWinterMark {Name = "на строительство объектов ЖКХ", RowNumber = 72, Measure = "млн руб", Okei = "385"});
                marks.Add(
                    new WorkInWinterMark {Name = "на капитальный ремонт, модернизацию  объектов ЖКХ", RowNumber = 73, Measure = "млн руб", Okei = "385"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Финансовые средства муниципальных образований и предприятий ЖКХ, выделяемые для подготовки ЖКХ к зиме из них:",
                        RowNumber = 74,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "на приобретение топлива для предприятий и образований",
                        RowNumber = 75,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "для формирования аварийного запаса материально-технических ресурсов:",
                        RowNumber = 76,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(new WorkInWinterMark {Name = "на строительство объектов ЖКХ", RowNumber = 77, Measure = "млн руб", Okei = "385"});
                marks.Add(
                    new WorkInWinterMark {Name = "на капитальный ремонт,модернизацию объектов ЖКХ", RowNumber = 78, Measure = "млн руб", Okei = "385"});
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Дополнительные средства из федерального бюджета на подготовку ЖКХ к ОЗП",
                        RowNumber = 79,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(
                    new WorkInWinterMark
                    {
                        Name = "Задолженность предприятий ЖКХ за ранее потребленные ТЭР, в том числе:",
                        RowNumber = 80,
                        Measure = "млн руб",
                        Okei = "385"
                    });
                marks.Add(new WorkInWinterMark {Name = "уголь", RowNumber = 81, Measure = "млн руб", Okei = "385"});
                marks.Add(new WorkInWinterMark {Name = "жидкое топливо", RowNumber = 82, Measure = "млн руб", Okei = "385"});
                marks.Add(new WorkInWinterMark {Name = "газ", RowNumber = 83, Measure = "млн руб", Okei = "385"});
                marks.Add(new WorkInWinterMark {Name = "теплоэнергию", RowNumber = 84, Measure = "млн руб", Okei = "385"});
                marks.Add(new WorkInWinterMark {Name = "электроэнергию", RowNumber = 85, Measure = "млн руб", Okei = "385"});
                marks.Add(new WorkInWinterMark {Name = "Объекты образования", RowNumber = 86, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. детские сады", RowNumber = 87, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч.  школы", RowNumber = 88, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "объекты здравоохранения", RowNumber = 89, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "объекты культуры", RowNumber = 90, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "объекты спорта", RowNumber = 91, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "объекты занятости и соцзащиты ", RowNumber = 92, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "в т.ч. Центры занятости населения", RowNumber = 93, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "учреждения социальной защиты", RowNumber = 94, Measure = "ед", Okei = "---"});
                marks.Add(new WorkInWinterMark {Name = "установка 2-контурных котлов", RowNumber = 95, Measure = "ед", Okei = "---"});

                foreach (var mark in marks)
                {
                    this._workWinterMarkService.Save(mark);
                }
            }
            catch (Exception exc)
            {
                errorMsg = exc.Message;
            }

            return new BaseDataResult(errorMsg == null, errorMsg);
        }
    }
}