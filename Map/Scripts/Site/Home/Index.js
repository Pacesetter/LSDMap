///<refernce path="../../typings/jquery/jquery.d.ts"/>
///<refernce path="../../typings/leaflet/leaflet.d.ts"/>
var LSDMap;
(function (LSDMap) {
    (function (Home) {
        var Index = (function () {
            function Index(container) {
                this.container = container;
                var map = L.map("map").setView(L.latLng([51.505, -110.09]), 8);

                var osm = new L.TileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png", {
                    attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap</a> contributors"
                }).addTo(map);
                var baseLayers = { "OpenStreeMap": osm };

                var box = L.polygon([
                    L.latLng([51, -110]),
                    L.latLng([49, -110]),
                    L.latLng([49, -108]),
                    L.latLng([51, -108]),
                    L.latLng([51, -110])], { color: "red", fillOpacity: 0 }).addTo(map);
                var overlays = { "LSDs": box };

                L.control.layers(baseLayers, overlays).addTo(map);
            }
            return Index;
        })();
        Home.Index = Index;
    })(LSDMap.Home || (LSDMap.Home = {}));
    var Home = LSDMap.Home;
})(LSDMap || (LSDMap = {}));
//# sourceMappingURL=Index.js.map
