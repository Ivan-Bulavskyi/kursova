var app = angular.module("app");

app.controller('FooterController', function ($scope, $sce, $mdDialog, UserService) {


    $scope.contacts = function(ev) {
        $mdDialog.show({
            controller: ContactController,
            templateUrl: 'contacts.tmpl.html',
            arent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
        });
    };

    $scope.about = function (ev) {
        $mdDialog.show({
            controller: AboutController,
            templateUrl: 'about.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen // Only for -xs, -sm breakpoints.
        });
    };

    function AboutController($scope, $mdDialog) {
        $scope.subscribeForm = true;

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.subscribe = function () {

            if ($scope.Email) {
                $scope.subscribeForm = false;
                console.log($scope.Email);
                var result = UserService.Subscribe($scope.Email);
                result.then(function(res) {
                    if (res.data == "success") {
                        $scope.subscribeSuccess = true;
                    }
                    else {
                        $scope.subscribeForm = true;
                        $scope.subscribeFail = true;
                    }
                });
            }
        }

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
    }

    function ContactController($scope, $mdDialog) {
        $scope.feedForm = true;
        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.sendFeedback = function () {

            if ($scope.feedbackForm.username && $scope.feedbackForm.email && $scope.feedbackForm.Text) {
                $scope.feedForm = false;
                var req = {
                    Id: 0,
                    Email: $scope.feedbackForm.email,
                    Name: $scope.feedbackForm.username,
                    Text: $scope.feedbackForm.Text
                };
                //$scope.subscribeForm = false;
                //console.log(feedback);
                var result = UserService.Feedback(req);
                result.then(function (res) {
                    if (res.data === "success") {
                        $scope.feedSucess = true;
                    }
                    else {
                        $scope.feedFail = true;
                    }
                });
            }
        }

        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
    }

});