function exeLogInfoData(num, type) {
    loadLogInfoData(num);
    loadLogInfopage();
}

function loadLogInfopage() {
    var myPageCount = parseInt($("#LogInfoPageCount").val());
    var myPageSize = parseInt($("#LogInfoPageSize").val());
    var countindex = myPageCount % myPageSize > 0 ? (myPageCount / myPageSize) + 1 : (myPageCount / myPageSize);
    $("#LogInfoCountindex").val(countindex);

    $.jqPaginator('#LofInfoListPage', {
        totalPages: parseInt($("#LogInfoCountindex").val()),
        visiblePages: parseInt($("#LogInfoVisiblePages").val()),
        currentPage: 1,
        first: '<li class="first"><a href="javascript:RefreshList();">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:RefreshList();"><i class="arrow arrow2"></i>上一页</a></li>',
        next: '<li class="next"><a href="javascript:RefreshList();">下一页<i class="arrow arrow3"></i></a></li>',
        last: '<li class="last"><a href="javascript:RefreshList();">末页</a></li>',
        page: '<li class="page"><a href="javascript:RefreshList();">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type == "change") {
                exeLogInfoData(num, type);
                $('#LogInfoCurrentPage').val(num);
            }
        }
    });
}

function exeUserIPMapData(num, type) {
    loadUserIPMapData(num);
    loadUserIPMappage();
}

function loadUserIPMappage() {
    var myUserIPPageCount = parseInt($("#UserIPMapPageCount").val());
    var myUserIPPageSize = parseInt($("#UserIPMapPageSize").val());
    var userIPPageCountindex = myUserIPPageCount % myUserIPPageSize > 0 ? (myUserIPPageCount / myUserIPPageSize) + 1 : (myUserIPPageCount / myUserIPPageSize);
    $("#UserIPMapCountindex").val(userIPPageCountindex);

    $.jqPaginator('#UserIPMapListPage', {
        totalPages: parseInt($("#UserIPMapCountindex").val()),
        visiblePages: parseInt($("#UserIPMapVisiblePages").val()),
        currentPage: parseInt($('#UserIPMapCurrentPage').val()),
        first: '<li class="first"><a href="javascript:RefreshUserIPMap();">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:RefreshUserIPMap();"><i class="arrow arrow2"></i>上一页</a></li>',
        next: '<li class="next"><a href="javascript:RefreshUserIPMap();">下一页<i class="arrow arrow3"></i></a></li>',
        last: '<li class="last"><a href="javascript:RefreshUserIPMap();">末页</a></li>',
        page: '<li class="page"><a href="javascript:RefreshUserIPMap();">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type == "change") {
                $('#UserIPMapCurrentPage').val(num);
                exeUserIPMapData(num, type);
            }
        }
    });
}

function exeIPMonitorData(num, type) {
    loadIPMonitorData(num);
    loadIPMonitorPage();
}

function loadIPMonitorPage() {
    var myIPMonitorPageCount = parseInt($("#IPMonitorPageCount").val());
    var myIPMonitorPageSize = parseInt($("#IPMonitorPageSize").val());
    var userIPMonitorPageCountindex = myIPMonitorPageCount % myIPMonitorPageSize > 0 ? (myIPMonitorPageCount / myIPMonitorPageSize) + 1 : (myIPMonitorPageCount / myIPMonitorPageSize);
    $("#IPMonitorCountindex").val(userIPMonitorPageCountindex);

    $.jqPaginator('#IPMonitorListPage', {
        totalPages: parseInt($("#IPMonitorCountindex").val()),
        visiblePages: parseInt($("#IPMonitorVisiblePages").val()),
        currentPage: 1,
        first: '<li class="first"><a href="javascript:RefreshIPMinotorList();">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:RefreshIPMinotorList();"><i class="arrow arrow2"></i>上一页</a></li>',
        next: '<li class="next"><a href="javascript:RefreshIPMinotorList();">下一页<i class="arrow arrow3"></i></a></li>',
        last: '<li class="last"><a href="javascript:RefreshIPMinotorList();">末页</a></li>',
        page: '<li class="page"><a href="javascript:RefreshIPMinotorList();">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type == "change") {
                exeIPMonitorData(num, type);
                $('#IPMonitorCurrentPage').val(num);
            }
        }
    });
}

