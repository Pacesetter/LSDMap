///<refernce path="../../typings/jquery/jquery.d.ts"/>
///<refernce path="../../typings/leaflet/leaflet.d.ts"/>

module LSDMap.Home {
    export class Index {
        constructor(public container:JQuery) {
            var map = L.map('map').setView(L.latLng([51.505, -110.09]), 8);
            
            // add an OpenStreetMap tile layer
            new L.TileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="http://osm.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(map);
        }
    }
}