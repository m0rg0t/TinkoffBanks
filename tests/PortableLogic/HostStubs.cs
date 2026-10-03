using System;
namespace GalaSoft.MvvmLight { }
namespace GART.Data { public class ARItem { public string Content {get;set;} } }
namespace System.Device.Location { public class GeoCoordinate { public GeoCoordinate(double latitude, double longitude) { throw new NotSupportedException("Location is outside portable test scope"); } public double GetDistanceTo(GeoCoordinate other) { throw new NotSupportedException(); } } }
namespace TinkoffBanks.ViewModel { public static class ViewModelLocator { public static Coordinates MainStatic = new Coordinates(); } public class Coordinates { public double Latitued; public double Longitude; } }
