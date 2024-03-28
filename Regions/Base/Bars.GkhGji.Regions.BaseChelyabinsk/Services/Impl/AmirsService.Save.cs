namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Services.DataContracts;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Сервис импорта из АМИРС - сохранение
    /// </summary>
    public partial class AmirsService
    {
        public IDomainService<JurInstitution> JurInstitutionDomain { get; set; }
        public IDomainService<ConcederationResult> ConcederationResultDomain { get; set; }

        private JurInstitution GetJudicalOffice(string code)
        {
            var ji = JurInstitutionDomain.GetAll()
                .Where(x => x.Code == code).FirstOrDefault();
            return ji;
        }

        private ConcederationResult GetOrCreateCR(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            var cr = ConcederationResultDomain.GetAll()
                .Where(x => x.Name == code).FirstOrDefault();
            if (cr == null)
            {
                var newCR = new ConcederationResult
                {
                    Code = "Амирс",
                    Name = code
                };
                ConcederationResultDomain.Save(newCR);
                return newCR;
            }

            return cr;
        }

        private DateTime? GetDateTime(string dt)
        {
            if (string.IsNullOrEmpty(dt))
            {
                return null;
            }
            try
            {
                return Convert.ToDateTime(dt);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Сохранение
        /// </summary>
        /// <returns></returns>
        protected IDataResult SaveAmirs(AmirsData data)
        {
            IDomainService<InspectionGjiStage> InspectionStageDomain;
            IDomainService<Resolution> ResolutionDomain;
            IDomainService<DocumentGjiChildren> ChildrenDomain;
            IDomainService<State> StateDomain;

            IConfigProvider configProvider;
            IDomainService<State> stateDomain;

            if (data.resolution_type == "Рассмотрено")
            {


                using (this.Container.Using(InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>(),
                    ResolutionDomain = this.Container.ResolveDomain<Resolution>(),
                    ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>(),
                    configProvider = this.Container.Resolve<IConfigProvider>(),
                    stateDomain = this.Container.ResolveDomain<State>()))
                {
                    string statusStr = string.Empty;
                    try
                    {
                        var amirsState = stateDomain.GetAll()
                  .Where(x => x.Code == "АМИРС").FirstOrDefault();

                        //var protocol = ProtocolDomain.GetAll()
                        //        .Where(x => x.DocumentNumber == data.prot_num && x.DocumentDate.HasValue && x.DocumentDate.Value == data.prot_date).FirstOrDefault();
                        var protocol = ProtocolDomain.GetAll()
                            .Where(x => x.DocumentNumber == data.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();
                        var protocol197 = Protocol197Domain.GetAll()
                           .Where(x => x.DocumentNumber == data.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();

                        SanctionGji sanction = this.Container.Resolve<IDomainService<SanctionGji>>().GetAll()
                             .Where(x => x.Name == data.sanction).FirstOrDefault();

                        if (protocol == null && protocol197 == null)
                        {
                            return BaseDataResult.Error("Не удалось получить протокол");
                        }

                        if (protocol != null)
                        {
                            if (data.resolution_type == "Определение о возвращении протокола об административном правонарушении и других материалов дела")
                            {
                                //ToDo определение о возврате
                            }
                            var parChildList = ChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == protocol.Id).ToList();
                            var parChild = parChildList
                            .Where(x => !string.IsNullOrEmpty(x.Children.DocumentNumber))
                            .Where(x => x.Children.DocumentNumber.Substring(1, x.Children.DocumentNumber.Length - 1) == data.resolution_num.Substring(1, data.resolution_num.Length - 1) && x.Children.DocumentDate == data.resolution_date)
                            .FirstOrDefault();
                            if (parChild != null)
                            {
                                return BaseDataResult.Error($"К протоколу {data.prot_num} от {data.prot_date.ToShortDateString()} уже прикреплено постановление {data.resolution_num} от {data.resolution_date.ToShortDateString()}");
                            }

                            var resolution = new Resolution()
                            {
                                Inspection = protocol.Inspection,
                                TypeDocumentGji = TypeDocumentGji.Resolution,
                                Contragent = protocol.Contragent,
                                Executant = protocol.Executant,
                                DocumentNumber = data.resolution_num,
                                DocumentDate = data.resolution_date,
                                InLawDate = GetDateTime(data.in_law_date),
                                DecisionEntryDate = GetDateTime(data.in_law_date),
                                PhysicalPerson = protocol.PhysicalPerson,
                                PhysicalPersonInfo = protocol.PhysicalPersonInfo,
                                SectorNumber = data.judical_office,
                                JudicalOffice = GetJudicalOffice(data.judical_office),
                                TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                                ConcederationResult = GetOrCreateCR(data.resolution_type),
                                Sanction = sanction,
                                PenaltyAmount = data.penalty != "" ? Convert.ToDecimal(data.penalty) : 0,
                                Paided = YesNoNotSet.NotSet,
                                Description = protocol.Description,
                                State = amirsState != null ? amirsState : null
                            };

                            #region Формируем этап проверки
                            // Если у родительского документа есть этап у которого есть родитель
                            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
                            var parentStage = protocol.Stage;
                            if (parentStage != null && parentStage.Parent != null)
                            {
                                parentStage = parentStage.Parent;
                            }

                            InspectionGjiStage newStage = null;

                            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

                            if (currentStage == null)
                            {
                                // Если этап не найден, то создаем новый этап
                                currentStage = new InspectionGjiStage
                                {
                                    Inspection = protocol.Inspection,
                                    TypeStage = TypeStage.Resolution,
                                    Parent = parentStage,
                                    Position = 1
                                };
                                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == protocol.Inspection.Id)
                                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                                if (stageMaxPosition != null)
                                {
                                    currentStage.Position = stageMaxPosition.Position + 1;
                                }

                                // Фиксируем новый этап чтобы потом незабыть сохранить 
                                newStage = currentStage;
                            }

                            resolution.Stage = currentStage;
                            #endregion

                            #region формируем связь с родителем
                            var parentChildren = new DocumentGjiChildren
                            {
                                Parent = protocol,
                                Children = resolution
                            };
                            #endregion

                            #region Сохранение
                            using (var tr = Container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    if (newStage != null)
                                    {
                                        InspectionStageDomain.Save(newStage);
                                    }

                                    ResolutionDomain.Save(resolution);

                                    ChildrenDomain.Save(parentChildren);

                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                    return new BaseDataResult(false, "ошибка транзакции БД");
                                }
                            }
                            #endregion
                        }
                        else if (protocol197 != null)
                        {
                            if (data.resolution_type == "Определение о возвращении протокола об административном правонарушении и других материалов дела")
                            {
                                //ToDo определение о возврате
                            }
                            var parChildList = ChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == protocol197.Id).ToList();
                            var parChild = parChildList
                            .Where(x => !string.IsNullOrEmpty(x.Children.DocumentNumber))
                            .Where(x => x.Children.DocumentNumber.Substring(1, x.Children.DocumentNumber.Length - 1) == data.resolution_num.Substring(1, data.resolution_num.Length - 1) && x.Children.DocumentDate == data.resolution_date)
                            .FirstOrDefault();
                            if (parChild != null)
                            {
                                return BaseDataResult.Error($"К протоколу {data.prot_num} от {data.prot_date.ToShortDateString()} уже прикреплено постановление {data.resolution_num} от {data.resolution_date.ToShortDateString()}");
                            }

                            var resolution = new Resolution()
                            {
                                Inspection = protocol197.Inspection,
                                TypeDocumentGji = TypeDocumentGji.Resolution,
                                Contragent = protocol197.Contragent,
                                Executant = protocol197.Executant,
                                DocumentNumber = data.resolution_num,
                                InLawDate = GetDateTime(data.in_law_date),
                                DocumentDate = data.resolution_date,
                                JudicalOffice = GetJudicalOffice(data.judical_office),
                                DecisionEntryDate = GetDateTime(data.in_law_date),
                                PhysicalPerson = protocol197.PhysicalPerson,
                                ConcederationResult = GetOrCreateCR(data.resolution_type),
                                PhysicalPersonInfo = protocol197.PhysicalPersonInfo,
                                SectorNumber = data.judical_office,
                                TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                                Sanction = sanction,
                                PenaltyAmount = data.penalty != "" ? Convert.ToDecimal(data.penalty) : 0,
                                State = amirsState != null ? amirsState : null,
                                Paided = YesNoNotSet.NotSet,
                                Description = protocol197.Description
                            };

                            #region Формируем этап проверки
                            // Если у родительского документа есть этап у которого есть родитель
                            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
                            var parentStage = protocol197.Stage;
                            if (parentStage != null && parentStage.Parent != null)
                            {
                                parentStage = parentStage.Parent;
                            }

                            InspectionGjiStage newStage = null;

                            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

                            if (currentStage == null)
                            {
                                // Если этап не найден, то создаем новый этап
                                currentStage = new InspectionGjiStage
                                {
                                    Inspection = protocol197.Inspection,
                                    TypeStage = TypeStage.Resolution,
                                    Parent = parentStage,
                                    Position = 1
                                };
                                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == protocol197.Inspection.Id)
                                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                                if (stageMaxPosition != null)
                                {
                                    currentStage.Position = stageMaxPosition.Position + 1;
                                }

                                // Фиксируем новый этап чтобы потом незабыть сохранить 
                                newStage = currentStage;
                            }

                            resolution.Stage = currentStage;
                            #endregion

                            #region формируем связь с родителем
                            var parentChildren = new DocumentGjiChildren
                            {
                                Parent = protocol197,
                                Children = resolution
                            };
                            #endregion

                            #region Сохранение
                            using (var tr = Container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    if (newStage != null)
                                    {
                                        InspectionStageDomain.Save(newStage);
                                    }

                                    ResolutionDomain.Save(resolution);

                                    ChildrenDomain.Save(parentChildren);

                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                    return new BaseDataResult(false, "ошибка транзакции БД");
                                }
                            }
                            #endregion
                        }


                    }
                    catch (Exception exception)
                    {
                        return BaseDataResult.Error((exception.InnerException ?? exception).Message + "");
                    }
                }
            }
            else
            {
                using (this.Container.Using(InspectionStageDomain = this.Container.ResolveDomain<InspectionGjiStage>(),
                   ResolutionDomain = this.Container.ResolveDomain<Resolution>(),
                   ChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>(),
                   configProvider = this.Container.Resolve<IConfigProvider>(),
                   stateDomain = this.Container.ResolveDomain<State>()))
                {
                    var amirsState = stateDomain.GetAll()
                 .Where(x => x.Code == "АМИРС").FirstOrDefault();
                    var protocol = ProtocolDomain.GetAll()
                        .Where(x => x.DocumentNumber == data.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();
                    var protocol197 = Protocol197Domain.GetAll()
                       .Where(x => x.DocumentNumber == data.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();
                    if (protocol == null && protocol197 == null)
                    {
                        return BaseDataResult.Error("Не удалось получить протокол");
                    }
                    if (protocol != null)
                    {
                        protocol.Description = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString();
                        var parChildList = ChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == protocol.Id).ToList();
                        var parChild = parChildList
                     .Where(x => !string.IsNullOrEmpty(x.Children.DocumentNumber))
                         .Where(x => x.Children.DocumentNumber.Substring(1, x.Children.DocumentNumber.Length - 1) == data.resolution_num.Substring(1, data.resolution_num.Length - 1) && x.Children.DocumentDate == data.resolution_date)
                         .FirstOrDefault();
                        SanctionGji sanction = this.Container.Resolve<IDomainService<SanctionGji>>().GetAll()
                        .Where(x => x.Code == "0").FirstOrDefault();
                        if (parChild != null)
                        {
                          
                            var resolution = ResolutionDomain.Get(parChild.Children.Id);
                            resolution.Comment = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString();
                            resolution.Sanction = sanction;
                            resolution.PenaltyAmount = 0;
                            resolution.InLawDate = GetDateTime(data.in_law_date);
                            ResolutionDomain.Update(resolution);
                        }
                        else
                        {
                            var resolution = new Resolution()
                            {
                                Inspection = protocol.Inspection,
                                TypeDocumentGji = TypeDocumentGji.Resolution,
                                Contragent = protocol.Contragent,
                                Executant = protocol.Executant,
                                DocumentNumber = data.resolution_num,
                                ConcederationResult = GetOrCreateCR(data.resolution_type),
                                DocumentDate = data.resolution_date,
                                PhysicalPerson = protocol.PhysicalPerson,
                                InLawDate = GetDateTime(data.in_law_date),
                                DecisionEntryDate = GetDateTime(data.in_law_date),
                                JudicalOffice = GetJudicalOffice(data.judical_office),
                                Comment = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString(),
                                PhysicalPersonInfo = protocol.PhysicalPersonInfo,
                                SectorNumber = data.judical_office,
                                TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                                Sanction = sanction,
                                PenaltyAmount = data.penalty != "" ? Convert.ToDecimal(data.penalty) : 0,
                                State = amirsState != null ? amirsState : null,
                                Paided = YesNoNotSet.NotSet,
                                Description = protocol.Description
                            };
                            var parentStage = protocol.Stage;
                            if (parentStage != null && parentStage.Parent != null)
                            {
                                parentStage = parentStage.Parent;
                            }

                            InspectionGjiStage newStage = null;

                            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

                            if (currentStage == null)
                            {
                                // Если этап не найден, то создаем новый этап
                                currentStage = new InspectionGjiStage
                                {
                                    Inspection = protocol.Inspection,
                                    TypeStage = TypeStage.Resolution,
                                    Parent = parentStage,
                                    Position = 1
                                };
                                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == protocol.Inspection.Id)
                                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                                if (stageMaxPosition != null)
                                {
                                    currentStage.Position = stageMaxPosition.Position + 1;
                                }

                                // Фиксируем новый этап чтобы потом незабыть сохранить 
                                newStage = currentStage;
                            }

                            resolution.Stage = currentStage;

                            #region формируем связь с родителем
                            var parentChildren = new DocumentGjiChildren
                            {
                                Parent = protocol,
                                Children = resolution
                            };
                            #endregion

                            #region Сохранение
                            using (var tr = Container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    if (newStage != null)
                                    {
                                        InspectionStageDomain.Save(newStage);
                                    }

                                    ResolutionDomain.Save(resolution);

                                    ChildrenDomain.Save(parentChildren);

                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                    return new BaseDataResult(false, "ошибка транзакции БД");
                                }
                            }
                            #endregion
                        }
                    }
                    if (protocol197 != null)
                    {
                        protocol197.Description = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString();
                        var parChildList = ChildrenDomain.GetAll()
                            .Where(x => x.Parent.Id == protocol197.Id).ToList();
                        var parChild = parChildList
                     .Where(x => !string.IsNullOrEmpty(x.Children.DocumentNumber))
                         .Where(x => x.Children.DocumentNumber.Substring(1, x.Children.DocumentNumber.Length - 1) == data.resolution_num.Substring(1, data.resolution_num.Length - 1) && x.Children.DocumentDate == data.resolution_date)
                         .FirstOrDefault();
                        SanctionGji sanction = this.Container.Resolve<IDomainService<SanctionGji>>().GetAll()
                        .Where(x => x.Code == "0").FirstOrDefault();
                        if (parChild != null)
                        {

                            var resolution = ResolutionDomain.Get(parChild.Children.Id);
                            resolution.Comment = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString();
                            resolution.Sanction = sanction;
                            resolution.PenaltyAmount = 0;
                        }
                        else
                        {
                            var resolution = new Resolution()
                            {
                                Inspection = protocol197.Inspection,
                                TypeDocumentGji = TypeDocumentGji.Resolution,
                                Contragent = protocol197.Contragent,
                                Executant = protocol197.Executant,
                                DocumentNumber = data.resolution_num,
                                JudicalOffice = GetJudicalOffice(data.judical_office),
                                DocumentDate = data.resolution_date,
                                InLawDate = GetDateTime(data.in_law_date),
                                PhysicalPerson = protocol197.PhysicalPerson,
                                DecisionEntryDate = GetDateTime(data.in_law_date),
                                Comment = data.resolution_type + " N" + data.resolution_num + "  от " + data.resolution_date.ToShortDateString(),
                                PhysicalPersonInfo = protocol197.PhysicalPersonInfo,
                                SectorNumber = data.judical_office,
                                ConcederationResult = GetOrCreateCR(data.resolution_type),
                                TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                                Sanction = sanction,
                                PenaltyAmount = data.penalty != "" ? Convert.ToDecimal(data.penalty) : 0,
                                State = amirsState != null ? amirsState : null,
                                Paided = YesNoNotSet.NotSet,
                                Description = protocol197.Description
                            };
                            #region Формируем этап проверки
                            // Если у родительского документа есть этап у которого есть родитель
                            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
                            var parentStage = protocol197.Stage;
                            if (parentStage != null && parentStage.Parent != null)
                            {
                                parentStage = parentStage.Parent;
                            }

                            InspectionGjiStage newStage = null;

                            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

                            if (currentStage == null)
                            {
                                // Если этап не найден, то создаем новый этап
                                currentStage = new InspectionGjiStage
                                {
                                    Inspection = protocol197.Inspection,
                                    TypeStage = TypeStage.Resolution,
                                    Parent = parentStage,
                                    Position = 1
                                };
                                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == protocol197.Inspection.Id)
                                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                                if (stageMaxPosition != null)
                                {
                                    currentStage.Position = stageMaxPosition.Position + 1;
                                }

                                // Фиксируем новый этап чтобы потом незабыть сохранить 
                                newStage = currentStage;
                            }

                            resolution.Stage = currentStage;
                            #endregion

                            #region формируем связь с родителем
                            var parentChildren = new DocumentGjiChildren
                            {
                                Parent = protocol197,
                                Children = resolution
                            };
                            #endregion

                            #region Сохранение
                            using (var tr = Container.Resolve<IDataTransaction>())
                            {
                                try
                                {
                                    if (newStage != null)
                                    {
                                        InspectionStageDomain.Save(newStage);
                                    }

                                    ResolutionDomain.Save(resolution);

                                    ChildrenDomain.Save(parentChildren);

                                    tr.Commit();
                                }
                                catch
                                {
                                    tr.Rollback();
                                    return new BaseDataResult(false, "ошибка транзакции БД");
                                }
                            }
                            #endregion
                        }
                    }
                }

            }
            

            return new BaseDataResult();
        }
    }
}
