using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Register : System.Web.UI.Page
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        String userName = GenerateUsername();
        String gmail = txtEmail.Text.Trim();
        String password = txtPassword.Text.Trim();
        String conformPassword = txtComPassword.Text.Trim();

        if (password == conformPassword)
        {
            if (IsEmailValid(gmail))
            {
                if (IsPasswordStrong(password))
                {
                    conn = new SqlConnection(conStr);
                    SqlCommand cmdAdd = new SqlCommand("Insert into tblUsers (userName, gmail, password) Values (@userName,@gmail,@password)", conn);
                    cmdAdd.Parameters.AddWithValue("@userName", userName);
                    cmdAdd.Parameters.AddWithValue("@gmail", gmail);
                    cmdAdd.Parameters.AddWithValue("@password", password);

                    try
                    {
                        conn.Open();
                        cmdAdd.ExecuteNonQuery();


                        Response.Redirect("Login.aspx");
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "សូមអភ័យទោស គណនីនេះមិនត្រឹមត្រូវទេ!", true);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('លេខសម្ងាត់ត្រូវតែមានអក្សរធំ អក្សរតូច លេខ នឹងសញ្ញាសម្គាល់!');", true);

                }

            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('សូមបំពេញ Gmail ឲ្យបានត្រឹមត្រូវ!');", true);

            }
        }

    }

    
    public static string GenerateUsername()
    {
        
        String firstName = "Anonymous ";
    
        Random random = new Random();
        int randomNumber = random.Next(1000, 9999);

        // Create the username by combining parts of the names and random number
        string username = firstName +
                          randomNumber;

        return username;
    }


    public bool IsPasswordStrong(string password)
    {
        // Minimum 8 characters
        if (password.Length < 8)
            return false;

        // At least one lowercase letter
        if (!Regex.IsMatch(password, @"[a-z]"))
            return false;

        // At least one uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;

        // At least one number
        if (!Regex.IsMatch(password, @"\d"))
            return false;

        // At least one special character (e.g., !@#$%^&*)
        if (!Regex.IsMatch(password, @"[\W_]+"))
            return false;

        return true;
    }
    public bool IsEmailValid(string email)
    {
        // Regular expression pattern for validating email addresses
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Use Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
    }



}