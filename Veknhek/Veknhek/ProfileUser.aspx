<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProfileUser.aspx.cs" Inherits="ProfileUser" %>

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
                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("userName")%>'></asp:Label>
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
                        <div class="btn">
                            <asp:Button ID="Button1" runat="server" Text="កែប្រែ" CssClass="btn btn-danger py-2 px-5 tex-white fw-bolder fs-5 text-uppercase font" OnCommand="btnUpdate"/>
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
                        <div class="d-flex align-items-center justify-content-between">
                            <!-- User Icon -->
                            <div class="border rounded-pill me-2">
                                <i class="fa-solid fa-user text-white p-3 fs-3"></i>
                            </div>
                            <!-- User Info -->
                            <div class="d-flex flex-column fs-5">
                                <asp:Label ID="tblUserName" runat="server" Text='<%# Eval("userName") %>' CssClass="text-uppercase"></asp:Label>
                                <asp:Label ID="tblDate" runat="server" Text='<%# GetTimeAgo(Convert.ToDateTime(Eval("createdAt"))) %>' CssClass="text-white-50 date"></asp:Label>
                            </div>
                            <!-- Ellipsis -->
                            <div class="ms-auto position-relative more">
                                <!-- Ellipsis -->
                                <span class="fs-4 text-white-50 cursor-pointer">...</span>
    
                                <!-- Dropdown Box -->
                                <div class="position-absolute box_update rounded">
                                    <ul class="list-unstyled mb-0">
                                        <li class="py-1 cursor-pointer px-2 py-1">
                                            <asp:LinkButton 
                                                ID="btnDelete" 
                                                runat="server" 
                                                CommandName="Delete" 
                                                CommandArgument='<%# Eval("newsfeedId") %>' 
                                                OnCommand="btnDelete" 
                                                CssClass="text-decoration-none "
                                                OnClientClick='return showDeleteConfirmation(this);'>
                                                <i class="fa-solid fa-trash me-2"></i> Delete
                                            </asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
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

