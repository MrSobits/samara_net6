namespace Bars.GkhGji.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;

    public class TypeSurveyDictMigrationAction : BaseExecutionAction
    {
        public override Func<IDataResult> Action => this.Execute;

        public override string Description => "Перенос значений зависимых сущностей на использование справочников";

        public override string Name => "ГЖИ - Миграция значений справочника Типы обследования";

        private BaseDataResult Execute()
        {
            this.MigrateGoals();
            this.MigrateTasks();
            this.MigrateInspFoundations();

            return new BaseDataResult();
        }

        private void MigrateGoals()
        {
            var purposeDomain = this.Container.ResolveDomain<SurveyPurpose>();
            var goalsDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();

            using (this.Container.Using(purposeDomain, goalsDomain))
            {
                var purposes =
                    purposeDomain.GetAll()
                        .AsEnumerable()
                        .GroupBy(x => x.Name.ToLower())
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var maxCode = purposes.Values.Select(x => x.Code.ToLong()).SafeMax(x => x);
                var goals = goalsDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToLower())
                    .ToDictionary(x => x.Key, x => x.ToArray());
                var purposesToSave = new List<SurveyPurpose>();
                var goalsToSave = new List<TypeSurveyGoalInspGji>();
                foreach (var goalGroup in goals)
                {
                    SurveyPurpose purpose;
                    if (!purposes.TryGetValue(goalGroup.Key, out purpose))
                    {
                        var name = goalGroup.Value[0].Name;
                        purpose = new SurveyPurpose {Code = (++maxCode).ToString(), Name = name};
                        purposesToSave.Add(purpose);
                    }

                    foreach (var goal in goalGroup.Value)
                    {
                        goal.SurveyPurpose = purpose;
                        goalsToSave.Add(goal);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, purposesToSave);
                TransactionHelper.InsertInManyTransactions(this.Container, goalsToSave);
            }
        }

        private void MigrateTasks()
        {
            var objectiveDomain = this.Container.ResolveDomain<SurveyObjective>();
            var tasksDomain = this.Container.ResolveDomain<TypeSurveyTaskInspGji>();

            using (this.Container.Using(objectiveDomain, tasksDomain))
            {
                var objectives =
                    objectiveDomain.GetAll()
                        .AsEnumerable()
                        .GroupBy(x => x.Name.ToLower())
                        .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var maxCode = objectives.Values.Select(x => x.Code.ToLong()).SafeMax(x => x);
                var tasks = tasksDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToLower())
                    .ToDictionary(x => x.Key, x => x.ToArray());
                var objectivesToSave = new List<SurveyObjective>();
                var tasksToSave = new List<TypeSurveyTaskInspGji>();
                foreach (var taskGroup in tasks)
                {
                    SurveyObjective objective;
                    if (!objectives.TryGetValue(taskGroup.Key, out objective))
                    {
                        var name = taskGroup.Value[0].Name;
                        objective = new SurveyObjective {Code = (++maxCode).ToString(), Name = name};
                        objectivesToSave.Add(objective);
                    }

                    foreach (var task in taskGroup.Value)
                    {
                        task.SurveyObjective = objective;
                        tasksToSave.Add(task);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, objectivesToSave);
                TransactionHelper.InsertInManyTransactions(this.Container, tasksToSave);
            }
        }

        private void MigrateInspFoundations()
        {
            var normativeDocDomain = this.Container.ResolveDomain<NormativeDoc>();
            var inspFoundationDomain = this.Container.ResolveDomain<TypeSurveyInspFoundationGji>();

            using (this.Container.Using(normativeDocDomain, inspFoundationDomain))
            {
                var normativeDocs = normativeDocDomain.GetAll().ToArray();
                var normativeDocsDict =
                    normativeDocs.Select(x => new {key = x.Name.ToLower(), value = x})
                        .Union(normativeDocs.Select(x => new {key = x.FullName.ToLower(), value = x}))
                        .GroupBy(x => x.key)
                        .ToDictionary(x => x.Key, x => x.First().value);

                var maxCode = normativeDocs.Select(x => x.Code.ToInt()).SafeMax(x => x);
                var inspFoundations = inspFoundationDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.Name.ToLower())
                    .ToDictionary(x => x.Key, x => x.ToArray());
                var normativeDocsToSave = new List<NormativeDoc>();
                var inspFoundationsToSave = new List<TypeSurveyInspFoundationGji>();
                foreach (var inspFoundationGroup in inspFoundations)
                {
                    NormativeDoc normativeDoc;
                    if (!normativeDocsDict.TryGetValue(inspFoundationGroup.Key, out normativeDoc))
                    {
                        var name = inspFoundationGroup.Value[0].Name;
                        normativeDoc = new NormativeDoc
                        {
                            Code = ++maxCode,
                            Name = name.Length > 300 ? name.Substring(0, 300) : name,
                            FullName = name
                        };
                        normativeDocsToSave.Add(normativeDoc);
                    }

                    foreach (var inspFoundation in inspFoundationGroup.Value)
                    {
                        inspFoundation.NormativeDoc = normativeDoc;
                        inspFoundationsToSave.Add(inspFoundation);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, normativeDocsToSave);
                TransactionHelper.InsertInManyTransactions(this.Container, inspFoundationsToSave);
            }
        }
    }
}