namespace Bars.Gkh.Utils.DataStructures
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using B4.Utils;

    public class NodeList<T> : Collection<BinaryTreeNode<T>> where T : IComparable<T>
    {
        public NodeList() : base()
        {
        }

        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; ++i)
            {
                base.Items.Add(default(BinaryTreeNode<T>));
            }
        }

        public BinaryTreeNode<T> FindByValue(T value)
        {
            foreach (var item in Items)
            {
                if (item.Value.Equals(value))
                    return item;
            }

            return null;
        }
    }

    public class BinaryTreeNode<T> where T : IComparable<T>
    {
        public string Path
        {
            get
            {
                if (Parent == null)
                    return string.Empty;
                if (Parent.Left.Value.CompareTo(Value) == 0)
                    return Parent.Path + "L";
                if (Parent.Right.Value.CompareTo(Value) == 0)
                    return Parent.Path + "R";
                return string.Empty;
            }
        }

        public NodeList<T> Children { get; set; }

        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }

        public BinaryTreeNode<T> Parent { get; set; }

        public BinaryTreeNode(T data)
        {
            Value = data;
        }

        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            Value = data;
            var children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;

            Children = children;
        }

        public virtual T Value { get; set; }

        public BinaryTreeNode<T> Left
        {
            get
            {
                if (Children == null)
                    return null;

                return Children[0];
            }
            set
            {
                if (Children == null)
                    Children = new NodeList<T>(2);

                Children[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (Children == null)
                    return null;

                return Children[1];
            }
            set
            {
                if (Children == null)
                    Children = new NodeList<T>(2);

                Children[1] = value;
            }
        }
    }

    public class BinaryTree<T> where T : IComparable<T>
    {
        private BinaryTreeNode<T> root;

        public BinaryTree()
        {
            root = null;
        }

        public BinaryTreeNode<T> Root
        {
            get { return root; }
            set { root = value; }
        }

        public virtual void Clear()
        {
            root = null;
        }

        public BinaryTreeNode<T> Add(T data)
        {
            var node = new BinaryTreeNode<T>(data);
            var path = string.Empty;

            BinaryTreeNode<T> current = root, parent = null;

            while (current != null)
            {
                if (node.Value.CompareTo(current.Value) == 0)
                {
                    return current;
                }
                else if (node.CompareTo(current.Value) > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (node.CompareTo(current.Value) < 0)
                {
                    parent = current;
                    current = current.Left;
                }
            }

            if (parent == null)
                root = node;
            else
            {
                if (node.CompareTo(parent.Value) > 0)
                {
                    node.Parent = parent;
                    parent.Right = node;
                }
                else
                {
                    node.Parent = parent;
                    parent.Left = node;
                }
            }

            return node;
        }

        public virtual bool Remove(T data)
        {
            if (root == null)
                return false;

            BinaryTreeNode<T> current = root, parent = null;

            var result = data.CompareTo(current.Value);
            while (result != 0)
            {
                if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }

                if (current == null)
                    return false;
                else
                    result = data.CompareTo(current.Value);
            }

            // Если у удаляемого узла нет правого сына,
            // то место удаляемого занимет его левый сын
            if (current.Right == null)
            {
                if (parent == null)
                    root = current.Left;
                else
                {
                    result = parent.Value.CompareTo(current.Value);
                    if (result > 0)
                        parent.Left = current.Left;
                    else if (result < 0)
                        parent.Right = current.Left;
                }
            }
                // Если у правого сына удаляемого узла нет левого сына,
                // то правый сын удаляемого занимает место удаляемого
            else if (current.Right.Left == null)
            {
                if (parent == null)
                    root = current.Right;
                else
                {
                    result = parent.Value.CompareTo(current.Value);
                    if (result > 0)
                        parent.Left = current.Right;
                    else if (result < 0)
                        parent.Right = current.Right;
                }
            }
                // Находим самый левый узел удаляемого и ставим его вместо удаляемого
            else
            {
                BinaryTreeNode<T> leftMost = current.Right.Left, lmParent = current.Right;

                while (leftMost.Left != null)
                {
                    lmParent = leftMost;
                    leftMost = leftMost.Left;
                }

                lmParent.Left = leftMost.Right;

                leftMost.Left = current.Left;
                leftMost.Right = current.Right;

                if (parent == null)
                    root = leftMost;
                else
                {
                    result = parent.Value.CompareTo(current.Value);

                    if (result > 0)
                        parent.Left = leftMost;
                    else if (result < 0)
                        parent.Right = leftMost;
                }
            }

            return true;
        }

        public BinaryTreeNode<T> Find(T value)
        {
            if (root == null)
                return null;

            BinaryTreeNode<T> current = root, parent = null;

            var result = value.CompareTo(current.Value);
            while (result != 0)
            {
                if (result > 0)
                {
                    parent = current;
                    current = current.Right;
                }
                else if (result < 0)
                {
                    parent = current;
                    current = current.Left;
                }

                // Ничего не нашли
                if (current == null)
                    return null;
                result = value.CompareTo(current.Value);
            }

            return current as BinaryTreeNode<T>;
        }

        public static BinaryTree<T> FromList(IEnumerable<T> list)
        {
            var tree = new BinaryTree<T>();

            list.ForEach(x => tree.Add(x));

            return tree;
        }
    }
}