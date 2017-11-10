
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