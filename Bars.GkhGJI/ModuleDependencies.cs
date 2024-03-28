namespace Bars.GkhGji
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ModuleDependencies : BaseModuleDependencies
    {
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        public override IModuleDependencies Init()
        {
            References.Add(new EntityReference
                {
                    ReferenceName = "Деятельность ТСЖ",
                    BaseEntity = typeof(ManagingOrganization),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<ActivityTsj>>();
                        var itemCollection = service
                                .GetAll()
                                .Where(x => x.ManagingOrganization.Id == id)
                                .Select(x => x.Id)
                                .ToArray();

                        foreach (var item in itemCollection)
                        {
                            service.Delete(item);
                        }
                    }
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Дома протокола деятельности ТСЖ",
                    BaseEntity = typeof(RealityObject),
                    CheckAnyDependences = id => Container.Resolve<IDomainService<ActivityTsjProtocolRealObj>>().GetAll().Any(x => x.RealityObject.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Дома деятельности ТСЖ",
                    BaseEntity = typeof(RealityObject),
                    CheckAnyDependences = id => Container.Resolve<IDomainService<ActivityTsjRealObj>>().GetAll().Any(x => x.RealityObject.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Уставы деятельности ТСЖ",
                    BaseEntity = typeof(State),
                    CheckAnyDependences = id => Container.Resolve<IDomainService<ActivityTsjStatute>>().GetAll().Any(x => x.State.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Реестр обращений",
                    BaseEntity = typeof(ManagingOrganization),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<AppealCits>>();
                        var itemCollection = service
                                .GetAll()
                                .Where(x => x.ManagingOrganization.Id == id);

                        foreach (var item in itemCollection)
                        {
                            item.ManagingOrganization = null;
                            service.Update(item);
                        }
                    }
                });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Реестр обращений",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<AppealCits>>();
                        var list = service.GetAll().Where(x => x.ApprovalContragent.Id == id);
                        foreach (var value in list)
                        {
                            value.ApprovalContragent = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Реестр обращений",
                    BaseEntity = typeof(State),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.State.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Реестр обращений",
                    BaseEntity = typeof(Inspector),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.Surety.Id == id || x.Executant.Id == id || x.Tester.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Реестр обращений",
                    BaseEntity = typeof(ZonalInspection),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<AppealCits>>().GetAll().Any(x => x.ZonalInspection.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Место возникновения проверки реестра обращений",
                    BaseEntity = typeof(RealityObject),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<AppealCitsRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
                });


            References.Add(new EntityReference
            {
                ReferenceName = "Ответы  по обращению граждан",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<AppealCitsAnswer>>().GetAll().Any(x => x.Executor.Id == id)
            });

            References.Add(new EntityReference
                {
                    ReferenceName = "Уведомления о начале предпринимательской деятельности",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<BusinessActivity>>();
                        service.GetAll().Where(x => x.Contragent.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Подготовка к отопительному сезону",
                    BaseEntity = typeof(RealityObject),
                    DeleteAnyDependences = id =>
                        {
                            var service = Container.Resolve<IDomainService<HeatSeason>>();
                            var list = service.GetAll().Where(x => x.RealityObject.Id == id).Select(x => x.Id).ToList();
                            foreach (var value in list)
                            {
                                service.Delete(value);
                            }
                        }
                }); 

            References.Add(new EntityReference
                {
                    ReferenceName = "Документ подготовки к отопительному сезону",
                    BaseEntity = typeof(State),
                    CheckAnyDependences = id => Container.Resolve<IDomainService<HeatSeasonDoc>>().GetAll().Any(x => x.State.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Проверка по поручению руководителя",
                    BaseEntity = typeof(Inspector),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<BaseDispHead>>().GetAll().Any(x => x.Head.Id == id)
                });

            #region InspectionGji

            References.Add(new EntityReference
                {
                    ReferenceName = "Проверки ГЖИ",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = this.Container.Resolve<IDomainService<InspectionGji>>();
                        service.GetAll().Where(x => x.Contragent.Id == id)
                            .Select(x => x.Id).ForEach(x => service.Delete(x));
                    }
                });

            References.Add(new EntityReference
            {
                ReferenceName = "Основание деятельность ТСЖ",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseActivityTsj>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseDefault",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseDefault>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseDispHead",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseDispHead>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseHeatSeason",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseHeatSeason>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseInsCheck",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseInsCheck>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseJurPerson",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseJurPerson>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseJurPerson",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseJurPerson>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BasePlanAction",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BasePlanAction>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseProsClaim",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseProsClaim>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseProsResol",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseProsResol>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseProtocolMvd",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseProtocolMvd>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "BaseStatement",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<BaseStatement>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });
            
            #endregion

            References.Add(new EntityReference
                {
                    ReferenceName = "Инспектора в проверке",
                    BaseEntity = typeof(Inspector),
                    CheckAnyDependences = id => this.Container.Resolve<IDomainService<InspectionGjiInspector>>().GetAll().Any(x => x.Inspector.Id == id)
                });

            References.Add(new EntityReference
                {
                    ReferenceName = "Нарушение проверки ГЖИ",
                    BaseEntity = typeof(RealityObject),
                    CheckAnyDependences = id => Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll().Any(x => x.RealityObject.Id == id)
                });

            References.Add(new EntityReference
            {
                ReferenceName = "Реестр документов ГЖИ",
                BaseEntity = typeof(State),
                CheckAnyDependences = id => Container.Resolve<IDomainService<DocumentGji>>().GetAll().Any(x => x.State.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Инспекторы в реестре документов ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll().Any(x => x.Inspector.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Определение акта проверки ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ActCheckDefinition>>().GetAll().Any(x => x.IssuedDefinition.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Дома акта проверки ГЖИ",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Дома акта обследования ГЖИ",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ActSurveyRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Рапоряжения ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Disposal>>().GetAll().Any(x => x.IssuedDisposal.Id == id || x.ResponsibleExecution.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Предписание",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<Prescription>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Решения об отмене в предписании ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<PrescriptionCancel>>().GetAll().Any(x => x.IssuedCancel.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Представления ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Presentation>>().GetAll().Any(x => x.Official.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Представления ГЖИ",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<Presentation>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Протоколы ГЖИ",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<Protocol>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(
                new EntityReference
                {
                    ReferenceName = "Напоминание по действиям ГЖИ",
                    BaseEntity = typeof(Contragent),
                    DeleteAnyDependences = id =>
                    {
                        var service = Container.Resolve<IRepository<Reminder>>();
                        var list = service.GetAll().Where(x => x.Contragent.Id == id);
                        foreach (var value in list)
                        {
                            value.Contragent = null;
                            service.Update(value);
                        }
                    }
                });

            References.Add(new EntityReference
            {
                ReferenceName = "Определения в протоколе ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ProtocolDefinition>>().GetAll().Any(x => x.IssuedDefinition.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Постановления прокуратуры",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<ResolPros>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Постановления прокуратуры",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ResolPros>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Дома в постановлении прокуратуры",
                BaseEntity = typeof(RealityObject),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll().Any(x => x.RealityObject.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Постановления ГЖИ",
                BaseEntity = typeof(Contragent),
                DeleteAnyDependences = id =>
                {
                    var service = this.Container.Resolve<IDomainService<Resolution>>();
                    service.GetAll().Where(x => x.Contragent.Id == id)
                        .Select(x => x.Id).ForEach(x => service.Delete(x));
                }
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Постановления ГЖИ",
                BaseEntity = typeof(Municipality),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Resolution>>().GetAll().Any(x => x.Municipality.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Постановления ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<Resolution>>().GetAll().Any(x => x.Official.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Определения в постановлении ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ResolutionDefinition>>().GetAll().Any(x => x.IssuedDefinition.Id == id)
            });

            References.Add(new EntityReference
            {
                ReferenceName = "Оспаривания в постановлении ГЖИ",
                BaseEntity = typeof(Inspector),
                CheckAnyDependences = id => this.Container.Resolve<IDomainService<ResolutionDispute>>().GetAll().Any(x => x.Lawyer.Id == id)
            });

            return this;
        }
    }
}