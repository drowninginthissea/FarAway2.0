document.addEventListener("DOMContentLoaded", function () {
    initMaps();
});

async function initMaps() {
    await ymaps3.ready;

    const { YMap, YMapDefaultSchemeLayer, YMapDefaultFeaturesLayer, YMapMarker } = ymaps3;
    const maps = document.querySelectorAll(".map");

    maps.forEach(mapElement => {
        console.log(mapElement.dataset.coordinates)
        const coordinates = mapElement.dataset.coordinates.split(' ').map(parseFloat);
        const map = new YMap(
            mapElement,
            {
                location: {
                    center: [coordinates[0], coordinates[1]], // используем координаты парковки
                    zoom: 10
                }
            }
        );
        map.addChild(new YMapDefaultSchemeLayer());
        map.addChild(new YMapDefaultFeaturesLayer());

        const content = document.createElement('div');
        content.className = 'custom-marker';

        const marker = new YMapMarker(
            {
                coordinates: [coordinates[0], coordinates[1]],
            },
            content
        );

        map.addChild(marker);

        content.innerHTML = `
                            <div class="marker-icon">
                                <img src="/assets/icons/location.png" alt="Icon" />
                            </div>
                        `;
    });
}
