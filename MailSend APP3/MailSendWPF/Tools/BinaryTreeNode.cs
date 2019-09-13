using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Tools
{
    public class BinaryTreeNode<T> : Node<T>
    {
        public BinaryTreeNode() : base() { }
        public BinaryTreeNode(T data) : base(data, null, null) { }
        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right)
        {
            base.Value = data;
            NodeList<T> children = new NodeList<T>(2);
            children[0] = left;
            children[1] = right;

            base.Childrens = children;
        }

        public BinaryTreeNode<T> Left
        {
            get
            {
                if (base.Childrens == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Childrens[0];
            }
            set
            {
                if (base.Childrens == null)
                    base.Childrens = new NodeList<T>(2);

                base.Childrens[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (base.Childrens == null)
                    return null;
                else
                    return (BinaryTreeNode<T>)base.Childrens[1];
            }
            set
            {
                if (base.Childrens == null)
                    base.Childrens = new NodeList<T>(2);

                base.Childrens[1] = value;
            }
        }
    }

}
