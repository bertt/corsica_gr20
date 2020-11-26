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
    public class Program
    {

        static void Main(string[] args)
        {
            // var stage = 1;
            var input_files = Directory.GetFiles("../data/input");
            var stops = new string[] {"Calenzana", "Refuge Orto di u Piobbu", "Refuge Carrozu"};
            var stopPoints = new List<GpxWaypoint>();
            var stop_id = 0;
            var random = new Random();

            foreach(var file in input_files){
                var stagestops = new string[2]{stops[stop_id], stops[stop_id+1] };
                var randomColor = String.Format("#{0:X6}", random.Next(0x1000000));
                stopPoints.AddRange(WriteStage(file, stop_id==0, stagestops, randomColor));
                stop_id++;
            }

            WriteStopPoints(stopPoints);
        }

        private static List<GpxWaypoint> WriteStage(string file, bool isFirst, string[] stops, string color){
            var resstop = new List<GpxWaypoint>();

            var stage = Int32.Parse(Path.GetFileNameWithoutExtension(file).Split('_')[1]); 
            var gpxText = File.ReadAllText(file);
            var gpx = NetTopologySuite.IO.GpxFile.Parse(gpxText, null);
            var waypoints = gpx.Tracks[0].Segments[0].Waypoints;

            if(isFirst){
                var waypoint = waypoints.First();
                resstop.Add(waypoint.WithName(stops[0]));
                isFirst = false;
            }
            var last_waypoint = waypoints.Last();
            resstop.Add(last_waypoint.WithName(stops[1]));

            var fc = new FeatureCollection();

            var positions = new List<IPosition>();
            
            foreach(var waypoint in waypoints){
                var pos = GetPosition(waypoint);
                positions.Add(pos);
            }
            var linestring = new LineString(positions);
            var f = new Feature(linestring);
            f.Properties.Add("color",color);
            f.Properties.Add("thickness", 4);
            fc.Features.Add(f);
            var json = JsonConvert.SerializeObject(fc);
            File.WriteAllText($"../data/stage_{stage}.json", json);
            return resstop;
        }

        private static void WriteStopPoints(List<GpxWaypoint> stopPoints) {
            var fc = new FeatureCollection();

            foreach(var stop in stopPoints){
                var stopFeature = new Feature(new Point(GetPosition(stop))); 
                stopFeature.Properties.Add("name", stop.Name);
                stopFeature.Properties.Add("fontsize", 20);
                stopFeature.Properties.Add("background", "black");
                fc.Features.Add(stopFeature);
            }

             var json = JsonConvert.SerializeObject(fc);
             File.WriteAllText("../data/stops.json", json);
        }
        

        private static Position GetPosition(GpxWaypoint waypoint){
            return new Position(waypoint.Latitude, waypoint.Longitude);
        }
    }
}
