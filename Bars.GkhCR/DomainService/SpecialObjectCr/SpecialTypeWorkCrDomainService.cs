namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using System.Collections.Generic;
    using Entities;

    public class SpecialTypeWorkCrDomainService : BaseDomainService<SpecialTypeWorkCr>
    {
        public ISpecialTypeWorkCrHistoryService HistoryService { get; set; }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<SpecialTypeWorkCr>();
            this.InTransaction(() =>
            {
                var saveParam = this.GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var oldValue = new SpecialTypeWorkCr
                    {
                        Id = record.Entity.Id,
                        Volume = record.Entity.Volume,
                        Sum = record.Entity.Sum,
                        FinanceSource = record.Entity.FinanceSource,
                        YearRepair = record.Entity.YearRepair
                    };

                    var value = record.AsObject();
                    this.UpdateInternal(value);
                    values.Add(value);

                    // Запускаем создание истории для изменения вида работы 
                    this.HistoryService.HistoryAfterChange(value, oldValue);
                }
            });

            return new BaseDataResult(values);
        }

        /// <summary>Метод для формирования запроса. Перед вызовом необходимо открыть транзакцию</summary>
        /// <returns>Коллекцию объектов типа IQueryable</returns>
        public override IQueryable<SpecialTypeWorkCr> GetAll()
        {
            // возвращаем тольк оактивные виды работ
            // Если нужны всевиды работ то используйте IRepository
            return base.GetAll().Where(x => x.IsActive);
        }
    }
}