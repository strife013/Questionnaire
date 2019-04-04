
function PopWindow(strUrl, width, height) {
    if (width == undefined) {
        width = 640;
    }
    if (height == undefined) {
        height = window.screen.availHeight - 150;
    }
    // window.open(strUrl, "", "width=100%,height="+height+",top=" + (window.screen.availHeight - 55 - height) / 2 + ",left=" + (window.screen.availWidth - width) / 2 + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no");
    window.open(strUrl, "", "width=" + width + ",height=" + height + ",top=" + (window.screen.availHeight - 55 - height) / 2 + ",left=" + (window.screen.availWidth - width) / 2 + ",menubar=no,toolbar=no,scrollbars=yes,status=no,titlebar=no,resizable=yes,location=no");
} 
//得到url参数值
function getUrlValueByName(name) {
    var urlPara = window.location.search.substr(1).split("&");
    var paraCounts = urlPara.length;
    for (var i = 0; i < paraCounts; i++) {
        if (urlPara[i].substr(0, urlPara[i].indexOf("=")).indexOf(name) > -1) {
            return urlPara[i].substr(urlPara[i].indexOf("=") + 1);
        }
    }
    return "";
}
function SortCol(index, colindex, sortorder) {
    alert();
}
function OpenFullWindow(strUrl, strName) {

    window.open(strUrl);
}
String.prototype.template = function () {
    var args = arguments;
    return this.replace(/\{(\d+)\}/g, function (m, i) {
        return args[i];
    });
}


function showmiddlemessage(msg, time) {
    alert(msg);
}