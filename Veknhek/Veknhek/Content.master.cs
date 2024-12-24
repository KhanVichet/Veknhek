using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Content : System.Web.UI.MasterPage
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    String userName, userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadHashTage();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    private void LoadHashTage()
    {
        string newsfeedId = Request.QueryString["newsfeedId"];


        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();
           

            string query = @"SELECT 
                                hashtag,
                        
                                COUNT(*) AS hashtag_count
                            FROM 
                                tblHashtages
                            GROUP BY
                                hashtag
                            ORDER BY
                                hashtag_count DESC;
                            ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind data to a control (e.g., Repeater, DetailsView, or a custom control)
                rptHashtage.DataSource = dt;
                rptHashtage.DataBind();
            }




            conn.Close();
        }
    }
    protected void btnHashtage_Click(object sender , CommandEventArgs e)
    {
        if (e.CommandName != null)
        {
            string idHashtage = e.CommandArgument.ToString();
            Response.Redirect("Hashtage.aspx?idHashtage=" + idHashtage);
        }
    }
    protected void btn_refresh(object sender , CommandEventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void btnSetting(object sender, EventArgs e)
    {
       
        Response.Redirect("Update.aspx");
        
       
    }
    protected void btnLogout(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
        Session.Clear();

    }
}
