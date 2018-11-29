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