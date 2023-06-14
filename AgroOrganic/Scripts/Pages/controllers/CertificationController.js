var app = angular.module('app');

app.controller('CertificationController', function ($scope, $sce, PostService) {

    $scope.renderHtml = function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };

    $scope.getPosts = function () {
        var getData = PostService.GetCertificationPosts();
        getData.then(function (pst) {
            $scope.posts = pst.data;
            $scope.getPost($scope.posts[0].Id);
        }, function () {
            alert("Error occured!");
        });
    }

    $scope.getPost = function (id) {
        var getData = PostService.GetCertificationPostById(id);
        getData.then(function (pst) {
            $scope.post = pst.data;
        }, function () {
            alert("Error occured!");
        });
    }

});
