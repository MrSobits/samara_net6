namespace Bars.GkhCr.LogMap
{
    using System;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class TypeWorkCrLogMap : AuditLogMap<TypeWorkCr>
    {
        public static Func<TypeWorkCr, string> GetDescription = x =>
            x.ReturnSafe(
                y =>
                    string.Format(
                        "{0}, Программа КР: {1}, Вид работы {2}",
                        y.ObjectCr.RealityObject.Address,
                        y.ObjectCr.ProgramCr!= null? y.ObjectCr.ProgramCr.Name:"Без имени",
                        y.Work.Return(z => z.Name)));

        public TypeWorkCrLogMap()
        {
            Name("Виды работ КР (с Мониторингом СМР)");

            Description(GetDescription);

            MapProperty(x => x.FinanceSource, "FinanceSource", "Разрез финансирования", x => x.Return(y => y.Name));
            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.SumMaterialsRequirement, "SumMaterialsRequirement", "Потребность материалов");
            MapProperty(x => x.Volume, "Volume", "Объём");
            MapProperty(x => x.HasPsd, "HasPsd", "Наличие ПСД");
            MapProperty(x => x.Sum, "Sum", "Сумма");
            MapProperty(x => x.Description, "Description", "Примечание");
            
#warning после Того как будет реализаована доработка модулбя Логирования чтобы по одной сущности можно было создать несколько Лог мапов, то эти поля удалить и раскоментироват ьнижние классы 
            // поля относящиеся к графику выполнения работ
            MapProperty(x => x.DateStartWork, "DateStartWork", "График выполнения работ - Дата начала работ", v => v.Return(d => d.Value.ToString("dd.MM.yyyy")));
            MapProperty(x => x.DateEndWork, "DateEndWork", "График выполнения работ - Дата окончания работ", v => v.Return(d => d.Value.ToString("dd.MM.yyyy")));

            // поля относящиеся к Ходу выполнения работ
            MapProperty(x => x.VolumeOfCompletion, "VolumeOfCompletion", "Ход выполнения работ - Объем выполнения", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(x => x.CostSum, "CostSum", "Ход выполнения работ - Сумма расходов", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(x => x.PercentOfCompletion, "PercentOfCompletion", "Ход выполнения работ - Процент выполнения", v => v.Return(n => n.Value.ToInvariantString("F")));
            MapProperty(x => x.StageWorkCr, "StageWorkCr", "Ход выполнения работ - Этап работы", x => x.Return(y => y.Name));

            //поля относящиеся к Численности рабочих
            MapProperty(x => x.CountWorker, "CountWorker", "Численность рабочих");
        }
    }
    /*
    public class ScheduleExecutionWorkLogMap : AuditLogMap<TypeWorkCr>
    {
        public ScheduleExecutionWorkLogMap()
        {
            Name("Мониторинг СМР - График выполнения работ");

            Description(x => x.ObjectCr.RealityObject.Address);

            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.DateStartWork, "DateStartWork", "Дата начала работ");
            MapProperty(x => x.DateEndWork, "DateEndWork", "Дата окончания работ");
        }
    }

    public class ProgressExecutionWorkLogMap : AuditLogMap<TypeWorkCr>
    {
        public ProgressExecutionWorkLogMap()
        {
            Name("Мониторинг СМР - Ход выполнения работ");

            Description(x => x.ObjectCr.RealityObject.Address);

            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.VolumeOfCompletion, "VolumeOfCompletion", "Объем выполнения");
            MapProperty(x => x.CostSum, "CostSum", "Сумма расходов");
            MapProperty(x => x.PercentOfCompletion, "PercentOfCompletion", "Процент выполнения");
            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.StageWorkCr, "StageWorkCr", "Этап работы");
        }
    }
    public class WorkersCountLogMap : AuditLogMap<TypeWorkCr>
    {
        public WorkersCountLogMap()
        {
            Name("Мониторинг СМР - численность рабочих");

            Description(x => x.ObjectCr.RealityObject.Address);

            MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            MapProperty(x => x.CountWorker, "CountWorker", "Численность рабочих");
        }
    }*/
}
