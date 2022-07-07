/*****
* Developed By : Mehedi Hasan
* Last Modified : 22.11.2012
* Description : Common functionality using javascript and jQuery
*
* 
* *****/

/*********** Adviit Script **********/
/***********************************/



function deleteConfirm() {

    var answer = confirm("Do you want to delete?");
    if (answer) return true;
    else return false;
}


function showConfirm(message) {

    var answer = confirm(message);
    if (answer) return true;
    else return false;
}

function clearAll() {
    $(':input[type=text]').val('');
    $('select').val('');
    $('textarea').val('');
    return false;
}

function changeWidth(x) {

    if (x.value == "Default") {
        x.value = "100%";
        $('.TGheader').show();
        $('.footer').show();
        $('.menuBar').css('top', '50px');
        $('.content').css('margin-top', '80px');
    }
    $('.container').css('width', x.value);
 

    if (x.value == "Full") {
        $('.TGheader').toggle();
        $('.footer').toggle();
        $('.menuBar').css('top', '0px');
        $('.content').css('margin-top', '28px');
        //window.open(window.location.pathname, '', 'fullscreen=yes');
    }


}


function makeItName(name) {
    name.value = name.value.substring(0, 1).toUpperCase() + name.value.substring(1);
}


function toUpper(targetControl) {
    targetControl.value = targetControl.value.toUpperCase();
}


function setSearch(searchBox, isFocused) {

    if (searchBox.value.length == 0 && isFocused == false) {
        searchBox.value = "Search";
        //searchBox.style.width = "300px";
    }
    else if (searchBox.value == "Search" && isFocused == true) {
        searchBox.value = "";
    }

    if (isFocused == true) {
        //searchBox.style.width = "50%";
    }
}

function getDocHeight() {
    var D = document;
    return Math.max(
        Math.max(D.body.scrollHeight, D.documentElement.scrollHeight),
        Math.max(D.body.offsetHeight, D.documentElement.offsetHeight),
        Math.max(D.body.clientHeight, D.documentElement.clientHeight)
    );
}

function searchTable(inputVal, tableId, msgId) {
    var table = $('#' + tableId);
    table.find('tr').each(function (index, row) {
        var allCells = $(row).find('td');
        if (allCells.length > 0) {
            var found = false;
            allCells.each(function (index, td) {
                var regExp = new RegExp(inputVal, 'i');
                if (regExp.test($(td).text())) {
                    found = true;
                    return false;
                }
            });
            if (found == true) {
                $(row).show();
            }
            else $(row).hide();
        }
    });


   // document.getElementById(msgId).innerHTML = "Found : " + ($('tr:visible').length - 1) + " records";
}


function getTimeStamp() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();
    var s = today.getSeconds();

    h = checkTimeFormat(h);
    m = checkTimeFormat(m);
    s = checkTimeFormat(s);
    return h + ":" + m + ":" + s;
}

