using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Tools
{
    public class Node<T>
    {
        // Private member-variables
        private T data;
        private NodeList<T> childrens = null;
        private Node<T> parent = null;
  

        public Node() { }
        public Node(T data) : this(data, null, null) { }
        public Node(T data, Node<T> parent)
        {
            this.data = data;
            this.parent = parent;
        }
        public Node(T data, NodeList<T> childrens, Node<T> parent)
        {
            this.data = data;
            this.childrens = childrens;
            this.parent = parent;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public NodeList<T> Childrens
        {
            get
            {
                return childrens;
            }
            set
            {
                childrens = value;
            }
        }

        public Node<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }
    }
}


