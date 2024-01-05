$("#register-btn").click(() =>
    $("#password").val() === $("#confirm-password").val() ? Promise.reject("两次输入的密码不一致") :
        register_request($("#username").val(), $("#password").val())
            .then((res) => {
                if (!res.isSuccess)
                    return Promise.reject(res.errors);
                alert("注册成功");
                window.location.href = "./index.html";
            })
            .catch((err) => {
                alert(JSON.stringify(err));
            })
);
