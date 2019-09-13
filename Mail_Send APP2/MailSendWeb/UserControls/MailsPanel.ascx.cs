using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MailSendWeb.UserControls
{
    public partial class MailsPanel : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lstMails.Items.Add("1");
                lstMails.Items.Add("2");
                lstMails.Items.Add("3");
                lstMails.Items.Add("4");
                lstMails.Items.Add("5");
            }
        }
    }
}