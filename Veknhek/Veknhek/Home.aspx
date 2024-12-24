<%@ Page Title="" Language="C#" MasterPageFile="~/Content.master" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="status-box mb-3 d-flex flex-row px-3 py-2 shadow rounded-2 box_newsfeed ">
       <asp:TextBox ID="txtAreaStatus" runat="server" CssClass="form-control auto-expand me-3 font_post" 
            TextMode="MultiLine" Rows="1" placeholder="សូមគោរពគ្នាទៅវិញទៅមកក្នុងការចែករំលែក !">
       </asp:TextBox>

        <asp:Button ID="btnPost" runat="server" CssClass="btn btn-primary font" Text="បង្ហោះ" OnClick="btnPost_Click" />

    </div>

    <!-- Example of posts -->
 <asp:Repeater ID="rptNewsfeed" runat="server" >
 <ItemTemplate>
    <div class="card mb-3 shadow box_newsfeed p-2 overflow-auto">
        <div class="card-body">
            <div class="d-flex align-items-center ">
                <asp:LinkButton Id="btnprofile" runat="server" CommandArgument='<%# Eval("userId") %>' OnCommand="btn_profile">
                <div class=" border rounded-pill me-3">
                    <i class="fa-solid fa-user text-white p-3 fs-3"></i>
                </div>
                </asp:LinkButton>
                <div class="d-flex flex-column fs-5 fw-lighter text-uppercase">
                    <asp:Label ID="tblUserName" runat="server" Text='<%# Eval("userName") %>'></asp:Label>
                    <asp:Label ID="tblDate" runat="server" Text='<%# GetTimeAgo(Convert.ToDateTime(Eval("createdAt"))) %>' CssClass=" text-white-50 date"></asp:Label>
                </div>
                
            </div>
            <div class="my-4">
                <asp:Label ID="tblPost" runat="server" Text='<%# Eval("content") %>' CssClass="card-text font"></asp:Label>
            </div>
            
            <div class="d-flex justify-content-start tex-white ms-3">
                <div class="me-3">
                      <asp:LinkButton 
                        ID="btnLikes" 
                        runat="server" 
                        CommandArgument='<%# Eval("newsfeedId") %>' 
                        OnCommand="btnLikes_Click">
                        <i class="fa-solid fa-heart icon" 
                           style='<%# Convert.ToBoolean(Eval("hasLiked")) ? "color:red;" : "color:white;" %>'>
                        </i>
                    </asp:LinkButton>
                    <asp:Label ID="likes" runat="server" Text='<%# Eval("likeCount") +" likes" %>'></asp:Label>
                </div>
                <div class="">
                     <asp:LinkButton ID="btnComment" runat="server" CommandArgument='<%# Eval("newsfeedId") %>' OnCommand="btn_comment">
                            <i class="fa-solid fa-comment icon" ></i>
                     </asp:LinkButton>
                     <asp:Label ID="comment" runat="server" Text='<%# Eval("commentCount") +" comments" %>'></asp:Label>
                 </div>
              
            </div>
        </div>
    </div>
     </ItemTemplate>
 </asp:Repeater>


</asp:Content>

