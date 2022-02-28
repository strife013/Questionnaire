function GetValueByIndexNumByPre(preIndex) {
    return String.fromCharCode(preIndex); 
} 
function setradio(obj) { 
    if (obj.checked) obj.setAttribute('checked', true);
    else obj.removeAttribute('checked'); //要记得删除，要不默认的选择会随innerHTML一起返回
}
function settext(obj) {
    obj.setAttribute('value', obj.value);
}
function GetValueByIndexNumByPre(preIndex) {
    return String.fromCharCode(preIndex);
}
function GetValueByIndexNum(indexNum) {
    switch (indexNum) {
        case 0:
            return "A";
        case 1:
            return "B";
        case 2:
            return "C";
        case 3:
            return "D";
        case 4:
            return "E";
        case 5:
            return "F";
        case 6:
            return "G";
        case 7:
            return "H";
        case 8:
            return "I";
        case 9:
            return "J";
        case 10:
            return "K";
        case 11:
            return "L";
        case 12:
            return "M";
        case 13:
            return "N";
        case 14:
            return "O";
        case 15:
            return "P";
        case 16:
            return "Q";
        case 17:
            return "R";
        default:
    }
    return "";
}


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

function getTime(data, type) {
    var _data = data;
    //如果是13位正常，如果是10位则需要转化为毫秒
    if (String(data).length == 13) {
        _data = data
    } else {
        _data = data * 1000
    }
    const time = new Date(_data);
    const Y = time.getFullYear();
    const Mon = time.getMonth() + 1;
    const Day = time.getDate();
    const H = time.getHours();
    const Min = time.getMinutes();
    const S = time.getSeconds();
    //自定义选择想要返回的类型
    if (type == "Y") {
        return `${Y}-${Mon}-${Day}`
    } else if (type == "H") {
        return `${H}:${Min}:${S}`
    } else {
        return `${Y}-${Mon}-${Day} ${H}:${Min}:${S}`
    }
} 
function showmiddlemessage(msg, time) {
    alert(msg);
}