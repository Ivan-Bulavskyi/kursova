var app = angular.module("app", ["core", "ngRoute", "ngSanitize", "ui.tinymce", "ngAria", "ngMessages", "ngMaterial", 'ui.sortable']);

app.config(['$qProvider', function ($qProvider) {
    $qProvider.errorOnUnhandledRejections(false);
}]);