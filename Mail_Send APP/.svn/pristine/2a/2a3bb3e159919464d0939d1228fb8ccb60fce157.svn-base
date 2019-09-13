using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.Tools
{
    public class MailNode<T>:Node<T>
    {
        
        

        public MailNode():base(){
        }
        public MailNode(MailTypes.eMailTypes mailType):base()
        {
            this.mailType = mailType;
        }
        public MailNode(T data): base(data, null, null){
        }
        public MailNode(T data, NodeList<T> childrens, Node<T> parent): base(data, childrens, parent){
        }
        public MailNode(T data, Node<T> parent, MailTypes.eMailTypes mailType)
            : base(data, parent)
        {
            this.mailType = mailType;

        }
        private MailTypes.eMailTypes mailType;

        public MailTypes.eMailTypes MailType
        {
            get { return mailType; }
            set { mailType = value; }
        }

        

        
    }
}
