<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="container py-4 profile_content">
    <div class="row g-4 ">
        <!-- Profile Section -->
        <div class="col-md-4 profile shadow">
            <div class=" d-flex justify-content-center flex-column position-relative h-100">
                
                <div class="w-75 box_profile d-flex flex-column justify-content-center p-3 align-items-center rounded-3 shadow mx-auto">
                    <div class="my-2">
                        <image src="../Image/smile.png" width="100px"/>
                    </div>
                    <asp:Repeater ID="rptUser" runat="server"  >
                    <ItemTemplate>
                    <div class="text-white-50 my-2">
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("userName")%>'></asp:Label>
                    </div>
                       </ItemTemplate>
                </asp:Repeater>
                    <div class="w-100 d-flex flex-row my-2">
                        <asp:Repeater ID="rptCount" runat="server"  >
                        <ItemTemplate>
                        <div class="w-50 text-end pe-2 border-end border-1 d-flex flex-column">
                            <span class="font">ចំនួនសារ</span>
                            <asp:Label ID="Label2" runat="server" Text='<%# Eval("newsfeedCount")%>'></asp:Label>
                        </div>
                        <div class="w-50 ps-2 d-flex flex-column">
                            <span class="font">ចំនួនចូលចិត្ត</span>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("likeCount")%>'></asp:Label>
                        </div>
                        </ItemTemplate>
                     </asp:Repeater>
                    </div>
                    
                </div>
                    
                      <!-- Container for Back Button and Icon -->
                    <div class="w-100 position-absolute btn_back  py-3">
                        <div class="d-flex justify-content-center align-items-center gap-4">
                            
                           
                            <!-- Bootstrap Button -->
                            <asp:Button ID="btnBack" runat="server" Text="Go Back" CssClass="btn btn-primary px-4 py-2 fw-bold"  OnClick="btnBack_Click" />
                        </div>
                    </div>

            </div>
        </div>

        <!-- Newsfeed Section -->
        <div class="col-md-8 newsfeed_box ">
            <asp:Repeater ID="rptNewsfeed" runat="server" >
            <ItemTemplate>
             <div class=" mb-3 shadow box_newsfeed py-4 px-3 rounded-3">
                <div class="card-body">
                    <div class="d-flex align-items-center ">
                        <div class=" border rounded-pill me-2">
                            <i class="fa-solid fa-user text-white p-3 fs-3"></i>
                        </div>
                        <div class="d-flex flex-column fs-5  ">
                            <asp:Label ID="tblUserName" runat="server" Text='<%# Eval("userName") %>' CssClass="text-uppercase"></asp:Label>
                            <asp:Label ID="tblDate" runat="server" Text='<%# GetTimeAgo(Convert.ToDateTime(Eval("createdAt"))) %>' CssClass=" text-white-50 date"></asp:Label>
                        </div>
                    </div>
                    <div class="my-4">
                        <asp:Label ID="tblPost" runat="server" Text='<%# Eval("content") %>' CssClass="card-text"></asp:Label>
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
        </div>
    
       
  </div>
</div>
</asp:Content>

