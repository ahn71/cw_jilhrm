<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Login.aspx.cs" Inherits="SigmaERP.Login" %>
<!DOCTYPE html>
<html>
    <head id="Head1" runat="server">
    <title>Login</title>
    <link href="../style/login.css" rel="stylesheet" />
    <link href="../style/personnel.css" rel="stylesheet" />
        <link href="../AssetsNew/css/bootstrap.min.css" rel="stylesheet" />
        <link href="../AssetsNew/font-awesome/css/font-awesome.css" rel="stylesheet" />
        <style type="text/css">
            
            /* DEFAULTS
----------------------------------------------------------*/
            body {
                color: #424242;
                font-family: Arial,Helvetica,sans-serif;
                font-size: 12px;
                line-height: 20px;
                margin: 0;
                min-height: 100%;
                padding: 0;
                position: relative;
                top: 0px;
                left: 0px;
                height: 467px;
            }

/* PRIMARY LAYOUT ELEMENTS   
----------------------------------------------------------*/

.page
{
    width: 90%;

    margin: 70px auto 0px auto;
    border: 0px solid #496077;
    color: #424242;
    height: 393px;

}

.fixed
{
    position: fixed;
    background:#286090;
    width: 100%; 
    color: #eeeeee;
    border-bottom: 1px solid #fff;
    z-index: 999;
    top: 0px;
    left: 0px;
    bottom: 456px;
    height:33px;
}

.main
{
   background: url("/Images/logoLogin.png") no-repeat scroll left rgba(0, 0, 0, 0);
    float: right;
    height: 381px !important;
    margin: 0 8px 8px;
    min-height: 420px;
    padding: 0 12px;
    width: 97%;
}

.loginBox
{  
    /*border: 1px solid #D5D5D5;
    
    width:420px;
    position:fixed;
    top:39%;
    left:57%;
    margin-left:-210px;*/

    border: 1px solid #D5D5D5;
    margin: 80px auto 0;
    width: 420px;
}

.logo-login {
    margin:0 auto;
    width:100%;
    text-align:center;

}


.logImg
{
    float:left;
    width:39px;
}
.titleLogin-box
{
    /*margin-top:08px;*/
    /*float:right;
    width:372px;*/
     padding-top: 6px;
    text-align:center;
    background-color:#27235c;
    color:white;
    font-size:large;
    
}
.txtLoging
{
    float:right;
    width:265px;
    height: 21px;
    padding:07px;
    font-family: 'Cuprum',sans-serif;
}

.hedTitle
{
    height:38px;
    /*position:relative;*/
    text-align:center;
    background-color:#27235c;
    color:white;
    font-size:large;
    /*background:url("../Images/leftNavBg.png");*/
}

.title {
   background: url("../Images/darkBg.jpg") repeat-x scroll 0 0 rgba(0, 0, 0, 0);
    box-shadow: 0 1px 0 #FFFFFF;
    height: auto;
    margin-top:0px;
    padding-top:0px;
    color:White;
    padding:09px;
}


.header ul li.iMes a span {
    border:1px solid gray;
    width:81px;
    display:inline-block;
    height:44px;
    margin-top: 9px;
    text-align:center;
}
.header ul li a span {
    display:block;
    padding: 34px 10px 0;  
    margin-left:30px;

}

.header li 
{
    display: inline;
}
.logo
{
    height:104px;
    width:104px;
}

.headerLogo
{
    float:left;
    width:20%;
    height: 99px;
    text-align: left;
    padding:1px;
}
.headerRightside
{
    float:right;
    width:60%;
}


    

.headerItem:hover
{
    cursor:pointer;
    color:Green;
}

#footer {
    background:#286090;
    bottom: 0;
    clear: both;
    color: #EEEEEE;
    margin-top: 42px;
    width: 100%;
    height:35px;
    position: fixed;
    z-index: 100000;
    
}

.logStyle
{
    width:90%;
    height: 167px;
    padding:05px;  
}

.bottomUnderline
{
    border-bottom:1px solid gray;
}

