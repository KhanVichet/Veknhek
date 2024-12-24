using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Comment2 : System.Web.UI.Page
{

    public string conStr = ConfigurationManager.ConnectionStrings["DBDoc"].ToString();
    SqlConnection conn;
    String userName, userId;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadNewsfeed();
            loadComment();
        }
    }
    private void LoadNewsfeed()
    {
        string newsfeedId = Request.QueryString["newsfeedId"];


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
                                    n.newsfeedId = @newsfeedId;

                            ";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("@newsfeedId", newsfeedId);
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
    protected void btn_comment(object sender, CommandEventArgs e)
    {
        if (e.CommandName != null)
        {
            string newsfeedId = e.CommandArgument.ToString();
            Response.Redirect("Comment2.aspx?newsfeedId=" + newsfeedId);
        }

    }
    private void loadComment()
    {
        int newsfeedId = Convert.ToInt32(Request.QueryString["newsfeedId"]);


        using (SqlConnection conn = new SqlConnection(conStr))
        {
            conn.Open();

            // Fetch posts from the database
            SqlCommand showNewsfeed = new SqlCommand("SELECT c.commentAt, c.commentId, c.content, u.userName,(SELECT COUNT(*) FROM tblComment WHERE newsfeedId = @newsfeedId) AS TotalComments FROM tblComment AS c INNER JOIN tblUsers AS u ON c.userId = u.userId WHERE c.newsfeedId = @newsfeedId ORDER BY c.commentAt DESC", conn);
            showNewsfeed.Parameters.AddWithValue("@newsfeedId", newsfeedId);
            SqlDataAdapter daComment = new SqlDataAdapter(showNewsfeed);
            DataTable dtPosts = new DataTable();
            daComment.Fill(dtPosts);
            rptComment.DataSource = dtPosts;
            rptComment.DataBind();  // Bind the data


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
        String content = txtcomment.Text.Trim();
        List<string> badWords = new List<string> { "អាឆ្កែ", "មីឆ្កែ", "អាថោក", "មីថោក", "មីឡប់", "អាតានើប", "អាឆ្កួត", "មីឆ្កួត" };


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

        userId = Session["userId"].ToString();
        int newsfeedId = Convert.ToInt32(Request.QueryString["newsfeedId"]);
        DateTime commentAt = DateTime.Now;
        conn = new SqlConnection(conStr);
        SqlCommand cmdAdd = new SqlCommand("Insert into tblComment (content, commentAt, newsfeedId,userId) Values (@content,@commentAt,@newsfeedId,@userId)", conn);
        cmdAdd.Parameters.AddWithValue("@content", content);
        cmdAdd.Parameters.AddWithValue("@commentAt", commentAt);
        cmdAdd.Parameters.AddWithValue("@newsfeedId", newsfeedId);
        cmdAdd.Parameters.AddWithValue("@userId", userId);
        try
        {
            conn.Open();
            cmdAdd.ExecuteNonQuery();

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "បានជោគជ័យ", true);
            txtcomment.Text = "";
            LoadNewsfeed();
            loadComment();
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
}