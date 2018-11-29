//id 获得数据装配的位置 
//cPage 当前页数 
//allPage 总页数
//fn 回调函数，传递当前点击页码 
function ShowPage(id, cPage, allPage, fn) {
    var element = $(id);
    //初始化所需数据
    var options = {
        bootstrapMajorVersion: 3,//版本号。3代表的是第三版本
        currentPage: cPage, //当前页数
        numberOfPages: 6, //显示页码数标个数
        totalPages: allPage, //总共的数据所需要的总页数
        itemTexts: function (type, page, current) {
            //图标的更改显示可以在这里修改。
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "尾页";
                case "page":
                    return page;
            }
        },
        tooltipTitles: function (type, page, current) {
            //如果想要去掉页码数字上面的预览功能，则在此操作。例如：可以直接return。
            switch (type) {
                case "first":
                    return "首页";
                case "prev":
                    return "上一页";
                case "next":
                    return "下一页";
                case "last":
                    return "尾页";
                case "page":
                    return (page === current) ? "当前第 " + page + "页" : "转到第 " + page + "页";
            }
        },
        onPageClicked: function (e, originalEvent, type, page) {
            fn(page);
        }
    };
    element.bootstrapPaginator(options);
}

