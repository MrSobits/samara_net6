namespace Bars.GkhGji.Regions.BaseChelyabinsk.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Utils;

    using NHibernate;

    public class MigrateActCheckRemovalToActRemovalAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<ChelyabinskActCheck> ActCheckDomain { get; set; }

        public IDomainService<ChelyabinskActRemoval> ActRemovalDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<ActCheckWitness> ActCheckWitnessDomain { get; set; }

        public IDomainService<ActRemovalWitness> ActRemovalWitnessDomain { get; set; }

        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }

        public IDomainService<ActRemovalPeriod> ActRemovalPeriodDomain { get; set; }

        public IDomainService<ActCheckProvidedDoc> ActCheckProvDocDomain { get; set; }

        public IDomainService<ActRemovalProvidedDoc> ActRemovalProvDocDomain { get; set; }

        public IDomainService<ActCheckInspectedPart> ActCheckInspectedPartDomain { get; set; }

        public IDomainService<ActRemovalInspectedPart> ActRemovalInspectedPartDomain { get; set; }

        public IDomainService<ActCheckDefinition> ActCheckDefinitionDomain { get; set; }

        public IDomainService<ActRemovalDefinition> ActRemovalDefinitionDomain { get; set; }

        public IDomainService<ActCheckAnnex> ActCheckAnnexDomain { get; set; }

        public IDomainService<ActRemovalAnnex> ActRemovalAnnexDomain { get; set; }

        public override string Name => "Миграция вторичных актов проверки";

        public override string Description
            => @"Миграция вторичных актов проверки, созданных между приказом на проверку предписания и актом проверки предписания";

        public override Func<IDataResult> Action => this.Execute;

        public BaseDataResult Execute()
        {
            //удаляем вторичные акты проверки из дерева
            var actChecksRemoval = this.ActCheckDomain.GetAll()
                .Where(x => x.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
                .Where(x => x.Stage != null)
                .ToArray();

            var listStagesToDelete = new List<long>();
            var listWitnesses = new List<ActRemovalWitness>();
            var listPeriods = new List<ActRemovalPeriod>();
            var listProvDocs = new List<ActRemovalProvidedDoc>();
            var listInsParts = new List<ActRemovalInspectedPart>();
            var listDefinitions = new List<ActRemovalDefinition>();
            var listAnnexes = new List<ActRemovalAnnex>();

            this.InTransaction(
                () =>
                {
                    actChecksRemoval.ForEach(
                        x =>
                        {
                            if (x.Stage != null)
                            {
                                listStagesToDelete.Add(x.Stage.Id);

                                x.Stage = null;
                                this.DocumentGjiDomain.Update(x);

                                //переносим данные из вторичного акта проверки в акт проверки предписания
                                var actRemoval =
                                    Utils.GetChildDocumentByType(this.DocumentGjiChildrenDomain, x, TypeDocumentGji.ActRemoval) as ChelyabinskActRemoval;
                                if (actRemoval != null)
                                {
                                    actRemoval.AcquaintedWithDisposalCopy = x.AcquaintedWithDisposalCopy;
                                    actRemoval.DocumentPlace = x.DocumentPlace;
                                    actRemoval.DocumentTime = x.DocumentTime;

                                    this.ActRemovalDomain.Update(actRemoval);

                                    listWitnesses.AddRange(
                                        this.ActCheckWitnessDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalWitness()
                                                {
                                                    ActRemoval = actRemoval,
                                                    Fio = old.Fio,
                                                    ExternalId = old.Fio,
                                                    IsFamiliar = old.IsFamiliar,
                                                    Position = old.Position
                                                })
                                        );

                                    listPeriods.AddRange(
                                        this.ActCheckPeriodDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalPeriod()
                                                {
                                                    ActRemoval = actRemoval,
                                                    DateCheck = old.DateCheck,
                                                    DateStart = old.DateStart,
                                                    DateEnd = old.DateEnd
                                                })
                                        );

                                    listProvDocs.AddRange(
                                        this.ActCheckProvDocDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalProvidedDoc()
                                                {
                                                    ActRemoval = actRemoval,
                                                    DateProvided = old.DateProvided,
                                                    ProvidedDoc = old.ProvidedDoc
                                                })
                                        );

                                    listInsParts.AddRange(
                                        this.ActCheckInspectedPartDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalInspectedPart()
                                                {
                                                    ActRemoval = actRemoval,
                                                    Character = old.Character,
                                                    ExternalId = old.ExternalId,
                                                    Description = old.Description,
                                                    InspectedPart = old.InspectedPart
                                                })
                                        );

                                    listDefinitions.AddRange(
                                        this.ActCheckDefinitionDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalDefinition()
                                                {
                                                    ActRemoval = actRemoval,
                                                    DocumentNum = old.DocumentNum,
                                                    DocumentDate = old.DocumentDate,
                                                    DocumentNumber = old.DocumentNumber,
                                                    Description = old.Description,
                                                    ExecutionDate = old.ExecutionDate,
                                                    ExternalId = old.ExternalId,
                                                    IssuedDefinition = old.IssuedDefinition,
                                                    TypeDefinition = old.TypeDefinition
                                                })
                                        );

                                    listAnnexes.AddRange(
                                        this.ActCheckAnnexDomain.GetAll()
                                            .Where(y => y.ActCheck.Id == x.Id)
                                            .Select(
                                                old => new ActRemovalAnnex()
                                                {
                                                    ActRemoval = actRemoval,
                                                    Description = old.Description,
                                                    DocumentDate = old.DocumentDate,
                                                    ExternalId = old.ExternalId,
                                                    File = old.File,
                                                    Name = old.Name
                                                })
                                        );
                                }
                            }
                        });

                    listStagesToDelete.Distinct().ForEach(x => this.InspectionStageDomain.Delete(x));

                    TransactionHelper.InsertInManyTransactions(this.Container, listWitnesses);
                    TransactionHelper.InsertInManyTransactions(this.Container, listPeriods);
                    TransactionHelper.InsertInManyTransactions(this.Container, listProvDocs);
                    TransactionHelper.InsertInManyTransactions(this.Container, listInsParts);
                    TransactionHelper.InsertInManyTransactions(this.Container, listDefinitions);
                    TransactionHelper.InsertInManyTransactions(this.Container, listAnnexes);
                });

            return new BaseDataResult();
        }

        private void InTransaction(Action action)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var session = this.SessionProvider.GetCurrentSession();
                    var oldFlush = session.FlushMode;
                    session.FlushMode = FlushMode.Never;

                    action();

                    session.FlushMode = oldFlush;
                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}