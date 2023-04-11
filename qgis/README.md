Make Corsica map

Coastline: get from https://osmdata.openstreetmap.de/data/land-polygons.html

File: land_polygons.shp (1.1GB)

Crop the polygons and save to geojson format

```
$ ogr2ogr -clipsrc 8.217773 41.323201 9.725647 43.082931 corsica.geojson land_polygons.shp
```

Result: corsica.geojson (2.8MB)




