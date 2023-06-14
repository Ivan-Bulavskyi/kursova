var app = angular.module('app');

app.service("PostService", function ($http) {

    this.PostId = null;
    this.PostData = null;

    this.PostEdit = function (post) {
        var response = $http({
            method: "POST",
            url: "/admin/EditPost",
            data: JSON.stringify(post),
            dataType: "json"
        });
        return response;
    }

    this.MoveUp = function(post) {
        var response = $http({
            method: "POST",
            url: "/admin/MoveUp",
            data: JSON.stringify(post),
            dataType: "json"
        });
        return response;
    }

    this.MoveDown = function (post) {
        var response = $http({
            method: "POST",
            url: "/admin/MoveDown",
            data: JSON.stringify(post),
            dataType: "json"
        });
        return response;
    }



    this.DeletePost = function (id) {
        var del = {"postId": id};
        var response = $http({
            method: "POST",
            url: "/admin/DeletePost",
            data: JSON.stringify(del),
            dataType: "json"
        });
        return response;
    }

    this.PostCreate = function (post) {
        console.log("create");
        var response = $http({
            method: "post",
            url: "Posts",
            data: JSON.stringify(post),
            dataType: "json"
        });
        return response;
    }

    this.GetPostById = function (id) {
        this.PostId = id;
        return $http({
            url: "/admin/GetPost",
            method: "get",
            params: {
                id: id
            }
        });
    }

    this.GetTechnologyPostById = function (id) {
        return $http({
            url: "/Technology/GetPost",
            method: "get",
            params: {
                id: id
            }
        });
    }

    this.GetProductPostById = function (id) {
        return $http({
            url: "/Product/GetPost",
            method: "get",
            params: {
                id: id
            }
        });
    }

    this.GetCertificationPostById = function (id) {
        return $http({
            url: "/Certification/GetPost",
            method: "get",
            params: {
                id: id
            }
        });
    }

    this.GetAllPosts = function() {
        return $http.get("/admin/getposts");
    }

    this.GetProductPosts = function () {
        return $http.get("/Product/GetPosts");
    }

    this.GetTechnologyPosts = function () {
        return $http.get("/Technology/GetPosts");
    }

    this.GetCertificationPosts = function () {
        return $http.get("/Certification/GetPosts");
    }

});