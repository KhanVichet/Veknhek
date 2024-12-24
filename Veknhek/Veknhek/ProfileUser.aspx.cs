using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProfileUser : System.Web.UI.Page
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    String userName, userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadNewsfeed();
            LoadCount();
            LoadUser();
        }
    }
    private void LoadUser()
    {
        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();

            userId = Session["userId"].ToString();
            string query = @"
                             select * from tblUsers where userId = @currentUserId
                            ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@currentUserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind data to a control (e.g., Repeater, DetailsView, or a custom control)
                rptUser.DataSource = dt;
                rptUser.DataBind();
            }




            conn.Close();
        }
    }
    private void LoadNewsfeed()
    {

        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();

            userId = Session["userId"].ToString();
            string query = @"
                            SELECT 
                                n.newsfeedId, 
                                n.content, 
                                n.createdAt, 
                                u.userName,
                                u.userId,
                                (SELECT COUNT(*) 
                                 FROM tblComment 
                                 WHERE newsfeedId = n.newsfeedId) AS commentCount,
                                (SELECT COUNT(*) 
                                 FROM tblLikedBy 
                                 WHERE newsfeedId = n.newsfeedId) AS likeCount,
                                CASE 
                                    WHEN EXISTS (
                                        SELECT 1 
                                        FROM tblLikedBy 
                                        WHERE newsfeedId = n.newsfeedId 
                                          AND userId = @currentUserId
                                    ) THEN 1
                                    ELSE 0
                                END AS hasLiked
                            FROM 
                                tblNewsfeed n
                            INNER JOIN 
                                tblUsers u ON n.userId = u.userId
                            WHERE 
                                 u.userId = @currentUserId
                                
                            ORDER BY 
                                n.createdAt DESC


                            ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
               
                cmd.Parameters.AddWithValue("@currentUserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind data to a control (e.g., Repeater, DetailsView, or a custom control)
                rptNewsfeed.DataSource = dt;
                rptNewsfeed.DataBind();
            }




            conn.Close();
        }
    }
    private void LoadCount()
    {
        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();

            userId = Session["userId"].ToString();
            string query = @"
                            SELECT 
                            (SELECT COUNT(*) 
                             FROM tblNewsfeed 
                             WHERE userId = @currentUserId) AS newsfeedCount,

                            (SELECT COUNT(*) 
                             FROM tblLikedBy 
                             WHERE userId = @currentUserId) AS likeCount;
                            ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {

                cmd.Parameters.AddWithValue("@currentUserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind data to a control (e.g., Repeater, DetailsView, or a custom control)
                rptCount.DataSource = dt;
                rptCount.DataBind();
            }




            conn.Close();
        }
    }
    public string GetTimeAgo(DateTime createdAt)
    {
        TimeSpan timeSpan = DateTime.Now - createdAt;

        if (timeSpan.TotalSeconds < 60)
            return $"{ timeSpan.Seconds} seconds ago";
        if (timeSpan.TotalMinutes < 60)
            return $"{timeSpan.Minutes} minutes ago";
        if (timeSpan.TotalHours < 24)
            return $"{timeSpan.Hours} hours ago";
        if (timeSpan.TotalDays < 7)
            return $"{timeSpan.Days} days ago";
        if (timeSpan.TotalDays < 30)
            return $"{(int)(timeSpan.TotalDays / 7)} weeks ago";
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} months ago";
        return $"{(int)(timeSpan.TotalDays / 365)} years ago";
    }
    protected void btnLikes_Click(object sender, CommandEventArgs e)
    {
        int newsfeedId = Convert.ToInt32(e.CommandArgument);
        userId = Session["userId"].ToString();

        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();

            // Check if the user already liked the post
            string checkQuery = "SELECT COUNT(*) FROM tblLikedBy WHERE newsfeedId = @newsfeedId AND userId = @userId";
            using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
            {
                checkCmd.Parameters.AddWithValue("@newsfeedId", newsfeedId);
                checkCmd.Parameters.AddWithValue("@userId", userId);

                int likeCount = (int)checkCmd.ExecuteScalar();

                if (likeCount == 0)
                {
                    // Insert like
                    string insertQuery = "INSERT INTO tblLikedBy (userId, newsfeedId) VALUES (@userId, @newsfeedId)";
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@userId", userId);
                        insertCmd.Parameters.AddWithValue("@newsfeedId", newsfeedId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Remove like
                    string deleteQuery = "DELETE FROM tblLikedBy WHERE newsfeedId = @newsfeedId AND userId = @userId";
                    using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, conn))
                    {
                        deleteCmd.Parameters.AddWithValue("@userId", userId);
                        deleteCmd.Parameters.AddWithValue("@newsfeedId", newsfeedId);
                        deleteCmd.ExecuteNonQuery();
                    }
                }
            }

            conn.Close();
        }

       
        Response.Redirect(Request.RawUrl);
    }
    protected void btn_comment(object sender, CommandEventArgs e)
    {
        if (e.CommandName != null)
        {
            string newsfeedId = e.CommandArgument.ToString();
            Response.Redirect("Comment2.aspx?newsfeedId=" + newsfeedId);
        }

    }
   

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }
    protected void btnDelete(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            int newsfeedId = Convert.ToInt32(e.CommandArgument);

            using (SqlConnection conn = new SqlConnection(conStr))
            {
                conn.Open();
                string query = @"
                DELETE FROM tblHashtages WHERE newsfeedId = @newsfeedId;
                DELETE FROM tblComment WHERE newsfeedId = @newsfeedId;
                DELETE FROM tblLikedBy WHERE newsfeedId = @newsfeedId;
                DELETE FROM tblNewsfeed WHERE newsfeedId = @newsfeedId;
                DELETE FROM tblHashtages WHERE newsfeedId = @newsfeedId;
            ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newsfeedId", newsfeedId);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        LoadNewsfeed();
                        LoadCount();
                        LoadUser();
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Failed to delete post. Please try again.');", true);
                    }
                }
            }
        }
    }
    protected void btnUpdate(object sender, CommandEventArgs e)
    {
        Response.Redirect("Update.aspx");
    }


}