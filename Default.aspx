<%@ Page Title="Home Page" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="Azure.FileStorage._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <br />
    
    <span class="label label-default">Container name:</span>&nbsp;<asp:label id="ContainerName" runat="server" CssClass="label label-primary"></asp:label>
    
    <br />
    
    <span class="label label-default">Directory path:</span>&nbsp;<asp:label id="DirectoryPath" runat="server" CssClass="label label-primary"></asp:label>

    <br />
    
    <br />

    <div class="row">
        <asp:Literal ID="documentList" runat="server"></asp:Literal>
    </div>
    
    <br/>
    
    <asp:label id="Message" runat="server" CssClass="label label-danger"></asp:label>
    
    <br />
    
    <hr/>
    
    <span class="label label-primary">Upload Functionality:</span>
    
    <br /><br />

    <input id="FindFile" type="file" style="width: 392px; height: 22px" size="46" runat="server" name="FindFile" />
    <asp:Button ID="AddFile" runat="server" Text="Upload File" CssClass="btn btn"></asp:Button>

    <br />

    <hr/>
    
    <span class="label label-primary">Write To File Functionality:</span>
    
    <br /><br />

    <span class="label label-default">File name:</span>&nbsp;<asp:TextBox id="WriteToFileName" runat="server" Text="test.txt" CssClass="form-control"></asp:TextBox>
    <span class="label label-default">Content:</span>&nbsp;<asp:TextBox TextMode="MultiLine" id="FileContent" runat="server" Text="Hello world !!" CssClass="form-control" rows="5"></asp:TextBox>
    <asp:Button ID="WriteToFile" runat="server" Text="Write To File" CssClass="btn btn"></asp:Button>
    
    <br />
    
    <hr/>
    
    <span class="label label-primary">Delete File Functionality:</span>
    
    <br /><br />

    <span class="label label-default">File name:</span>&nbsp;<asp:TextBox id="DeleteFileName" runat="server" Text="test.txt" CssClass="form-control"></asp:TextBox>
    <asp:Button ID="DeleteFile" runat="server" Text="Delete File" CssClass="btn"></asp:Button>
    
    <br />
    
    <hr/>
    
    <span class="label label-primary">Rename File Functionality:</span>
    
    <br /><br />

    <span class="label label-default">Current file name:</span>&nbsp;<asp:TextBox id="RenameFileName" runat="server" Text="test.txt" CssClass="form-control"></asp:TextBox>
    <span class="label label-default">New file name:</span>&nbsp;<asp:TextBox id="NewFileName" runat="server" Text="test123.txt" CssClass="form-control"></asp:TextBox>
    <asp:Button ID="RenameFile" runat="server" Text="Rename File" CssClass="btn"></asp:Button>
    
    <br />
    
    <hr/>
    
    <span class="label label-primary">Download File Functionality:</span>
    
    <br /><br />

    <span class="label label-default">File name:</span>&nbsp;<asp:TextBox id="DownloadFileName" runat="server" Text="test.txt" CssClass="form-control"></asp:TextBox>
    <asp:Button ID="DownloadFile" runat="server" Text="Download File" CssClass="btn"></asp:Button>
    
</asp:Content>
