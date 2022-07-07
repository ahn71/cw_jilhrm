$(document).ready(function () {
    $('#show').click(function () {
        $('#elem').toggle('slow');
        //$('#elem').addClass('red');
        addColors();
    });

    function addColors() {
        $('#elem').addClass('red');
        $('#elem').append('This is a box');
    }

    $('#addContent').click(function () {
        var txtVal = $('#txtBox').val();
        $('#elem').append('<p id="p">' + txtVal + '</p>');
    })

    function removeContent() {
        alert('dfasdf');
        //$(con).remove();
    }


    $('#p1').click(function () {
        //$('#p1').remove(fadeOut('slow'));
        $('#p1').fadeOut(500, function () { $('#p1').remove(); });
    });


    //    $('#btnLogin').click(function () { //Check Valid Input 

    //        var username = $('#txtUsername').val();
    //        var pass = $('#txtPassword').val();
    //        if (username < 5) {

    //            $('#txtUsername').addClass('invalidInput');
    //            //alert("Input UserName");
    //            return false;
    //        }
    //        else if (pass < 6) {
    //            $('#txtPassword').addClass('invalidInput');
    //            return false;
    //        }
    //        else {
    //            $('#txtPassword').addClass('validInput');
    //            //alert("Ok");
    //            return false;
    //        }
    //    });


    $('#btnLogin').click(function (e) {
        var isValid = true;
        var myLength = $("#txtUsername").val().length;


        $('#txtUsername').each(function () {
            if ($.trim($(this).val()) == '') {
                isValid = false;
                $(this).css({
                    "border": "1px solid red",
                    "background": "#FFCECE"
                });
                $("#messageDiv").html('Successfully');
                
            }
            if (myLength < 4) {
                isValid = false;
                $("#messageDiv").html('Username must be 4-20 latter');
                $(this).css({
                    "border": "1px solid red",
                    "background": "#FFCECE"
                });

            }
            else {
                $(this).css({
                    "border": "",
                    "background": ""
                });
            }
        });
        if (isValid == false)
            e.preventDefault();



        var isValid = true;
        $('#txtPassword').each(function () {
            if ($.trim($(this).val()) == '') {
                isValid = false;
                $(this).css({
                    "border": "1px solid red",
                    "background": "#FFCECE"
                });               
            }
            else {
                $(this).css({
                    "border": "",
                    "background": ""
                });
               
            }
        });
        if (isValid == false)
            e.preventDefault();
    }); 

   

});