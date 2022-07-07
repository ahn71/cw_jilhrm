<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SigmaERP.ControlPanel.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../AssetsNew/css/bootstrap.min.css" rel="stylesheet" />
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
				                    <div class="panel panel-default">
					                    <div class="panel-heading">
						                    <strong> Sign in to continue</strong>
					                    </div>
					                    <div class="panel-body">
						                    <form role="form" action="#" method="POST">
							                    <fieldset>
								                    <div class="row">
									                    <div class="center-block">
										                    <img class="profile-img"
											                    src="https://lh5.googleusercontent.com/-b0-k99FZlyE/AAAAAAAAAAI/AAAAAAAAAAA/eu7opA4byxI/photo.jpg?sz=120" alt="">
									                    </div>
								                    </div>
								                    <div class="row">
									                    <div class="col-sm-12 col-md-10  col-md-offset-1 ">
										                    <div class="form-group">
											                    <div class="input-group">
												                    <span class="input-group-addon">
													                    <i class="glyphicon glyphicon-user"></i>
												                    </span> 
												                    <input class="form-control" placeholder="Username" name="loginname" type="text" autofocus>
											                    </div>
										                    </div>
										                    <div class="form-group">
											                    <div class="input-group">
												                    <span class="input-group-addon">
													                    <i class="glyphicon glyphicon-lock"></i>
												                    </span>
												                    <input class="form-control" placeholder="Password" name="password" type="password" value="">
											                    </div>
										                    </div>
										                    <div class="form-group">
											                    <input type="submit" class="btn btn-lg btn-primary btn-block" value="Sign in">
										                    </div>
									                    </div>
								                    </div>
							                    </fieldset>
						                    </form>
					                    </div>
					                    <div class="panel-footer ">
						                    Don't have an account! <a href="#" onClick=""> Sign Up Here </a>
					                    </div>
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
</body>
</html>
