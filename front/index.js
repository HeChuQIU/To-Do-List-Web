const isLogin = (res) => {
    return res === 'login success';
}

const username_input = $("#username");
const password_input = $("#password");

$("#btn-login").click(() =>
    localStorage.setItem("username", username_input.val()) ||
    localStorage.setItem("password", password_input.val()) ||
    login_request(username_input.val(), password_input.val())
        .then((res) => {
                if (res.isSuccess)
                    window.location.href = "./home.html";
                else
                    alert(res.errors);
            })
);
