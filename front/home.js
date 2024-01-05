const thingTextInput = $("#thing-text");
let things = ["Learn HTML", "Learn CSS", "Learn JS"];

function addThingWithText(text) {
    $("#thing-list").append(
        `<div>
            <span>${text}</span>
            <button class="btn-remove">X</button>
        </div>`
    );
}

function updateList() {
    $("#thing-list").empty();
    things.forEach(thing => addThingWithText(thing));
    $(".btn-remove").on("click", (e) => {
        const index = $(e.target).parent().index();
        things.splice(index, 1);
        updateList();
    });
}

function addThingToList() {
    const text = thingTextInput.val();
    if (text) {
        things.push(text);
        updateList();
    }
    thingTextInput.val("");
}

$("#btn-add").click(() => addThingToList());

$("#btn-logout").click(() => {
    localStorage.removeItem("username");
    localStorage.removeItem("password");
    window.location.href = "./index.html";
});

function upload() {
    return () => {
        const list = JSON.stringify(things);
        const username = localStorage.getItem("username");
        const password = localStorage.getItem("password");
        upload_request(username, password, list)
            .then(_ => alert("上传成功"))
            .catch(reason => alert(JSON.stringify(reason)));
    };
}

$("#btn-upload").click(upload());

function download() {
    return () => {
        const username = localStorage.getItem("username");
        const password = localStorage.getItem("password");
        download_request(username, password)
            .then(res => {
                if (!res.isSuccess) {
                    alert(JSON.stringify(res.errors));
                    return;
                }
                things = JSON.parse(res.resource);
                updateList();
            })
    };
}

$("#btn-download").click(download());

thingTextInput.keydown((e) => {
    const code = e.keyCode || e.charCode;
    if (code === 13) {
        addThingToList();
    }
})

const username = localStorage.getItem("username");
const password = localStorage.getItem("password");
login_request(username, password)
    .then(res => {
        if (!res.isSuccess) {
            window.location.href = "./index.html";
            return;
        }
        console.log(`用户名：${username}`);
        $("#user-name")[0].outerText = username;
        download();
    })

updateList();
