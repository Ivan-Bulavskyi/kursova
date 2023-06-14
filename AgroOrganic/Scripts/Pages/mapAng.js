var mapApp = angular.module("app");
mapApp.config(function ($mdDateLocaleProvider) {
    moment.locale("uk");
    $mdDateLocaleProvider.months = moment.months();
    $mdDateLocaleProvider.shortMonths = moment.monthsShort();
    $mdDateLocaleProvider.days = moment.weekdays();
    $mdDateLocaleProvider.shortDays = moment.weekdaysShort();

    $mdDateLocaleProvider.formatDate = function (date) {
        var date = moment(date, 'DD/MM/YYYY');
        return date.format('DD/MM/YYYY');
    };

    $mdDateLocaleProvider.parseDate = function (dateString) {
        var m = moment(dateString, 'DD/MM/YYYY', true);
        return m.isValid() ? m.toDate() : new Date(null);
    };
}
)
//mapApp.controller("MapController", function() {
//    angular.element(document).ready(function () {
//        map = L.map('mapid', { editable: true }).setView([30, -87], 10);
//        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoicmF2ZW5ub2QiLCJhIjoiY2l1Yjl1cmdsMDAwNDJwbGxjbTU4NjUxOSJ9.2RtOfwYTMgGU6tUreAIlFA', {
//            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
//            maxZoom: 18,
//            id: 'ravennod.21355hjc',
//            accessToken: 'pk.eyJ1IjoicmF2ZW5ub2QiLCJhIjoiY2l1Yjl1cmdsMDAwNDJwbGxjbTU4NjUxOSJ9.2RtOfwYTMgGU6tUreAIlFA'
//        }).addTo(map);
//    }
//    )
//}
//)

