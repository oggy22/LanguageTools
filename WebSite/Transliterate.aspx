<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Transliterate.aspx.cs" Inherits="Transliteration._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="div-1">
        <div class="div-1a">
            <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />
            <table id="table" width="400px">
                <tr>
                    <td>
                        <asp:DropDownList ID="SrcLangDropDownList" runat="server" />
                    </td>
                    <td>
                        <asp:DropDownList ID="DstLangDropDownList" AutoPostBack="true" runat="server" OnSelectedIndexChanged="Transliterate" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="SourceText" runat="server" Width="500" Height="150px" AutoPostBack="True" OnTextChanged="Transliterate"
                            TextMode="MultiLine" />
                    </td>
                    <td>
                        <asp:UpdatePanel ID="TextChanged_UpdatePanel" UpdateMode="Conditional" RenderMode="Inline"
                            runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="SourceText" EventName="TextChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TextBox ID="DestinationText" runat="server" AutoPostBack="true" Width="500"
                                    Height="150px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div-1dugme">
                            <asp:Label ID="NoteLabel" runat="server" Height="25px" /><br />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Width="125px" OnClick="Transliterate" Text="Transliterate" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="div-1b">
        <table>
        <tr>
        <td>
<script type="text/javascript"><!--
    google_ad_client = "ca-pub-3977808042266121";
    /* wskyskaper */
    google_ad_slot = "6738494640";
    google_ad_width = 160;
    google_ad_height = 600;
//-->
</script>
<script type="text/javascript"
src="http://pagead2.googlesyndication.com/pagead/show_ads.js">
</script>        </td>
        </tr>
        </table>
<%--            <img src="download.jpg" />
            <img src="download.jpg" />
            <img src="download.jpg" />
            <img src="download.jpg" />
--%>        </div>
    </div>
</asp:Content>
