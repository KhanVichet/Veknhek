using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : System.Web.UI.MasterPage
{
    String userName, userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userName"] == null)
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            userName = Session["userName"].ToString();
            if (userName != "")
            {
                lblUserName.Text = userName;

            }
        }
    }

    protected void btnProfile(object sender, EventArgs e)
    {
        Response.Redirect("ProfileUser.aspx");
    }
    protected void btn_comment(object sender, CommandEventArgs e)
    {
        if (e.CommandName != null)
        {
            string newsfeedId = e.CommandArgument.ToString();
            Response.Redirect("Comment2.aspx?newsfeedId=" + newsfeedId);
        }

    }
}
