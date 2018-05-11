var latexServer = "http://www.forkosh.com/mimetex.cgi?";

function setLatex() {
    var img = editor.selection.getRange().getClosedNode();
    if (img && img.className == "edui-faked-matheq") {
        var src = img.getAttribute("src");
        if (src.indexOf(latexServer) >= 0) {
            var latex = src.replace(latexServer, "");
            return latex;
        }
    }
    return "";

}

function getLatex() {
    var latex = ($G((browser.ie) ? "f_editor1" : "f_editor2")).getLatex();
    return latex;
}

function removeLoading() {
    $G("loading").style.display = "none";
    $G("eqeditor").style.display = "block";
}

dialog.onok = function () {
    var latex = getLatex();
    var src = latexServer + latex;
    editor.execCommand('inserthtml', "<img class='edui-faked-matheq' src='" + src + "' style='vertical-align:middle;'/>");
    dialog.close();
}