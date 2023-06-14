var app = angular.module('app');

app.controller("PostsController", function ($scope, $sce, $mdDialog, PostService) {

    $scope.orderByField = "RoleId";
    $scope.reverseSort = false;

    $scope.categories = [{ name: "Технології", id: 1 },
                         { name: "Сертифікація", id: 2 },
                         { name: "Продукція", id: 3 }];

    $scope.renderHtml = function(htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    };

    $scope.LoadPosts = function () {
        var getData = PostService.GetAllPosts();
        getData.then(function(pst) {
            $scope.posts = pst.data;
        }, function() {
            alert("Error occured!");
        });
    }


    $scope.LoadPost = function(id) {
        var getData = PostService.GetPostById(id);
        console.log(id);
        getData.then(function(pst) {
            $scope.post = pst.data;
        }, function() {
            alert("Error occured!");
        });
    }

    $scope.MoveUp = function(post) {
        PostService.MoveUp(post)
            .then(function() {
                location.reload();
            });
    }

    $scope.MoveDown = function (post) {
        PostService.MoveDown(post)
            .then(function() {
                location.reload();
            });
    }

    $scope.CreatePost = function (MyPost, post) {
        if (MyPost.$valid && post.CategoryId) {
            PostService.PostCreate(post);
            location.reload();
        }
    }


    $scope.EditPost = function(MyPost, Post) {
        if (MyPost.$valid && Post.CategoryId) {
            PostService.PostEdit(Post);
            location.reload();
        }

    }

    $scope.DeletePost = function (id) {
        if (confirm("Видалити цей пост?")) {
            PostService.DeletePost(id);
            location.reload();
        }
    }

    $scope.tinymceOptions = {
        //plugins: 'link image code',
        //toolbar: 'undo redo | bold italic | alignleft aligncenter alignright | code'
        language: "uk",
        height: 500,
        theme: 'modern',
        plugins: [
          'advlist autolink lists link image charmap print preview hr anchor pagebreak',
          'searchreplace wordcount visualblocks visualchars code fullscreen',
          'insertdatetime media nonbreaking save table contextmenu directionality',
          'emoticons template paste textcolor colorpicker textpattern imagetools codesample toc'
        ],
        toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
        toolbar2: 'print preview media | forecolor backcolor emoticons | codesample',
        image_advtab: true,
        //templates: [
        //  { title: 'Test template 1', content: 'Test 1' },
        //  { title: 'Test template 2', content: 'Test 2' }
        //],
        content_css: [
          '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
          '//www.tinymce.com/css/codepen.min.css'
        ],
        file_browser_callback: function (field_name, url, type, win) {
            var w = window,
            d = document,
            e = d.documentElement,
            g = d.getElementsByTagName('body')[0],
            x = w.innerWidth || e.clientWidth || g.clientWidth,
            y = w.innerHeight || e.clientHeight || g.clientHeight;

            var cmsURL = '/scripts/libs/filemanager/index.html?&field_name=' + field_name;

            if (type == 'image') {
                cmsURL = cmsURL + "&type=images";
            }

            tinyMCE.activeEditor.windowManager.open({
                file: cmsURL,
                title: 'AgroOrganic',
                width: x * 0.8,
                height: y * 0.8,
                resizable: "yes",
                close_previous: "no"
            });

        }
    };

    $scope.status = '  ';
    $scope.customFullscreen = false;

    $scope.showAdvanced = function (ev, post) {
        $mdDialog.show({
            controller: DialogController,
            templateUrl: 'dialog1.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
            locals: {
                post: post
            }
        });
    };

    function DialogController($scope, $mdDialog, post) {
        $scope.post = post;
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
    }




});