function exeIPMonitorRecordData(num, type) {
    loadIPMonitorRecordData(num);
    loadIPMonitorRecordPage();
}

function loadIPMonitorRecordPage() {
    var myIPMonitorRecordPageCount = parseInt($("#IPMonitorRecordPageCount").val());
    var myIPMonitorRecordPageSize = parseInt($("#IPMonitorRecordPageSize").val());
    var userIPMonitorPageCountindex = myIPMonitorRecordPageCount % myIPMonitorRecordPageSize > 0 ? (myIPMonitorRecordPageCount / myIPMonitorRecordPageSize) + 1 : (myIPMonitorRecordPageCount / myIPMonitorRecordPageSize);
    $("#IPMonitorRecordCountindex").val(userIPMonitorPageCountindex);

    $.jqPaginator('#IPMonitorRecordPage', {
        totalPages: parseInt($("#IPMonitorRecordCountindex").val()),
        visiblePages: parseInt($("#IPMonitorRecordVisiblePages").val()),
        currentPage: 1,
        first: '<li class="first"><a href="javascript:RefreshIPMinotorRecord();">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:RefreshIPMinotorRecord();"><i class="arrow arrow2"></i>上一页</a></li>',
        next: '<li class="next"><a href="javascript:RefreshIPMinotorRecord();">下一页<i class="arrow arrow3"></i></a></li>',
        last: '<li class="last"><a href="javascript:RefreshIPMinotorRecord();">末页</a></li>',
        page: '<li class="page"><a href="javascript:RefreshIPMinotorRecord();">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type == "change") {
                exeIPMonitorRecordData(num, type);
                $('#IPMonitorRecordCurrentPage').val(num);
            }
        }
    });
}

function exeIPMonitorAlertData(num, type) {
    loadIPMonitorAlertData(num);
    loadIPMonitorAlertPage();
}

function loadIPMonitorAlertPage() {
    var myIPMonitorAlertPageCount = parseInt($("#IPMonitorAlertPageCount").val());
    var myIPMonitorAlertPageSize = parseInt($("#IPMonitorAlertPageSize").val());
    var userIPMonitorAlertCountindex = myIPMonitorAlertPageCount % myIPMonitorAlertPageSize > 0 ? (myIPMonitorAlertPageCount / myIPMonitorAlertPageSize) + 1 : (myIPMonitorAlertPageCount / myIPMonitorAlertPageSize);
    $("#IPMonitorAlertCountindex").val(userIPMonitorAlertCountindex);

    $.jqPaginator('#IPMonitorAlertPage', {
        totalPages: parseInt($("#IPMonitorAlertCountindex").val()),
        visiblePages: parseInt($("#IPMonitorAlertVisiblePages").val()),
        currentPage: 1,
        first: '<li class="first"><a href="javascript:RefreshIPMinotorAlert();">首页</a></li>',
        prev: '<li class="prev"><a href="javascript:RefreshIPMinotorAlert();"><i class="arrow arrow2"></i>上一页</a></li>',
        next: '<li class="next"><a href="javascript:RefreshIPMinotorAlert();">下一页<i class="arrow arrow3"></i></a></li>',
        last: '<li class="last"><a href="javascript:RefreshIPMinotorAlert();">末页</a></li>',
        page: '<li class="page"><a href="javascript:RefreshIPMinotorAlert();">{{page}}</a></li>',
        onPageChange: function (num, type) {
            if (type == "change") {
                exeIPMonitorAlertData(num, type);
                $('#IPMonitorAlertCurrentPage').val(num);
            }
        }
    });
}