var app = angular.module('app');

app.controller('TechnologyController', function ($scope, $sce, PostService) {
    "use strict";
    $scope.renderHtml = function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };


    $scope.getPosts = function () {
        var getData = PostService.GetTechnologyPosts();
        getData.then(function (pst) {
            $scope.posts = pst.data;
            $scope.getPost($scope.posts[0].Id);
        }, function () {
            alert("Error occured!");
        });
    }

    $scope.getPost = function (id) {
        var getData = PostService.GetTechnologyPostById(id);

        getData.then(function (pst) {
            $scope.post = pst.data;
        }, function () {
            alert("Error occured!");
        });
    }



});



