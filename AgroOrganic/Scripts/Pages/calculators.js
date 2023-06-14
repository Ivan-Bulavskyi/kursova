


var mapApp = angular.module("app");


mapApp.controller("CalculatorsController", /*["$scope", "$http", "$timeout",*/ function ($scope, $http, $timeout, $mdDialog) {

    $scope.init = function () {

    };

    $scope.recommendations = function (e, tabActive) {

        $scope.recomendPopup = {
            SortableOptions: {
                update: function (e, ui) {
                },
                stop: function (e, ui) {
                }
            }
        };


        $scope.whileLoadingRecommendation = false;
        $scope.onloadRecommendationSuccess = false;

        $mdDialog.show({
            controller: RecommendationController,
            templateUrl: 'recommendation.tmpl.html',
            parent: angular.element(document.body),
            targetEvent: e,
            clickOutsideToClose: true,
            fullscreen: $scope.customFullscreen, // Only for -xs, -sm breakpoints.
            params: {
                tabActive: tabActive,//myPlan|culcAllComb
                mapId: $scope.mapId,
                region: $scope.mapLayers.find(function (x) { return x.Name == "Район" }),
                mechanical: $scope.mapLayers.find(function (x) { return x.Name == "Механічний склад" }),
                soil: $scope.mapLayers.find(function (x) { return x.Name == "Грунт" }),
                humusMapLayer: $scope.mapLayers.find(function (x) { return x.Name == "Вміст гумусу у %" }),
                area: $scope.clickedPolygon != null ? (L.GeometryUtil.geodesicArea($scope.clickedPolygon._latlngs[0]) / 10000).toFixed(2) : 50,
                npk: $scope.mapLayers.find(function (x) { return x.Name == "NPK" }),
                useMech: true
            }
        });
    }
    $scope.getNewRecommendations = function (e) {

        $scope.whileLoadingRecommendation = true;
        $scope.onloadRecommendationSuccess = false;
        var request = {
            method: 'GET',
            url: '/api/recommendation/getNew/' + mapId
        }

        $http(request)
            .then(function (recom) {
                $scope.newRecommendation = recom.data;
                $scope.onloadRecommendationSuccess = true;
            },
                function () {
                    alert("Error in mapAng.js::getNewRecommendations(). Invalid id passed!");
                });
        $scope.whileLoadingRecommendation = false;
    }

    function RecommendationController($scope, $mdDialog, $timeout, params) {


        $scope.init = function () {
            $timeout(function () {
                $scope.selectedProducts.push({ product: angular.copy($scope.cultures.siderates) });
                $scope.selectedProductsFert.push({ product: angular.copy($scope.cultures.siderates) });
                $("#recommendation").mCustomScrollbar({
                    theme: "minimal-dark"
                });
            });

            $scope.recom.years = 1;
        };

        $scope.yearsMax = 5;

        $scope.tabSelected = 0;
        switch (params.tabActive) {
            case 'myPlan':
                $scope.tabSelected = 0;
                break;
            case 'culcAllComb':
                $scope.tabSelected = 1;
                break;
            case 'fertilizersNeeds':
                $scope.tabSelected = 2;
                break; 
            default:
                $scope.tabSelected = 0;
                break;
        }
        $scope.tabRedirectTo_CulcAllComb = () => {
            $scope.tabSelected = 1;
            $scope.optimalPlan = [];
            $scope.selectedProducts.map(p => {
                $scope.optimalPlan.push(p.product);
            });
            $scope.yearsMax = $scope.optimalPlan.slice(0, 5).length;
            $timeout(() => {
                $scope.years = $scope.optimalPlan.slice(0, 5).length;
                $scope.yearsAllCombinationsChanged();
            });

        }

        $scope.params = params;
        $scope.LoadSuccess = false;
        $scope.newOptimalProduct = null;
        $scope.recom = {};
        $scope.selectedProducts = [];
        $scope.selectedProductsFert = [];
        $scope.years = 5;
        $scope.maxFertilizerPerHectar = 4;
        $scope.allAddedFertilizer = 20;

        $scope.yearsAllCombinationsChanged = function () {
            if ($scope.allAddedFertilizer > $scope.maxFertilizerPerHectar * $scope.years) {
                $scope.allAddedFertilizer = $scope.maxFertilizerPerHectar * $scope.years;
            }
        }

        $scope.cultures = {
            siderates: {
                name: "Сидерати",
                title: "Siderates",
                imgSrc: "/Content/siderates.jpg",
                productivity: 300,
                maxHarvest: 30,
            },
            soy: {
                name: "Соя",
                title: "Soy",
                imgSrc: "/Content/soy.jpg",
                productivity: 14,
                maxHarvest: 4,
            },
            winterWheatPure: {
                name: "Озима пшениця",
                title: "WinterWheatPure",
                imgSrc: "/Content/winter_wheat.jpg",
                productivity: 50.16,
                maxHarvest: 4,
            },
            winterWheat: {
                name: "Озима пшениця з післяжнивними сидератами",
                title: "WinterWheat",
                imgSrc: "/Content/crop.jpg",
                productivity: 45.6,
                productivityAfter: 300,
                maxHarvest: 4,
            },
            wheatAndSiderates: {
                name: "Жито з післяжнивними сидератами",
                title: "WheatAndSiderates",
                imgSrc: "/Content/crop.jpg",
                productivity: 21,
                productivityAfter: 250,
                maxHarvest: 4,
            },
            oat: {
                name: "Овес",
                title: "Oat",
                imgSrc: "/Content/oat.jpg",
                productivity: 24.4,
                maxHarvest: 4,
            },

            buckwheat: {
                name: "Гречка",
                title: "Buckwheat",
                imgSrc: "/Content/buckwheat.jpg",
                productivity: 10,
                maxHarvest: 4,
            },
            wheat: {
                name: "Жито",
                title: "Wheat",
                imgSrc: "/Content/crop.jpg",
                productivity: 29.7,
                maxHarvest: 4,
            },
            maize: {
                name: "Кукурудза",
                title: "Maize",
                imgSrc: "/Content/maize.jpg",
                productivity: 60.6,
                maxHarvest: 5,
            },
            perennialGrasses: {
                name: "Багаторічні трави (конюшина)",
                title: "PerennialGrasses",
                imgSrc: "/Content/perennial_grasses.jpg",
                productivity: 30,
                maxHarvest: 4,
            }
        };

        $scope.products = [
            $scope.cultures.siderates,
            $scope.cultures.winterWheatPure,
            $scope.cultures.wheat,
            $scope.cultures.wheatAndSiderates,
            $scope.cultures.maize,
            $scope.cultures.perennialGrasses,
            $scope.cultures.soy,
            $scope.cultures.winterWheat,
            $scope.cultures.buckwheat,
            $scope.cultures.oat
        ];

        $scope.fertilizers = {
            biohumus: {
                name: "Біогумус",
                title: "Biohumus",
                max: 4
            },
            ecosoil: {
                name: "Екосойл",
                title: "Ecosoil",
                max: 0.4
            },
            phosphorusFlour: {
                name: "Фосфоритне борошно",
                title: "Phosphorus flour",
                max: 2
            },
            thatch: {
                name: "Солома",
                title: "Thatch",
                max: 4
            },
            siderates: {
                name: "Сидерати",
                title: "Siderates",
                max: 30
            },
            sapropel: {
                name: "Сапропель",
                title: "Sapropel",
                max: 0.4
            }
        };

        $scope.fertilizerTypes = [
            //$scope.fertilizers.biohumus,
            $scope.fertilizers.ecosoil,
            $scope.fertilizers.phosphorusFlour,
            //$scope.fertilizers.thatch,
            //$scope.fertilizers.siderates,
            $scope.fertilizers.sapropel
        ];

        $scope.optimalPlan = [
            angular.copy($scope.cultures.siderates),
            angular.copy($scope.cultures.soy),
            angular.copy($scope.cultures.winterWheatPure),
            angular.copy($scope.cultures.winterWheat),
            angular.copy($scope.cultures.buckwheat),
            angular.copy($scope.cultures.oat),
            angular.copy($scope.cultures.wheat),
            angular.copy($scope.cultures.wheatAndSiderates),
            angular.copy($scope.cultures.maize),
            angular.copy($scope.cultures.perennialGrasses)
        ];

        $scope.addYear = function () {

            $scope.selectedProducts.push({ product: angular.copy($scope.cultures.siderates) });
            $timeout(() => {
                var productSelector = $("#plan-dropdowns [product-id=" + ($scope.selectedProducts.length - 1) + "] [product-selector]");
                productSelector.focus();
                productSelector.triggerHandler('click');
            });
        }
        $scope.deleteYear = function ($index) {
            $scope.selectedProducts.splice($index, 1);
        }

        $scope.addYearFert = function () {

            $scope.selectedProductsFert.push({ product: angular.copy($scope.cultures.siderates) });
            $timeout(() => {
                var productSelector = $("#plan-dropdowns-fert [product-id=" + ($scope.selectedProductsFert.length - 1) + "] [product-selector]");
                productSelector.focus();
                productSelector.triggerHandler('click');
            });
        }
        $scope.deleteYearFert = function ($index) {
            $scope.selectedProductsFert.splice($index, 1);
        }

        $scope.processResult = function (recommendation) {
            var product = $scope.products.find(function (x) {
                return recommendation.Culture == x.title
            });

            recommendation.Income = Number(recommendation.Income * 1000).toLocaleString('en-US');
            recommendation.SummaryProductCost = Number(recommendation.SummaryProductCost * 1000).toLocaleString('en-US');
            recommendation.AdditionalExepencesOnCertification = Number((recommendation.AdditionalExepencesOnCertification * 1000).toFixed(2)).toLocaleString('en-US');
            recommendation.imgSrc = product.imgSrc;
            recommendation.Culture = product.name;
        }

        $scope.processResultFert = function (recommendation) {
            var product = $scope.products.find(function (x) {
                return recommendation.Culture == x.title
            });

            recommendation.imgSrc = product.imgSrc;
            recommendation.Culture = product.name;
        }

        $scope.getNPKBalance = function (selectedProduct) {
            //Get NPK
            var data = [
                {
                    Name: $scope.fertilizers.biohumus.name,
                    Value: selectedProduct.product.fertilizer
                },
                {
                    Name: selectedProduct.product.fertilizerType2?.name,
                    Value: selectedProduct.product.fertilizer2
                },
                {
                    Name: selectedProduct.product.fertilizerType3?.name,
                    Value: selectedProduct.product.fertilizer3
                }
            ];

            var request = {
                method: 'POST',
                url: "/api/npkbalance/get?culture=" + selectedProduct.product.title,
                data: data
            }
            $http(request)
                .then(function (response) {
                    selectedProduct.NPKBalance = response.data;
                });

            var data = [
                {
                    Culture: selectedProduct.product.title,
                    AddedFertilizer: selectedProduct.product.fertilizer,
                    //FertilizerType: x.product.fertilizerType.name,
                    Productivity: selectedProduct.product.productivity,
                    ProductivityAfter: selectedProduct.product.productivityAfter
                }
            ];

            //Get HumusBalance
            var area = params.area;
            var region = params.region;
            var mechanical = params.mechanical.Value;
            var soil = params.soil.Value;
            var useMech = params.useMech;

            var initialHumusContent = 0;
            if (typeof params.humusMapLayer.Value === 'string') {
                var result = params.humusMapLayer.Value.match(/\d[,]*\d*/);
                initialHumusContent = parseFloat(result[0].replace(',', '.'));
            }
            else
                initialHumusContent = params.humusMapLayer.Value;

            if ($scope.mapId > 0)
                region = "Радивилівський";

            var request = {
                method: 'POST',
                url: "/api/recommendation/getHumusBalance?region=" + region + "&initialHumusContent=" + initialHumusContent + "&area=" + area + "&mechanical=" + mechanical + "&soil=" + soil + "&useMech=" + useMech,
                data: data
            }
            $http(request)
                .then(function (response) {
                    selectedProduct.HumusBalance = response.data;
                });
        }

        $scope.showRecommendation = function () {
            var data = $scope.selectedProducts.map(function (x) {
                return {
                    Culture: x.product.title,
                    AddedFertilizer: x.product.fertilizer,
                    //FertilizerType: x.product.fertilizerType.name,
                    Productivity: x.product.productivity,
                    ProductivityAfter: x.product.productivityAfter
                }
            });

            var area = params.area;
            var fieldId = params.mapId;
            var region = params.region;
            var mechanical = params.mechanical.Value;
            var soil = params.soil.Value;
            var useMech = params.useMech;

            var initialHumusContent = 0;
            if (typeof params.humusMapLayer.Value === 'string') {
                var result = params.humusMapLayer.Value.match(/\d[,]*\d*/);
                initialHumusContent = parseFloat(result[0].replace(',', '.'));
            }
            else
                initialHumusContent = params.humusMapLayer.Value;

            if ($scope.mapId > 0)
                region = "Радивилівський";

            //if ($scope.mapId > 0) {
            var request = {
                method: 'POST',
                url: "/api/recommendation/get?region=" + region + "&initialHumusContent=" + initialHumusContent + "&area=" + area + "&mechanical=" + mechanical + "&soil=" + soil + "&useMech=" + useMech,
                data: data
            }

            $http(request)
                .then(function (response) {
                    $scope.recommendations = JSON.parse(response.data);
                    $scope.recommendations.forEach($scope.processResult)
                    $scope.LoadSuccess = true;

                    // scroll to recomendation list
                    $timeout(() => {
                        var $recomendationList = $("#recomendation-list");
                        var $recommendationBody = $("#recommendation .mCustomScrollBox ");
                        $recommendationBody.animate({
                            scrollTop: $recomendationList.offset().top
                        }, "slow");
                    });
                },
                function (response) {
                    console.log(response);

                });
            //}
        };

        $scope.showRecommendationFert = function () {
            var data = $scope.selectedProductsFert.map(function (x) {
                return {
                    Culture: x.product.title,
                    AddedFertilizer: x.product.fertilizer,
                    PlannedHarvest: x.product.harvest
                }
            });

            var request = {
                method: 'POST',
                url: "/api/recommendation/getFertNeeds?" + "&mechanical=" + params.mechanical.Value + "&soil=" + params.soil.Value + "&NPK=" + params.npk.Value,
                data: data
            }

            $http(request)
                .then(function (response) {
                    $scope.recommendationsFert = JSON.parse(response.data);
                    $scope.recommendationsFert.forEach($scope.processResultFert)
                    $scope.LoadSuccessFert = true;

                    // scroll to recomendation list
                    $timeout(() => {
                        var $recomendationList = $("#recomendation-list-fert");
                        var $recommendationBody = $("#recommendation .mCustomScrollBox ");
                        $recommendationBody.animate({
                            scrollTop: $recomendationList.offset().top
                        }, "slow");
                    });
                },
                function (response) {
                    console.log(response);
                });

        };

        $scope.add = function (a, b) {
            return a + b;
        }

        $scope.processTotalIncome = function (allCombinations) {
            allCombinations.forEach(function (item) {
                item.TotalIncome = Number(item.map(function (x) {
                    return x.Income * 1000;
                }).reduce($scope.add, 0)).toLocaleString('en-US');
            });
        }

        $scope.getBestRecommendations = function () {
            var data = $scope.optimalPlan.slice(0, $scope.years).map(function (x) {
                return {
                    Culture: x.title,
                    Productivity: x.productivity,
                    ProductivityAfter: x.productivityAfter,
                }
            });

            var fieldId = params.mapId;
            var region = params.region;
            var area = params.area;
            var mechanical = params.mechanical.Value;
            var soil = params.soil.Value;
            var useMech = params.useMech;

            var initialHumusContent = 0;
            if (typeof params.humusMapLayer.Value === 'string') {
                var result = params.humusMapLayer.Value.match(/\d[,]*\d*/);
                initialHumusContent = parseFloat(result[0].replace(',', '.'));
            }
            else
                initialHumusContent = params.humusMapLayer.Value;

            if ($scope.mapId > 0)
                region = "Радивилівський";

            //if ($scope.mapId > 0) {
            var request = {
                method: 'POST',
                url: "/api/recommendation/getAllAvailableCombinations?region=" + region + "&initialHumusContent=" + initialHumusContent + "&allAddedFertilizer=" + $scope.allAddedFertilizer + "&area=" + area + "&mechanical=" + mechanical + "&soil=" + soil + "&useMech=" + useMech,
                data: data
            }

            $http(request)
                .then(function (response) {
                    $scope.allCombinations = response.data;
                    $scope.processTotalIncome($scope.allCombinations);
                    $scope.AllCombinationsLoadSuccess = true;
                    $scope.fillChart(angular.copy($scope.allCombinations));
                    $scope.addPlotly(angular.copy($scope.allCombinations));
                },
                function (response) {
                    console.log(response);
                });
            //}
        };

        $scope.removeFromCombinations = function (index) {
            $scope.allCombinations.splice(index, 1);
            $scope.processTotalIncome($scope.allCombinations);
            $scope.fillChart(angular.copy($scope.allCombinations));
            $scope.addPlotly(angular.copy($scope.allCombinations));
        }

        $scope.fillChart = function (allCombinations) {
            var dataSets = [];

            allCombinations.forEach(function (recomndations, i, arr) {
                var expectedLevelOfHumus = [
                    params.humusMapLayer.Value,
                    ...recomndations.map(function (x) {
                        return x.ExpectedLevelOfHumus
                    })
                ];
                dataSets.push({
                    label: '',
                    recomndations: recomndations,
                    data: expectedLevelOfHumus,
                    borderColor: '#' + (Math.random() * 0xFFFFFF << 0).toString(16),
                    borderWidth: 1,
                    fill: false,
                    planIndex: i + 1
                });
            });

            var labels = [];
            for (var i = 0; i <= $scope.optimalPlan.length; i++) {
                labels.push(i);
            }

            var options = {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: dataSets
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                reverse: false
                            }
                        }],
                        xAxes: [{
                            ticks: {
                                max: 5 //TODO: automatically
                            }
                        }]
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        callbacks: {
                            label: function (tooltipItem, data) {
                                var addedFertilizer = data.datasets[tooltipItem.datasetIndex].recomndations[tooltipItem.index].AddedFertilizer;
                                var expectedLevelOfHumus = data.datasets[tooltipItem.datasetIndex].recomndations[tooltipItem.index].ExpectedLevelOfHumus;

                                var label = "Внесено добрив: " + addedFertilizer + '\n' + "План #: " + data.datasets[tooltipItem.datasetIndex].planIndex;

                                return label;
                                console.log(tooltipItem);
                                console.log(data);
                            }
                        }
                    }
                }
            }

            if ($scope.myChart) {
                $scope.myChart.destroy();
            }

            var ctx = document.getElementById('myChart').getContext('2d');
            $scope.myChart = new Chart(ctx, options);
        }

        $scope.addPlotly = function (allCombinations) {
            console.log("PlotLy");

            var x_axis = [];
            for (var i = 0; i <= $scope.optimalPlan.length; i++) {
                x_axis.push(i);
            }

            var dataSetsHumus = [];
            var dataSetsIncome = [];
            allCombinations.forEach(function (recomndations, i, arr) {
                var expectedLevelOfHumus = [
                    params.humusMapLayer.Value,
                    ...recomndations.map(function (x) {
                        return x.ExpectedLevelOfHumus
                    })
                ];
                dataSetsHumus.push({
                    x: x_axis,
                    //recomndations: recomndations,
                    y: expectedLevelOfHumus,
                    type: 'scatter'
                });
                var income = [
                    params.humusMapLayer.Value,
                    ...recomndations.map(function (x) {
                        return x.Income
                    })
                ];
                dataSetsIncome.push({
                    x: x_axis,
                    //recomndations: recomndations,
                    y: income,
                    type: 'scatter'
                });
            });

            if ($scope.myDiv1) {
                $scope.myDiv1.destroy();
            }
            if ($scope.myDiv2) {
                $scope.myDiv2.destroy();
            }

            Plotly.newPlot('myDiv1', dataSetsHumus, { title: 'Humus', showlegend: false }, { showSendToCloud: true });

            Plotly.newPlot('myDiv2', dataSetsIncome, { title: 'Income', showlegend: false }, { showSendToCloud: true });
        }


        $scope.addNewOptimal = function () {
            if ($scope.newOptimalProduct != null) {
                $scope.optimalPlan.push($scope.newOptimalProduct);
                $scope.newOptimalProduct = null;
            }
        }

        $scope.deleteOptimalArrayItem = function ($index) {
            $scope.optimalPlan.splice($index, 1);
        }

        $scope.hide = function () {
            $mdDialog.hide();
        };

        $scope.cancel = function () {
            $mdDialog.cancel();
        };

        $scope.init();
    }
});