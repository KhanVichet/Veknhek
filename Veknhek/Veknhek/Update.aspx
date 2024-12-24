<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Update.aspx.cs" Inherits="Update" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="mt-3 ">
        <div class="row justify-content-center">
            
            <div class="col-md-5">
                <div class=" rounded-5 p-4 text-white update">
                    <div class="d-flex justify-content-center">
                        <img src="../Image/smile.png" width="60px" />
                    </div>
                    <div class="card-header text-center fs-4">
                        <asp:Label ID="tblUserName" runat="server" Text=""></asp:Label>
                    </div>

                    <div class="card-body">
                        <!-- Bio -->
                        <div class="mb-3 d-flex flex-row">
                            <label for="bio" class="form-label w-25">Bio</label>
                            <asp:TextBox class="form-control" ID="BioTextBox" runat="server" name="Bio" rows="2" placeholder="បង្ហាញពីអាម្មរណ៍របស់អ្នក....!"></asp:TextBox>
                        </div>

                        <!-- Current Email -->
                        <div class="mb-3 d-flex flex-row">
                            <label for="name" class="form-label w-25 font">អ៊ីមែលបច្ចុប្បន្ន</label>
                            <asp:Label ID="CurrentEmailLabel" runat="server" Text=""></asp:Label>
                        </div>

                        <!-- Update Email -->
                        <div class="mb-3 d-flex flex-row">
                            <label for="email" class="form-label w-25 my-auto font">ផ្លាស់ប្ដូរអ៊ីមែល</label>
                            <asp:TextBox ID="NewEmailTextBox" runat="server" TextMode="Email" CssClass="form-control" 
                                placeholder="សូមបញ្ចូលអ៊ីមែលថ្មីរបស់អ្នក"></asp:TextBox>
                        </div>

                        <!-- Change Password -->
                        <div class="mb-3 d-flex flex-row">
                            <label for="email" class="form-label w-25 my-auto font">ផ្លាស់ប្ដូរកូដសម្ងាត់</label>
                            <asp:TextBox ID="CurrentPasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="សូមបញ្ចូលកូដសម្ងាត់បច្ចុប្បន្ន"></asp:TextBox>
                        </div>
                        <div class="mb-3 d-flex flex-row">
                            <label for="email" class="form-label w-25 my-auto"></label>
                            <asp:TextBox ID="NewPasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="សូមបញ្ចូលកូដសម្ងាត់ថ្មី"></asp:TextBox>
                        </div>
                        <div class="mb-3 d-flex flex-row">
                            <label for="email" class="form-label w-25 my-auto"></label>
                            <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password" CssClass="form-control" 
                                placeholder="សូមបញ្ចូលកូដសម្ងាត់ថ្មីម្ដងទៀត"></asp:TextBox>
                        </div>

                        <!-- Submit Button -->
                        <div class="text-center">
                            <asp:Button ID="UpdateButton" runat="server" Text="Update Profile" CssClass="btn btn-primary font"  OnClick="UpdateButton_Click" OnClientClick="return showUpdateUser(this);"/>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>


