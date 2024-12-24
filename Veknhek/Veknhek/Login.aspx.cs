using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
    }

    protected void txtEmail_TextChanged(object sender, EventArgs e)
    {
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text.Trim();
        if(email == "" || password == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "សូមអភ័យទោស គណនីនេះមិនត្រឹមត្រូវទេ!", true);
        }
       

        using (SqlConnection conn = new SqlConnection(conStr))
        {
            try
            {
                conn.Open();
                string query = "SELECT * FROM tblUsers WHERE gmail = @Gmail AND password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Gmail", email);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Set session and redirect to main page
                    Session["userName"] = reader["userName"].ToString();
                    Session["userId"] = reader["userId"].ToString();
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "សូមអភ័យទោស គណនីនេះមិនត្រឹមត្រូវទេ!", true);
                }
                
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "សូមអភ័យទោស គណនីនេះមិនត្រឹមត្រូវទេ!", true);
                return;
            }
        }
    }
       
       
}


