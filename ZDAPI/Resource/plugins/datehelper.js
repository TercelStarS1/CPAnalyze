///获得当前日期
function GetNowDate() {
    var myDate = new Date(); 
    var year = myDate.getFullYear();    //获取完整的年份(4位,1970-????)
    var month = myDate.getMonth()+1;       //获取当前月份(0-11,0代表1月)
    var day = myDate.getDate();        //获取当前日(1-31) 
    
    
    return year + "-" + month + "-" + day;
}

///日期提前几个月
function GetDateByMonth(date, num) { 
    var dt = new Date(date); 
    var mydate = dt.setMonth(dt.getMonth() - num); 
    return timestampToDate(mydate);
}

//获取日期 显示年月格式
function GetDateYM(now) {
    var d = new Date(now);
    var year = d.getFullYear();
    var month = d.getMonth() + 1; 
    return year + "-" + month;
}
///
function formatDate(now) {
    var d = new Date(now);
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var date = d.getDate();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var second = d.getSeconds();
    return year + "年" + month + "月" + date + "日";
}

function formatDate1(now) {
    var d = new Date(now);
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var date = d.getDate();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var second = d.getSeconds();
    return month + "-" + date ;
}

function formatDate2(now) {
    var d = new Date(now);
    var year = d.getFullYear();
    var month = d.getMonth() + 1;
    var date = d.getDate();
    var hour = d.getHours();
    var minute = d.getMinutes();
    var second = d.getSeconds();
    return year +"-" +month + "-" + date;
}

 
//将时间戳转换成日期格式
function timestampToDate(timestamp) {
    var date = new Date(timestamp);//时间戳为10位需*1000，时间戳为13位的话不需乘1000
    var Y = date.getFullYear() + '-';
    var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    var D = date.getDate();
    return Y + M + D;
}
//将时间戳转换成日期时间格式
function timestampToTime(timestamp) {
    var date = new Date(timestamp * 1000);//时间戳为10位需*1000，时间戳为13位的话不需乘1000
    var Y = date.getFullYear() + '-';
    var M = (date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1) + '-';
    var D = date.getDate() + ' ';
    var h = date.getHours() + ':';
    var m = date.getMinutes() + ':';
    var s = date.getSeconds();
    return Y + M + D + h + m + s;
}

//将日期格式转换成时间戳
function dateToTimesTAMP(date) {
    var date = new Date(date);
    var time13 = Date.parse(date);
    return time13;
}

//将时间日期格式转换成时间戳
function dateToTimesTAMP(date) {
    var date = new Date(date); 
    var time10 = date.getTime();
    return time10;
}

//计算日期相减天数 
function DateMinus(d1,d2) {
    var sdate = new Date(d1);
    var now = new Date(d2);
    var days = now.getTime() - sdate.getTime();
    var day = parseInt(days / (1000 * 60 * 60 * 24));
    return day;
}

//根据月份返回月份数组
function GetMonthArray(d1, d2) {
    var result = new Array();
    var sd1 = new Date(d1);
    var sd2 = new Date(d2);

    var y1 = sd1.getFullYear();
    var y2 = sd2.getFullYear();
    var m1 = sd1.getMonth() + 1;
    var m2 = sd2.getMonth() + 1;
    if (y1 == y2) {
        for (var i = m1; i <= m2; i++) {
            result.push(y1 + "-" + i);
        }
    }
    if (y1 < y2) {
        for (var i = y1; i <= y2; i++) {
            console.log(i, y2, m1, m2);
            if (i == y2) {
                for (var j = 1; j <= m2; j++) {
                    result.push(i + "-" + j);
                }
            }
            else if (i == y1) {
                for (var j = m1; j <= 12; j++) {
                    result.push(i + "-" + j);
                }
            }
            else {
                for (var j = 1; j <= 12; j++) {
                    result.push(i + "-" + j);
                }
            }
        }
    }
    return result;

}