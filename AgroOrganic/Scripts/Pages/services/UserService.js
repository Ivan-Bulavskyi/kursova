var app = angular.module('app');

app.service("UserService", function ($http) {

    this.Subscribe = function (email) {
        var request = { Email: email, Id: 0};
        var response = $http({
            method: "POST",
            url: "/api/user/subscribe",
            data: JSON.stringify(request),
            dataType: "json"
        });
        return response;
    }

    this.Feedback = function (feedback) {
        console.log(feedback);
        var response = $http({
            method: "POST",
            url: "/api/user/Feedback",
            data: JSON.stringify(feedback),
            dataType: "json"
        });
        return response;
    }

});