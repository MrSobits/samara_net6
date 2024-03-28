namespace Bars.GkhCr.LogMap
{
    using System;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class SpecialTypeWorkCrLogMap : AuditLogMap<SpecialTypeWorkCr>
    {
        public static Func<SpecialTypeWorkCr, string> GetDescription = x =>
            x.ReturnSafe(
                y =>
                    string.Format(
                        "{0}, Программа КР: {1}, Вид работы {2}",
                        y.ObjectCr.RealityObject.Address,
                        y.ObjectCr.ProgramCr.Name,
                        y.Work.Return(z => z.Name)));

        public SpecialTypeWorkCrLogMap()
        {
            this.Name("Виды работ КР (с Мониторингом СМР)");

            this.Description(SpecialTypeWorkCrLogMap.GetDescription);

            this.MapProperty(x => x.FinanceSource, "FinanceSource", "Разрез финансирования", x => x.Return(y => y.Name));
            this.MapProperty(x => x.Work, "Work", "Вид работы", x => x.Return(y => y.Name));
            this.MapProperty(x => x.SumMaterialsRequirement, "SumMaterialsRequirement", "Потребность материалов");
            this.MapProperty(x => x.Volume, "Volume", "Объём");
            this.MapProperty(x => x.HasPsd, "HasPsd", "Наличие ПСД");
            this.MapProperty(x => x.Sum, "Sum", "Сумма");
            this.MapProperty(x => x.Description, "Description", "Примечание");
            
#warning после Того как будет реализаована доработка модулбя Логирования чтобы по одной сущности можно было создать несколько Лог мапов, то эти поля удалить и раскоментироват ьнижние классы 
            // поля относящиеся к графику выполнения работ
            this.MapProperty(x => x.DateStartWork, "DateStartWork", "График выполнения работ - Дата начала работ");
            this.MapProperty(x => x.DateEndWork, "DateEndWork", "График выполнения работ - Дата окончания работ");

            // поля относящиеся к Ходу выполнения работ
            this.MapProperty(x => x.VolumeOfCompletion, "VolumeOfCompletion", "Ход выполнения работ - Объем выполнения", v => v.Return(n => n.Value.ToInvariantString("F")));
            this.MapProperty(x => x.CostSum, "CostSum", "Ход выполнения работ - Сумма расходов", v => v.Return(n => n.Value.ToInvariantString("F")));
            this.MapProperty(x => x.PercentOfCompletion, "PercentOfCompletion", "Ход выполнения работ - Процент выполнения", v => v.Return(n => n.Value.ToInvariantString("F")));
            this.MapProperty(x => x.StageWorkCr, "StageWorkCr", "Ход выполнения работ - Этап работы", x => x.Return(y => y.Name));

            //поля относящиеся к Численности рабочих
            this.MapProperty(x => x.CountWorker, "CountWorker", "Численность рабочих");
        }
    }
}
