//控制本地存储参数
$("#contentdiv").before(localStorage.getItem('headerhtmlYC'));
$("#contentdiv").after(localStorage.getItem('bottomhtmlYC'));
$(".main-header").after(localStorage.getItem('changepwdYC'));
$("#dhdiv").html(localStorage.getItem('dhcontentYC'));
$("#loginUser").html(localStorage.getItem('loginUserYC'));
console.log($.cookie('loginUserYC'));
if (!$.cookie('loginUserYC'))  
{
    window.location.href = "/index.html";
}
$(".sidebar li a").each(function (index) {
    if ($(this).text() == $("#showaction").text()) {
        $("title").html("饲料销量分析 - " + $(this).text());
        $(this).parent().attr("class", "active");
        $(this).parents("li").addClass("active");
        return;
    }
}) 

$("#oldpwd").blur(function () {
    if (!$(this).val()) {
        $(this).css("border-color", "#db4c4a");
    }
});
$("#oldpwd").focus(function () {
    $(this).css("border-color", "");
});

$("#newpwd").blur(function () {
    if (!$(this).val()) {
        $(this).css("border-color", "#db4c4a");
    }
});
$("#newpwd").focus(function () {
    $(this).css("border-color", "");
    $("#newpwdc").css("border-color", "");
});
$("#newpwdc").focus(function () {
    $(this).css("border-color", "");
    $("#newpwd").css("border-color", "");
});

$("#newpwdc").blur(function () {
    if (!$(this).val()) {
        $(this).css("border-color", "#db4c4a");
    }
});
$("#btnChangePwd").click(function () {
    $.ajax({
        type: "Post",
        url: "/api/Account/UserEditPwdYC",
        dataType: 'json',
        data: { CODE: $.cookie('loginUserYC'), NAME: $("#oldpwd").val(), PASSWORD: $("#newpwd").val() },
        success: function (data) {
            if (data.num == 0) {
                $("#changePWD").modal('hide');
                alert("修改成功");
                $("#oldpwd").val("");
                $("#newpwd").val("");
                $("#newpwdc").val("");
            }
            else {  
                alert(data.remark);
            }
        }
    }); 

});

//检验输入框
function clearNoFloat(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符  
    obj.value = obj.value.replace(/^\./g, "");  //验证第一个字符是数字而不是. 
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的. 
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数 
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", "."); 
}

//检验输入框
function clearNoNum(obj) {
    obj.value = obj.value.replace(/[^\d]/g, "");  //清除“数字”和“.”以外的字符  
    obj.value = obj.value.replace(/^\./g, "");  //验证第一个字符是数字而不是.   

    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
}