using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using NetTopologySuite.IO;
using Newtonsoft.Json;

namespace data_processing
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
             var gpx = File.ReadAllText("../test1.gpx");
             var file = NetTopologySuite.IO.GpxFile.Parse(gpx, null);
             var waypoints = file.Tracks[0].Segments[0].Waypoints;
             var fc = new FeatureCollection();

             var positions = new List<IPosition>();
             
             foreach(var waypoint in waypoints){
                 var pos = GetPosition(waypoint);
                 positions.Add(pos);
             }
             var linestring = new LineString(positions);
             var f = new Feature(linestring);
             f.Properties.Add("color","#f30e32");
             f.Properties.Add("thickness", 4);
             fc.Features.Add(f);
             var json = JsonConvert.SerializeObject(fc);
             File.WriteAllText("../stage_1.json", json);

             // write stops

            fc = new FeatureCollection();
            var first = new Feature(new Point(GetPosition(waypoints.First()))); 
            first.Properties.Add("name", "Calenzana");
            first.Properties.Add("fontsize", 20);
            first.Properties.Add("background", "black");

            var last = new Feature(new Point(GetPosition(waypoints.Last()))); 
            last.Properties.Add("name", "Refuge Orto di u Piobbu");
            last.Properties.Add("fontsize", 20);
            last.Properties.Add("background", "black");


            fc.Features.Add(first);
            fc.Features.Add(last);

             json = JsonConvert.SerializeObject(fc);
             File.WriteAllText("../stops.json", json);
        }

        private static Position GetPosition(GpxWaypoint waypoint){
            return new Position(waypoint.Latitude, waypoint.Longitude);
        }
    }
}
