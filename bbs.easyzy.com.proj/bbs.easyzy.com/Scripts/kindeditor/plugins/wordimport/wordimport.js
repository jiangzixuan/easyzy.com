KindEditor.plugin('wordimport', function (K) {
    var self = this, name = 'wordimport';
    self.clickToolbar(name, function () {
        var lang = self.lang(name + '.'),
			html = '<div style="padding:10px 20px;">' +
				       '<div style="margin-bottom:10px;">' + lang.comment + '</div>' +
                       '<iframe name="uploadIframe" id="uploadIframe" style="display: none;"></iframe>' +
                       '<form id="uploadForm" name="uploadForm" method="post" enctype="multipart/form-data" target="uploadIframe">' +
                           '<ul class="table">' +
                               '<li><span id="fileAddress"><input type="file" id="wordFile" name="wordFile" style="width: 360px;" /></span></li>' +
                               '<li>' + lang.remark + '</li>' +
                           '</ul>' +
                       '</form>' +
	 		       '</div>',
			dialog = self.createDialog({
			    name: name,
			    width: 400,
			    title: self.lang(name),
			    body: html,
			    yesBtn: {
			        name: self.lang('yes'),
			        click: function (e) {
			            if (dialog.isLoading) {
			                return;
			            }
			            var filePath = jQuery("#wordFile").val();
			            if (filePath == "") {
			                alert(self.lang('pleaseSelectFile'));
			                return;
			            }
			            var allowExt = ".doc|.docx";
			            var fileExt = filePath.substr(filePath.lastIndexOf(".")).toUpperCase();
			            var arrExt = allowExt.toUpperCase().split("|");
			            var isTrue = false;
			            for (var i = 0; i < arrExt.length; i++) {
			                if (arrExt[i] == fileExt) { isTrue = true; break; }
			            }
			            if (isTrue == false) {
			                var fObj = document.getElementById("fileAddress");
			                if (fObj) {
			                    fObj.innerHTML = fObj.innerHTML;
			                }
			                alert(self.lang('invalidWord'));
			                return;
			            }
			            dialog.showLoading(self.lang('uploadLoading'));
			            jQuery.ajaxFileUpload({
			                url: "/kindeditor/plugins/wordimport/wordimport.ashx?postType=WordImport&rdn=" + Math.random(),
			                secureuri: false,
			                fileElementId: 'wordFile',
			                dataType: 'text',
			                success: function (data, status) {
			                    if (status == "success") {
			                        self.insertHtml(data).hideDialog().focus();
			                    }
			                    else {
			                        alert(self.lang('uploadError'));
			                        return;
			                    }
			                },
			                error: function (data, status, e) {
			                    alert(self.lang('uploadError'));
			                    return;
			                }
			            });
			        }
			    }
			}),
			div = dialog.div,
			iframe = K('iframe', div),
			doc = K.iframeDoc(iframe);
        if (!K.IE) {
            doc.designMode = 'on';
        }
        doc.open();
        doc.write('<!doctype html><html><head><title>WordImport</title></head>');
        if (!K.IE) {
            doc.write('<br />');
        }
        doc.write('</body></html>');
        doc.close();
        if (K.IE) {
            doc.body.contentEditable = 'true';
        }
        iframe[0].contentWindow.focus();
    });
});

// ajaxfileupload-min.js
jQuery.extend({createUploadIframe:function(d,c){var b="jUploadFrame"+d;if(window.ActiveXObject){var a=document.createElement('<iframe id="'+b+'" name="'+b+'" />');if(typeof c=="boolean")a.src="javascript:false";else if(typeof c=="string")a.src=c}else{var a=document.createElement("iframe");a.id=b;a.name=b}a.style.position="absolute";a.style.top="-1000px";a.style.left="-1000px";document.body.appendChild(a);return a},createUploadForm:function(d,e){var c="jUploadForm"+d,g="jUploadFile"+d,a=jQuery('<form  action="" method="POST" name="'+c+'" id="'+c+'" enctype="multipart/form-data"></form>'),b=jQuery("#"+e),f=jQuery(b).clone();jQuery(b).attr("id",g);jQuery(b).before(f);jQuery(b).appendTo(a);jQuery(a).css("position","absolute");jQuery(a).css("top","-1200px");jQuery(a).css("left","-1200px");jQuery(a).appendTo("body");return a},ajaxFileUpload:function(a){a=jQuery.extend({},jQuery.ajaxSettings,a);var e=a.fileElementId,c=jQuery.createUploadForm(e,a.fileElementId),i=jQuery.createUploadIframe(e,a.secureuri),d="jUploadFrame"+e,h="jUploadForm"+e;a.global&&!jQuery.active++&&jQuery.event.trigger("ajaxStart");var g=false,b={};a.global&&jQuery.event.trigger("ajaxSend",[b,a]);var f=function(h){var e=document.getElementById(d);try{if(e.contentWindow){b.responseText=e.contentWindow.document.body?e.contentWindow.document.body.innerHTML:null;b.responseXML=e.contentWindow.document.XMLDocument?e.contentWindow.document.XMLDocument:e.contentWindow.document}else if(e.contentDocument){b.responseText=e.contentDocument.document.body?e.contentDocument.document.body.innerHTML:null;b.responseXML=e.contentDocument.document.XMLDocument?e.contentDocument.document.XMLDocument:e.contentDocument.document}}catch(j){jQuery.handleError(a,b,null,j)}if(b||h=="timeout"){g=true;var f;try{f=h!="timeout"?"success":"error";if(f!="error"){var i=jQuery.uploadHttpData(b,a.dataType);a.success&&a.success(i,f);a.global&&jQuery.event.trigger("ajaxSuccess",[b,a])}else jQuery.handleError(a,b,f)}catch(j){f="error";jQuery.handleError(a,b,f,j)}a.global&&jQuery.event.trigger("ajaxComplete",[b,a]);a.global&&!--jQuery.active&&jQuery.event.trigger("ajaxStop");a.complete&&a.complete(b,f);jQuery(e).unbind();setTimeout(function(){try{jQuery(e).remove();jQuery(c).remove()}catch(d){jQuery.handleError(a,b,null,d)}},100);b=null}};a.timeout>0&&setTimeout(function(){!g&&f("timeout")},a.timeout);try{var c=jQuery("#"+h);jQuery(c).attr("action",a.url);jQuery(c).attr("method","POST");jQuery(c).attr("target",d);if(c.encoding)c.encoding="multipart/form-data";else c.enctype="multipart/form-data";jQuery(c).submit()}catch(j){jQuery.handleError(a,b,null,j)}if(window.attachEvent)document.getElementById(d).attachEvent("onload",f);else document.getElementById(d).addEventListener("load",f,false);return{abort:function(){}}},uploadHttpData:function(c,b){var a=!b;a=b=="xml"||a?c.responseXML:c.responseText;b=="script"&&jQuery.globalEval(a);if(b=="json")eval("data = "+a);b=="html"&&jQuery("<div>").html(a).evalScripts();return a}})