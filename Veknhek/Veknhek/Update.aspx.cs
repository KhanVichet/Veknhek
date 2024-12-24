using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Update : System.Web.UI.Page
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    string userId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userId"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        userId = Session["userId"].ToString();

        if (!IsPostBack)
        {
            LoadUserProfile();
        }
    }

    private void LoadUserProfile()
    {
        using (SqlConnection connection = new SqlConnection(conStr))
        {
            string query = "SELECT * FROM tblUsers WHERE userId = @UserId";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                CurrentEmailLabel.Text = reader["gmail"].ToString();
                tblUserName.Text= reader["userName"].ToString() ;
                BioTextBox.Text = reader["bio"].ToString();
            }

            connection.Close();
        }
    }

    

    
    private void UpdateEmail(String newEmail)
    {
        userId = Session["userId"].ToString();
        string updateQuery = "UPDATE tblUsers SET gmail = @NewEmail WHERE userId = @UserId";

        using (SqlConnection connection = new SqlConnection(conStr))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    // Add parameters to avoid SQL injection
                    command.Parameters.AddWithValue("@NewEmail", newEmail);
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Email updated successfully.");
                        NewEmailTextBox.Text = "";
                        LoadUserProfile();
                    }
                    else
                    {
                        Console.WriteLine("No user found with the provided ID.");
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"An error occurred while updating the email: {ex.Message}");
            }
        }
    }
    private bool ValidateCurrentPassword(string currentPassword)
    {
        string query = "SELECT password FROM tblUsers WHERE userId = @UserId";
        using (SqlConnection connection = new SqlConnection(conStr))
        {
            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    string storedPassword = command.ExecuteScalar()?.ToString();
                    return currentPassword == storedPassword; // Replace with hash comparison if necessary
                }
            }
            catch (SqlException)
            {
                return false;
            }
        }
    }
    private void UpdatePassword(string newPassword)
    {
        string updateQuery = "UPDATE tblUsers SET password = @NewPassword WHERE userId = @UserId";

        using (SqlConnection connection = new SqlConnection(conStr))
        {
            
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", newPassword); // Hash if necessary
                    command.Parameters.AddWithValue("@UserId", userId);

                    command.ExecuteNonQuery();
                }
            
            
        }
    }


    private void UpdateBio(string newBio)
    {
        string currentBio = GetCurrentBio();

        if (currentBio == newBio)
        {
            return;
        }

        string updateQuery = "UPDATE tblUsers SET bio = @NewBio WHERE userId = @UserId";

        using (SqlConnection connection = new SqlConnection(conStr))
        {
            
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@NewBio", newBio);
                    command.Parameters.AddWithValue("@UserId", userId);

                    int rowsAffected = command.ExecuteNonQuery();

                    
                }
            
            
        }
    }
    private string GetCurrentBio()
    {
        string query = "SELECT bio FROM tblUsers WHERE userId = @UserId";
        string currentBio = string.Empty;

        using (SqlConnection connection = new SqlConnection(conStr))
        {
           
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        currentBio = reader["bio"].ToString();
                    }
                }
            
           
        }

        return currentBio;
    }



    protected void UpdateButton_Click(object sender, EventArgs e)
    {
        string newbio = BioTextBox.Text.Trim();
        string newEmail = NewEmailTextBox.Text.Trim();
        string currentPassword = CurrentPasswordTextBox.Text.Trim();
        string newPassword = NewPasswordTextBox.Text.Trim();
        string confirmPassword = ConfirmPasswordTextBox.Text.Trim();


        if (!string.IsNullOrEmpty(newEmail))
        {
            UpdateEmail(newEmail);
        }
        if (!string.IsNullOrEmpty(newbio))
        {
            UpdateBio(newbio);
        }
        if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword))
        {
            if (newPassword != confirmPassword)
            {
        
                return;
            }

            if (ValidateCurrentPassword(currentPassword))
            {
                UpdatePassword(newPassword);
            }
            
        }
    }


}
