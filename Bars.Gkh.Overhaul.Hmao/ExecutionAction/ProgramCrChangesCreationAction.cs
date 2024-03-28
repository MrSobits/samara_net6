namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class ProgramCrChangesCreationAction : BaseExecutionAction
    {
        public IDomainService<ProgramCr> programCrDomain { get; set; }

        public IDomainService<ProgramCrChangeJournal> programCrChangeDomain { get; set; }

        public IDomainService<TypeWorkCrVersionStage1> tyoeWorkVersionDomain { get; set; }

        public override string Name => "ДПКР ХМАО - Проставление истории для Программы КР";

        public override string Description => @"Если у программы КР нет истории изменения, то метод проставляет историю изменения";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var listToSave = new List<ProgramCrChangeJournal>();

            var dlistProgramCrByDpkr = this.tyoeWorkVersionDomain.GetAll().Select(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id).Distinct().ToList();

            // Если в программе есть работы импортированные из ДПКР то проверяем если нет записи 
            // об изменении с признаком = 'Импортировано из ДПКР' то создаем его
            listToSave.AddRange(
                this.programCrDomain.GetAll()
                    .Where(x => dlistProgramCrByDpkr.Contains(x.Id))
                    .Where(x => !this.programCrChangeDomain.GetAll().Any(y => y.ProgramCr.Id == x.Id && y.TypeChange == TypeChangeProgramCr.FromDpkr))
                    .Select(x => new {x.Id, x.ObjectCreateDate})
                    .AsEnumerable()
                    .Select(
                        x =>
                            new ProgramCrChangeJournal()
                            {
                                ProgramCr = new ProgramCr {Id = x.Id},
                                MuCount = 46,
                                TypeChange = TypeChangeProgramCr.FromDpkr,
                                UserName = "Администратор",
                                ChangeDate = x.ObjectCreateDate
                            })
                    .ToList());

            // Если программа сформировна в ручную и нет записи об изменении то создаем ее
            listToSave.AddRange(
                this.programCrDomain.GetAll()
                    .Where(x => !dlistProgramCrByDpkr.Contains(x.Id))
                    .Where(x => !this.programCrChangeDomain.GetAll().Any(y => y.ProgramCr.Id == x.Id && y.TypeChange == TypeChangeProgramCr.Manually))
                    .Select(x => new {x.Id, x.ObjectCreateDate})
                    .AsEnumerable()
                    .Select(
                        x =>
                            new ProgramCrChangeJournal()
                            {
                                ProgramCr = new ProgramCr {Id = x.Id},
                                MuCount = 46,
                                TypeChange = TypeChangeProgramCr.Manually,
                                UserName = "Администратор",
                                ChangeDate = x.ObjectCreateDate
                            })
                    .ToList());

            if (listToSave.Any())
            {
                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(this.programCrChangeDomain.Save);
                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        throw exc;
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}