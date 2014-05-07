///<refernce path="../../typings/jquery/jquery.d.ts"/>
///<refernce path="../../typings/leaflet/leaflet.d.ts"/>

module LSDMap.Home {
    export class Index {
        map : L.Map;
        constructor(public container: JQuery) {
            this.map = L.map("map").setView(L.latLng([51.505, -110.09]), 8);

            var osm = new L.TileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png", {
                attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap</a> contributors"
            }).addTo(this.map);
            var baseLayers = { "OpenStreeMap": osm };


            var box = L.polygon([L.latLng([51, -110]),
                L.latLng([49, -110]),
                L.latLng([49, -108]),
                L.latLng([51, -108]),
                L.latLng([51, -110])], { color: "red", fillOpacity: 0 }).addTo(this.map);
            var overlays = { "LSDs": box };

            L.control.layers(baseLayers, overlays).addTo(this.map);

            this.map.addEventListener("zoomend", (e) => {
                if (this.map.getZoom() == 10)
                    this.GetBoundaries();
            });
        }

        GetBoundaries() {
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
                },
            };
            $.getJSON("/api/Boundaries", data, (json) => console.dir(json)); 
        }
    }
}