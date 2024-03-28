namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using AutoMapper;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Report.ReportManager;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Провайдер создания задач выгрузки документов на оплату
    /// </summary>
    internal class DocumentTaskCreater
    {
        private readonly IWindsorContainer container;
        
        private readonly IMapper mapper;

        /// <summary>
        /// Конструктор провайдера
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public DocumentTaskCreater(IWindsorContainer container, IMapper mapper)
        {
            this.container = container;
            this.mapper = mapper;
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат создания задачи</returns>
        public DocumentTaskCreateResult CreateTasks(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAs<long>("periodId");
            var reportPerAccount = baseParams.Params.GetAs<bool>("reportPerAccount");
            var accountIdsList = baseParams.Params.GetAs<string>("accountIds");
            var isPartially = baseParams.Params.GetAs<bool>("isPartially");

            var accountIds = accountIdsList.ToLongArray();

            //создает шаблоны квитанций для месяца, если они не существуют. Берутся актуальные шаблоны для последнего месяца
            this.CreateTemplateCopies(periodId);

            if (!reportPerAccount && accountIds.Length == 1 && !isPartially)
            {
                // Если выбрана печать по всем адресам, находящимся в собственности орг-ии
                var personalAccountDomain = this.container.ResolveDomain<BasePersonalAccount>();

                using (this.container.Using(personalAccountDomain))
                {
                    // Запрос такой для обхода маппинга
                    var owner = personalAccountDomain.GetAll()
                        .Where(x => x.Id == accountIds[0])
                        .Select(x => x.AccountOwner)
                        .FirstOrDefault();

                    if (owner != null && owner.OwnerType == PersonalAccountOwnerType.Legal)
                    {
                        accountIds = personalAccountDomain.GetAll()
                            .Where(x => x.AccountOwner.Id == owner.Id)
                            .Select(x => x.Id)
                            .ToArray();
                    }
                }
            }

            IDataResult outDirectoryResult = this.PrepareOutputDirectory();

            var descriptors = outDirectoryResult.Success
                ? this.CreateDescriptors(baseParams, periodId, accountIds, reportPerAccount, isPartially)
                : new TaskDescriptor[0];

            var result = new DocumentTaskCreateResult();
            foreach (var taskDescriptor in descriptors)
            {
                if (taskDescriptor.ExecutorCode == PhysicalOwnerDocumentExecutor.Id
                    || taskDescriptor.ExecutorCode == LegalOwnerDocumentExecutor.Id
                    || taskDescriptor.ExecutorCode == LegalOneHouseOwnerDocumentExecutor.Id
                    || taskDescriptor.ExecutorCode == LegalPartiallyOwnerDocumentExecutor.Id)
                {
                    result.DocumentTaskDescriptors.Add(taskDescriptor);
                }
                else
                {
                    result.SnapshotTaskDescriptors.Add(taskDescriptor);
                }
            }
            result.CanExecuteTasks = this.CanExecuteTasks(result.DocumentTaskDescriptors);

            return result;
        }

        private void CreateTemplateCopies(long periodId)
        {
            var templateRepo = this.container.ResolveRepository<PaymentDocumentTemplate>();
            var chargeRepo = this.container.ResolveRepository<ChargePeriod>();

            using (this.container.Using(templateRepo, chargeRepo))
            {
                var period = chargeRepo.Get(periodId);

                var manager = new PaymentDocReportManager(templateRepo);
                manager.CreateTemplateCopyIfNotExist(period);
            }
        }

        private IDataResult PrepareOutputDirectory()
        {
            var configProv = this.container.Resolve<IConfigProvider>();
            using (this.container.Using(configProv))
            {
                var ftpRoot = configProv.GetConfig().AppSettings.GetAs<string>("FtpPath");

                try
                {
                    if (ftpRoot.IsNotEmpty())
                    {
                        if (!Directory.Exists(ftpRoot))
                        {
                            Directory.CreateDirectory(ftpRoot);
                        }

                        return new BaseDataResult();
                    }

                    return new BaseDataResult(false, "Не удалось получить путь к ftp директории");
                }
                catch
                {
                    return new BaseDataResult(false, "Не удалось получить путь к ftp директории");
                }
            }
        }

        private TaskDescriptor[] CreateDescriptors(BaseParams baseParams, long periodId, long[] accountIds, bool reportPerAccount, bool isPartially)
        {
            var previewHelper = new PaymentDocPreviewHelper(this.container) { OptimizePath = true };
            int count;
            var preview = previewHelper.GetPreview(accountIds, baseParams, out count);

            if (count == 0)
            {
                return new TaskDescriptor[0];
            }

            var periodDomain = this.container.ResolveDomain<ChargePeriod>();
            var paymentDocumentLogDomain = this.container.ResolveDomain<PaymentDocumentLog>();
            var configProvider = this.container.Resolve<IConfigProvider>();
            var isZeroPaymentDoc = baseParams.Params.GetAs<bool>("isZeroPaymentDoc");
            var sourceUiForm = baseParams.Params.GetAs("sourceUIForm", string.Empty);
            var uid = Guid.NewGuid().ToString();

            using (this.container.Using(periodDomain, configProvider, paymentDocumentLogDomain))
            {
                var period = periodDomain.Get(periodId);
                var periodDto = mapper.Map<PeriodDto>(period);
                var startDate = DateTime.UtcNow;

                var createContext = new CreateTaskContext
                {
                    Root = preview,
                    Period = periodDto,
                    IsZeroPaymentDoc = isZeroPaymentDoc,
                    ReportPerAccount = reportPerAccount,
                    StartDate = startDate,
                    SourceUiForm = sourceUiForm,
                    Uid = uid,
                    IsPartially = isPartially
                };

                TaskDescriptor[] tasks;

                if (isPartially)
                {
                    tasks = this.CreatePartiallyLegalTask(createContext).ToArray();
                }
                else
                {
                    tasks = this.CreatePhysicalTask(createContext)
                        .Union(this.CreateLegalTask(createContext))
                        .ToArray();
                }

                if (tasks.IsNotEmpty())
                {
                    var paymentDocumentLog = new PaymentDocumentLog
                    {
                        AllCount = count,
                        Description = "Печать документов на оплату",
                        StartTime = startDate,
                        Uid = uid,
                        Period = period
                    };
                    paymentDocumentLogDomain.Save(paymentDocumentLog);
                }

                return tasks;
            }
        }

        private List<TaskDescriptor> CreateLegalTask(CreateTaskContext createContext)
        {
            var root = createContext.Root.Children;

            var data = root.Where(x => x.Children.IsNotEmpty()).Select(
                 x =>
                 {
                     var node = TreeNode.CreateRoot(x.Name, x.Path);
                     node.Id = x.Id;
                     node.IconCls = x.IconCls;
                     node.Parent = createContext.Root;
                     this.AddSubtree(node, x.Children.Where(child => !child.IsPhysical));

                     return node;
                 })
                 .ToList();

            var taskDescriptors = new List<TaskDescriptor>();
            foreach (var children in data)
            {
                var context = createContext.Clone();
                context.Root = children;
                context.ExecutorType = ExecutorType.Legal;
                taskDescriptors.AddRange(this.CreateTasks(context));
            }

            // сделано так, потому что есть возможность печатать квитанции по юр лицам с 1 помещением в папке физ лиц,
            // и соответсвенно эти ЛС находятся в поддереве Физ лиц
            var physicalRoot = createContext.Root.Children.FirstOrDefault(x => x.IsPhysical && x.Name == "ФЛ");
            var oneHouseContext = createContext.Clone();
            oneHouseContext.Root = physicalRoot;
            oneHouseContext.ExecutorType = ExecutorType.LegalOneHouse;
            taskDescriptors.AddRange(this.CreateTasks(oneHouseContext));

            //создание тасков для юр лиц с одним помещением в поддереве "Электронная почта"
            var emailData = root.Where(x => x.Children.IsNotEmpty() && (x.Name == "Электронная почта" || x.Name == "Физ. лица электронная почта")).Select(
                 x =>
                 {
                     var node = TreeNode.CreateRoot(x.Name, x.Path);
                     node.Id = x.Id;
                     node.IconCls = x.IconCls;
                     node.Parent = createContext.Root;
                     this.AddSubtree(node, x.Children.Where(child => child.IsPhysical));

                     return node;
                 })
                 .ToList();

            foreach (var child in emailData)
            {
                var context = createContext.Clone();
                context.Root = child;
                context.ExecutorType = ExecutorType.LegalOneHouse;
                taskDescriptors.AddRange(this.CreateTasks(context));
            }

            return taskDescriptors;
        }

        private List<TaskDescriptor> CreatePartiallyLegalTask(CreateTaskContext createContext)
        {
            var root = createContext.Root.Children;

            var data = root.Where(x => x.Children.IsNotEmpty()).Select(
                    x =>
                    {
                        var node = TreeNode.CreateRoot(x.Name, x.Path);
                        node.Id = x.Id;
                        node.IconCls = x.IconCls;
                        node.Parent = createContext.Root;
                        this.AddSubtree(node, x.Children.Where(child => !child.IsPhysical));

                        return node;
                    })
                .ToList();

            var taskDescriptors = new List<TaskDescriptor>();
            foreach (var children in data)
            {
                var context = createContext.Clone();
                context.Root = children;
                context.ExecutorType = ExecutorType.PartiallyLegal;
                taskDescriptors.AddRange(this.CreateTasks(context));
            }

            return taskDescriptors;
        }

        private List<TaskDescriptor> CreatePhysicalTask(CreateTaskContext createContext)
        {
            var root = createContext.Root.Children;

            var data = root.Where(x => x.Children.IsNotEmpty()).Select(
                x =>
                {
                    var node = TreeNode.CreateRoot(x.Name, x.Path);
                    node.Id = x.Id;
                    node.IconCls = x.IconCls;
                    node.Parent = createContext.Root;
                    this.AddSubtree(node, x.Children.Where(child => child.IsPhysical));

                    return node;
                })
                .ToList();

            var taskDescriptors = new List<TaskDescriptor>();
            foreach (var children in data)
            {
                var context = createContext.Clone();
                context.Root = children;
                context.ExecutorType = ExecutorType.Physical;

                taskDescriptors.AddRange(this.CreateTasks(context));
            }

            return taskDescriptors;
        }

        /// <summary>
        /// Рекурсивный метод восстановления поддерерва
        /// </summary>
        /// <param name="node">Вершина</param>
        /// <param name="children">Потомки</param>
        private void AddSubtree(TreeNode node, IEnumerable<TreeNode> children)
        {
            if (children.IsEmpty())
            {
                return;
            }

            children.ForEach(x =>
            {
                var child = node.AddChild(x.Name, x.Path == x.Parent.Path ? null : x.Path);
                child.Value = x.Value;
                child.Id = x.Id;
                child.IconCls = x.IconCls;
                child.Parent = node;

                this.AddSubtree(child, x.Children);
            });
        }

        private List<TaskDescriptor> CreateTasks(CreateTaskContext createContext)
        {
            var result = new List<TaskDescriptor>();
            var map = new Dictionary<string, List<TreeNode>>();
            this.Visit(createContext.Root, map, createContext.ExecutorType == ExecutorType.Physical);

            var allTreeNodes = map.Values.SelectMany(x => x).ToList();

            foreach (var treeNodes in allTreeNodes.Split(55555))
            {
                var primarySources = new List<PayDocPrimarySource>(55555);

                foreach (var treeNode in treeNodes)
                {
                    primarySources.Add(new PayDocPrimarySource(treeNode.Value.Id, treeNode.Value.OwnerId));
                }
                
                if (primarySources.Any())
                {
                    var createSnapshotTaskDescriptor = this.CreateSnapshotTask(
                        primarySources,
                        createContext);

                    result.Add(createSnapshotTaskDescriptor);
                }
            }

            result.AddRange(map.Select(x => this.CreateTask(x, createContext)));

            return result;
        }
        
        private TaskDescriptor CreateSnapshotTask(List<PayDocPrimarySource> primarySources, CreateTaskContext createContext)
        {
            string executorCode = LegalOneHouseOwnerDocumentSnapshotExecutor.Id;

            switch (createContext.ExecutorType)
            {
                case ExecutorType.Physical:
                    executorCode = PhysicalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.Legal:
                    executorCode = LegalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.PartiallyLegal:
                    executorCode = LegalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.LegalOneHouse:
                    executorCode = LegalOneHouseOwnerDocumentSnapshotExecutor.Id;
                    break;
            }

            var baseParams = new BaseParams
            {
                Params = DynamicDictionary.Create()
                    .SetValue("isZeroPaymentDoc", createContext.IsZeroPaymentDoc)
                    .SetValue("periodDto", createContext.Period)
                    .SetValue("primarySources", primarySources)
            };

            return new TaskDescriptor("Формирование слепков документов на оплату", executorCode, baseParams)
            {
                Description = "Количесво идентификаторов: {0}".FormatUsing(primarySources.Count),
                IsExclusive = true
            };
        }

        private TaskDescriptor CreateTask(KeyValuePair<string, List<TreeNode>> pathToNodeList, CreateTaskContext createContext)
        {
            string executorCode = LegalOneHouseOwnerDocumentExecutor.Id;
            string dependencyExecutorCode = LegalOneHouseOwnerDocumentSnapshotExecutor.Id;

            switch (createContext.ExecutorType)
            {
                case ExecutorType.Physical:
                    executorCode = PhysicalOwnerDocumentExecutor.Id;
                    dependencyExecutorCode = PhysicalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.Legal:
                    executorCode = LegalOwnerDocumentExecutor.Id;
                    dependencyExecutorCode = LegalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.PartiallyLegal:
                    executorCode = LegalPartiallyOwnerDocumentExecutor.Id;
                    dependencyExecutorCode = LegalOwnerDocumentSnapshotExecutor.Id;
                    break;
                case ExecutorType.LegalOneHouse:
                    executorCode = LegalOneHouseOwnerDocumentExecutor.Id;
                    dependencyExecutorCode = LegalOneHouseOwnerDocumentSnapshotExecutor.Id;
                    break;
            }

            var args = DynamicDictionary.Create()
                .SetValue("isZeroPaymentDoc", createContext.IsZeroPaymentDoc)
                .SetValue("periodDto", createContext.Period)
                .SetValue("primarySources", pathToNodeList.Value.Select(x => new PayDocPrimarySource(x.Value.Id, x.Value.OwnerId)).ToList())
                .SetValue("path", pathToNodeList.Key.ToUpper())
                .SetValue("reportPerAccount", createContext.ReportPerAccount)
                .SetValue("startDate", createContext.StartDate)
                .SetValue("uid", createContext.Uid);

            if (createContext.SourceUiForm.IsNotEmpty())
            {
                args.SetValue("sourceUIForm", createContext.SourceUiForm);
            }

            var baseParams = new BaseParams { Params = args };

            return new TaskDescriptor("Формирование документов на оплату", executorCode, baseParams)
            {
                Description = baseParams.Params.GetAs<string>("path"),
                SuccessCallback = LogFileMergeCallback.Id,
                FailCallback = LogFileMergeCallback.Id,
                Dependencies = new[]
                {
                    new Dependency
                    {
                        Scope = DependencyScope.InsideExecutors,
                        Key = dependencyExecutorCode
                    }
                }
            };
        }

        private void Visit(TreeNode node, Dictionary<string, List<TreeNode>> map, bool isPhysical)
        {
            if (node != null && !node.Children.IsEmpty())
            {
                if (node.Children.Any(x => x.IsLeaf))
                {
                    List<TreeNode> nodes;
                    if (map.TryGetValue(node.Path, out nodes))
                    {
                        nodes.AddRange(node.Children
                            .WhereIf(isPhysical, x => x.Value.OwnerType == PersonalAccountOwnerType.Individual)
                            .WhereIf(!isPhysical, x => x.Value.OwnerType == PersonalAccountOwnerType.Legal));
                    }
                    else
                    {
                        var childs = node.Children
                            .WhereIf(isPhysical, x => x.Value.OwnerType == PersonalAccountOwnerType.Individual)
                            .WhereIf(!isPhysical, x => x.Value.OwnerType == PersonalAccountOwnerType.Legal)
                            .ToList();

                        if (childs.Count > 0)
                        {
                            map.Add(node.Path, childs);
                        }
                    }
                }
                else
                {
                    foreach (var treeNode in node.Children)
                    {
                        this.Visit(treeNode, map, isPhysical);
                    }
                }
            }
        }

        /// <summary>
        /// Проверяем наличие включенной настройки "Основная информация по лс"
        /// Если она выключена для квитанции требуемого типа, ставить задачи нет смысла
        /// </summary>
        private bool CanExecuteTasks(List<TaskDescriptor> taskDescriptors)
        {
            var builderConfigDomain = this.container.ResolveDomain<BuilderConfig>();

            try
            {
                var codes = taskDescriptors.Select(x => x.ExecutorCode).Distinct();

                var configs = builderConfigDomain.GetAll()
                    .Where(x => x.Enabled)
                    .Where(x => x.Path.Contains("MainInfoBuilder.MainInfo"))
                    .Select(x => x.PaymentDocumentType)
                    .ToHashSet();

                return codes.All(x => configs.Contains(this.ToPaymentDocType(x)));
            }
            finally
            {
                this.container.Release(builderConfigDomain);
            }
        }

        private PaymentDocumentType ToPaymentDocType(string executorCode)
        {
            return executorCode == PhysicalOwnerDocumentExecutor.Id
                ? PaymentDocumentType.Individual
                : executorCode == LegalOneHouseOwnerDocumentExecutor.Id
                    ? PaymentDocumentType.Legal
                    : PaymentDocumentType.RegistryLegal;
        }
    }
}