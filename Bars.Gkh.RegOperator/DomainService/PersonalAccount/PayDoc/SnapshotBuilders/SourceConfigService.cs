namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.MicroKernel;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с источниками для документов на оплату
    /// </summary>
    public class SourceConfigService: ISourceConfigService
    {
        private IDictionary<string, BuilderConfig> configs; 
        private IWindsorContainer container { get; set; }
        private IDomainService<BuilderConfig> builderConfigDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">container</param>
        /// <param name="builderConfigDomain">builderConfigDomain</param>
        public SourceConfigService(IWindsorContainer container, IDomainService<BuilderConfig> builderConfigDomain)
        {
            this.container = container;
            this.builderConfigDomain = builderConfigDomain;
            this.configs = this.builderConfigDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Path)
                .ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// Получить дерево настроек
        /// </summary>
        /// <returns></returns>
        public TreeNode GetConfigTree()
        {
            var builderRegistrations = this.container.Kernel.GetHandlers(typeof(IBuilderInfo));

            // IBuilderInfo - зарегистрирован как SessionScoped, Release не нужен
            var buildersDict = builderRegistrations.GroupBy(this.GetEnumValue)
                .ToDictionary(
                x => x.Key, 
                y => y.Select(x => this.container.Resolve<IBuilderInfo>(x.ComponentModel.Name)).ToArray());

            var root = TreeNode.CreateRoot();

            foreach (PaymentDocumentType type in Enum.GetValues(typeof(PaymentDocumentType)))
            {
                this.CreateSubtree(root.AddChild(type), buildersDict[type]);
            }

            return root;
        }

        /// <summary>
        /// Получение списка всех зарегистрированных источников
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список зарегистрированных источников</returns>
        public List<ISnapshotBuilder> GetSourceList(PaymentDocumentType type)
        {
            var builderRegistrations = this.container.Kernel.GetHandlers(typeof(ISnapshotBuilder));
            
            return builderRegistrations
                .Where(x => this.GetEnumValue(x) == type)
                .Select(x => this.container.Resolve<ISnapshotBuilder>(x.ComponentModel.Name))
                .ToList();
        }

        /// <summary>
        /// Получить все настройки определенного типа в виде списка
        /// </summary>
        /// <param name="type">Тип</param>
        /// <returns>Список настроек</returns>
        public List<string> GetConfigList(PaymentDocumentType type)
        {
            return this.configs.Values
                .Where(x => x.Enabled)
                .Where(x => x.PaymentDocumentType == type)
                .Select(
                    x =>
                    {
                        var ar = x.Path.Split('.');
                        return ar[ar.Length - 1];
                    })
                .ToList();
        }

        private PaymentDocumentType GetEnumValue(IHandler handler)
        {
            return (PaymentDocumentType)Enum.Parse(typeof(PaymentDocumentType), handler.ComponentModel.Name.Split('.')[0]);
        }

        private void CreateSubtree(
            TreeNode root, 
            IEnumerable<IBuilderInfo> infos)
        {
            foreach (var builderInfo in infos)
            {
                var key = root.Path + '.' + builderInfo.Code;
                this.CreateSubtree(root.AddChild(builderInfo, this.configs.Get(key)), builderInfo.GetChildren());
            }
        }      
    }
}