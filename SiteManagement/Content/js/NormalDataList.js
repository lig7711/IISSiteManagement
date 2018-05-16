var _pageConfigs = {};
var _result = {};

//table初始化
function TableStart(TableID, PageConfig, Columns) {
    //默认每页显示数量
    if (typeof (PageConfig.DefaultSize) == "undefined") {
        PageConfig.DefaultSize = "10";
    }
    //页面数量下拉设置
    if (typeof (PageConfig.PageSizeSelect) == "undefined") {
        PageConfig.PageSizeSelect = '5,10,20,50,100';
    }
    //同步方式
    if (typeof (PageConfig.async) == "undefined") {
        PageConfig.async = true;
    }
    //当前页面
    if (typeof (PageConfig.PageNow) == "undefined") {
        PageConfig.PageNow = "1";
    }
    //请求方式
    if (typeof (PageConfig.type) == "undefined") {
        PageConfig.type = "post";
    }
    //编码方式
    if (typeof (PageConfig.contentType) == "undefined") {
        PageConfig.contentType = "application/json;charset=utf-8";
    }
    //返回数据类型
    if (typeof (PageConfig.dataType) == "undefined") {
        PageConfig.dataType = "json";
    }
    //是否显示分页
    if (PageConfig.Pagination != false) {
        PageConfig.Pagination = true;
    }
    //参数
    if (typeof (PageConfig.data) == "undefined") {
        PageConfig.data = "";
    }
    else {
        PageConfig.PageNow = "1";
    }
    //排序字段
    if (typeof (PageConfig.orderBy) == "undefined") {
        PageConfig.orderBy = "";
    }
    //排序方式
    if (typeof (PageConfig.order) == "undefined") {
        PageConfig.order = "false";
    }
    //表头默认颜色
    if (typeof (PageConfig.tableColor) == "undefined") {
        PageConfig.tableColor = "#f5f8fa";
    }
    //主键ID;
    if (typeof (PageConfig.pakeId) == "undefined") {
        PageConfig.pakeId = "Id";
    }
    //是否默认全部选中，需配合后台实现
    if (typeof (PageConfig.allChecked) == "undefined" || PageConfig.allChecked != true) {
        PageConfig.allChecked = false;
    }
    //全选按钮点击事件动作定义，true：控制所有CheckBox，false：控制当前页CheckBox
    if (typeof (PageConfig.allCkAction) == "undefined" || PageConfig.allCkAction != true) {
        PageConfig.allCkAction = false;
    }
    //返回参数
    _pageConfigs[TableID] = { "columns": Columns, "PageConfig": PageConfig };
    _result[TableID] = { "total": 0, "selectIds": [], "pageTotal": 0, "AllIds": [] };

}

