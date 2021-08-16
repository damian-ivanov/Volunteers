var myMarker;
var createCoordinatesElement = document.getElementById('coordinates');

function onMapClick(e) {

    if (myMarker != null) {
        myMarker.remove();
    }

    createCoordinatesElement.value = e.latlng;
    myMarker = L.marker(e.latlng, draggable = true).addTo(mymap);
}

mymap.on('click', onMapClick);