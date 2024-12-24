<%@ Page Title="" Language="C#" MasterPageFile="~/Content.master" AutoEventWireup="true" CodeFile="Comment2.aspx.cs" Inherits="Comment2" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
  <div class="main-comment">
         <asp:Repeater ID="rptNewsfeed" runat="server" >
     <ItemTemplate>
         <div class="card mb-3 shadow box_newsfeed p-2">
    <div class="card-body">
        <div class="d-flex align-items-center ">
            <div class=" border rounded-pill me-3">
                <i class="fa-solid fa-user text-white p-3 fs-3"></i>
            </div>
            <div class="d-flex flex-column fs-5  ">
                <asp:Label ID="tblUserName" runat="server" Text='<%# Eval("userName") %>' CssClass="text-uppercase"></asp:Label>
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
                        OnCommand="btnLikes_Click"
                       >
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

 <div class="card shadow border-0 ">
      <div class="card-header comment-title text-white" id="commentHeader">
         Comments
     </div>
     <div class="list-group list-group-flush">
         <!-- Single Comment -->
       <asp:Repeater ID="rptComment" runat="server" >
         <ItemTemplate>
         <div class="list-group-item comment cart" id="comment">
             <div class="d-flex justify-content-between">
                 <div class="d-flex flex-row align-items-baseline text-uppercase">
                     <i class="fa-solid fa-user text-white p-3 fs-4"></i>
                      <asp:Label ID="Label1" runat="server" Text='<%# Eval("userName") %>'></asp:Label>
                  
                 </div>
                 
                <asp:Label ID="tblDate" runat="server" Text='<%# GetTimeAgo(Convert.ToDateTime(Eval("commentAt"))) %>' CssClass=" text-white-50 mt-3 date"></asp:Label>

             </div>
             <div class="mx-4 my-3">
                 <asp:Label ID="Label2" runat="server" Text='<%# Eval("content") %>' CssClass="font"></asp:Label>
             </div>
         </div>
         </ItemTemplate>
         </asp:Repeater>

         
 </div>
     </div>
    </div>
 <div class="mb-3 d-flex flex-row px-3 py-2 shadow rounded-2 comment_post ">
     <asp:TextBox ID="txtcomment" runat="server" CssClass="form-control auto-expand me-3"
         TextMode="MultiLine" Rows="1" placeholder="សូមគោរពគ្នាទៅវិញទៅមកក្នុងការចែករំលែក !">
     </asp:TextBox>

     <asp:Button ID="btnPost" runat="server" CssClass="btn btn-primary font" Text="បង្ហោះ" OnClick="btnPost_Click" />
 </div>
    

  

</asp:Content>

