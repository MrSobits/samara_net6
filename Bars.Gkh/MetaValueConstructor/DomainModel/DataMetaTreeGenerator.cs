namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Генератора дерева
    /// </summary>
    public class DataMetaTreeGenerator
    {
        /// <summary>
        /// Сгенерировать дерево мета-информации
        /// </summary>
        /// <param name="values"> Список мета-информации </param>
        /// <param name="rootElement">Родительский элемент</param>
        /// <returns>
        /// </returns>
        public DataTreeNode GetMetaTree<TElement>(IList<TElement> values, TElement rootElement = null) where TElement : class, IDataMetaInfo, IHasParent<TElement>
        {
            var root = new DataTreeNode(rootElement);
            this.AddNode(root, values);
            return root;
        }


        private void AddNode<TElement>(DataTreeNode root, IList<TElement> children) where TElement : IDataMetaInfo, IHasParent<TElement>
        {
            // факторы и коэффициенты всегда имеют потомков
            if (root.Level <= 1)
            {
                root.IsLeaf = false;
            }

            // рекурсивно добавляем элементы
            foreach (var source in children.Where(x => object.Equals(x.Parent?.Id ?? 0, root.Id)).OrderBy(x => x.Id))
            {
                this.AddNode(root.AddChildren(source), children);
            }
        }
    }
}