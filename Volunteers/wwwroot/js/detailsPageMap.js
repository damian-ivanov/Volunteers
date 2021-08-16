var mymap = L.map('mapid');
L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoibGktZGFtaWFuIiwiYSI6ImNrc2M0eGdkMzBhaG0ycW80ODR2YW02NXMifQ.5BeIupHNqQt7XkskdhMpfQ', {
    attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
    maxZoom: 18,
    id: 'mapbox/streets-v11',
    tileSize: 512,
    zoomOffset: -1,
    accessToken: 'your.mapbox.access.token'
}).addTo(mymap);

var getCoordinatesElement = document.getElementById('projectCoordinates').innerHTML;

var array = getCoordinatesElement.split(", ");

mymap.setView([array[0], array[1]], 13);
var marker = L.marker([array[0], array[1]]).addTo(mymap);