function Initialization(TableID, PageConfig, Columns) {
    //默认每页显示数量
    if (typeof (PageConfig.DefaultSize) == "undefined") {
        PageConfig.DefaultSize = "10";
    }
    //页面数量下拉设置
    if (typeof (PageConfig.PageSizeSelect) == "undefined") {
        PageConfig.PageSizeSelect = '5,10,20,50,100';
    }
    //同步方式
    if (typeof (PageConfig.async) == "undefined") {
        PageConfig.async = true;
    }
    //当前页面
    if (typeof (PageConfig.PageNow) == "undefined") {
        PageConfig.PageNow = "1";
    }
    //请求方式
    if (typeof (PageConfig.type) == "undefined") {
        PageConfig.type = "post";
    }
    //编码方式
    if (typeof (PageConfig.contentType) == "undefined") {
        PageConfig.contentType = "application/json;charset=utf-8";
    }
    //返回数据类型
    if (typeof (PageConfig.dataType) == "undefined") {
        PageConfig.dataType = "json";
    }
    //是否显示分页
    if (PageConfig.Pagination != false) {
        PageConfig.Pagination = true;
    }
    //参数
    if (typeof (PageConfig.data) == "undefined") {
        PageConfig.data = "";
    }
    else {
        PageConfig.PageNow = "1";
    }
    //排序字段
    if (typeof (PageConfig.orderBy) == "undefined") {
        PageConfig.orderBy = "";
    }
    //排序方式
    if (typeof (PageConfig.order) == "undefined") {
        PageConfig.order = "false";
    }
    //表头默认颜色
    if (typeof (PageConfig.tableColor) == "undefined") {
        PageConfig.tableColor = "#f5f8fa";
    }
    //主键ID;
    if (typeof (PageConfig.pakeId) == "undefined") {
        PageConfig.pakeId = "Id";
    }
    //是否默认全部选中，需配合后台实现
    if (typeof (PageConfig.allChecked) == "undefined" || PageConfig.allChecked != true) {
        PageConfig.allChecked = false;
    }
    //全选按钮点击事件动作定义，true：控制所有CheckBox，false：控制当前页CheckBox
    if (typeof (PageConfig.allCkAction) == "undefined" || PageConfig.allCkAction != true) {
        PageConfig.allCkAction = false;
    }
    //返回参数
    _pageConfigs[TableID] = { "columns": Columns, "PageConfig": PageConfig };
    _result[TableID] = { "total": 0, "selectIds": [], "pageTotal": 0, "AllIds": [] };

    //初始化Table  
    PageAjax(PageConfig, TableID, true);
}
//后台请求
function PageAjax(pageConfig, TableID, fristLoad) {

    //加载之前回掉
    if (typeof (_pageConfigs[TableID].PageConfig.openAjax) != "undefined") {
        //扩展
        _pageConfigs[TableID].PageConfig.openAjax();
    }
    //ajax从后台获取数据
    if (typeof (pageConfig.Result) == "undefined") {
        // var ajaxData = $.extend({ pageSize: pageConfig.DefaultSize, pageIndex: pageConfig.PageNow, orderBy: pageConfig.orderBy, order: pageConfig.order }, pageConfig.data);
        $.ajax({
            type: pageConfig.type,
            contentType: pageConfig.contentType,
            url: pageConfig.url,
            data: '{ pageSize:"' + pageConfig.DefaultSize + '",pageIndex:"' + pageConfig.PageNow + '",orderBy:"' + pageConfig.orderBy + '",order:"' + pageConfig.order + '"' + pageConfig.data + '}',
            dataType: pageConfig.dataType,
            async: pageConfig.async,
            timeout: 60000,
            beforeSend: function (XMLHttpRequest) {
                showWait();
            },
            success: function (data) {
                if (pageConfig.Pagination) {
                    $("#" + TableID).html(GetTable(data, TableID, fristLoad) + GetPagination(data.Total, TableID));
                } else {
                    $("#" + TableID).html(GetTable(data, TableID, fristLoad));
                }
                showOrderby(TableID);
                _result[TableID].total = data.Total;
                InitializationCheckBox(TableID);
                //table加载完成后回掉
                if (typeof (_pageConfigs[TableID].PageConfig.tableSuccess) != "undefined") {
                    _pageConfigs[TableID].PageConfig.tableSuccess(_result[TableID].selectIds, data.Total, data.OtherData);
                }
                if (typeof (_pageConfigs[TableID].PageConfig.portalData) != "undefined") {
                    //扩展
                    _pageConfigs[TableID].PageConfig.portalData(data);
                }
            },
            complete: function (XMLHttpRequest, status) {
                clearWait();
                //超时,status还有success,error等值的情况
                if (status == 'timeout') {
                    AlertInfo("ajax请求超时", false);
                }
            }
        });
    }
    //前台传递的数据集
    else {
        showWait();
        if (pageConfig.Pagination) {
            $("#" + TableID).html(GetTable(pageConfig.Result, TableID, fristLoad) + GetPagination(pageConfig.Result.length, TableID));
        } else {
            $("#" + TableID).html(GetTable(pageConfig.Result, TableID, fristLoad));
        }
        showOrderby(TableID);
        _result[TableID].total = pageConfig.Result.length;
        InitializationCheckBox(TableID);
        //table加载完成后回掉
        if (typeof (_pageConfigs[TableID].PageConfig.tableSuccess) != "undefined") {
            _pageConfigs[TableID].PageConfig.tableSuccess(_result[TableID].selectIds, pageConfig.Result.length);
        }
        if (typeof (_pageConfigs[TableID].PageConfig.portalData) != "undefined") {
            //扩展
            _pageConfigs[TableID].PageConfig.portalData(data);
        }
        clearWait();
    }

}

//刷新表格
function RefreshTable(TableID) {
    _pageConfigs[TableID].PageConfig.PageNow = 1;
    PageAjax(_pageConfigs[TableID].PageConfig, TableID, false);
}


