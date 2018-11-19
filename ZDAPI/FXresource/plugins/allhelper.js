//控制本地存储参数
$("#contentdiv").before(localStorage.getItem('headerhtml'));    
$("#contentdiv").after(localStorage.getItem('bottomhtml')); 
$(".main-header").after(localStorage.getItem('changepwd'));
$("#dhdiv").html(localStorage.getItem('dhcontent')); 
$("#loginUser").html(localStorage.getItem('loginUser'));
 
if (localStorage.getItem('loginUser') != $.cookie('loginUser')  ) {
    window.location.href = "/login.html";
}
if (!$.cookie('loginUser'))
{
    window.location.href = "/login.html";
}
$(".sidebar li a").each(function (index) {
    if ($(this).text() == $("#showaction").text()) {
        $("title").html("工作平台 - " + $(this).text());
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
        url: "/api/Account/UserEditPwd",
        dataType: 'json',
        data: { CODE: "0518", SCHEMA: $("#oldpwd").val(), PASSWORD: $("#newpwd").val() },
        success: function (data) {
            if (data.num == 0) {
                $("#changePWD").modal('hide'); 
                alert("修改成功");
            }
            else { 

                alert(data.remark);
            }
        }
    }); 

});