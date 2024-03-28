namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Узел дерева настроек ичточников данных для документа на оплату
    /// </summary>
    public class TreeNode : IBuilderInfo
    {
        /// <summary>
        /// Выбрана ли настройка
        /// </summary>
        public bool @checked => this.Enabled;

        /// <summary>
        /// Является ли листом
        /// </summary>
        public bool leaf;

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Тип квитанции
        /// </summary>
        public PaymentDocumentType PaymentDocumentType { get; private set; }

        /// <summary>
        /// Полное наименование источника
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Родительский узел
        /// </summary>
        [JsonIgnore]
        public TreeNode Parent { get; private set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; private set; }

        private bool enabled;

        /// <summary>
        /// Выбрана ли настройка
        /// </summary>
        public bool Enabled
        {
            get { return this.Children.Any() ? this.Children.All(x => x.Enabled) : this.enabled; }
            private set { this.enabled = value; }
        }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        public List<TreeNode> Children { get; private set; }

        private TreeNode(TreeNode parent)
        {
            this.Parent = parent;
            this.Children = new List<TreeNode>();
        }

        private TreeNode(IBuilderInfo info, TreeNode parent) : this(parent)
        {
            this.Name = info.Name;
            this.Code = info.Code;
            this.Description = info.Description;
        }

        /// <summary>
        /// Добавить дочерний узел
        /// </summary>
        /// <param name="info">Узел</param>
        /// <param name="nodeConfig">Настройка для него</param>
        /// <returns>Узел</returns>
        public TreeNode AddChild(IBuilderInfo info, BuilderConfig nodeConfig = null)
        {
            var path = this.GetPath(info.Code);

            //что переданная настройка соответствует узлу
            if (nodeConfig != null && nodeConfig.Path != path)
            {
                throw new InvalidOperationException();
            }

            var treeNode = new TreeNode(info, this)
            {
                Id = nodeConfig?.Id ?? 0,
                Enabled = nodeConfig?.Enabled ?? false,
                Path = path,
                PaymentDocumentType = this.PaymentDocumentType,
                leaf = true
            };

            this.Children.Add(treeNode);
            this.leaf = false;

            return treeNode;
        }

        /// <summary>
        /// Добавление корневого узла типа документа 
        /// </summary>
        /// <param name="ownerType">Тип документа</param>
        /// <returns>Узел</returns>
        public TreeNode AddChild(PaymentDocumentType ownerType)
        {
            var treeNode = new TreeNode(this)
            {
                Path = ownerType.ToString(),
                Code = ownerType.ToString(),
                Name = ownerType.GetDisplayName(),
                PaymentDocumentType = ownerType
            };

            this.Children.Add(treeNode);
            return treeNode;
        }

        /// <summary>
        /// Создание корня
        /// </summary>
        /// <returns>Узел</returns>
        public static TreeNode CreateRoot()
        {
            return new TreeNode(null);          
        }

        /// <summary>
        /// Получить дочерние узлы
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IBuilderInfo> GetChildren()
        {
            throw new NotImplementedException();
        }

        private string GetPath(string code)
        {
            return this.Path.IsNotEmpty()
                ? this.Path + '.' + code
                : code;
        }
    }
}