//测试Table重绘
function GetTable(JsonStr, TableID, fristLoad) {
    //  Config配置
    var Config = _pageConfigs[TableID].PageConfig;
    //  列配置
    var Columns = _pageConfigs[TableID].columns;
    //默认全部选中
    if (Config.allChecked && JsonStr.IdList.length != 0) {
        if (fristLoad) {
            _result[TableID].selectIds = $.parseJSON(JSON.stringify(JsonStr.IdList));
        }
        else {
            var QxCkIds = chaji_array(_result[TableID].AllIds, _result[TableID].selectIds)
            _result[TableID].selectIds = chaji_array(JsonStr.IdList, QxCkIds);
        }
        _result[TableID].AllIds = $.parseJSON(JSON.stringify(JsonStr.IdList));
    }
    var dateJson = JsonStr.Data;
    var tablehtml = "";
    tablehtml = "<div class=\"ibox\"><table class=\"table table-striped table-hover\" style=\"table-layout:fixed;margin-bottom: -1px;width:100%\">"
    tablehtml += " <thead><tr>";
    for (var i = 0; i < Columns.length; i++) {
        if (!Columns[i].hidden) {
            var windhStr = "";
            if (typeof (Columns[i].width) != "undefined") {
                windhStr = "width:" + Columns[i].width + "px;";
            }
            switch (Columns[i].field) {
                case "CheckBox":
                    if (Config.CkAction) {
                        tablehtml += "<th style=\"text-align:center;" + windhStr + "\"><input id=\"CkAll_" + TableID + "\" onclick=\"CheckGlobal('" + TableID + "');\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\" /><span class=\"labels\"></span></label ></th>";
                    }
                    else {
                        tablehtml += "<th style=\"text-align:center;" + windhStr + "\"><input id=\"CkAll_" + TableID + "\" onclick=\"CheckAll('" + TableID + "');\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\" /><span class=\"labels\"></span></label ></th>";
                    }
                    break;
                case "Radio":
                    tablehtml += "<th style=\"text-align:center;" + windhStr + "\"><input id=\"CkAll_" + TableID + "\" disabled=\"true\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\"/><span class=\"labels\"></span></label ></th>";
                    break;
                default:
                    if (typeof (Columns[i].orderBy) != "undefined") {
                        tablehtml += "<th id=\"th_" + Columns[i].field + "\" onclick=\"OrderBy('" + Columns[i].field + "','" + TableID + "')\"  style=\"text-align:center;" + windhStr + "\">" + Columns[i].title + " <span class=\"fa fa-sort\"></span> </th>";
                    }
                    else {
                        tablehtml += "<th  style=\"text-align:center;" + windhStr + "\">" + Columns[i].title + "</th>";
                    }
                    break;
            }
        }
    }
    tablehtml += " </tr></thead><tbody>";
    for (var i = 0; i < dateJson.length; i++) {
        tablehtml += "<tr>";
        for (var j = 0; j < Columns.length; j++) {
            if (!Columns[j].hidden) {
                var widthStyle = "";
                if (typeof (Columns[j].width) != "undefined") {
                    widthStyle = "width:" + Columns[j].width + "px;"
                }
                switch (Columns[j].align) {
                    case "center": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:center;padding-bottom: 0px;" + widthStyle + "\">"; break;
                    case "left": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:left;padding-bottom: 0px;" + widthStyle + "\">"; break;
                    case "right": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:right;padding-bottom: 0px;" + widthStyle + "\">"; break;
                    default: tablehtml += "<td>"; break;
                }
                if (typeof (Columns[j].formatter) == "undefined") {
                    switch (Columns[j].field) {
                        case "CheckBox":
                            tablehtml += "<label style=\"margin-bottom:0px;\"><input  type=\"checkbox\" style=\"margin-top: 10px;\" onclick=\"CheckChildren(this,'checkbox','" + TableID + "');\" class=\"tc\" id=\"Ckb_" + dateJson[i][_pageConfigs[TableID].PageConfig.pakeId] + "\"/><span class=\"labels\"></span></label ></td>";
                            break;
                        case "Radio":
                            tablehtml += "<label style=\"margin-bottom:0px;\"><input type=\"checkbox\" style=\"margin-top: 10px;\" onclick=\"CheckChildren(this,'Radio','" + TableID + "');\" class=\"tc\" id=\"Ckb_" + dateJson[i][_pageConfigs[TableID].PageConfig.pakeId] + "\"/><span class=\"labels\"></span></label ></td>";
                            break;
                        case "NO":
                            tablehtml += ((Config.PageNow - 1) * _pageConfigs[TableID].PageConfig.DefaultSize) + i + 1 + "</td>";
                            break;
                        default:
                            var re = "";
                            if (Columns[j].field == "Edit") {
                                var rowJson = JSON.stringify(dateJson[i]).replace(/\"/g, "|");
                                re = " <a class=\"data-fresh\" onclick=\"Edit('" + rowJson + "',this,'" + TableID + "')\"><li class=\"fa fa-pencil\"></li></a>";
                            }
                            else {
                                if (Columns[j].Edit) {
                                    re = '<div class="EditBox' + dateJson[i]['' + Config.pakeId + ''] + '"><span>' + dateJson[i]['' + Columns[j].field + ''] + '</span><input style="text-align: center;display:none" type="text" name="' + Columns[j].field + '" class="form-control"></div></td>';
                                }
                                else {
                                    re = dateJson[i]['' + Columns[j].field + ''] + "</td>";
                                }
                            }
                            tablehtml += re;
                            break;
                    }
                }
                else {
                    var formatterRe = Columns[j].formatter(dateJson[i]['' + Columns[j].field + ''], dateJson[i])
                    tablehtml += formatterRe + "</td>";
                }
            }
        }
        tablehtml += "</tr>";
    }
    tablehtml += "</tbody></table></div>";

    return tablehtml;
}
//生成table
//function GetTable(JsonStr, TableID, fristLoad) {
//    //  Config配置
//    var Config = _pageConfigs[TableID].PageConfig;
//    //  列配置
//    var Columns = _pageConfigs[TableID].columns;
//    //默认全部选中
//    if (Config.allChecked && JsonStr.IdList.length != 0)
//    {
//        if (fristLoad) {
//            _result[TableID].selectIds = $.parseJSON(JSON.stringify(JsonStr.IdList));
//        }
//        else {
//            var QxCkIds = chaji_array(_result[TableID].AllIds, _result[TableID].selectIds)
//            _result[TableID].selectIds = chaji_array(JsonStr.IdList, QxCkIds);
//        }
//        _result[TableID].AllIds = $.parseJSON(JSON.stringify(JsonStr.IdList));
//    }
//    var dateJson = JsonStr.Data;
//    var tablehtml = "";
//    tablehtml = "<div style=\"padding-bottom: 10px;width: 100%;overflow:auto;overflow-y:hidden;float: left;\" class=\"table-scrollable easy-table\"><table style=\"table-layout:fixed;margin-bottom: -1px;width:100%\" class=\"table table-condensed  table-data-list\">"
//    tablehtml += " <thead><tr>";
//    for (var i = 0; i < Columns.length; i++) {
//        if (!Columns[i].hidden) {
//            var windhStr = "";
//            if (typeof (Columns[i].width) != "undefined") {
//                windhStr = "width:" + Columns[i].width + "px;";
//            }
//            switch (Columns[i].field) {
//                case "CheckBox":
//                    if (Config.CkAction) {
//                        tablehtml += "<th  style=\"font-weight:bolder;padding-bottom: 4px;background-color: " + Config.tableColor + ";text-align:center;border-bottom-width: 1px;" + windhStr + "\"><label style=\"margin-bottom:0px;\"><input id=\"CkAll_" + TableID + "\" onclick=\"CheckGlobal('" + TableID + "');\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\" /><span class=\"labels\"></span></label ></th>";
//                    }
//                    else {
//                        tablehtml += "<th  style=\"font-weight:bolder;padding-bottom: 4px;background-color: " + Config.tableColor + ";text-align:center;border-bottom-width: 1px;" + windhStr + "\"><label style=\"margin-bottom:0px;\"><input id=\"CkAll_" + TableID + "\" onclick=\"CheckAll('" + TableID + "');\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\" /><span class=\"labels\"></span></label ></th>";
//                    }
//                    break;
//                case "Radio":
//                    tablehtml += "<th  style=\"font-weight:bolder;padding-bottom: 4px;background-color: " + Config.tableColor + ";text-align:center;border-bottom-width: 1px;" + windhStr + "\"><label style=\"margin-bottom:0px;\"><input id=\"CkAll_" + TableID + "\" disabled=\"true\" type=\"checkbox\" class=\"tc\" style=\"margin-top: 10px;\"/><span class=\"labels\"></span></label ></th>";
//                    break;
//                default:
//                    if (typeof (Columns[i].orderBy) != "undefined") {
//                        tablehtml += "<th id=\"th_" + Columns[i].field + "\" onclick=\"OrderBy('" + Columns[i].field + "','" + TableID + "')\"  style=\"font-weight:bolder;color:black;cursor:pointer;background-color: " + Config.tableColor + ";text-align:center;border-bottom-width: 1px;" + windhStr + "\">" + Columns[i].title + " <span class=\"glyphicon glyphicon-sort-by-attributes\"></span> </th>";
//                    }
//                    else {
//                        tablehtml += "<th  style=\"font-weight:bolder;background-color: " + Config.tableColor + ";text-align:center;border-bottom-width: 1px;" + windhStr + "\">" + Columns[i].title + "</th>";
//                    }
//                    break;
//            }
//        }
//    }
//    tablehtml += " </tr></thead><tbody class=\"data-append\">";
//    for (var i = 0; i < dateJson.length; i++) {
//        tablehtml += "<tr class=\"data-remove\">";
//        for (var j = 0; j < Columns.length; j++) {
//            if (!Columns[j].hidden) {
//                var widthStyle = "";
//                if (typeof (Columns[j].width) != "undefined") {
//                    widthStyle = "width:" + Columns[j].width + "px;"
//                }
//                switch (Columns[j].align) {
//                    case "center": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:center;padding-bottom: 0px;" + widthStyle + "\">"; break;
//                    case "left": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:left;padding-bottom: 0px;" + widthStyle + "\">"; break;
//                    case "right": tablehtml += "<td style=\"line-height: 26px;padding-top: 0px;text-align:right;padding-bottom: 0px;" + widthStyle + "\">"; break;
//                    default: tablehtml += "<td>"; break;
//                }
//                if (typeof (Columns[j].formatter) == "undefined") {
//                    switch (Columns[j].field) {
//                        case "CheckBox":
//                            tablehtml += "<label style=\"margin-bottom:0px;\"><input  type=\"checkbox\" style=\"margin-top: 10px;\" onclick=\"CheckChildren(this,'checkbox','" + TableID + "');\" class=\"tc\" id=\"Ckb_" + dateJson[i][_pageConfigs[TableID].PageConfig.pakeId] + "\"/><span class=\"labels\"></span></label ></td>";
//                            break;
//                        case "Radio":
//                            tablehtml += "<label style=\"margin-bottom:0px;\"><input type=\"checkbox\" style=\"margin-top: 10px;\" onclick=\"CheckChildren(this,'Radio','" + TableID + "');\" class=\"tc\" id=\"Ckb_" + dateJson[i][_pageConfigs[TableID].PageConfig.pakeId] + "\"/><span class=\"labels\"></span></label ></td>";
//                            break;
//                        case "NO":
//                            tablehtml += ((Config.PageNow - 1) * _pageConfigs[TableID].PageConfig.DefaultSize) + i + 1 + "</td>";
//                            break;
//                        default:
//                            var re = "";
//                            if (Columns[j].field == "Edit") {
//                                var rowJson = JSON.stringify(dateJson[i]).replace(/\"/g, "|");
//                                re = " <a class=\"data-fresh\" onclick=\"Edit('" + rowJson + "',this,'" + TableID + "')\"><li class=\"glyphicon glyphicon-pencil\"></li></a>";
//                            }
//                            else {
//                                if (Columns[j].Edit) {
//                                    re = '<div class="EditBox' + dateJson[i]['' + Config.pakeId + ''] + '"><span>' + dateJson[i]['' + Columns[j].field + ''] + '</span><input style="text-align: center;display:none" type="text" name="' + Columns[j].field + '" class="form-control"></div></td>';
//                                }
//                                else {
//                                    re = dateJson[i]['' + Columns[j].field + ''] + "</td>";
//                                }
//                            }
//                            tablehtml += re;
//                            break;
//                    }
//                }
//                else {
//                    var formatterRe = Columns[j].formatter(dateJson[i]['' + Columns[j].field + ''], dateJson[i])
//                    tablehtml += formatterRe + "</td>";
//                }
//            }
//        }
//        tablehtml += "</tr>";
//    }
//    tablehtml += "</tbody></table></div>";

//    return tablehtml;
//}
//生成分页
function GetPagination(CountTotal, TableID) {
    var PageSize = _pageConfigs[TableID].PageConfig.DefaultSize;
    var PageSizeSelect = _pageConfigs[TableID].PageConfig.PageSizeSelect;
    var PageNow = _pageConfigs[TableID].PageConfig.PageNow;
    //总页数
    _result[TableID].pageTotal = Math.ceil(CountTotal / PageSize);
    var pageTotal = Math.ceil(CountTotal / PageSize);
    var PageHtml = "";
    PageHtml += "<div style=\"margin-top: 10px;height: 30px;width: 100%;float: left; margin-bottom: 50px;\">"
    //pageGo开始
    PageHtml += "<div class=\"pagination\" style=\"float:right;margin: 0px;\">"
    PageHtml += " <div class=\"pagego\" style=\"float:left;\">"
    PageHtml += " <input style=\"width: 35px;height: 32px;margin-left:2px;border: 1px solid #ddd;\" id=\"GoText_" + TableID + "\" type=\"text\" value=\"\" onkeyup=\"this.value=this.value.match(/^[0-9]*$/)\" onafterpaste=\"this.value=this.value.match(/^[1-9]\\d*$/)\" class=\"pageinput data-pageinput\">"
    PageHtml += "</div>"
    PageHtml += "<div style=\"float:left;\"><input onclick=\"GoPage('" + TableID + "');\" type=\"button\" class=\"btn btn-default\" value=\"Go\" style=\"height: 32px;border: 1px solid #ddd;width: 37px;margin-left: -2px;padding-right: 8px;padding-left: 8px;\">"
    PageHtml += "</div></div>";
    //pageGo结束
    //UL开始
    PageHtml += " <ul class=\"pagination\" style=\"float: right;margin: 0px;\"> "
    if (PageNow == 1) {
        classStr = "class=\"disabled\"";
        PageHtml += " <li class=\"disabled\"><a><i class=\"fa fa-angle-double-left\"></i></a></li> "
        PageHtml += " <li class=\"disabled\"><a>‹</a></li> "
    }
    else {
        PageHtml += " <li><a onclick=\"Jump('Frist','" + TableID + "');\"><i class=\"fa fa-angle-double-left\"></i></a></li> "
        PageHtml += " <li><a onclick=\"Jump('Last','" + TableID + "');\">‹</a></li> "
    }
    if (pageTotal <= 7) {
        for (var i = 1; i <= pageTotal; i++) {
            if (i == PageNow) {
                PageHtml += " <li class=\"active\"><a onclick=\"Jump('" + i + "','" + TableID + "');\">" + i + "</a></li> "
            }
            else {
                PageHtml += " <li><a onclick=\"Jump('" + i + "','" + TableID + "');\">" + i + "</a></li> "
            }
        }
    }
    else {
        //前后都够
        if (PageNow - 3 >= 1 && parseInt(PageNow) + 3 <= pageTotal) {
            for (var j = PageNow - 3; j <= parseInt(PageNow) + 3; j++) {
                if (PageNow == j) {
                    PageHtml += " <li class=\"active\"><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
                else {
                    PageHtml += " <li ><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
            }
        }
        //前够，后不够
        if (PageNow - 3 >= 1 && parseInt(PageNow) + 3 > pageTotal) {
            for (var j = PageNow - (3 + ((parseInt(PageNow) + 3) - pageTotal)); j <= pageTotal; j++) {
                if (PageNow == j) {
                    PageHtml += " <li class=\"active\"><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
                else {
                    PageHtml += " <li ><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
            }
        }
        //前不够，后够
        if (PageNow - 3 < 1 && parseInt(PageNow) + 3 <= pageTotal) {
            for (var j = PageNow - (3 - (Math.abs(PageNow - 3) + 1)); j <= parseInt(PageNow) + (Math.abs(PageNow - 3) + 1 + 3); j++) {
                if (PageNow == j) {
                    PageHtml += " <li class=\"active\"><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
                else {
                    PageHtml += " <li ><a onclick=\"Jump('" + j + "','" + TableID + "');\">" + j + "</a></li> "
                }
            }
        }
    }
    if (PageNow == pageTotal || pageTotal < 1) {
        PageHtml += " <li class=\"disabled\"><a>›</a></li> "
        PageHtml += " <li class=\"disabled\"><a><i class=\"fa fa-angle-double-right\"></i></a></li> "
    }
    else {
        PageHtml += " <li><a onclick=\"Jump('Next','" + TableID + "');\">›</a></li> "
        PageHtml += " <li><a onclick=\"Jump('Most','" + TableID + "');\"><i class=\"fa fa-angle-double-right\"></i></a></li> "
    }
    PageHtml += " </ul>"
    //ul结束

    //页面信息开始
    PageHtml += "<div style=\"float:right;margin-top: 10px;font-size: 12px;margin-right: 30px;\">";
    PageHtml += "<div style=\"float:right;\">记录/页，共<span>" + CountTotal + "</span>条记录</div>";
    PageHtml += " <select id=\"PageSelect\" onchange=\"PageSizeSelect(this,'" + TableID + "');\" style=\"width:52px;height:19px;padding: 0px;float:right;border:0px;font-size: 12px;margin-top: -2px;\" class=\"form-control\" >";
    var PageSizeSelectList = PageSizeSelect.split(',');
    for (var i = 0; i < PageSizeSelectList.length; i++) {
        if (PageSizeSelectList[i] == PageSize) {
            PageHtml += " <option selected value=\"" + PageSizeSelectList[i] + "\">" + PageSizeSelectList[i] + "</option>";
        }
        else {
            PageHtml += " <option value=\"" + PageSizeSelectList[i] + "\">" + PageSizeSelectList[i] + "</option>";
        }
    }
    PageHtml += " </select> ";
    PageHtml += "<div style=\"float:right;\">显示第<span>" + PageNow + "</span>-<span>" + pageTotal + "</span>，每条</div>";
    //页面信息结束
    PageHtml += "</div>";
    return PageHtml;
}
//翻页
function Jump(page, tableId) {
    var config = _pageConfigs[tableId].PageConfig
    switch (page) {
        case "Frist":
            config.PageNow = 1;
            break;
        case "Last":
            if (config.PageNow == 1) {
                return;
            }
            config.PageNow = parseInt(config.PageNow) - 1;
            break;
        case "Next":
            if (config.PageNow == _result[tableId].total) {
                return;
            }
            config.PageNow = parseInt(config.PageNow) + 1;
            break;
        case "Most":
            config.PageNow = _result[tableId].pageTotal;
            break;
        default:
            config.PageNow = page;
    }
    //翻页前回掉
    if (typeof (_pageConfigs[tableId].PageConfig.openJump) != "undefined") {
        _pageConfigs[tableId].PageConfig.openJump(config.PageNow);
    }
    PageAjax(config, tableId, false);
}
//跳转
function GoPage(tableId) {
    var _goText = parseInt($("#GoText_" + tableId).val());
    if (_goText != "" && _goText > 0 && _goText <= _result[tableId].pageTotal) {
        _pageConfigs[tableId].PageConfig.PageNow = _goText;
        PageAjax(_pageConfigs[tableId].PageConfig, tableId, false);
    }
    $("#GoText_" + tableId).val("")
}
//切换每页显示数量
//function PageSelect(obj, tableId) {
//    _pageConfigs[tableId].PageConfig.DefaultSize = obj.value;
//    _pageConfigs[tableId].PageConfig.PageNow = 1;
//    PageAjax(_pageConfigs[tableId].PageConfig, tableId, false);
//}
function PageSizeSelect(obj, tableId) {
    _pageConfigs[tableId].PageConfig.DefaultSize = obj.value;
    _pageConfigs[tableId].PageConfig.PageNow = 1;
    PageAjax(_pageConfigs[tableId].PageConfig, tableId, false);
}
//排序
function OrderBy(column, tableId) {
    if (_pageConfigs[tableId].PageConfig.orderBy == "") {
        _pageConfigs[tableId].PageConfig.orderBy = column
        _pageConfigs[tableId].PageConfig.order = "false"
    }
    else {
        if (_pageConfigs[tableId].PageConfig.order == "true") {
            _pageConfigs[tableId].PageConfig.orderBy = column
            _pageConfigs[tableId].PageConfig.order = "false"
        }
        else {
            _pageConfigs[tableId].PageConfig.orderBy = column
            _pageConfigs[tableId].PageConfig.order = "true"
        }
    }
    PageAjax(_pageConfigs[tableId].PageConfig, tableId, false);

}
function showOrderby(tableId) {
    if (_pageConfigs[tableId].PageConfig.orderBy != "") {
        var arr = _pageConfigs[tableId].PageConfig.orderBy.split(' ');
        var li = $("#" + tableId + " th[id *= '" + arr[0].trim() + "'] span")
        if (_pageConfigs[tableId].PageConfig.order == "false") {
            li.prop("class", "fa fa-sort-amount-asc")
        }
        else {
            li.prop("class", "fa fa-sort-amount-desc")
        }
    }
}
//行内编辑
function Edit(rowData, obj, tableId) {
    rowData = rowData.replace(/\|/g, "\"")
    var JsonRowData = eval('(' + rowData + ')')
    var Divs = $("#" + tableId + " div[class = 'EditBox" + JsonRowData[_pageConfigs[tableId].PageConfig.pakeId] + "']");


    if (obj.firstChild.className == "fa fa-check") {

        if (typeof (_pageConfigs[tableId].PageConfig.endEdit) != "undefined") {
            _pageConfigs[tableId].PageConfig.endEdit(Divs, JsonRowData);
        }
        Divs.each(function (n) {
            $.each(JsonRowData, function (name, value) {
                if (name == Divs.eq(n).find("input").prop("name")) {
                    JsonRowData[name] = Divs.eq(n).find("input").val()
                }
            })
        })

        $.ajax({
            url: _pageConfigs[tableId].PageConfig.editUrl,
            data: JsonRowData,
            type: 'post',
            dataType: 'json',
            success: function (data) {
                if (data.Success) {
                    if (_pageConfigs[tableId].PageConfig.editReload) {
                        PageAjax(_pageConfigs[tableId].PageConfig, tableId, false);
                    }
                    else {
                        Divs.each(function (n) {
                            $(Divs).eq(n).find("span").html($(Divs).eq(n).find("input").val()).show();
                            $(Divs).eq(n).find("input").hide();
                        })
                        obj.firstChild.className = "fa fa-pencil";
                        obj.parentNode.removeChild(obj.parentNode.children[1]);
                    }
                }
                else {
                    Divs.each(function (n) {
                        $(Divs).eq(n).find("span").show();
                        $(Divs).eq(n).find("input").hide();
                    })
                    obj.firstChild.className = "fa fa-pencil";
                    obj.parentNode.removeChild(obj.parentNode.children[1]);
                    AlertInfo(data.Message, false);
                }
                if (typeof (_pageConfigs[tableId].PageConfig.ComplateEdit) != "undefined") {
                    _pageConfigs[tableId].PageConfig.ComplateEdit(data.Success);
                }
            }
        })

    }
    else {
        Divs.each(function (n) {
            $(Divs).eq(n).find("input").val($(Divs).eq(n).find("span").html()).show();
            $(Divs).eq(n).find("span").hide();
        })
        obj.firstChild.className = "fa fa-check";
        obj.parentNode.insertAdjacentHTML("beforeEnd", "<a onclick=\"CrearEdit(this," + JsonRowData[_pageConfigs[tableId].PageConfig.pakeId] + ",'" + tableId + "')\"><li style=\"margin-left: 10px;\" class=\"fa fa-eraser\"></li></a>")
        //回调
        if (typeof (_pageConfigs[tableId].PageConfig.openEdit) != "undefined") {
            _pageConfigs[tableId].PageConfig.openEdit(Divs, JsonRowData);
        }
    }
}
function CrearEdit(obj, rowID, tableId) {
    var Div = $("#" + tableId + " div[class = 'EditBox" + rowID);
    obj.parentNode.children[0].firstChild.className = "fa fa-pencil";
    obj.parentNode.removeChild(obj)
    Div.each(function (n) {
        $(Div).eq(n).find("span").show();
        $(Div).eq(n).find("input").hide();
        $(Div).eq(n).find("select").remove();
    });


}

//初始化checkBox
function InitializationCheckBox(tableID) {
    var pageAllCheckbox = $("#" + tableID + " [type=checkbox][id !='CkAll_" + tableID + "']");
    $(pageAllCheckbox).each(function () {
        var index = $.inArray(parseInt($(this)[0].id.split("_")[1]), _result[tableID].selectIds);
        if (index < 0) {
            $(this).prop("checked", false);
        } else {
            $(this).prop("checked", true);
        }
    })

    var IsAllChecked = true;
    $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
        if ($(this).prop("checked") == false) {
            IsAllChecked = false;
        }
    })
    if (IsAllChecked) {
        $("#CkAll_" + tableID).prop("checked", true);
    }
    else {
        $("#CkAll_" + tableID).prop("checked", false);
    }
}

//全选
function CheckAll(tableID) {
    //勾选
    if ($("#CkAll_" + tableID).prop("checked") == true) {
        $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
            if ($(this).attr("disabled") != "disabled") {
                $(this).prop("checked", true);
                var index = $.inArray(parseInt($(this).attr("id").split("_")[1]), _result[tableID].selectIds);
                if (index == -1) {
                    _result[tableID].selectIds.push(parseInt($(this).attr("id").split("_")[1]));
                }
            }
        });
    }
    //取消
    else {
        $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
            if ($(this).attr("disabled") != "disabled") {
                $(this).prop("checked", false);
                var index = $.inArray(parseInt($(this).attr("id").split("_")[1]), _result[tableID].selectIds);
                if (index != -1) {
                    _result[tableID].selectIds.splice(index, 1);
                }
            }
        });
    }

    //回调
    if (typeof (_pageConfigs[tableID].PageConfig.checkAll) != "undefined") {
        _pageConfigs[tableID].PageConfig.checkAll(_result[tableID].selectIds);
    }

}

