

$(function () {
    //填写表单时的验证
    $("input[type='text']").keyup(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            if (checkValue.indexOf("required") >= 0)//必填
            {
                if ($(this).val().trim().length == 0) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Input can not be empty");
                }
                else {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-success");
                    clearDanger($(this));
                }
            }
            if (checkValue.indexOf("tel") >= 0)//电话
            {
                var isPhone = /^(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;

                if (!isPhone.test($(this).val().trim())) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Wrong Phone number");
                }
                else {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-success");
                    clearDanger($(this));
                }
            }
            if (checkValue.indexOf("number") >= 0)//数字
            {
                if (isNaN($(this).val().trim())) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Must be number");
                }
                else {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-success");
                    clearDanger($(this));
                }
            }
            if (checkValue.indexOf("maxLength") >= 0)//字数限制
            {
                var maxNum = /\d*-\d*/
                var maxLengthArr = maxNum.exec(checkValue)[0].split("-");
                if ($(this).val().trim().length > parseInt(maxLengthArr[1]) || $(this).val().trim().length < parseInt(maxLengthArr[0])) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Length out of limit");
                }
                else {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-success");
                    clearDanger($(this));
                }
            }
        }
    });
    $("textarea").keyup(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this)/*.parent().parent()*/.parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Input can not be empty");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }

        if (checkValue.indexOf("maxLength") >= 0)//字数限制
        {
            var maxNum = /\d*-\d*/
            var maxLengthArr = maxNum.exec(checkValue)[0].split("-");
            if ($(this).val().trim().length > parseInt(maxLengthArr[1]) || $(this).val().trim().length < parseInt(maxLengthArr[0])) {
                $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Length out of limit");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    $("#Date").keyup(function keyupfunc() {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this).parent().parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Date can not be empty");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    $("#Date").change(function keyupfunc() {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this).parent().parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Date can not be empty");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    //代叫车司机应时间答专用
    $("#Date2").keyup(function keyupfunc() {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this).parent().parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Date can not be empty");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    $("#Date2").change(function keyupfunc() {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this).parent().parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Date can not be empty");
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    $("select").change(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            if (checkValue.indexOf("required") >= 0)//必填
            {
                if ($(this).val() == 0) {
                    $(this).parent().parent().attr("class", "form-group has-error");
                    $(this).parent().find("i").remove();
                    $(this).parent().append("<i style=\"width:100%;text-align:left;\" class=\"fa fa-warning text-danger\">Please select</i>");
                } else {
                    $(this).parent().parent().attr("class", "form-group has-success");
                    $(this).parent().find("i").remove();
                }
            }
        }
    })
})

//提交表单时的验证
function submitCheck(boxName) {
    var box = $("body");
    if (typeof (boxName) != "undefined") {
        box = $("." + boxName);
    }
    var reCheck = true;
    box.find("input[type='text']").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {

            if (checkValue.indexOf("required") >= 0)//必填
            {
                if ($(this).val().trim().length == 0) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Input can not be empty");
                    reCheck = false;
                    return reCheck;
                }
            }
            if (checkValue.indexOf("tel") >= 0)//电话
            {
                var isPhone = /^(\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;

                if (!isPhone.test($(this).val().trim())) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Wrong number");
                    reCheck = false;
                    return reCheck;
                }

            }
            if (checkValue.indexOf("number") >= 0)//数字
            {
                if (isNaN($(this).val().trim())) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Must be number");
                    reCheck = false;
                    return reCheck;
                }
            }
            if (checkValue.indexOf("maxLength") >= 0)//字数限制
            {
                var maxNum = /\d*-\d*/
                var maxLengthArr = maxNum.exec(checkValue)[0].split("-");
                if ($(this).val().trim().length > parseInt(maxLengthArr[1]) || $(this).val().trim().length < parseInt(maxLengthArr[0])) {
                    $(this)/*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Length out of limit");
                    reCheck = false;
                    return reCheck;
                }

            }

        }
    });
    box.find("textarea").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {

            if (checkValue.indexOf("required") >= 0)//必填
            {
                if ($(this).val().trim().length == 0) {
                    $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Input can not be empty");
                    reCheck = false;
                    return reCheck;
                }
            }
            if (checkValue.indexOf("maxLength") >= 0)//字数限制
            {
                var maxNum = /\d*-\d*/
                var maxLengthArr = maxNum.exec(checkValue)[0].split("-");
                if ($(this).val().trim().length > parseInt(maxLengthArr[1]) || $(this).val().trim().length < parseInt(maxLengthArr[0])) {
                    $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group has-error");
                    IsDanger($(this), "Length out of limit");
                    reCheck = false;
                    return reCheck;
                }

            }
        }
    });
    box.find("select").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            if ($(this).val() == 0) {
                $(this).parent().parent().attr("class", "form-group has-error");
                $(this).parent().find("i").remove();
                $(this).parent().append("<i style=\"width:100%;text-align:left;\" class=\"fa fa-warning text-danger\">Please select</i>")
                reCheck = false;
                return reCheck;
            }
        }
    })
    box.find($("#Date")).each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) == "undefined") return;
        if (checkValue.indexOf("required") >= 0)//必填
        {
            if ($(this).val().trim().length == 0) {
                $(this).parent().parent().parent().attr("class", "form-group has-error");
                IsDanger($(this), "Date can not be empty");
                reCheck = false;
                return reCheck;
            }
            else {
                $(this)/*.parent()*//*.parent()*/.parent().parent().parent().attr("class", "form-group has-success");
                clearDanger($(this));
            }
        }
    })
    return reCheck;
}



function ClearCheckType() {
    $("input").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            $(this)/*.parent()*/.parent().parent().attr("class", "form-group");
            clearDanger($(this));
        }
    })
    $("textarea").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            $(this)/*.parent()*//*.parent()*/.parent().parent().attr("class", "form-group");
            clearDanger($(this));
        }
    });
    $("select").each(function () {
        var checkValue = $(this).attr("check-type");
        if (typeof (checkValue) != "undefined") {
            $(this).parent().parent().attr("class", "form-group");
            $(this).parent().find("i").remove();
        }
    })
}


function IsDanger(obj, msg) {
    if (typeof (obj.parent().parent().find("i")[0]) == "undefined") {
        obj.parent().parent().find("i").remove();
        obj.parent().parent().append("<i style=\"width:100%;text-align:center;\" class=\"fa fa-warning text-danger\">" + msg + "</i>");
    }
}

function onloadClearDanger(obj) {
    obj.children().children().children().children().attr("class", "form-group");
    $("#dateDiv").attr("class", "form-group");
    $("#dateDiv2").attr("class", "form-group");
    obj.find("i").remove();
}
function clearDanger(obj) {
    obj.parent().parent().find("i").remove();
}

