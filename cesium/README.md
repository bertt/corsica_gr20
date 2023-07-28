# Corsica Terrain

Process of creating terrain tiles for Corsica

Step 1] Download DTM's 

There are two parts (North/South) at https://geoservices.ign.fr/rgealti

A Download:
```
$ wget --no-check-certificate https://wxs.ign.fr/9u5z4x13jqu05fb3o9cux5e1/telechargement/prepackage/RGEALTI-5M_PACK_CORSE_16-06-2021$RGEALTI_2-0_5M_ASC_LAMB93-IGN78C_D02A_2020-04-16/file/RGEALTI_2-0_5M_ASC_LAMB93-IGN78C_D02A_2020-04-16.7z
```

B Download:
```
$wget --no-check-certificate https://wxs.ign.fr/9u5z4x13jqu05fb3o9cux5e1/telechargement/prepackage/RGEALTI-5M_PACK_CORSE_16-06-2021$RGEALTI_2-0_5M_ASC_LAMB93-IGN78C_D02B_2020-04-16/file/RGEALTI_2-0_5M_ASC_LAMB93-IGN78C_D02B_2020-04-16.7z
```
Unzip and collect the ASC files in a folder (431 files)

Step 2] Process

Commands to process the ASC files to terrain tiles:

```
// convert files from asx to tif
$ time find *.asc | parallel --bar 'gdalwarp -s_srs EPSG:2154 {} {=s:asc:tif:=}'

// warp use epsg:5699 (is EPSG:2154 + EPSG:5721)
$ docker run -it -v $(pwd}:/data geodan/terrainwarp -c EPSG:5699 -m 1

// tile
$ docker run -it -v $(pwd}:/data geodan/terraintiler
```

