///<refernce path="../../typings/jquery/jquery.d.ts"/>
///<refernce path="../../typings/leaflet/leaflet.d.ts"/>

module LSDMap.Home {
    export class Index {
        map: L.Map;
        boundaries: L.MultiPolygon;
        boundaryLabels: L.LayerGroup;
        markers: L.MarkerClusterGroup;
        constructor(public container: JQuery) {
            L.Icon.Default.imagePath = "/Content/images";
            this.map = L.map("map").setView(L.latLng([47, -100]), 4);

            var osm = new L.TileLayer("http://{s}.tile.osm.org/{z}/{x}/{y}.png", {
                attribution: "&copy; <a href='http://osm.org/copyright'>OpenStreetMap</a> contributors"
            }).addTo(this.map);
            var baseLayers = { "OpenStreeMap": osm };

            this.boundaries = L.multiPolygon([], { color: "blue", fillOpacity: 0, weight: 2, clickable: false });
            this.boundaries.addTo(this.map);

            this.boundaryLabels = L.layerGroup([]);
            this.boundaryLabels.addTo(this.map);

            this.markers = new L.MarkerClusterGroup();//random place markers
            this.markers.addLayer(new L.Marker(L.latLng([55.465678, -118.345802])));
            this.markers.addLayer(new L.Marker(L.latLng([55.465678, -118.645802])));
            this.markers.addLayer(new L.Marker(L.latLng([55.465678, -118.145802])));
            this.markers.addLayer(new L.Marker(L.latLng([55.465678, -106.145802])));
            this.markers.addLayer(new L.Marker(L.latLng([54.465678, -106.245802])));
            this.markers.addLayer(new L.Marker(L.latLng([55.965678, -106.745802])));
            this.markers.addLayer(new L.Marker(L.latLng([50, -100.6])));
            this.markers.addLayer(new L.Marker(L.latLng([51, -100])));
            this.markers.addLayer(new L.Marker(L.latLng([55, -100])));
            this.markers.addTo(this.map);

            var overlays = { "Boundaries": this.boundaries, 'Boundary Labels': this.boundaryLabels, 'Markers': this.markers };

            L.control.layers(baseLayers, overlays).addTo(this.map);

            this.map.addEventListener("zoomend", (e) => {
                if (this.map.getZoom() >= 10)
                    this.GetBoundaries();
                if (this.map.getZoom() < 10) 
                    this.ClearBoundaries();
            });
            this.map.addEventListener("dragend", (e) => {
                if (this.map.getZoom() >= 10)
                    this.GetBoundaries();
            });

            
        }

        ClearBoundaries() {
            this.boundaries.setLatLngs([]);
            this.boundaryLabels.clearLayers();
        }

        GetBoundaries() {
            var data = {
                zoomLevel: this.map.getZoom(),
                northEast: {
                    Lat: this.map.getBounds().getNorthEast().lat, Lng: this.map.getBounds().getNorthEast().lng
                },
                northWest: {
                    Lat: this.map.getBounds().getNorthWest().lat, Lng: this.map.getBounds().getNorthWest().lng
                },
                southEast: {
                    Lat: this.map.getBounds().getSouthEast().lat, Lng: this.map.getBounds().getSouthEast().lng
                },
                southWest: {
                    Lat: this.map.getBounds().getSouthWest().lat, Lng: this.map.getBounds().getSouthWest().lng
                },
            };
            $.getJSON("/api/Boundaries", data, (json) => this.PlotPoints(json)); 
        }

        PlotPoints(data) {
            var latLongs = [];
            this.boundaryLabels.clearLayers();
            for (var i = 0; i < data.length; i++)
            {
                var points = [];
                for(var j = 0; j<data[i].Coordinates.length; j++)
                {
                    points.push(L.latLng(data[i].Coordinates[j].Lat,
                                         data[i].Coordinates[j].Lng));
                }
                latLongs.push(points);
                var iconOptions: L.IconOptions = { iconUrl: "/Content/images/marker-icon.png", iconSize: new L.Point(0, 0)};
                var icon: L.Icon = L.icon(iconOptions);
                var marker = L.marker(L.latLng([data[i].CenterCoordinates.Lat, data[i].CenterCoordinates.Lng]), {icon: icon});
                marker.bindLabel(data[i].Name, { noHide: true, offset: [0,0]});
                this.boundaryLabels.addLayer(marker);
            }
            this.boundaries.setLatLngs(latLongs);
        }
    }
}