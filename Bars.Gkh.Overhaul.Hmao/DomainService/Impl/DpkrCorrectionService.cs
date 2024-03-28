﻿namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;

    public class DpkrCorrectionService : IDpkrCorrectionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ChangeNumber(BaseParams baseParams)
        {
            var currIndex = baseParams.Params.GetAs<int>("currIndex");
            var destIndex = baseParams.Params.GetAs<int>("destIndex");
            var moId = baseParams.Params.GetAs<long>("moId");
            
            var versionRecordService = Container.ResolveDomain<VersionRecord>();

            if (!versionRecordService.GetAll().Any(x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId && x.IndexNumber == destIndex))
            {
                return new BaseDataResult(false, "Указанный номер не существует");
            }

            var data = versionRecordService.GetAll()
                .Where(x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId)
                .Where(x => x.IndexNumber <= Math.Max(currIndex, destIndex))
                .Where(x => x.IndexNumber >= Math.Min(currIndex, destIndex))
                .ToList();

            var listToSave = new List<VersionRecord>();

            //если текущий индекс больше того на который переводим - значит движемся вверх по очереди, к началу
            var isMoveUp = currIndex > destIndex;

            foreach (var record in data)
            {
                if (record.IndexNumber == currIndex)
                {
                    record.IndexNumber = destIndex;
                }
                else
                {
                    record.IndexNumber += isMoveUp ? 1 : -1;
                }

                listToSave.Add(record);
            }

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listToSave.ForEach(x =>
                    {
                        if (x.Id > 0)
                        {
                            versionRecordService.Update(x);
                        }
                        else
                        {
                            versionRecordService.Save(x);
                        }
                    });
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
    }
}