.userNameDiv
{
    border-bottom: 1px solid #E7E7E7;
    padding: 19px 0;
    position: relative;
    height:27px;
}
.userIdDiv
{
    border-bottom: 1px solid #E7E7E7;
    padding: 19px 0;
    position: relative;
    height:27px;
}
.PasswordDiv
{
    border-bottom: 1px solid #E7E7E7;
    padding: 15px 0;
    position: relative;
    height:27px;
}
.textName
{
    float: left;
    height: 25px;
    padding-left: 59px;
    width: 10%;
}
.textBox
{
    float: right;
    text-align: left;
    width: 68%;
}
.btnBox
{
    width:137px;
    text-align:right;
    float:right;
    padding-right:57px;
}
.textBtn
{
    width:123px;
    float:left;
    text-align: left;
    height: 38px;
    padding-left:55px;
}

.chckboxStyle
{
    color:black;
    padding-bottom:03px;
    font-size:12px;
}

.txtBoxStyle
{
   -moz-box-sizing: border-box;
    background: none repeat scroll 0 0 #FFFFFF;
    border: 1px solid #D5D5D5;
    font-family: Arial,Helvetica,sans-serif;
    font-size: 11px;
    padding: 5px;
    width: 100%;
}
.btnLogin
{
    /*background: url("../Images/greyishBtn.png") repeat-x scroll 0 0 rgba(0, 0, 0, 0);*/
    border: 1px solid #4F5A68;
    background-color:gray;
    color: #FFFFFF;
    padding:02px;
    font-size:12px;
}

.btnLogin:hover
{
    background-position:0 -25px;  
}

.fleft
{
    float:left;
    width:30%;
    padding:05px;
    color:White;

}
.fright
{
    float:right;
    text-align:right;
    width:40%;
    padding-top:07px;
    padding-right:100px;
    color:White;
}

.invalidInput
{
    border:1px solid #980000;
}

.validInput
{
    border: 1px solid #D5D5D5;
}

.panel-heading {
    padding: 5px 15px;
    
 
    
}
            #imglogo img {
                height:50px;
                width:150px;
            }


/*.panel-footer {
	padding: 1px 15px;
	color: #A0A0A0;
    height: 30px;
    background-color: #27235C;
}*/

