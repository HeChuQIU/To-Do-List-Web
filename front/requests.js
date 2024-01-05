const base_url = "http://localhost:5144";

const login_url = base_url + "/login";
const register_url = base_url + "/register";
const upload_url = base_url + "/upload";
const download_url = base_url + "/download";

const login_request = (username, password) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: login_url,
            beforeSend: function (request) {
                request.setRequestHeader("Access-Control-Allow-Origin", "*");
            },
            type: "GET",
            data: {
                username: username,
                password: password
            },
            crossDomain: true, // 允许跨域
            success: (res) => resolve(res),
            error: (err) => reject(err)
        })
    });
}

const register_request = (username, password) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: register_url,
            beforeSend: function (request) {
                request.setRequestHeader("Access-Control-Allow-Origin", "*");
            },
            type: "GET",
            data: {
                username: username,
                password: password
            },
            crossDomain: true, // 允许跨域
            success: (res) => resolve(res),
            error: (err) => reject(err)
        })
            .then((res) => {
                console.log(res);
                return new Promise((resolve, reject) => {
                    return res.isSuccess ? resolve(res) : reject(res.errors);
                });
            })
    });
}

const upload_request = (username, password, toDoList) => {
    return new Promise((resolve, reject) => {
            $.ajax({
                url: upload_url,
                beforeSend: function (request) {
                    request.setRequestHeader("Access-Control-Allow-Origin", "*");
                },
                type: "GET",
                data: {
                    username: username,
                    password: password,
                    toDoList: toDoList
                },
                crossDomain: true, // 允许跨域
                success: (res) => resolve(res),
                error: (err) => reject(err)
            })
                .then((res) => {
                    console.log(res);
                    return new Promise((resolve, reject) => {
                        return res.isSuccess ? resolve(res) : reject(res.errors);
                    });
                })
        }
    )
};

const download_request = (username, password) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: download_url,
            beforeSend: function (request) {
                request.setRequestHeader("Access-Control-Allow-Origin", "*");
            },
            type: "GET",
            data: {
                username: username,
                password: password
            },
            crossDomain: true, // 允许跨域
            success: (res) => resolve(res),
            error: (err) => reject(err)
        })
            .then((res) => {
                console.log(res);
                return new Promise((resolve, reject) => {
                    return res.isSuccess ? resolve(res) : reject(res.errors);
                });
            })
    });
}