function checkTimeFormat(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function scrollWin(selector, interval) {

    $('html,body').animate({
        scrollTop: $(selector).offset().top - 100
    }, interval);

    $(selector).fadeTo('slow', 0.2).fadeTo('slow', 1.0);
}

function scrollWin(selector, interval,topMinus) {

    $('html,body').animate({
        scrollTop: $(selector).offset().top - topMinus
    }, interval);

    $(selector).fadeTo('slow', 0.2).fadeTo('slow', 1.0);
}


function checkSearchText(Id) {
    var txt = document.getElementById(Id);
    if (txt.value.length == 0) {
        txt.focus();
        return false;
    }

    return true;
}

function autoNavigate(url) {
    try {
        setTimeout("navigateHere('" + url + "');", 3000);
    }
    catch (e) {
        alert(e.Message);
    }
}

function navigateHere(url) {
    window.location = url;
}

function autoClosePopUp(ModalPopupExtenderId) {
    try {
        setTimeout("closePopup('" + ModalPopupExtenderId + "');", 2000);
    }
    catch (e) {
        alert(e.Message);
    }
}

function closePopup(ModalPopupExtenderId) {
    $find(ModalPopupExtenderId).hide();
}




function setFocus(id) {
    $('#' + id).focus();
    goToByScroll(id);
}


function goToByScroll(id) {

    id = id.replace("link", "");

    $('html,body').animate({
        scrollTop: $("#" + id).offset().top
    },
        'slow');
}


function compareDateWithToday(dateValue) {
    var one_day = 1000 * 60 * 60 * 24;
    var today = new Date();
    cDate = changeDateFormat(dateValue);
    one_day = Math.ceil((cDate - today) / one_day);

    if (one_day >= 0) return true;
    else return false;
}

function compareDateWithToday(id, warningMessage) {
    var dateValue = $('#' + id).val();

    var one_day = 1000 * 60 * 60 * 24;
    var today = new Date();
    cDate = changeDateFormat(dateValue);
    one_day = Math.ceil((cDate - today) / one_day);

    if (one_day >= 0) return true;
    else {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        return false;
    }
}

function changeDateFormat(dateValue) {
    var tmp = dateValue.split('/');
    var date = tmp[1] + '/' + tmp[0] + '/' + tmp[2];
    var newDate = new Date(date);
    return newDate;
}

function compareTwoDate(dateValue1, dateValue2) {

    var date1 = new Date();
    var date2 = new Date();

    date1 = changeDateFormat(dateValue1);
    date2 = changeDateFormat(dateValue2);

    if (date1 < date2) return true;
    else return false;
}


function checkIsDate(dateValue) {
    var dt = new Date(dateValue);
    if (dt.getFullYear() > 1 && dt.getDay() > 0) return true;
    else return false;
}

function isValidDate(id, warningMessage) {
    var dateValue = $('#' + id).val();
    var dt = new Date(dateValue);
    if (dt.getFullYear() > 1 && dt.getDay() >= 0) return true;
    else {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        return false;
    }
}

function getDays(date1, date2) {
    var one_day=1000*60*60*24; 

    var x = date1.split("/");
    var y = date2.split("/");


    var d1=new Date(x[2],(x[1]-1),x[0]);
    var d2 = new Date(y[2], (y[1] - 1), y[0]);

    var month1=x[1]-1;
    var month2=y[1]-1;
 
    return Math.ceil((d2.getTime()-d1.getTime())/(one_day)); 
}

function checkNumeric(element) {
    if (isNaN(element.val())) {
        element.val('');
        return false;
    }
    else return true;
}

function isNonZeroValue(value) {
    if (isNaN(value) == true) return false;
    else {
        if (parseInt(value) > 0) return true;
        else return false;
    }
}


function setGreen(elementId) {
    $('#' + elementId).addClass('validInput');
}

function setInvalid(elementId) {
    $('#' + elementId).addClass('invalidInput');
}




function validateText(id, minLength, maxLength, warningMessage) {
    var txt = document.getElementById(id);
    var len = txt.value.trim().length;

    if (len < minLength || len > maxLength) {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        txt.title = warningMessage;
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}

function validateTextLetters(id, minLength, maxLength, warningMessage) {

    if (validateText(id, minLength, maxLength, warningMessage + " [Letters Only]") == false) return false;

    var txt = document.getElementById(id);
    var len = txt.value.trim().length;


    if (isLettersOnly(txt.value) == false) {
        showMessage("Letters only", 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}

function validateTextForAlphaNumeric(id, minLength, maxLength, warningMessage) {

    if (validateText(id, minLength, maxLength, warningMessage + " [Letters and numeric Only] ") == false) return false;

    var txt = document.getElementById(id);
    var len = txt.value.trim().length;

    if (isAlphanumeric(txt.value) == false) {
        showMessage("Letters and numbers only", 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}


function validateTextForNumeric(id, minLength, maxLength, warningMessage) {

    if (validateText(id, minLength, maxLength, warningMessage + " [Numbers Only] ") == false) return false;

    var txt = document.getElementById(id);
    var len = txt.value.trim().length;

    if (isNumeric(txt.value) == false) {
        showMessage("Letters and numbers only", 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}


function validateCombo(id, defaultValue, warningMessage) {
    var txt = document.getElementById(id);

    if (txt.value.trim().length == 0) {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    var selectedVal = $('#' + id + " option:selected").val();

    if (selectedVal == defaultValue) {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}


function validateEmails(firstEmailId, secondEmailId, minLength, warningMessage) {

    if (validateText(firstEmailId, minLength, 50, 'Enter an email') == false || validateText(secondEmailId, minLength, 50, 'Enter an email') == false) return false;

    var txt = document.getElementById(firstEmailId);
    var txt2 = document.getElementById(secondEmailId);

    if (txt.value.trim() != txt2.value.trim()) {
        showMessage(warningMessage, 'warning');
        setInvalid(firstEmailId);
        txt.focus();
        return false;
    }

    setGreen(firstEmailId);
    return true;
}

function validatePasswords(firstPasswordId, secondPasswordId, minLength, warningMessage) {
    if (validateText(firstPasswordId, minLength, 20, warningMessage) == false || validateText(secondPasswordId, minLength, 20, warningMessage) == false) return false;

    var txt = document.getElementById(firstPasswordId);
    var txt2 = document.getElementById(secondPasswordId);

    if (txt.value != txt2.value) {
        showMessage("Password does not match", 'warning');
        setInvalid(firstPasswordId);
        txt.focus();
        return false;
    }

    setGreen(firstPasswordId);
    return true;
}


function validateRadio(divId, warningMessage) {
    if ($('#' + divId + ':not(:has(:radio:checked))').length == 1) {
        showMessage(warningMessage, 'warning');
        return false;
    }

    return true;
}

function isValidEmail(id, warningMessage) {
    var txt = document.getElementById(id);
    if (isValidEmailAddress(txt.value) == false) {
        showMessage(warningMessage, 'warning');
        setInvalid(id);
        txt.focus();
        return false;
    }

    setGreen(id);
    return true;
}

function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailAddress);
}


function isLettersOnly(elementValue) {
    var alphaExp = /^[a-zA-Z]+$/;
    if (elementValue.match(alphaExp)) return true;
    else return false;
}

function isAlphanumeric(elementValue) {
    var alphaExp = /^[0-9a-zA-Z]+$/;
    if (elementValue.match(alphaExp)) return true;
    else return false;
}

function isNumeric(elementValue) {
    var alphaExp = /^[0-9]+$/;
    if (elementValue.match(alphaExp)) return true;
    else return false;
}

function closePopWindow(extenderId, message) {

    $find(extenderId).hide();
    document.getElementById('lblMessage').innerHTML = message;
}

function autoCloseWindow()
{
    alert('close Please');
    setTimeout("closeThisWindow()",2000);
}

function closeThisWindow()
{
    this.close();
}

function getParameterByName(name) {
    name = name.toLowerCase();
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search.toLowerCase());
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}


function setFocus() {
    setTimeout("forceToFocus('')", 500);
}

function setFocus(elemId) {
    setTimeout("forceToFocus('" + elemId + "')", 500);
}

function setFocus(elemId,interval) {
    setTimeout("forceToFocus('" + elemId + "')", interval);
}

function forceToFocus(elemId) {
    if (elemId.length == 0) $(".popBox input:text:visible:first").focus();
    else $("#" + elemId).focus();
}

function setTopMostFocus()
{
    $('.popBox .topFocus').each(function ()
    {
        $(this).focus();
    });
}


function setInMiddle(divId)
{
    try
    {
        var left = $(window).width() / 2 - ($("#" + divId).width() / 2);
        var top = $(window).height() / 2 - (($("#" + divId).height() / 2)+20);
        $("#" + divId).css('top', top).css('left',left);

    }
    catch (e)
    {
        alert(e.message);
    }

}

function getCookie(c_name) {
    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start == -1) {
        c_start = c_value.indexOf(c_name + "=");
    }
    if (c_start == -1) {
        c_value = null;
    }
    else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end == -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
    }
    return c_value;
}

function delCookie(name) {
    document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}

Date.prototype.addHours = function (h) {
    this.setHours(this.getHours() + h);
    return this;
}
function showMessage(message, messageType) {

    try {
        $('#lblErrorMessage').hide();

        clearTimeout(timeOutId);

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
