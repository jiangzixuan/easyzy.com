/* 以下是业务相关公共方法 */
$(function () {
    $(".top-funcs-pic").on("click", function () {
        if ($(".top-funcs").hasClass("active")) {
            $(".top-funcs").removeClass("active");
        }
        else {
            $(".top-funcs").addClass("active");
        }
    })

    $(".head .logo").click(function () {
        window.location.href = "/";
    })

    //退出
    $(".exit").on("click", function () {
        $.post("Exit",
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