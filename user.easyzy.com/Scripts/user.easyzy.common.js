
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
        cancel: (cancelfunction == '' ? 'function () {}' : cancelfunction)
    }).showModal();

    return d;
}

/*判断值是否为Null Undefined*/
function IsNull(obj) {
    return obj === undefined || obj === null;
}

/*判断值是否为空值*/
function IsEmpty(obj) {
    return obj === '';
}

/*判断值是否j是数字*/
function isNumber(Number) {
    var r = new RegExp("^\\d+(\\.\\d+)?$");
    return Number.match(r);
}

/*验证用户名，长度5-16，仅支持字母、数字和下划线*/
function isUserName(obj) {
    var r = new RegExp("^[0-9a-zA-Z_]{5,16}$");
    return obj.match(r);
}

/*验证密码，长度6~18，仅支持字母、数字和下划线*/
function isPassword(obj) {
    var r = new RegExp("^[0-9a-zA-Z_]{6,18}$");
    return obj.match(r);
}