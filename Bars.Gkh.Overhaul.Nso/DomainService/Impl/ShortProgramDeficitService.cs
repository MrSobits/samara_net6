namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;
    using Gkh.Entities;

    public class ShortProgramDeficitService: IShortProgramDeficitService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<Municipality> ServiceMunicipality { get; set; }

        public IDomainService<ProgramVersion> VersionDomain { get; set; }

        public IDomainService<ShortProgramDifitsit> ShortProgramDeficitDomain { get; set; }

        public IDomainService<ShortProgramRecord> ShortProgramRecordDomain { get; set; } 

        public IDataResult SaveDeficit(BaseParams baseParams)
        {
            var records = baseParams.Params.GetAs<DeficitProxy[]>("records");
            var listToSave = new List<ShortProgramDifitsit>();

            long versionId = 0;
            
            foreach (var rec in records)
            {
                var newRec = new ShortProgramDifitsit
                {
                    Municipality = ServiceMunicipality.Load(rec.MuId),
                    BudgetRegionShare = rec.Sha,
                    Difitsit = rec.Def,
                    Year = rec.Year,
                    Version = VersionDomain.Load(rec.VersionId)
                };

                versionId = rec.VersionId;

                listToSave.Add(newRec);
            }

            var ids = ShortProgramDeficitDomain.GetAll()
                .Where(x => x.Version.Id == versionId)
                .Select(x => x.Id)
                .ToList();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        ShortProgramDeficitDomain.Delete(id);
                    }

                    foreach (var entity in listToSave)
                    {
                        ShortProgramDeficitDomain.Save(entity);
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        // Метод создания краткосрочной программы
        public IDataResult CreateShortProgram(BaseParams baseParams)
        {
            var version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                throw new ValidationException("Не найдена основная версия!");
            }

            var listToSave = new List<ShortProgramRecord>();

            // получаем дефициты 
            var dataDeficit = ShortProgramDeficitDomain.GetAll()
                                         .Where(x => x.Version.Id == version.Id)
                                         .AsEnumerable()
                                         .GroupBy(x => x.Municipality.Id)
                                         .ToDictionary(x => x.Key, y => y.ToList());

            // Поулчаем словарь существующих записей
            var currentDic = ShortProgramRecordDomain.GetAll()
                                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                                        .AsEnumerable()
                                        .GroupBy(x => x.Id)
                                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var dataRecords = ShortProgramRecordDomain.GetAll()
                                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == version.Id)
                                        .Select( x => new
                                                {
                                                    x.Id,
                                                    MunicipalityId = x.Stage2.Stage3Version.RealityObject.Municipality.Id,
                                                    x.Year,
                                                    Deficit = x.Difitsit
                                                })
                                        .ToList();

            foreach (var rec in dataRecords)
            {
                // Если у записи Дифицит > 0 то значит требуется его раскидать
                // Алгоритм раскидывания дефицита слудующий:
                // Необходимо по году и МО получить Долю из справочника dataDeficit
                // Даллее по формуле (Дифицит * Долю ) уходит в поле 
                if (currentDic.ContainsKey(rec.Id) && rec.Deficit > 0)
                {
                    var curRec = currentDic[rec.Id];
                    var share = 0M;
                    if (dataDeficit.ContainsKey(rec.MunicipalityId))
                    {
                        share = dataDeficit[rec.MunicipalityId].Where(x => x.Year == rec.Year).Select(x => x.BudgetRegionShare).FirstOrDefault();
                    }

                    // Высчитываем сумму из Бюджета Региона
                    curRec.BudgetRegion = (rec.Deficit * share / 100).RoundDecimal();

                    // Суммой из Бюджета МО будет оставшая часть от Дефицита - БюджетРегиона
                    curRec.BudgetMunicipality = rec.Deficit - curRec.BudgetRegion;

                    listToSave.Add(curRec);
                }
            }

            // Сохраняем то что получилось
            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // Удаление записей Краткосрочной программы
                    listToSave.ForEach(x => ShortProgramRecordDomain.Update(x));
                    tx.Commit();
                }
                catch (ValidationException e)
                {
                    tx.Rollback();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        private class DeficitProxy
        {
            public long MuId { get; set; }
            public decimal Def { get; set; }
            public int Year { get; set; }
            public decimal Sha { get; set; }
            public long VersionId { get; set; }
        }
    }
}