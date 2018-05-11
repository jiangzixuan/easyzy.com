/****************** 以下是业务相关公共方法 ****************************/
var suggest = function () {
    letDivCenter(".prop-suggest");
    $(".prop-suggest .thanks").hide();
    $(".prop-suggest").show();
}

var closesuggest = function () {
    $(".prop-suggest").hide();
}

var savesuggest = function () {
    var content = $("#cs_txt_content").val();
    if (trim(content) === "") {
        $("#cs_txt_content").focus();
        return false;
    }
    var name = $("#cs_UserName").val();
    var phone = $("#cs_Phone").val();
    $.post("/Common/SaveSuggest",
        { content: content, name: name, phone: phone },
        function (data) {
            $(".prop-suggest .thanks").show();
            window.setTimeout('closesuggest();', 3000);
        })
}


$(function () {
    uaredirect("http://m.easyzy.com"); //跳转移动版

    $(".head .udiv").mouseover(function () {
        $(".head .uinfo i").css("background-position", "0 -22px");
        $(".head .uprop").show();
    }).mouseout(function () {
        $(".head .uinfo i").css("background-position", "0 -2px");
        $(".head .uprop").hide();
    })

    //退出
    $(".exit").on("click", function () {
        $.post("/Common/Exit",
            {},
            function () {
                window.location.href = "/";
            });
    })
})



/* 以下是一些公共方法 */
function dialogAlert(content) {
    dialog({
        title: '系统提示',
        content: content
    }).showModal();
}

function dialogAlertNoModal(content) {
	dialog({
		title: '系统提示',
		content: content
	}).show();
}

function dialogConfirm(content, okfunction, cancelfunction) {
	var d = dialog({
		title: '系统提示',
		content: content,
		okValue: '确定',
		ok: okfunction,
		cancelValue: '取消',
		cancel: (cancelfunction === '' ? 'function () {}' : cancelfunction)
	}).showModal();

	return d;
}

function trim(str) {   //删除左右两端的空格
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function ltrim(str) { //删除左边的空格
    return str.replace(/(^\s*)/g, "");
}

function rtrim(str) { //删除右边的空格
    return str.replace(/(\s*$)/g, "");
}

//Div居中
function letDivCenter(divName) {
    var top = ($(window).height() - $(divName).height()) / 2;
    var left = ($(window).width() - $(divName).width()) / 2;
    var scrollTop = $(document).scrollTop();
    var scrollLeft = $(document).scrollLeft();
    $(divName).css({ position: 'absolute', 'top': top + scrollTop, left: left + scrollLeft }).show();
}