mapApp.controller("MapController", /*["$scope", "$http", "$timeout",*/ function ($scope, $http, $timeout, $mdDialog) {
    $scope.sideBarTabs = {
        activeName: 'suitability'
    };
    $scope.sideBarTabChanged = tabName =>
        $scope.sideBarTabs.activeName = tabName;

    $scope.loading = false;
    $scope.zipLayer = null;
    $scope.map = null;
    $scope.tooltip = null;
    $scope.marker = null;
    $scope.searchBox = null;
    $scope.clickedPolygonProperties = null;
    $scope.saveAndCancelButtonsDisabled = 'disabled';
    $scope.addButtonDisabled = '';
    $scope.newPolygon = null;
    $scope.clickedPolygon = null;
    $scope.clickedRightMousePolygon = null;
    $scope.indicator = {};
    $scope.mapTile = "openStreetMap";
    $scope.kadastrDisplay = true;
	$scope.zipLayerDisplay=false;
    $scope.currentState = null;
    $scope.polygonCreateDate = {};
    $scope.mapId = -1;
    $scope.region = "";
    var kadastrLayer;
    var openStreetMap;
    var roadMutant;
    var satMutant;
    var terrainMutant;
    var hybridMutant;
    var currentMapLayer;

    $scope.currentPolygons = [];
    var PHOSPHORUS = {
        EXTRA_LOW: { minValue: 0, maxValue: 26, color: "#D43733" },
        LOW: { minValue: 26, maxValue: 51, color: "#FF8C00" },
        MEDIUM: { minValue: 51, maxValue: 101, color: "#FDFD7A" },
        INCREASED: { minValue: 101, maxValue: 151, color: "#73FC70" },
        HIGH: { minValue: 151, maxValue: 250, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 250, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var PH = {
        VERY_HIGLY_ACIDIC: { minValue: 0, maxValue: 4.1, color: "#CF2824" },
        HIGLY_ACIDIC: { minValue: 4.1, maxValue: 4.6, color: "#E0AACA" },
        MEDIUM_ACIDIC: { minValue: 4.6, maxValue: 5.1, color: "#EAD092" },
        LOW_ACIDIC: { minValue: 5.1, maxValue: 5.6, color: "#F5F67C" },
        CLOSE_TO_NEUTRAL: { minValue: 5.6, maxValue: 6.1, color: "#B2F1B0" },
        NEUTRAL: { minValue: 6.1, maxValue: 7.1, color: "#79B877" },
        LOW_ALKALINE: { minValue: 7.1, maxValue: 7.6, color: "#7DE8E8" },
        MEDIUM_ALKALINE: { minValue: 7.6, maxValue: 8.1, color: "#8B8ACC" },
        HIGH_ALKALINE: { minValue: 8.1, maxValue: 8.5, color: "#D8B7D9" },
        VERY_HIGH_ALKALINE: { minValue: 8.6, maxValue: Number.MAX_SAFE_INTEGER, color: "#CDB789" }
    }
    var AZOTE = {
        EXTRA_LOW: { minValue: 0, maxValue: 101, color: "#D43733" },
        LOW: { minValue: 101, maxValue: 151, color: "#FF8C00" },
        MEDIUM: { minValue: 151, maxValue: 200, color: "#F6F67C" },
        INCREASED: { minValue: 200, maxValue: Number.MAX_SAFE_INTEGER, color: "#53A551" },
    }
    var ORGANIC = {
        EXTRA_LOW: { minValue: 0, maxValue: 1.1, color: "#D43733" },
        LOW: { minValue: 1.1, maxValue: 2.1, color: "#FF8C00" },
        MEDIUM: { minValue: 2.1, maxValue: 3.1, color: "#FDFD7A" },
        INCREASED: { minValue: 3.1, maxValue: 4.1, color: "#73FC70" },
        HIGH: { minValue: 4.1, maxValue: 5.0, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 5.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var POTASSIUM = {
        EXTRA_LOW: { minValue: 0, maxValue: 41, color: "#D43733" },
        LOW: { minValue: 41, maxValue: 81, color: "#FF8C00" },
        MEDIUM: { minValue: 81, maxValue: 120, color: "#FDFD7A" },
        INCREASED: { minValue: 121, maxValue: 170, color: "#73FC70" },
        HIGH: { minValue: 171, maxValue: 250, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 250, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var SULFUR = {
        EXTRA_LOW: { minValue: 0, maxValue: 3.1, color: "#D43733" },
        LOW: { minValue: 3.1, maxValue: 6.1, color: "#FF8C00" },
        MEDIUM: { minValue: 6.1, maxValue: 9.1, color: "#FDFD7A" },
        INCREASED: { minValue: 9.1, maxValue: 12.1, color: "#73FC70" },
        HIGH: { minValue: 12.1, maxValue: 15.0, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 15.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var MANGANESE = {
        EXTRA_LOW: { minValue: 0, maxValue: 5.1, color: "#D43733" },
        LOW: { minValue: 5.1, maxValue: 7.1, color: "#FF8C00" },
        MEDIUM: { minValue: 7.1, maxValue: 10.1, color: "#FDFD7A" },
        INCREASED: { minValue: 10.1, maxValue: 15.1, color: "#73FC70" },
        HIGH: { minValue: 15.1, maxValue: 20.0, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 20.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var ZINC = {
        EXTRA_LOW: { minValue: 0, maxValue: 1.1, color: "#D43733" },
        LOW: { minValue: 1.1, maxValue: 1.6, color: "#FF8C00" },
        MEDIUM: { minValue: 1.6, maxValue: 2.1, color: "#FDFD7A" },
        INCREASED: { minValue: 2.1, maxValue: 3.1, color: "#73FC70" },
        HIGH: { minValue: 3.1, maxValue: 5.0, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 5.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var COPPER = {
        EXTRA_LOW: { minValue: 0, maxValue: 0.11, color: "#D43733" },
        LOW: { minValue: 0.11, maxValue: 0.16, color: "#FF8C00" },
        MEDIUM: { minValue: 0.16, maxValue: 0.21, color: "#FDFD7A" },
        INCREASED: { minValue: 0.21, maxValue: 0.31, color: "#73FC70" },
        HIGH: { minValue: 0.31, maxValue: 0.50, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 0.50, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var KOBAL = {
        EXTRA_LOW: { minValue: 0, maxValue: 0.071, color: "#D43733" },
        LOW: { minValue: 0.071, maxValue: 0.11, color: "#FF8C00" },
        MEDIUM: { minValue: 0.11, maxValue: 0.16, color: "#FDFD7A" },
        INCREASED: { minValue: 0.16, maxValue: 0.21, color: "#73FC70" },
        HIGH: { minValue: 0.21, maxValue: 0.30, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 0.30, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var MOLYBDENUM = {
        EXTRA_LOW: { minValue: 0, maxValue: 0.05, color: "#D43733" },
        LOW: { minValue: 0.05, maxValue: 0.08, color: "#FF8C00" },
        MEDIUM: { minValue: 0.08, maxValue: 0.11, color: "#FDFD7A" },
        INCREASED: { minValue: 0.11, maxValue: 0.16, color: "#73FC70" },
        HIGH: { minValue: 0.16, maxValue: 0.22, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 0.22, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var BORON = {
        EXTRA_LOW: { minValue: 0, maxValue: 0.15, color: "#D43733" },
        LOW: { minValue: 0.15, maxValue: 0.23, color: "#FF8C00" },
        MEDIUM: { minValue: 0.23, maxValue: 0.34, color: "#FDFD7A" },
        INCREASED: { minValue: 0.34, maxValue: 0.51, color: "#73FC70" },
        HIGH: { minValue: 0.51, maxValue: 0.70, color: "#8EFCFD" },
        VERY_HIGH: { minValue: 0.70, maxValue: Number.MAX_SAFE_INTEGER, color: "#0000FF" }
    }
    var CESIUM = {
        EXTRA_LOW: { minValue: 0, maxValue: 1.0, color: "#73FC70" },
        LOW: { minValue: 1.0, maxValue: 5.1, color: "#8EFCFD" },
        MEDIUM: { minValue: 5.1, maxValue: 15.0, color: "#FDFD7A" },
        INCREASED: { minValue: 15.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#D43733" }
    }
    var STRONTIUM = {
        EXTRA_LOW: { minValue: 0, maxValue: 0.02, color: "#73FC70" },
        LOW: { minValue: 0.02, maxValue: 0.16, color: "#8EFCFD" },
        MEDIUM: { minValue: 0.16, maxValue: 3.0, color: "#FDFD7A" },
        INCREASED: { minValue: 3.0, maxValue: Number.MAX_SAFE_INTEGER, color: "#D43733" }
    }

    $scope.init = function () {
        $scope.map = L.map('map', { editable: true }).setView([50.30382, 25.19354], 13);
		
		//!!! qgis layer maps
	/*	qgis_soil = L.WMS.layer("http://organicportal.in.ua:8888/cgi-bin/qgis_mapserv.fcgi", "Soil", {
           // pane: 'pane_Soil_1',
            format: 'image/png',
            uppercase: true,
            transparent: true,
            continuousWorld : true,
            tiled: true,
            //info_format: 'text/html',
            opacity: 1,
            attribution: '',
			maxZoom: 16,
            minZoom: 10,
        });*/
		
		qgis_soil=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Soil",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_ph=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"PH",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_humus=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Humus",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_filtration=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Filtration",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_fertility=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Fertility",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		//!!! new
		qgis_gleying=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Gleying",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_mechanical=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Mechanical",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_salinity=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Salinity",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_salt_licks=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Salt_licks",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		qgis_swampy=L.tileLayer.wms("http://organicportal.in.ua:8087/cgi-bin/qgis_mapserv.php",{
			layers:"Swampy",
			maxZoom: 16,
            minZoom: 8,
			opacity: 0.5,
			format: 'image/png',
			transparent: true,
		});
		//!!! EOF qgis layer maps
		
        openStreetMap = L.tileLayer('https://api.mapbox.com/styles/v1/{username}/{style_id}/tiles/{tilesize}/{z}/{x}/{y}?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            maxZoom: 16,
            minZoom: 8,
            username: 'zeonet',
            style_id: 'ckog38jpb3bp817ol51efke28',
            tilesize: 512,
            accessToken: 'pk.eyJ1IjoiemVvbmV0IiwiYSI6ImNpcnc2Zm9nMDAwaWxoem0xcGhscm9jNDEifQ.nauCIhnSpecMYifvK-9BxA'
        }).addTo($scope.map);
        
		
		currentMapLayer = openStreetMap; 
		 
        // $(window).on("resize", function () { $("#map").height($(window).height()); $("#map").width($(window).width() - 400); $scope.map.invalidateSize(); }).trigger("resize");

        roadMutant = L.gridLayer.googleMutant({
            maxZoom: 16,
            minZoom: 8,
            type: 'roadmap'
        });

        satMutant = L.gridLayer.googleMutant({
            maxZoom: 16,
            minZoom: 8,
            type: 'satellite'
        });

        terrainMutant = L.gridLayer.googleMutant({
            maxZoom: 16,
            minZoom: 8,
            type: 'terrain'
        });

        hybridMutant = L.gridLayer.googleMutant({
            maxZoom: 16,
            minZoom: 8,
            type: 'hybrid'
        });

        //kadastrLayer = L.tileLayer.wms('http://212.26.144.110/geowebcache/service/wms', {
        //    layers: 'kadastr',
        //    format: 'image/png',
        //    crs: L.CRS.EPSG900913,
        //    maxZoom: 16,
        //    minZoom: 8,
        //    attribution: "© ЦДЗК",
        //    transparent: true
        //}).addTo($scope.map);
        $scope.CreateLeafletControls();
        $scope.getData();
        $scope.tooltip = L.DomUtil.get('tooltip');
        var throttledDrag = _.throttle($scope.dragEnd, 1500, { leading: false });
        $scope.map.on('dragend', throttledDrag);
        $scope.map.on('zoomend', throttledDrag);
        $scope.map.on('editable:drawing:start', addTooltip);
        $scope.map.on('editable:drawing:end', removeTooltip);
        $scope.map.on('editable:drawing:click', updateTooltip);
        //GetAgroValues();

        $scope.checkLegend = function (string) {
            if ($scope.indicator.value == undefined) return false;
            return $scope.indicator.value.toString() == string;
        }

        var legendList = [
            { Key: "SOIL",       Value: "Грунт",       qgis: 'qgisSoil' },
            { Key: "HUMUS",      Value: "Вміст гумусу у %",       qgis: "qgisHumus" },
            { Key: "PH",         Value: "Кислотність грунту (pH сольовий)",          qgis: "qgisPH" },
            { Key: "MECHANICAL", Value: "Механічний склад",    qgis: "qgisMechanical" },
            { Key: "FILTRATION", Value: "Фільтрація",  qgis: "qgisFiltration" },
            { Key: "GLEYING",    Value: "Глейовість",  qgis: "qgisGleying" },
            { Key: "SALT_LICKS", Value: "Солонці",     qgis: "qgisSaltLicks" },
            { Key: "SWAMPY",     Value: "Мочаристі",   qgis: "qgisSwampy" },
            { Key: "SALINITY",   Value: "Засоленість", qgis: "qgisSalinity" },
            { Key: "FERTILITY",  Value: "Родючість",   qgis: "qgisFertility" },
            { Key: "NPK_RIVNE",  Value: "NPK",         qgis: "qgisNPK" },
            { Key: "HROMADU",    Value: "Район",       qgis: "qgisRegion" }, 
        ];

        $scope.mapLayers = [];
        for (var i = 0; i < legendList.length; i++) {
            $scope.mapLayers.push({ "Name": legendList[i].Value, "qgis": legendList[i].qgis });
            GetLegend(undefined, i, legendList[i].Key);
        }

        $scope.map.on('click', function (e) { setAgroValues(e, legendList) });

        CheckIfUserIsAdmin();
        addGoogleSearch();
        reverseGeoCode();
    };
    function ShowSingleNotify(args) {
        var
            CSS = args.css ? args.css : {},
            kind = args.kind ? args.kind : false,
            delay = args.delay ? args.delay : 5000

        if (kind) {
            var palette = []
            palette['danger'] = '#B8312F'
            palette['info'] = '#2C82C9'
            palette['success'] = '#41A85F'
            palette['warning'] = '#F37934'
            CSS.background = palette[kind]
        }//if kind

        var
            text = args.text ? args.text : "",
            title = args.title ? "<p>" + args.title + "</p>" : "",
            el = $('<div id=single-notify class=' + kind + '-notify>')
                .html(title + text) //add content
                .css(CSS); //stylish
        if (!title && !text) return
        $('#single-notify').remove()//remove previous popup
        el.appendTo('#map').delay(delay).fadeOut()
    }
    function setAgroValues(e, legendList) {
        $scope.clickedPolygon = null;
        if ($scope.marker == null)
            $scope.marker = L.marker(e.latlng).addTo($scope.map);
        else
            $scope.marker.setLatLng(e.latlng);
        $scope.mapLayers = [];
        for (var i = 0; i < legendList.length; i++) {
            $scope.mapLayers.push({ "Name": legendList[i].Value, "qgis": legendList[i].qgis });
            GetLegend(e, i, legendList[i].Key);
        }
    }
    function GetLegend(e, id, name) {
        $.ajax({
            type: 'GET',
            defaultHeaders: { 'Access-Control-Allow-Origin': '*' },
            dataType: 'json',
            url: 'http://organicportal.in.ua:8087/cgi-bin/legend.php?layer=' + name,
            success: function (legend) {
                $scope.mapLayers[id].Legend = legend;
                for (var j = 0; j < legend.length; j++)
                    $scope.mapLayers[id].Legend[j].Color = 'rgb(' + legend[j].RColor + ',' + legend[j].GColor + ',' + legend[j].BColor + ')';
                
                GetFeatures(e, id, name, legend);
            },
            error: function () {
                GetFeatures(e, id, name, []);
            },
        });
    }
    function GetFeatures(e, id, name, legend) {
        if (e == undefined) return;
        $.ajax({
            type: 'GET',
            defaultHeaders: { 'Access-Control-Allow-Origin': '*' },
            dataType: 'json',
            url: 'http://organicportal.in.ua:8087/cgi-bin/features.php?layer=' + name +
                '&lat=' + e.latlng.lat + '&lon=' + e.latlng.lng,
            success: function(jsonData) {
                if (jsonData == undefined || jsonData[0] == undefined) return;

                var dataValue = '';
                if (jsonData[0].Name != undefined)
                    dataValue = jsonData[0].Name;
                else if (jsonData[0].name != undefined)
                    dataValue = jsonData[0].name;
                else if (jsonData[0].Name_1 != undefined)
                    dataValue = jsonData[0].Name_1;
                else if (jsonData[0].Качества_Почв != undefined)
                    dataValue = jsonData[0].Качества_Почв;
                else if (jsonData[0].rayon != undefined) {
                    $scope.mapId = -1;//jsonData[0].id;
                    dataValue = jsonData[0].rayon.split(' ')[0];
                }
                else if (jsonData[0].n != undefined)
                    dataValue = jsonData[0].n + "," + jsonData[0].p + "," + jsonData[0].k;

                var color = 'white';
                var foundLegend = legend.find(x => x.Name.includes(dataValue));
                if (foundLegend != undefined) {
                    color = 'rgb(' + foundLegend.RColor + ',' + foundLegend.GColor + ',' + foundLegend.BColor + ')';
                }

                var width = 0;
                if (jsonData[0].id != undefined)
                    width = (legend.length - jsonData[0].id + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Мехсостав != undefined)
                    width = (legend.length - jsonData[0].Мехсостав + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].id_Фильтрация != undefined)
                    width = (legend.length - jsonData[0].id_Фильтрация + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Оглеенные != undefined)
                    width = (legend.length - jsonData[0].Оглеенные + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Засоленные != undefined)
                    width = (legend.length - jsonData[0].Засоленные + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Грунт_1 != undefined)
                    width = (legend.length - jsonData[0].Грунт_1 + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Солонцы != undefined)
                    width = (legend.length - jsonData[0].Солонцы + 1) / (legend.length + 1) * 100;
                else if (jsonData[0].Мочаристые != undefined)
                    width = (legend.length - jsonData[0].Мочаристые + 1) / (legend.length + 1) * 100;

                $scope.mapLayers[id].Value = dataValue;
                $scope.mapLayers[id].Color = color;
                $scope.mapLayers[id].Width = width;
                $scope.$apply();
            }
        });
    }
    function GetAgroValues() {
        $http({
            method: 'GET',
            url: '/api/MapLayers?'
        }).then(function successCallback(response) {
                $scope.mapLayers = response.data;
                for (var i = 0; i < $scope.mapLayers.length; i++) {
                    $scope.mapLayers[i].Value = null;
                }
                console.log($scope.mapLayers);
            },
            function errorCallback(response) {
                alert("Error loading main agro values");
                console.log(response);
            });
    }
    $scope.checkRegion = function () {
        if ($scope.marker == null) return false;
        var region = $scope.mapLayers.find(function (x) { return x.Name == "Район" }).Value;
        if (region == "Березнівський") return true;
        if (region == "Володимирецький") return true;
        if (region == "Гощанський") return true;
        if (region == "Демидівський") return true;
        if (region == "Дубенський") return true;
        if (region == "Дубровицький") return true;
        if (region == "Зарічненський") return true;
        if (region == "Здолбунівський") return true;
        if (region == "Корецький") return true;
        if (region == "Костопільський") return true;
        if (region == "Млинівський") return true;
        if (region == "Острозький") return true;
        if (region == "Радивилівський") return true;
        if (region == "Рівненський") return true;
        if (region == "Рокитнівський") return true;
        if (region == "Сарненський") return true;
        return false;
    }
    function CheckIfUserIsAdmin() {
        $http({
            method: 'POST',
            url: '/Account/CheckIfAdmin/',
            data: {}
        }).then(function successCallback(response) {
            if (response.data.isAdmin == true) {
                $scope.registerDeleteDialog();
                $scope.registerContextMenu();
                L.NewHandControl = L.Control.extend({
                    onAdd: function (map) {
                        var container = L.DomUtil.create('div', 'leaflet-control leaflet-bar'),
                            link = L.DomUtil.create('a', '', container);
                        link.href = '#';
                        link.title = 'Hand';
                        link.innerHTML = 'Hand';
                        L.DomEvent.on(link, 'click', L.DomEvent.stop)
                            .on(link, 'click', function () {
                                $scope.map.editTools.stopDrawing();
                            }, this);

                        return container;
                    },
                    options: {
                        position: 'topleft'
                    }
                });

                $scope.map.addControl(new L.NewHandControl());
            }
        }, function errorCallback(response) {
            console.log("Error checking if admin");
        });
    }
    function addGoogleSearch() {
        var input = document.getElementById('usr');
        var options = {
            types: ['(cities)'],
            componentRestrictions: { country: ["ua"] }
        }
        $scope.searchBox = new google.maps.places.Autocomplete(input, options);
        $scope.searchBox.addListener('place_changed', googleSearchPlaceChanged);
    }
    function googleSearchPlaceChanged() {
        var place = $scope.searchBox.getPlace();
        if (!place.geometry) {
            return;
        }
        console.log(place);
        var latlng = L.latLng(place.geometry.location.lat(), place.geometry.location.lng());
        $scope.map.panTo(latlng);
        reverseGeoCode();
    }
    function reverseGeoCode() {
        var mapCenter = $scope.map.getCenter();
        var geocoder = new google.maps.Geocoder;
        geocoder.geocode({ 'location': mapCenter }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                    var currentCity = "";
                    var currentRegion = "";
                    for (var i = 0; i < results[1].address_components.length; i++) {
                        if (results[1].address_components[i].types[0] == "locality") {
                            currentCity = results[1].address_components[i].long_name;
                        }
                        else if (results[1].address_components[i].types[0] == "administrative_area_level_1") {
                            currentRegion = results[1].address_components[i].long_name;
                        }
                    }

                    $("#current-address-city").text(currentCity);
                    $("#current-address-region").text(currentRegion);
                } else {
                    console.log('No reverseGeoCode results found')
                }
            } else {
                console.log("Geocoder failed due to:");
                console.log(status);
            }
        });
    }
    $scope.changeKadastrDisplay = function (kadastrDisplay) {
        if (kadastrDisplay == false) {
            kadastrLayer.remove();
        }
        else {
            kadastrLayer.addTo($scope.map);
            kadastrLayer.bringToFront();

        }
    }
	//moove zip layer to maps
	$scope.changeZipLayerDisplay=function(zipLayerDisplay){
		
		if (zipLayerDisplay) $scope.zipLayer.addTo($scope.map);
		if (!zipLayerDisplay) $scope.zipLayer.remove();
		
		console.log("changeZipLayerDisplay: "+zipLayerDisplay);
	}
	
    $scope.changeMapTile = function (mapTile,bg="none") {
		//!!! ads qgis layers
		
		//console.log(">in("+mapTile+","+bg+")");
		
		if (bg!="none") $scope.changeMapTile(bg);
		
		if (mapTile == "qgisSoil") {
            //currentMapLayer.remove();
            qgis_soil.addTo($scope.map);
            currentMapLayer = qgis_soil;
            console.log("qgisSoil,"+bg);
        }
		if (mapTile == "qgisPH") {
            //currentMapLayer.remove();
            qgis_ph.addTo($scope.map);
            currentMapLayer = qgis_ph;
            console.log("qgisPH,"+bg);
        }
		if (mapTile == "qgisHumus") {
            //currentMapLayer.remove();
            qgis_humus.addTo($scope.map);
            currentMapLayer = qgis_humus;
            console.log("qgisHumus,"+bg);
        }
		if (mapTile == "qgisFiltration") {
            //currentMapLayer.remove();
            qgis_filtration.addTo($scope.map);
            currentMapLayer = qgis_filtration;
            console.log("qgisFiltration,"+bg);
        }
		if (mapTile == "qgisFertility") {
            //currentMapLayer.remove();
            qgis_fertility.addTo($scope.map);
            currentMapLayer = qgis_fertility;
            console.log("qgisFertility,"+bg);
        }
		//!!! add new layers
		if (mapTile == "qgisGleying") {
            //currentMapLayer.remove();
            qgis_gleying.addTo($scope.map);
            currentMapLayer = qgis_gleying;
            console.log("qgisGleying,"+bg);
        }
		if (mapTile == "qgisMechanical") {
            //currentMapLayer.remove();
            qgis_mechanical.addTo($scope.map);
            currentMapLayer = qgis_mechanical;
            console.log("qgisMechanical,"+bg);
        }
		if (mapTile == "qgisSalinity") {
            //currentMapLayer.remove();
            qgis_salinity.addTo($scope.map);
            currentMapLayer = qgis_salinity;
            console.log("qgisSalinity,"+bg);
        }
		if (mapTile == "qgisSaltLicks") {
            //currentMapLayer.remove();
            qgis_salt_licks.addTo($scope.map);
            currentMapLayer = qgis_salt_licks;
            console.log("qgisSaltLicks,"+bg);
        }
		if (mapTile == "qgisSwampy") {
            //currentMapLayer.remove();
            qgis_swampy.addTo($scope.map);
            currentMapLayer = qgis_swampy;
            console.log("qgisSwampy,"+bg);
        }
		//!!! eof qgis layers
		
		
		if (bg=="none"){
        if (mapTile == "openStreetMap") {
            currentMapLayer.remove();
            openStreetMap.addTo($scope.map);
            currentMapLayer = openStreetMap;
            console.log("roadMutant");
        }
        else if (mapTile == "roadMutant") {
            currentMapLayer.remove();
            roadMutant.addTo($scope.map);
            currentMapLayer = roadMutant;
            console.log("roadMutant");
        }
        else if (mapTile == "satMutant") {
            currentMapLayer.remove();
            satMutant.addTo($scope.map);
            currentMapLayer = satMutant;
            console.log("satMutant");
        }
        else if (mapTile == "terrainMutant") {
            currentMapLayer.remove();
            terrainMutant.addTo($scope.map);
            currentMapLayer = terrainMutant;
            console.log("terrainMutant");
        }
        else if (mapTile == "hybridMutant") {
            currentMapLayer.remove();
            hybridMutant.addTo($scope.map);
            currentMapLayer = hybridMutant;
            console.log("hybridMutant");
        }
        if ($scope.map.hasLayer(kadastrLayer)) {
            kadastrLayer.bringToFront();
			console.log("kadastr to front");
        }
		}
    }
    $scope.registerDeleteDialog = function () {
        $("#dialog-confirm").dialog({
            autoOpen: false,
            resizable: false,
            height: "auto",
            width: 400,
            modal: true,
            buttons: {
                "Видалити": function () {
                    var del_id = $scope.clickedRightMousePolygon.feature.id;
                    console.log(del_id);
                    $http({
                        method: 'DELETE',
                        url: '/api/PolygonLayers/' + del_id,
                        data: {
                            id: del_id
                        },
                        headers: {
                            'Content-type': 'application/json;charset=utf-8'
                        }
                    }).then(function successCallback(response) {
                        $scope.getData();
                        $scope.polygonCreateDate.value = null;
                        if ($scope.marker != null) {
                            $scope.marker.remove();
                            $scope.marker = null;
                        }
                        $scope.currentState = null;
                        for (var i = 0; i < $scope.mapLayers.length; i++) {

                            $scope.mapLayers[i].Value = null;
                        }
                        //alert("Success");
                    }, function errorCallback(response) {
                        alert("Error deleting polygon");
                    });
                    $(this).dialog("close");
                },
                "Відмінити": function () {
                    $(this).dialog("close");
                }
            }
        });
    }
    $scope.registerContextMenu = function () {
        $("#map").contextMenu({
            selector: ".leaflet-pane",
            reposition: false,
            callback: function (key, options) {
                if (key == "edit") {
                    $scope.currentState = "editExistingPolygon";
                    $scope.clickedPolygon = $scope.clickedRightMousePolygon;
                    for (var i = 0; i < $scope.mapLayers.length; i++) {

                        var result = $.grep($scope.clickedRightMousePolygon.feature.properties, function (e) { return e.idM == $scope.mapLayers[i].Id; });
                        $scope.mapLayers[i].Value = (result[0] != undefined) ? result[0].value : 0;
                    }
                    var polygonDate = new Date($scope.clickedRightMousePolygon.feature.date);
                    //polygonDate.setDate(polygonDate.getDate() + 1);
                    //var formattedDate = (polygonDate.getDate() + 1) + "." + polygonDate.getMonth() + "." + polygonDate.getFullYear();
                    $scope.polygonCreateDate.value = polygonDate;
                    $scope.$apply();
                    //$("#accordion").accordion('option', 'active', 1);
                    CloseAllTabs();
                    $("#input").show();
                    if ($scope.marker == null) {
                        $scope.marker = L.marker($scope.clickedRightMousePolygon.getCenter()).addTo($scope.map);
                    }
                    else {
                        $scope.marker.setLatLng($scope.clickedRightMousePolygon.getCenter());
                    }
                }
                else if (key == "delete") {
                    $("#dialog-confirm").dialog("open");
                }
            },
            items: {
                "edit": { name: "Редагувати", icon: "fa-edit" },
                "delete": { name: "Видалити", icon: "fa-trash-o" }
            }

        });
    }

    $scope.colorPolygons = function (indicator) {
        console.log(indicator);
        var currentEnumIndicator = null;
        switch (indicator) {
            case "Фосфор (P)":
                {
                    currentEnumIndicator = PHOSPHORUS;
                    break;
                }
            case "pH грунту":
                {
                    currentEnumIndicator = PH;
                    break;
                }
            case "Азот (N)":
                {
                    currentEnumIndicator = AZOTE;
                    break;
                }
            case "Органічна речовина":
                {
                    currentEnumIndicator = ORGANIC;
                    break;
                }
            case "Калій (К)":
                {
                    currentEnumIndicator = POTASSIUM;
                    break;
                }
            case "Сірка (S)":
                {
                    currentEnumIndicator = SULFUR;
                    break;
                }
            case "Марганець (Mn)":
                {
                    currentEnumIndicator = MANGANESE;
                    break;
                }
            case "Цинк (Zn)":
                {
                    currentEnumIndicator = ZINC;
                    break;
                }
            case "Мідь (Cu)":
                {
                    currentEnumIndicator = COPPER;
                    break;
                }
            case "Кобальт (Co)":
                {
                    currentEnumIndicator = KOBAL;
                    break;
                }
            //case "Молібден":
            //    {
            //        currentEnumIndicator = MOLYBDENUM;
            //        break;
            //    }
            case "Бор (B)":
                {
                    currentEnumIndicator = BORON;
                    break;
                }
            case "Цезій (Cs)":
                {
                    currentEnumIndicator = CESIUM;
                    break;
                }
            case "Стронцій (Sr)":
                {
                    currentEnumIndicator = STRONTIUM;
                    break;
                }
        }

        for (var i = 0; i < $scope.currentPolygons.length; i++) {
            var result = $.grep($scope.currentPolygons[i].feature.properties, function (e) { return e.name == indicator; });
            var resultValue = result[0].value;

            for (var grade in currentEnumIndicator) {
                if (resultValue >= currentEnumIndicator[grade].minValue && resultValue < currentEnumIndicator[grade].maxValue) {
                    var polyColor = {
                        'color': currentEnumIndicator[grade].color,
                        fillOpacity: 0.7,
                    };
                    $scope.currentPolygons[i].layer.setStyle(polyColor);
                }
            }
        }
    }
    function addTooltip(e) {
        L.DomEvent.on(document, 'mousemove', moveTooltip);
        $scope.tooltip.innerHTML = 'Натисніть на карту, щоб почати ввід полігону.';
        $scope.tooltip.style.display = 'block';
        $scope.tooltip.style.left = ($("#map").width() / 2 - $("#tooltip").width() / 2 + 'px');
        $scope.tooltip.style.top = $("#map").height() / 2 - $("#tooltip").height() / 2 + 'px';
    }

    function removeTooltip(e) {
        $scope.tooltip.innerHTML = '';
        $scope.tooltip.style.display = 'none';
        L.DomEvent.off(document, 'mousemove', moveTooltip);
    }

    function moveTooltip(e) {
        if (e.target.id == "map") {
            $scope.tooltip.style.left = e.layerX + 20 + 'px';
            $scope.tooltip.style.top = e.layerY - 10 + 'px';
        }
    }

    function updateTooltip(e) {

        if ((e.layer.editor._drawnLatLngs.length + 1) < e.layer.editor.MIN_VERTEX) {
            tooltip.innerHTML = 'Натисніть на карту, щоб продовжити ввід полігону.';
        }
        else {
            tooltip.innerHTML = 'Натисніть на останню точку, щоб завершити ввід полігону.';
        }
    }
    $scope.CreateLeafletControls = function () {
        L.EditControl = L.Control.extend({

            options: {
                position: 'topleft',
                callback: null,
                kind: '',
                html: ''
            },

            onAdd: function (map) {
                var container = L.DomUtil.create('div', 'leaflet-control leaflet-bar'),
                    link = L.DomUtil.create('a', '', container);

                link.href = '#';
                link.title = 'Create a new ' + this.options.kind;
                link.innerHTML = this.options.html;
                L.DomEvent.on(link, 'click', L.DomEvent.stop)
                    .on(link, 'click', function () {
                        window.LAYER = this.options.callback.call(map.editTools);
                    }, this);

                return container;
            }

        });
        L.NewPolygonControl = L.EditControl.extend({

            options: {
                position: 'topleft',
                callback: $scope.map.editTools.startPolygon,
                kind: 'polygon',
                html: 'Poly'
            }

        });

        $scope.map.on('editable:drawing:commit', $scope.PolygonCreated);
        $scope.map.on('editable:drawing:cancel', $scope.DrawingCanceled);
    }
    $scope.AddButtonClicked = function () {
        var disabled = $("#AddButton").tooltip("option", "disabled");
        if (disabled == true) {
            $scope.map.editTools.startPolygon();
        }

    }
    $scope.DrawingCanceled = function (e) {
        $scope.map.removeLayer(e.layer);

    }
    $scope.PolygonCreated = function (e) {
        var highlight = {
            'color': '#f44242'
        };
        $scope.map._layers[e.layer._leaflet_id].setStyle(highlight);
        $scope.saveAndCancelButtonsDisabled = '';
        $scope.newPolygon = e;
        $("#AddButton").tooltip("enable");
        //$("#SavePolyButton").attr("title", "Продовжіть введення данних або відмініть поточний полігон.");
        ShowSingleNotify({ title: 'Продовжіть введення данних або відмініть поточний полігон.', kind: 'warning' });
        //$("#SavePolyButton").tooltip("open");

        $scope.addButtonDisabled = 'disabled';
        $scope.$apply();
    }
    function CloseAllTabs() {
        $("#fields").hide();
        $("#input").hide();
        $("#legend").hide();
        // $("#filter-map").hide();
        //  $("#filter").hide();
        $("#analyse").hide();
    }
    $scope.CommitValuesNew = function () {
        if ($scope.currentState == "saveNewPolygon") {
            var valuesArr = [];
            for (var i = 0; i < $scope.mapLayers.length; i++) {
                var valueLayer = {
                    IdMapLayer: $scope.mapLayers[i].Id,
                    IdPolygon: 0,
                    Value: $scope.mapLayers[i].Value != undefined ? $scope.mapLayers[i].Value : 0
                }
                valuesArr.push(valueLayer);
            }
            var geojson = JSON.stringify($scope.newPolygon.layer.toGeoJSON());
            var polyPoint = "POINT (" + $scope.newPolygon.layer.getCenter().lng + " " + $scope.newPolygon.layer.getCenter().lat + ")";
            var dateUTC = null;
            if ($scope.polygonCreateDate.value) {
                dateUTC = new Date($scope.polygonCreateDate.value);
                dateUTC = new Date(dateUTC.getTime() - (60000 * dateUTC.getTimezoneOffset()));
            }
            var poly =
                {
                    geoJSON: geojson,
                    latitude: $scope.newPolygon.layer.getCenter().lat,
                    longitude: $scope.newPolygon.layer.getCenter().lng,
                    point: polyPoint,
                    values: valuesArr,
                    date: dateUTC
                }
            console.log(dateUTC);
            $http({
                method: 'POST',
                url: '/api/PolygonLayers/',
                data: JSON.stringify(poly)
            }).then(function successCallback(response) {
                $scope.getData();
                ShowSingleNotify({ title: 'Вашу ділянку збережено!', kind: 'success' });
            }, function errorCallback(response) {
                alert("Error committing polygon");
            });
            $scope.saveAndCancelButtonsDisabled = 'disabled';
            $("#AddButton").tooltip("disable");
            $scope.addButtonDisabled = '';
            //$scope.currentState = null;
        }
        else if ($scope.currentState == "editExistingPolygon") {
            var valuesArr = [];

            for (var i = 0; i < $scope.mapLayers.length; i++) {
                console.log($scope.mapLayers[i]);
                var valueLayer = {
                    IdMapLayer: $scope.mapLayers[i].Id,
                    IdPolygon: 0,
                    Value: $scope.mapLayers[i].Value != undefined ? $scope.mapLayers[i].Value : 0
                }
                valuesArr.push(valueLayer);
            }
            var dateUTC = null;
            if ($scope.polygonCreateDate.value) {
                dateUTC = new Date($scope.polygonCreateDate.value);
                dateUTC = new Date(dateUTC.getTime() - (60000 * dateUTC.getTimezoneOffset()));
            }
            var poly =
                {
                    id: $scope.clickedPolygon.feature.id,
                    //geoJSON: geojson,
                    // latitude: $scope.clickedPolygon.getCenter().lat,
                    // longitude: $scope.clickedPolygon.getCenter().lng,
                    //point: polyPoint,
                    values: valuesArr,
                    date: dateUTC
                }
            $http({
                method: 'PUT',
                url: '/api/PolygonLayers/',
                data: JSON.stringify(poly)
            }).then(function successCallback(response) {
                $scope.getData();
                ShowSingleNotify({ title: 'Успішно внесено зміни до вашої ділянки!', kind: 'success' });
            }, function errorCallback(response) {
                alert("Error editing polygon");
                console.log(response);
            });
            //$scope.currentState = null;
        }
    }
    $scope.getData = function () {
        if ($scope.loading) {
            return;
        }
        $scope.loading = true;

        var mapCenter = $scope.map.getCenter();

        var mapBounds = $scope.map.getBounds();
        var par = {
            latitude: mapCenter.lat,
            longitude: mapCenter.lng,
            NELat: mapBounds._northEast.lat,
            NELng: mapBounds._northEast.lng,
            SWLat: mapBounds._southWest.lat,
            SWLng: mapBounds._southWest.lng,
            extraFilterParametersYouShoulPass: null
        };

        $http({
            method: 'GET',
            url: '/api/PolygonLayers?',
            params: par
        }).
            then(function successCallback(response) {
                $scope.loading = false;
                $scope.displayResults(response.data);
                console.log(response.data);
            }, function errorCallback(response) {
                alert("Error loading");
                console.log(response);
                $scope.loading = false;
            })
    }
    $scope.displayResults = function (results) {
        if ($scope.zipLayer != null) {
            //for (l in $scope.zipLayer._layers)
            //{
            //    for(r in results.features)
            //    {
            //        if(l.feature.id==r.id)
            //        {
            //            l.feature.properties = r.properties;
            //            delete results.features.r;
            //        }
            //    }

            //}
            $scope.map.removeLayer($scope.zipLayer);
        }
		
        $scope.currentPolygons = [];
        $scope.zipLayer = L.geoJson(results, { onEachFeature: $scope.AddEventPolygonClicked });
        
		var polygon=null;
		if (zipLayerDisplay) $scope.zipLayer.addTo($scope.map);; 
		
        console.log($scope.indicator.value);
        if ($scope.indicator.value != null) {
            console.log("Colored polygons");
            $scope.colorPolygons($scope.indicator.value);
        }
        //If new polygon added click on it
        if ($scope.newPolygon != null && $scope.currentState == "saveNewPolygon") {
            var newPolygonLatLng = $scope.newPolygon.layer.getCenter();

            var result = $.grep($scope.currentPolygons, function (e) {
                return e.layer.getCenter().equals(newPolygonLatLng);
            });
            var resultValue = result[0];
            $timeout(function () {
                resultValue.layer.fire('click');
            }, 300);
            $scope.map.removeLayer($scope.newPolygon.layer);
            $scope.newPolygon = null;
        }
	
    }
    $scope.RightMouseButtonClicked = function (e) {
        console.log("RightMouseButtonClicked");
        $scope.clickedRightMousePolygon = e.target;
        console.log(e);
    }
    $scope.AddEventPolygonClicked = function (feature, layer) {
        layer.on('click', $scope.PolygonClicked);
        layer.on('contextmenu', $scope.RightMouseButtonClicked);
        var compoundObject = { "feature": feature, "layer": layer };
        $scope.currentPolygons.push(compoundObject);
    }
    $scope.cancelButtonClicked = function () {
        if ($scope.saveAndCancelButtonsDisabled != "disabled") {
            $scope.map.removeLayer($scope.newPolygon.layer);
            $scope.newPolygon = null;
            $("#AddButton").tooltip("disable");
            $scope.addButtonDisabled = '';
            $scope.saveAndCancelButtonsDisabled = 'disabled';
            $scope.currentState = null;
            for (var i = 0; i < $scope.mapLayers.length; i++) {
                $scope.mapLayers[i].Value = null;
            }
            if ($scope.marker != null) {
                $scope.marker.remove();
                $scope.marker = null;
            }

        }
    }
    $scope.saveButtonClicked = function () {
        if ($scope.saveAndCancelButtonsDisabled != "disabled") {
            for (var i = 0; i < $scope.mapLayers.length; i++) {
                switch ($scope.mapLayers[i].Name) {
                    case "pH грунту":
                        {
                            $scope.mapLayers[i].Value = 6.0;
                            break;
                        }
                    case "Органічна речовина":
                        {
                            $scope.mapLayers[i].Value = 3.0;
                            break;
                        }
                    case "Азот (N)":
                        {
                            $scope.mapLayers[i].Value = 150;
                            break;
                        }
                    case "Фосфор (P)":
                        {
                            $scope.mapLayers[i].Value = 120;
                            break;
                        }
                    case "Калій (К)":
                        {
                            $scope.mapLayers[i].Value = 300;
                            break;
                        }
                    case "Марганець (Mn)":
                        {
                            $scope.mapLayers[i].Value = 10;
                            break;
                        }
                    case "Сірка (S)":
                        {
                            $scope.mapLayers[i].Value = 4.0;
                            break;
                        }
                    case "Цинк (Zn)":
                        {
                            $scope.mapLayers[i].Value = 5.0;
                            break;
                        }
                    case "Мідь (Cu)":
                        {
                            $scope.mapLayers[i].Value = 1.0;
                            break;
                        }
                    case "Бор (B)":
                        {
                            $scope.mapLayers[i].Value = 0.40;
                            break;
                        }
                    case "Кобальт (Co)":
                        {
                            $scope.mapLayers[i].Value = 1.0;
                            break;
                        }
                    case "Кадмій (Cd)":
                        {
                            $scope.mapLayers[i].Value = 0.10;
                            break;
                        }
                    case "Свинець (Pb)":
                        {
                            $scope.mapLayers[i].Value = 1.0;
                            break;
                        }
                    case "Ртуть (Hg)":
                        {
                            $scope.mapLayers[i].Value = 0.01;
                            break;
                        }
                    case "Цезій (Cs)":
                        {
                            $scope.mapLayers[i].Value = 1.0;
                            break;
                        }
                    case "Стронцій (Sr)":
                        {
                            $scope.mapLayers[i].Value = 0.01;
                            break;
                        }
                }
            }
            $scope.polygonCreateDate.value = null;
            if ($scope.marker == null) {
                $scope.marker = L.marker($scope.newPolygon.layer.getCenter()).addTo($scope.map);
            }
            else {
                $scope.marker.setLatLng($scope.newPolygon.layer.getCenter());
            }
            $scope.currentState = "saveNewPolygon";
            CloseAllTabs();
            $("#input").show();
            //$("#accordion").accordion('option', 'active', 1);
        }


    }
    $scope.AreaItemEdit = function (e) {
        $scope.AreaItemHovered(e);
        CloseAllTabs();
        $("#input").show();
    }
    $scope.AreaItemDelete = function (e) {
        $scope.AreaItemHovered(e);
        $scope.clickedRightMousePolygon = e;
        $("#dialog-confirm").dialog("open");
    }
    $scope.AreaItemHovered = function (e) {
        console.log("Area item clicked");
        console.log(e);
        $scope.clickedPolygonProperties = e.feature.properties;
        $scope.clickedPolygon = e;
        var polygonDate = new Date(e.feature.date);
        //polygonDate.setDate(polygonDate.getDate() + 1);
        //var formattedDate = (polygonDate.getDate() + 1) + "." + polygonDate.getMonth() + "." + polygonDate.getFullYear();
        $scope.polygonCreateDate.value = polygonDate;
        $scope.currentState = "editExistingPolygon";
        for (var i = 0; i < $scope.mapLayers.length; i++) {

            var result = $.grep(e.feature.properties, function (e) { return e.idM == $scope.mapLayers[i].Id; });
            $scope.mapLayers[i].Value = (result[0] != undefined) ? result[0].value : 0;
        }
        if ($scope.marker == null) {
            $scope.marker = L.marker(e.layer.getCenter()).addTo($scope.map);
        }
        else {
            $scope.marker.setLatLng(e.layer.getCenter());
        }
    }

    $scope.PolygonClicked = function (e) {
        console.log("Polygon clicked");
        console.log(e.target);
        var clickedPolygonID = e.target.feature.id;
        $scope.mapId = clickedPolygonID;
        $scope.clickedPolygonProperties = e.target.feature.properties;
        $scope.clickedPolygon = e.target;
        var polygonDate = new Date(e.target.feature.date);
        //polygonDate.setDate(polygonDate.getDate() + 1);
        //var formattedDate = (polygonDate.getDate() + 1) + "." + polygonDate.getMonth() + "." + polygonDate.getFullYear();
        $scope.polygonCreateDate.value = polygonDate;
        $scope.currentState = "editExistingPolygon";
        for (var i = 0; i < $scope.mapLayers.length; i++) {

            var result = $.grep(e.target.feature.properties, function (e) { return e.idM == $scope.mapLayers[i].Id; });
            $scope.mapLayers[i].Value = (result[0] != undefined) ? result[0].value : 0;
        }
        $scope.$apply();
        CloseAllTabs();
        $("#analyse").show();
        if ($scope.marker == null) {
            $scope.marker = L.marker(e.target.getCenter()).addTo($scope.map);
        }
        else {
            $scope.marker.setLatLng(e.target.getCenter());
        }
        setProgressBarsProgress();

    }
    $scope.dragEnd = function (distance) {
        $scope.getData();
    }

    function getOptimalDataArr() {
        this.data = [{
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
        return this.data;
    }

    var progressBarsTimers = [];

    function setProgressBarsProgress() {

        var tableAnalyseTrs = $("#analyse > table > tbody > tr");

        for (var i = 0; i < tableAnalyseTrs.length; i++) {
            var elem = $($(tableAnalyseTrs[i]).children("td")[3]).children(".progress").children(".progress-bar");
            $(elem).css("width", "1%");
        }

        var tableAnalyseTrs = $("#analyse > table > tbody > tr");

        for (var i = 0; i < tableAnalyseTrs.length; i++) {
            var nameOfComponent = $($(tableAnalyseTrs[i]).children("td")[0]).text();
            var valueOfComponent = $($(tableAnalyseTrs[i]).children("td")[1]).text();

            var progressBarConfig = getProgressBarConfiguration(nameOfComponent, valueOfComponent);
            console.log(nameOfComponent + ": " + progressBarConfig);
            var elem = $($(tableAnalyseTrs[i]).children("td")[3]).children(".progress").children(".progress-bar");
            $($(tableAnalyseTrs[i]).children("td")[3]).children(".progress").children(".progress-bar").css("background-color", progressBarConfig.color);
            move(elem, progressBarConfig.percent);
        }
    }

    function move(elem, widthFull) {
        var width = 1;
        var step = widthFull / 5;
        var id = setInterval(frame, 50);

        function frame() {
            if (width >= widthFull) {
                clearInterval(id);
            } else {
                width += step;
                $(elem).css("width", width + "%");
            }
        }
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

    function calculateOnePercent(beginInterval, endInterval) {
        return (endInterval - beginInterval) / 100;
    }

    function getProgressBarConfiguration(nameOfComponent, valueOfComponent) {
        var optValForCurrentElem = getOptimalDataAboutElemntByName(nameOfComponent);

        if (isInOptimalRange(optValForCurrentElem, valueOfComponent)) {
            var onePecent = calculateOnePercent(optValForCurrentElem.optimalRange.b, optValForCurrentElem.optimalRange.e);
            var totalPercents = ((valueOfComponent - optValForCurrentElem.optimalRange.b) / onePecent) * 0.33 + 66;
            return { color: "lightgreen", percent: totalPercents };
        }

        if (hasOneAverageAndBadRange(optValForCurrentElem)) {
            if (isInAverageRange(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.averageRanges[0].b, optValForCurrentElem.averageRanges[0].e);
                var totalPercents = ((valueOfComponent - optValForCurrentElem.averageRanges[0].b) / onePecent) * 0.33 + 33;
                return { color: "yellow", percent: totalPercents };
            }
            if (isInBadRange(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.badRanges[0].b, optValForCurrentElem.badRanges[0].e);
                var totalPercentsInRange = ((valueOfComponent - optValForCurrentElem.badRanges[0].b) / onePecent) * 0.33;
                return { color: "red", percent: totalPercentsInRange };
            }
        } else {
            if (isInAverageRangeBelowOptimal(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.averageRanges[0].b, optValForCurrentElem.averageRanges[0].e);
                var totalPercents = ((valueOfComponent - optValForCurrentElem.averageRanges[0].b) / onePecent) * 0.33 + 33;
                return { color: "yellow", percent: totalPercents };
            }
            if (isInAverageRangeAboveOptimal(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.averageRanges[1].b, optValForCurrentElem.averageRanges[1].e);
                var totalPercents = ((valueOfComponent - optValForCurrentElem.averageRanges[1].b) / onePecent) * 0.33 + 33;
                return { color: "yellow", percent: totalPercents };
            }
            if (isInBadRangeBelowOptimal(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.badRanges[0].b, optValForCurrentElem.badRanges[0].e);
                var totalPercents = ((valueOfComponent - optValForCurrentElem.badRanges[0].b) / onePecent) * 0.33;
                return { color: "red", percent: totalPercents };
            }
            if (isInBadRangeAboveOptimal(optValForCurrentElem, valueOfComponent)) {
                var onePecent = calculateOnePercent(optValForCurrentElem.badRanges[1].b, optValForCurrentElem.badRanges[1].e);
                var totalPercents = ((valueOfComponent - optValForCurrentElem.badRanges[1].b) / onePecent) * 0.33;
                return { color: "red", percent: totalPercents };
            }
        }
    }


    function hasOneAverageAndBadRange(optValForCurrentElem) {
        if (optValForCurrentElem.averageRanges.length > 1) {
            return false;
        }
        return true;
    }

    function isInOptimalRange(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.optimalRange.b <= valueOfComponent && optValForCurrentElem.optimalRange.e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function isInAverageRangeBelowOptimal(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.averageRanges[0].b <= valueOfComponent && optValForCurrentElem.averageRanges[0].e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function isInAverageRangeAboveOptimal(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.averageRanges[1].b <= valueOfComponent && optValForCurrentElem.averageRanges[1].e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function isInBadRangeBelowOptimal(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.badRanges[0].b <= valueOfComponent && optValForCurrentElem.badRanges[0].e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function isInBadRangeAboveOptimal(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.badRanges[1].b <= valueOfComponent && optValForCurrentElem.badRanges[1].e >= valueOfComponent) {
            return true;
        }
        return false;
    }

    function isInAverageRange(optValForCurrentElem, valueOfComponent) {
        return isInAverageRangeBelowOptimal(optValForCurrentElem, valueOfComponent);
    }

    function isInBadRange(optValForCurrentElem, valueOfComponent) {
        if (optValForCurrentElem.badRanges[0].b <= valueOfComponent && optValForCurrentElem.badRanges[0].e >= valueOfComponent) {
            return true;
        }
        return false;
    }

});



// new code
$('.in-check').click(function(){
    $(this).toggleClass('active');
});


$('.in-radio').click(function(){
    $(this).closest('.agro-list-item').find('.in-radio').removeClass('active');
    $(this).addClass('active');
});