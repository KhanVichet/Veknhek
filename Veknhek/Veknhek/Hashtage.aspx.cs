using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hashtage : System.Web.UI.Page
{
    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    String userName, userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!IsPostBack)
        {
            LoadNewsfeed();
           
        }
    }
    protected void btn_comment(object sender, CommandEventArgs e)
    {
        if (e.CommandName != null)
        {
            string newsfeedId = e.CommandArgument.ToString();
            Response.Redirect("Comment2.aspx?newsfeedId=" + newsfeedId);
        }

    }
    private void LoadNewsfeed()
    {



        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();
            userId = Session["userId"].ToString();
            string hashtag = Request.QueryString["idHashtage"];
            

            string query = @"
                            SELECT 
                                n.newsfeedId, 
                                n.content, 
                                n.createdAt, 
                                u.userId, 
                                u.userName,
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
                            INNER JOIN 
                                tblHashtages h ON n.newsfeedId = h.newsfeedId
                            WHERE 
                                h.hashtag = @hashtageId
                            ORDER BY 
                                n.createdAt DESC;


                                ";

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Add the currentUserId parameter
                cmd.Parameters.AddWithValue("@currentUserId", userId);
                cmd.Parameters.AddWithValue("@hashtageId", hashtag);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind data to the Repeater
                rptNewsfeed.DataSource = dt;
                rptNewsfeed.DataBind();
            }

            conn.Close();
        }
    }
    public string GetTimeAgo(DateTime createdAt)
    {
        TimeSpan timeSpan = DateTime.Now - createdAt;

        if (timeSpan.TotalSeconds < 60)
            return $"{timeSpan.Seconds} seconds ago";
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


    protected void btnPost_Click(object sender, EventArgs e)
    {
        string content = txtAreaStatus.Text.Trim();
        List<string> badWords = new List<string> { "អាឆ្កែ", "មីឆ្កែ", "អាថោក", "មីថោក", "មីឡប់", "អាតានើប", "អាឆ្កួត", "មីឆ្កួត" };

        // Mask bad words
        foreach (string badWord in badWords)
        {
            string maskedWord = new string('*', badWord.Length);
            content = System.Text.RegularExpressions.Regex.Replace(
                content,
                badWord,
                maskedWord,
                System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );
        }

        // Extract hashtags from content
        List<string> hashtags = new List<string>();
        var matches = System.Text.RegularExpressions.Regex.Matches(content, @"#\w+");
        foreach (Match match in matches)
        {
            // Remove the '#' symbol and normalize to lowercase
            hashtags.Add(match.Value.Substring(1).ToLower()); // Remove the # and convert to lowercase
        }

        userId = Session["userId"].ToString();
        DateTime createdAt = DateTime.Now;

        string insertQuery = @"
                        INSERT INTO tblNewsfeed (content, createdAt, userId)
                        OUTPUT INSERTED.newsfeedId
                        VALUES (@content, @createdAt, @userId)";

        conn = new SqlConnection(conStr);
        SqlCommand cmdAdd = new SqlCommand(insertQuery, conn);
        cmdAdd.Parameters.AddWithValue("@content", content);
        cmdAdd.Parameters.AddWithValue("@createdAt", createdAt);
        cmdAdd.Parameters.AddWithValue("@userId", userId);

        try
        {
            conn.Open();
            int newsfeedId = (int)cmdAdd.ExecuteScalar();

            // Insert hashtags into tblHashtags (without the #)
            foreach (string hashtag in hashtags)
            {
                string insertHashtagQuery = "INSERT INTO tblHashtages (newsfeedId, hashtag) VALUES (@newsfeedId, @hashtag)";
                using (SqlCommand cmdAddHashtag = new SqlCommand(insertHashtagQuery, conn))
                {
                    cmdAddHashtag.Parameters.AddWithValue("@newsfeedId", newsfeedId);
                    cmdAddHashtag.Parameters.AddWithValue("@hashtag", hashtag); // Insert hashtag without '#'
                    cmdAddHashtag.ExecuteNonQuery();
                }
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "បានជោគជ័យ", true);
            txtAreaStatus.Text = "";
            LoadNewsfeed();
            Response.Redirect("Home.aspx");
        }
        catch (Exception ex)
        {
            // Log the exception for debugging
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "បរាជ័យ!", true);
        }
        finally
        {
            conn.Close();
        }
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


        Response.Redirect(Request.Url.AbsoluteUri);
    }

    protected void btn_profile(object sender, CommandEventArgs e)
    {
        userId = Session["userId"].ToString();

        String UserId = e.CommandArgument.ToString();

        if (UserId == userId)
        {
            Response.Redirect("ProfileUser.aspx");
        }else {
            Response.Redirect("User.aspx?UserId=" + UserId);
        }
    }
    
}