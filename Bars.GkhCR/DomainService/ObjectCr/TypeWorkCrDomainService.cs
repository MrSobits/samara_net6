using System.Collections.Generic;

namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;


    using Entities;

    public class TypeWorkCrDomainService : BaseDomainService<TypeWorkCr>
    {
        public ITypeWorkCrHistoryService HistoryService { get; set; }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<TypeWorkCr>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var oldValue = new TypeWorkCr
                    {
                        Id = record.Entity.Id,
                        Volume = record.Entity.Volume,
                        Sum = record.Entity.Sum,
                        FinanceSource = record.Entity.FinanceSource,
                        YearRepair = record.Entity.YearRepair
                    };

                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);

                    // Запускаем создание истории для изменения вида работы 
                    HistoryService.HistoryAfterChange(value, oldValue);
                }
            });

            return new BaseDataResult(values);
        }

        /// <summary>Метод для формирования запроса. Перед вызовом необходимо открыть транзакцию</summary>
        /// <returns>Коллекцию объектов типа IQueryable</returns>
        public override IQueryable<TypeWorkCr> GetAll()
        {
            // возвращаем тольк оактивные виды работ
            // Если нужны всевиды работ то используйте IRepository
            return base.GetAll().Where(x => x.IsActive);
        }
    }
}