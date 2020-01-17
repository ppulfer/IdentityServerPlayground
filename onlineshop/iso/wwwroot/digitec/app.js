/// <reference path="oidc-client.js" />

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);

var config = {
    authority: "http://identityserver:8888",
    client_id: "digitec",
    redirect_uri: "http://digitec.local/callback.html",
    response_type: "id_token token",
    scope:"openid profile api1",
    post_logout_redirect_uri : "http://digitec.local/",
    silent_redirect_uri: "http://digitec.local/silent.html",
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })
};
var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        renewToken();
        log("User not logged in");
    }
});

function renewToken() {
    mgr.signinSilent()
        .then(function () {
            log("silent renew success");            
        }).catch(function (err) {
            log("silent renew error", err);
        });
}

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "http://api:8020/identity";

        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
            log(xhr.status, JSON.parse(xhr.responseText));
        }
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}