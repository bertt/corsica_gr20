Make Corsica map

Coastline: get from https://osmdata.openstreetmap.de/data/land-polygons.html

File: land_polygons.shp (1.1GB)

Crop the polygons and save to geojson format

```
$ ogr2ogr -clipsrc 8.217773 41.323201 9.725647 43.082931 corsica.geojson land_polygons.shp
```

Result: corsica.geojson (2.8MB)

Install geojson-merge

```
$ npm install -g geojson-merge
```


Merge the GeoJSON files:

```
$ geojson-merge stage_1.json stage_2.json stage_3.json stage_4.json stage_5.json stage_6.json stage_7.json stage_8.json stage_9.json stage_10.json stage_11.json stage_12.json stage_13.json stage_14.json stage_15.json > all.geojson
```

terrain

Get terrain with script from:

https://portal.opentopography.org/rasterOutput?jobId=rt1681249060085
