﻿KindEditor.plugin('flash', function (K) {
    var self = this, name = 'flash', lang = self.lang(name + '.'),
		allowFlashUpload = K.undef(self.allowFlashUpload, true),
		allowFileManager = K.undef(self.allowFileManager, false),
		formatUploadUrl = K.undef(self.formatUploadUrl, true),
		extraParams = K.undef(self.extraFileUploadParams, {}),
		filePostName = K.undef(self.filePostName, 'imgFile'),
        iframePath = self.pluginsPath + 'fileupload/fileupload.html',
		uploadJson = K.undef(self.uploadJson, self.basePath + 'upload_json.ashx');
    self.plugin.flash = {
        edit: function () {
            var html = [
				'<div style="padding:20px;">',
            // url
				'<div class="ke-dialog-row">',
				'<label for="keUrl" style="width:40px;">' + lang.url + '</label>',
				'<input class="ke-input-text" type="text" id="keUrl" name="url" value="" style="width:190px;" />&ensp;',
				'<input type="button" class="ke-upload-button" value="' + lang.upload + '" />&ensp;',
				'<span class="ke-button-common ke-button-outer">',
				'<input type="button" class="ke-button-common ke-button" style="width:65px;" name="viewServer" value="' + lang.viewServer + '" />',
				'</span>',
				'</div>',
            // width & height
				'<div class="ke-dialog-row">',
				'<label for="keWidth" style="width:40px;">' + lang.width + '</label>',
				'<input type="text" id="keWidth" class="ke-input-text ke-input-number" name="width" value="856" maxlength="4" /> ',
				'<label for="keHeight" style="width:40px; margin-left:26px;">' + lang.height + '</label>',
				'<input type="text" id="keHeight" class="ke-input-text ke-input-number" name="height" value="480" maxlength="4" /> ',
                '</div>',
            // iframe
				'<div class="ke-dialog-row">',
                '<input type="hidden" id="hdfFileType" value="*.mp4;*.swf;*.wmv" />',
				'<iframe src="' + iframePath + '" id="iframeFileUpload" width="385" height="84" frameborder="no" border="0" scrolling="no" allowtransparency="yes"></iframe>',
				'</div>',
				'</div>'
			].join('');
            var dialog = self.createDialog({
                name: name,
                width: 423,
                title: self.lang(name),
                body: html,
                yesBtn: {
                    name: self.lang('yes'),
                    click: function (e) {
                        var url = K.trim(urlBox.val()),
							width = widthBox.val(),
							height = heightBox.val();
                        if (url == 'http://' || K.invalidUrl(url)) {
                            alert(self.lang('invalidUrl'));
                            urlBox[0].focus();
                            return;
                        }
                        if (!/^\d*$/.test(width)) {
                            alert(self.lang('invalidWidth'));
                            widthBox[0].focus();
                            return;
                        }
                        if (!/^\d*$/.test(height)) {
                            alert(self.lang('invalidHeight'));
                            heightBox[0].focus();
                            return;
                        }
                        var html = K.mediaImg(self.themesPath + 'common/blank.gif', {
                            src: url,
                            type: K.mediaType('.swf'),
                            width: width,
                            height: height,
                            quality: 'high',
                            allowfullscreen: 'true'
                        });
                        if (url.toLowerCase().indexOf('.mp4') >= 0 || url.toLowerCase().indexOf('.swf') >= 0) {
                            html += '<br /><br /><a target=\"_blank\" ';
                            html += 'href=\"/FlowPlayer/flowplayer.aspx?url=';
                            html += url.substring(36, 59);
                            html += '\">' + lang.urlTitle + '</a>';
                        }
                        self.insertHtml(html).hideDialog().focus();
                    }
                }
            }),
			div = dialog.div,
			urlBox = K('[name="url"]', div),
			viewServerBtn = K('[name="viewServer"]', div),
			widthBox = K('[name="width"]', div),
			heightBox = K('[name="height"]', div);
            urlBox.val('http://');

            if (allowFlashUpload) {
                var uploadbutton = K.uploadbutton({
                    button: K('.ke-upload-button', div)[0],
                    fieldName: filePostName,
                    extraParams: extraParams,
                    url: K.addParam(uploadJson, 'dir=flash'),
                    afterUpload: function (data) {
                        dialog.hideLoading();
                        if (data.error === 0) {
                            var url = data.url;
                            if (formatUploadUrl) {
                                url = K.formatUrl(url, 'absolute');
                            }
                            urlBox.val(url);
                            if (self.afterUpload) {
                                self.afterUpload.call(self, url, data, name);
                            }
                            alert(self.lang('uploadSuccess'));
                        } else {
                            alert(data.message);
                        }
                    },
                    afterError: function (html) {
                        dialog.hideLoading();
                        self.errorDialog(html);
                    }
                });
                uploadbutton.fileBox.change(function (e) {
                    dialog.showLoading(self.lang('uploadLoading'));
                    uploadbutton.submit();
                });
            } else {
                K('.ke-upload-button', div).hide();
            }

            if (allowFileManager) {
                viewServerBtn.click(function (e) {
                    self.loadPlugin('filemanager', function () {
                        self.plugin.filemanagerDialog({
                            viewType: 'LIST',
                            dirName: 'flash',
                            clickFn: function (url, title) {
                                if (self.dialogs.length > 1) {
                                    K('[name="url"]', div).val(url);
                                    if (self.afterSelectFile) {
                                        self.afterSelectFile.call(self, url);
                                    }
                                    self.hideDialog();
                                }
                            }
                        });
                    });
                });
            } else {
                viewServerBtn.hide();
            }

            var img = self.plugin.getSelectedFlash();
            if (img) {
                var attrs = K.mediaAttrs(img.attr('data-ke-tag'));
                urlBox.val(attrs.src);
                widthBox.val(K.removeUnit(img.css('width')) || attrs.width || 0);
                heightBox.val(K.removeUnit(img.css('height')) || attrs.height || 0);
            }
            urlBox[0].focus();
            urlBox[0].select();
        },
        'delete': function () {
            self.plugin.getSelectedFlash().remove();
            // [IE] 删除图片后立即点击图片按钮出错
            self.addBookmark();
        }
    };
    self.clickToolbar(name, self.plugin.flash.edit);
});