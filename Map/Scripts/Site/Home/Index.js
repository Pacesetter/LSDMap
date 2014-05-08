///<refernce path="../../typings/jquery/jquery.d.ts"/>
///<refernce path="../../typings/leaflet/leaflet.d.ts"/>
var LSDMap;
(function (LSDMap) {
    (function (Home) {
        var Index = (function () {
            function Index(container) {
                var _this = this;
                this.container = container;
                this.map = L.map("map").setView(L.latLng([47, -100]), 4);

                var osm = new L.TileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png", {
                    attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap</a> contributors"
                }).addTo(this.map);
                var baseLayers = { "OpenStreeMap": osm };

                this.boundaries = L.multiPolygon([], { color: "blue", fillOpacity: 0, weight: 2, clickable: false });
                this.boundaries.addTo(this.map);

                this.markers = L.layerGroup([L.marker(L.latLng([47, -100]))]);
                this.markers.addTo(this.map);

                var overlays = { "Boundaries": this.boundaries, 'Markers': this.markers };

                L.control.layers(baseLayers, overlays).addTo(this.map);

                this.map.addEventListener("zoomend", function (e) {
                    if (_this.map.getZoom() >= 10)
                        _this.GetBoundaries();
                    if (_this.map.getZoom() < 10)
                        _this.ClearBoundaries();
                });
                this.map.addEventListener("dragend", function (e) {
                    if (_this.map.getZoom() >= 10)
                        _this.GetBoundaries();
                });
            }
            Index.prototype.ClearBoundaries = function () {
                this.boundaries.setLatLngs([]);
                this.markers.clearLayers();
            };

            Index.prototype.GetBoundaries = function () {
                var _this = this;
                var data = {
                    zoomLevel: this.map.getZoom(),
                    northEast: {
                        Latitude: this.map.getBounds().getNorthEast().lat, Longitude: this.map.getBounds().getNorthEast().lng
                    },
                    northWest: {
                        Latitude: this.map.getBounds().getNorthWest().lat, Longitude: this.map.getBounds().getNorthWest().lng
                    },
                    southEast: {
                        Latitude: this.map.getBounds().getSouthEast().lat, Longitude: this.map.getBounds().getSouthEast().lng
                    },
                    southWest: {
                        Latitude: this.map.getBounds().getSouthWest().lat, Longitude: this.map.getBounds().getSouthWest().lng
                    }
                };
                $.getJSON("/api/Boundaries", data, function (json) {
                    return _this.PlotPoints(json);
                });
            };

            Index.prototype.PlotPoints = function (data) {
                var latLongs = [];
                this.markers.clearLayers();
                for (var i = 0; i < data.length; i++) {
                    var points = [];
                    for (var j = 0; j < data[i].Coordinates.length; j++) {
                        points.push(L.latLng(data[i].Coordinates[j].Latitude, data[i].Coordinates[j].Longitude));
                    }
                    latLongs.push(points);
                    var iconOptions = { iconUrl: "/Content/images/marker-icon.png", iconSize: new L.Point(0, 0) };
                    var icon = L.icon(iconOptions);
                    var marker = L.marker(L.latLng([data[i].CenterCoordinates.Latitude, data[i].CenterCoordinates.Longitude]), { icon: icon });
                    marker.bindLabel(data[i].Name, { noHide: true, offset: [0, 0] });
                    this.markers.addLayer(marker);
                }
                this.boundaries.setLatLngs(latLongs);
            };
            return Index;
        })();
        Home.Index = Index;
    })(LSDMap.Home || (LSDMap.Home = {}));
    var Home = LSDMap.Home;
})(LSDMap || (LSDMap = {}));
//# sourceMappingURL=Index.js.map