.profile-img {
	width: 96px;
	height: 96px;
	margin: 0 auto 10px;
	display: block;
	-moz-border-radius: 50%;
	-webkit-border-radius: 50%;
	border-radius: 50%;
}





        </style>
    </head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="fixed">
        </div>
        <%-----------------------------------------------------%>
        <div class="page">
            <asp:UpdatePanel ID="uplMessage" runat="server">
                <ContentTemplate>
                    <p class="message" id="lblMessage" clientidmode="Static" runat="server"></p>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="main">
                <%--   <div class="logo-login"><a class="logos"><img src="/images/logo.png" style="opacity:0.1; filter:alpha(opacity=100)" /></a>   </div>--%>
               
                    <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                        <ContentTemplate>
                           <div class="row">
			                    <div class="col-sm-6 col-md-4 col-md-offset-4">			                   
					                    
					                    <div class="panel-body">
						                    <fieldset>
                                                <div class="row">
                                                    <div class="col-xs-12 col-sm-6 col-sm-offset-3">
                                                        <asp:Image ID="imglogo" runat="server" ImageUrl="~/images/optimal_logo100.png" style="margin-bottom: 20px" width="163px" Height="60px" />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12 col-md-10  col-md-offset-1 ">
                                                        <div class="form-group">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i aria-hidden="true" class="fa fa-home"></i></span>
                                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control select_width" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i aria-hidden="true" class="fa fa-user"></i></span>
                                                                <asp:TextBox ID="txtUsername" runat="server" ClientIDMode="Static" CssClass="txtBoxStyle form-control" placeholder="User Name"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i aria-hidden="true" class="fa fa-unlock-alt"></i></span>
                                                                <asp:TextBox ID="txtPassword" runat="server" ClientIDMode="Static" CssClass="txtBoxStyle form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="input-group">
                                                                <asp:CheckBox ID="chkRememberMe" runat="server" CssClass="chckboxStyle" Style="float: left;" Text="Keep me logged in" Visible="false" Width="141px" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-primary btn-block" OnClick="btnLogin_Click" OnClientClick="return validateLogIn();" Text="Login" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
					                    </div>
					                    
                                    
			                    </div>
		                    </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
              
                <%--<div id="messageDiv">sf</div>--%>
            </div>
        </div>
        <%-----------------------------------------------------%>
        <div id="footer">
            <div class="fleft"></div>
            <div class="fright"><a href="http://www.optimalbd.com" target="_blank">
                <img src="/images/optimal_logo.png" width="84" alt="Optimal IT Limited" /></a></div>
        </div>
        <%--- MESSAGE BOX ---%>
        <div id="lblErrorMessage" style="display: none; min-width: 100px; position: fixed; top: 45px; z-index: 1; background-color: #5EA8DE; color: white; padding: 0px 30px 0px 15px; border-radius: 5px; text-align: center;">
            <p style="float: left; width: auto; padding-right: 30px;"></p>
            <div style="position: absolute; right: 10px; top: 13px; vertical-align: middle;">
                <img src="/images/master/close2.png" style="color: black; height: 8px; width: 8px; cursor: pointer;" 
                    onclick="$('#lblErrorMessage').fadeOut('slow');" />
            </div>
        </div>
    </form>

    <script src="../AssetsNew/js/jquery.js"></script>
    <script src="/Scripts/adviitJS.js" type="text/javascript"></script>
    <script src="/Scripts/custom.js" type="text/javascript"></script>
    <script src="/Scripts/master.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('#lblMessage').text().length > 1) {
                showMessage($('#lblMessage').text(), '');
            }
        });
        function validateLogIn() {
            if (validateText('txtUsername', 1, 20, 'Enter a valid username') == false) return false;
            if (validateText('txtPassword', 3, 20, 'Password must be 3-20 characters long') == false) return false;
            return true;
        }
    </script>
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        function InitializeRequest(sender, args) {

        }

        function EndRequest(sender, args) {

            if ($('#lblMessage').text().length > 1) {
                showMessage($('#lblMessage').text(), '');
            }


            //setTimeout("setPopupInMiddle()",1000);
        }
        function showMessage(message, messageType) {

            try {
                $('#lblErrorMessage').hide();

                

                var backColor = '#141614';
                var foreColor = '#FFF';
                var timeOut = 15000;


                if (message.indexOf('->') == -1) {
                    if (messageType.length == 0) message = "info->" + message;
                    else message = messageType + "->" + message;
                }

                var msg = message.split('->');
                messageType = msg[0];
                var msgBox = $('#lblErrorMessage');
                msgBox.css('width', 'auto');


                if (messageType == 'warning') {
                    backColor = '#FFCD3C';
                    foreColor = 'Black';
                }
                else if (messageType == 'success') {
                    timeOut = 5000;
                    backColor = '#5BD45B';
                }
                else if (messageType == 'error') backColor = '#EF494B';

                msgBox.css('background-color', backColor);
                msgBox.css('color', foreColor);

                if (msg[1].length == 0) {
                    hideErrorMessage();
                    return;
                }

                $('#lblErrorMessage p').html(msg[1]);


                msgBox.css('z-index', '999999999');
                if (msgBox.width() > 600) msgBox.css('width', '600px');


                if ($('.popBox:visible').length == 1) {
                    var pos = $('.popBox:visible').offset();

                    msgBox.css('position', 'absolute');
                    msgBox.css('top', pos.top + 8);
                    msgBox.css('right', '').css('left', pos.left + ($('.popBox:visible').width() / 2 - msgBox.width() / 2));
                }
                else {
                    msgBox.css('position', 'fixed');
                    msgBox.css('top', 37);
                    msgBox.css('left', '50%');
                    msgBox.css('margin-left', '-' + (msgBox.width() / 2) + "px");
                }

                msgBox.fadeIn(500);
                timeOutId = setInterval("hideErrorMessage()", timeOut);

                $('#lblMessage').text('');
            }
            catch (e) {
                console.log(e.message);
            }
        }
    </script>
</body>
</html>



