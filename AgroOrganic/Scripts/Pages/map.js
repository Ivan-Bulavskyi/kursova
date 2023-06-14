$(document).ready(function () {
    map = L.map('mapid', { editable: true }).setView([30, -87], 10);
    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=pk.eyJ1IjoicmF2ZW5ub2QiLCJhIjoiY2l1Yjl1cmdsMDAwNDJwbGxjbTU4NjUxOSJ9.2RtOfwYTMgGU6tUreAIlFA', {
        attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
        maxZoom: 18,
        id: 'ravennod.21355hjc',
        accessToken: 'pk.eyJ1IjoicmF2ZW5ub2QiLCJhIjoiY2l1Yjl1cmdsMDAwNDJwbGxjbTU4NjUxOSJ9.2RtOfwYTMgGU6tUreAIlFA'
    }).addTo(map);
    fetchData();
    CreateLeafletControls();
    var throttledDrag = _.throttle(dragEnd, 1500, { leading: false });
    map.on('dragend', throttledDrag);
    map.on('zoomend', throttledDrag);

    var info = L.control();

    info.onAdd = function (map) {
        this._div = L.DomUtil.create('div', 'info');
        this._div.innerHTML = "<button type=\"button\" class=\"btn btn-default\" id=\"commitButton\">Закомітить Полігон</button>";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"ID полігона\" class=\"form-control\" id=\"polygonID\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"pH грунту\" class=\"form-control\" id=\"ph\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Органічна речовина\" class=\"form-control\" id=\"organic\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Нітрати (NO3)\" class=\"form-control\" id=\"nitrats\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Фосфор (P)\" class=\"form-control\" id=\"p\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Калій (K)\" class=\"form-control\" id=\"k\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Кальцій (Ca)\" class=\"form-control\" id=\"ca\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Магній (Mg)\" class=\"form-control\" id=\"mg\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Натрій (Na)\" class=\"form-control\" id=\"na\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Сірка (S)\" class=\"form-control\" id=\"s\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Цинк (Zn)\" class=\"form-control\" id=\"zn\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Залізо (Fe)\" class=\"form-control\" id=\"fe\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Марганець (Mn)\" class=\"form-control\" id=\"mn\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Мідь (Cu)\" class=\"form-control\" id=\"cu\">";
        this._div.innerHTML += "<input type=\"text\" placeholder=\"Бор (B)\" class=\"form-control\" id=\"b\">";
        L.DomEvent.on(this._div, 'click', L.DomEvent.stop)
                     .on(this._div, 'click', CommitValues, this);
        return this._div;
    };

    info.addTo(map);
})
var loading = false;
var map = null;
var apiResults = null;
var zipLayer = null;
var zipLayers = [];
function CommitValues(e) {
    if (e.target.id == "commitButton") {
        console.log("commitButton");
        var values =
           {
               IdPolygon: document.getElementById("polygonID").value,
               Ph: document.getElementById("ph").value,
               Organic: document.getElementById("organic").value,
               Nitrats: document.getElementById("nitrats").value,
               P: document.getElementById("p").value,
               K: document.getElementById("k").value,
               Ca: document.getElementById("ca").value,
               Mg: document.getElementById("mg").value,
               Na: document.getElementById("na").value,
               S: document.getElementById("s").value,
               Zn: document.getElementById("zn").value,
               Fe: document.getElementById("fe").value,
               Mn: document.getElementById("mn").value,
               Cu: document.getElementById("cu").value,
               B: document.getElementById("b").value

           }
        $.ajax({
            url: '/api/Values/',
            type: 'POST',
            data: JSON.stringify(values),
            contentType: "application/json;charset=utf-8",
            success: function (data) {
                alert("Success");
            },
            error: function (x, y, z) {
                alert(x + '\n' + y + '\n' + z);
            }
        });
    }
}
function PolygonClicked(e) {
    console.log("Polygon clicked");
    console.log(e.target);
    var clickedPolygonID = e.target.feature.properties.id;
    console.log(e.target._leaflet_id);
    var highlight = {
        'color': '#333333',
        'weight': 2,
        'opacity': 1
    };
    map._layers[e.target._leaflet_id].setStyle(highlight);
    e.target.toggleEdit();
    document.getElementById("polygonID").value = clickedPolygonID;
}

function dragEnd(distance) {
    console.trace("DragEnd");
    fetchData();
}
function AddEventPolygonClicked(feature, layer) {
    layer.on('click', PolygonClicked);
}
function displayResults(results) {
    if (zipLayer !== null) {
        map.removeLayer(zipLayer);
    }
    zipLayer = L.geoJson(results, { onEachFeature: AddEventPolygonClicked });
    var polygon = zipLayer.addTo(map);
    //zipLayer.enableEdit();

}

//get the GeoJSON data from the server
function fetchData() {
    //don't load data with an outstanding request
    if (loading) {
        return;
    }
    loading = true;

    var mapCenter = map.getCenter();

    var mapBounds = map.getBounds();
    var params = {
        latitude: mapCenter.lat,
        longitude: mapCenter.lng,
        NELat: mapBounds._northEast.lat,
        NELng: mapBounds._northEast.lng,
        SWLat: mapBounds._southWest.lat,
        SWLng: mapBounds._southWest.lng,
        extraFilterParametersYouShoulPass: null
    };

    $.ajax({
        url: '/api/PolygonLayers?' + $.param(params)
    }).done(function (results) {
        console.trace(results);
        loading = false;
        displayResults(results);
    }).fail(function (jqXHR, textStatus) {
        alert("Error loading");
        loading = false;
    });
}
function CreateLeafletControls() {
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
            callback: map.editTools.startPolygon,
            kind: 'polygon',
            html: 'Poly'
        }

    });

    map.addControl(new L.NewPolygonControl());

    map.on('editable:drawing:commit', PolygonCreated);

}
function PolygonCreated(e) {
    var geojson = JSON.stringify(e.layer.toGeoJSON());
    var polyPoint = "POINT (" + e.layer.getCenter().lng + " " + e.layer.getCenter().lat + ")";
    var poly =
        {
            geoJSON: geojson,
            latitude: e.layer.getCenter().lat,
            longitude: e.layer.getCenter().lng,
            point: polyPoint
        }
    $.ajax({
        url: '/api/PolygonLayers/',
        type: 'POST',
        data: JSON.stringify(poly),
        contentType: "application/json;charset=utf-8",
        success: function (data) {
            alert("Success");
        },
        error: function (x, y, z) {
            alert(x + '\n' + y + '\n' + z);
        }
    });
}



