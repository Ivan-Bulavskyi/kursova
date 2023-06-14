var app = angular.module('app');

app.controller('AdvicesController', function ($scope, $http) {
    "use strict";

    var chemElem = [];

    $scope.init = function () {
        getOptimalDataArr();
    };

    $scope.showAdvices = function () {

        $scope.whileLoadingRecommendation = false;
        $scope.onloadRecommendationSuccess = false;

        $(".description").show();
        $scope.showPlantsToGrow();
        $scope.showSymptoms();
        $(".recomendation-info-rate span").text(new Date().getDate() + "/" + ((new Date().getMonth() < 10) ? "0" + new Date().getMonth() : new Date().getMonth()) + "/" + new Date().getFullYear());
    };

    $scope.showSymptoms = function () {
        $http.get("/Agro/GetSymptoms").then(function (data) {
            $(".chem-elem-desc > *").remove();
            var symptomps = data.data.symptoms;
            var tableAnalyseTrs = $("#analyse > table > tbody > tr");
            var symptompsToShow = [];

            for (var i = 0; i < tableAnalyseTrs.length; i++) {
                var nameOfComponent = $($(tableAnalyseTrs[i]).children("td")[0]).text();
                var valueOfComponent = $($(tableAnalyseTrs[i]).children("td")[1]).text();
                var optimalData = getOptimalDataAboutElemntByName(nameOfComponent);
                var symptompsToShow = symptomps.filter(function (item) {
                    return item.ElementName == nameOfComponent
                });
                console.log();


                if (!isInOptimalRange(optimalData, valueOfComponent)) {
                    var sympt = symptomps.filter(function (item) { return item.ElementName == nameOfComponent });
                    if (sympt.length) {
                        if (valueOfComponent > optimalData.optimalRange.e) {
                            $(".chem-elem-desc").append("<div class='panel panel-danger'><div class='panel-heading'>" + nameOfComponent + ". Вміст - " + valueOfComponent + " Оптимальні показники [" + optimalData.optimalRange.b + "-" + optimalData.optimalRange.e + "] </div><div class='panel-body'>" + sympt[0].Description + "</div></div>");
                        } else {
                            $(".chem-elem-desc").append("<div class='panel panel-warning'><div class='panel-heading'>" + nameOfComponent + ". Вміст - " + valueOfComponent + " Оптимальні показники [" + optimalData.optimalRange.b + "-" + optimalData.optimalRange.e + "] </div><div class='panel-body'>" + sympt[1].Description + "</div></div>");
                        }
                    }
                }
            }
        },
            function () {
                alert("Some erorr occurs")
            });
    }

    $scope.showPlantsToGrow = function () {
        $http.get("/Agro/GetAdvices").then(function (data) {
            $(".grow-cultures-list.should > *").remove();
            var advices = data.data.advices;
            var tableAnalyseTrs = $("#analyse > table > tbody > tr");
            var plantsToDisplay = [];
            var symptompsToShow = [];

            for (var i = 0; i < tableAnalyseTrs.length; i++) {
                var nameOfComponent = $($(tableAnalyseTrs[i]).children("td")[0]).text();
                var valueOfComponent = $($(tableAnalyseTrs[i]).children("td")[1]).text();
                var optimalData = getOptimalDataAboutElemntByName(nameOfComponent);

                if (isInOptimalRange(optimalData, valueOfComponent)) {
                    var plantsList = advices.filter(function (elem) {
                        return elem.ElementName == nameOfComponent
                    });

                    for (var j = 0; j < plantsList.length; j++) {

                        if (!containsObject(plantsList[j], plantsToDisplay)) {
                            plantsToDisplay.push(plantsList[j]);
                        }
                    }
                }
            }

            for (var j = 0; j < plantsToDisplay.length; j++) {
                $(".grow-cultures-list.should").append("<div class='grow-cultures-list-item'><div class='list-item-img'><img src='" + plantsToDisplay[j].ImageName + "'/></div><div class='list-item-title text-center'>" + plantsToDisplay[j].PlantName + "</div></div>");
            }
        },
            function () {
                alert("Some erorr occurs")
            });
    }


    function getOptimalDataAboutElemntByName(name) {
        var optimalDataArr = getOptimalDataArr();

        for (var i = 0; i < optimalDataArr.length; i++) {
            if (optimalDataArr[i].nameElem == name) {
                return optimalDataArr[i];
            }
        }
        return {};
    }

    //function getOptimalDataArr() {
    //    $http.get("/Agro/GetRanges").then(function (data) {
    //        console.log(data.data.chemElemRanges);
    //        chemElem  = data.data.chemElemRanges;
    //    }, function () { alert('Some errorsOccurs') });
    //}

    function getOptimalDataArr() {
        return [{
            nameElem: "Азот (N)",
            badRanges: [{ b: 0, e: 100 }],
            averageRanges: [{ b: 101, e: 199 }],
            optimalRange: { b: 200, e: 400 },
        }, {
            nameElem: "pH грунту",
            badRanges: [{ b: 0, e: 4.1 }, { b: 8.5, e: 18 }],
            averageRanges: [{ b: 4.2, e: 5.5 }, { b: 7.1, e: 8.4 }],
            optimalRange: { b: 5.6, e: 7.0 },
        }, {
            nameElem: "Фосфор (P)",
            badRanges: [{ b: 0, e: 41 }],
            averageRanges: [{ b: 42, e: 249 }],
            optimalRange: { b: 250, e: 500 },
        },
        {
            nameElem: "Калій (К)",
            badRanges: [{ b: 0, e: 41 }],
            averageRanges: [{ b: 42, e: 249 }],
            optimalRange: { b: 250, e: 500 },
        },
        {
            nameElem: "Марганець (Mn)",
            badRanges: [{ b: 0, e: 5 }],
            averageRanges: [{ b: 5.1, e: 10 }],
            optimalRange: { b: 10, e: 20 },
        },
        {
            nameElem: "Мідь (Cu)",
            badRanges: [{ b: 3.1, e: 10 }],
            averageRanges: [{ b: 1.82, e: 3 }],
            optimalRange: { b: 0, e: 1.81 },
        },
        {
            nameElem: "Цинк (Zn)",
            badRanges: [{ b: 23.1, e: 50 }],
            averageRanges: [{ b: 12.1, e: 23 }],
            optimalRange: { b: 0, e: 12 },
        },
        {
            nameElem: "Бор (B)",
            badRanges: [{ b: 0, e: 0.15 }],
            averageRanges: [{ b: 0.16, e: 0.32 }],
            optimalRange: { b: 0.33, e: /*0.66*/1 },
        },
        {
            nameElem: "Кобальт (Co)",
            badRanges: [{ b: 0, e: 0.07 }, { b: 11, e: 22 }],
            averageRanges: [{ b: 0.08, e: 0.15 }, { b: 5.1, e: 10.0 }],
            optimalRange: { b: 0.16, e: 5.0 },
        },
        {
            nameElem: "Кадмій (Cd)",
            badRanges: [{ b: 0.71, e: 1.5 }],
            averageRanges: [{ b: 0.35, e: 0.70 }],
            optimalRange: { b: 0, e: 0.34 },
        },
        {
            nameElem: "Свинець (Pb)",
            badRanges: [{ b: 6.1, e: 14 }],
            averageRanges: [{ b: 2.86, e: 6 }],
            optimalRange: { b: 0, e: 2.85 },
        },
        {
            nameElem: "Ртуть (Hg)",
            badRanges: [{ b: 2.1, e: 5 }],
            averageRanges: [{ b: 0.91, e: 2 }],
            optimalRange: { b: 0, e: 0.9 },
        },
        {
            nameElem: "Цезій (Cs)",
            badRanges: [{ b: 556, e: 1000 }],
            averageRanges: [{ b: 38, e: 555 }],
            optimalRange: { b: 0, e: 37 },
        },
        {
            nameElem: "Стронцій (Sr)",
            badRanges: [{ b: 111, e: 222 }],
            averageRanges: [{ b: 0.38, e: 110 }],
            optimalRange: { b: 0, e: 0.37 },
        },
        {
            nameElem: "Гумус",
            badRanges: [{ b: 0, e: 1.1 }, { b: 11, e: 22 }],
            averageRanges: [{ b: 1.2, e: 4 }, { b: 5.1, e: 10 }],
            optimalRange: { b: 4.1, e: 5 },
        },
        {
            nameElem: "Сірка (S)",
            badRanges: [{ b: 0, e: 3 }, { b: 25, e: 50 }],
            averageRanges: [{ b: 3.1, e: 8 }, { b: 16, e: 25 }],
            optimalRange: { b: 8.1, e: 15 },
        },
        {
            nameElem: "Агрохімічна оцінка",
            badRanges: [{ b: 0, e: 33 }],
            averageRanges: [{ b: 34, e: 66 }],
            optimalRange: { b: 67, e: 100 },
        },
        {
            nameElem: "Еколого-агрохімічна оцінка",
            badRanges: [{ b: 0, e: 33 }],
            averageRanges: [{ b: 34, e: 66 }],
            optimalRange: { b: 67, e: 100 },
        }
        ];
    }

    function isInOptimalRange(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.optimalRange.b <= valueOfComponent && optValForCurrentElem.optimalRange.e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function containsObject(obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].PlantName == obj.PlantName) {
                return true;
            }
        }

        return false;
    }

    $scope.init();
});