//子checkBox点击
function CheckChildren(obj, checkType, tableID) {
    if (checkType == "Radio") {
        _result[tableID].selectIds = new Array();
        if (obj.checked) {
            $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
                $(this).prop("checked", false);
            });
            obj.checked = true;
            _result[tableID].selectIds.push(parseInt(obj.id.split("_")[1]));
        }
    }
    else {
        if (obj.checked) {
            var index = $.inArray(parseInt(obj.id.split("_")[1]), _result[tableID].selectIds);
            if (index == -1) {
                _result[tableID].selectIds.push(parseInt(obj.id.split("_")[1]));
            }
        }
        else {
            var index = $.inArray(parseInt(obj.id.split("_")[1]), _result[tableID].selectIds);
            if (index != -1) {
                _result[tableID].selectIds.splice(index, 1);
            }
        }
        var IsAllChecked = true;
        $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
            if ($(this).prop("checked") == false) {
                IsAllChecked = false;
            }
        })
        if (IsAllChecked) {
            $("#CkAll_" + tableID).prop("checked", true);
        }
        else {
            $("#CkAll_" + tableID).prop("checked", false);
        }
    }

    //回掉
    if (typeof (_pageConfigs[tableID].PageConfig.checkChildren) != "undefined") {
        _pageConfigs[tableID].PageConfig.checkChildren(_result[tableID].selectIds);
    }
}

//全局控制checkbox
function CheckGlobal(tableID) {
    //勾选
    if ($("#CkAll_" + tableID).prop("checked") == true) {
        _result[tableID].selectIds = _result[tableID].AllIds;
        $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
            if ($(this).attr("disabled") != "disabled") {
                $(this).prop("checked", true);
            }
        });
    }
    //取消
    else {
        _result[tableID].selectIds = [];
        $("#" + tableID).find("input[type='checkbox'][id !='CkAll_" + tableID + "']").each(function () {
            if ($(this).attr("disabled") != "disabled") {
                $(this).prop("checked", false);
            }
        });
    }
}

//求差集
function chaji_array(arr1, arr2) {
    var arr3 = [];
    for (var i = 0; i < arr1.length; i++) {
        var flag = true;
        for (var j = 0; j < arr2.length; j++) {
            if (arr2[j] == arr1[i]) {
                flag = false;
            }
        }
        if (flag) {
            arr3.push(arr1[i]);
        }
    }
    return arr3;
}







