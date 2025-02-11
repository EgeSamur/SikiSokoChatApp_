namespace SikiSokoChatApp.Shared.Helpers;

public static class GeographyHelper
{
    private const double EarthRadiusKm = 6371.0;

    public static (double Latitude, double Longitude) GenerateRandomPoint(double latitude, double longitude, double maxDistanceKm)
    {
        var random = new Random();
        
        // Rastgele bir açı (bearing) [0, 360) derece arasında
        double bearing = random.NextDouble() * 2 * Math.PI;
        
        // Rastgele bir mesafe [0, maxDistanceKm) km arasında
        double distanceKm = random.NextDouble() * maxDistanceKm;

        // Mesafe radyan olarak
        double distanceRad = distanceKm / EarthRadiusKm;

        // Başlangıç koordinatları radyan olarak
        double latRad = ToRadians(latitude);
        double lonRad = ToRadians(longitude);

        // Yeni koordinatları hesapla
        double newLatRad = Math.Asin(Math.Sin(latRad) * Math.Cos(distanceRad) + Math.Cos(latRad) * Math.Sin(distanceRad) * Math.Cos(bearing));
        double newLonRad = lonRad + Math.Atan2(Math.Sin(bearing) * Math.Sin(distanceRad) * Math.Cos(latRad), Math.Cos(distanceRad) - Math.Sin(latRad) * Math.Sin(newLatRad));

        // Radyanları dereceye çevir
        double newLat = ToDegrees(newLatRad);
        double newLon = ToDegrees(newLonRad);

        return (newLat, newLon);
    }
    
    public static double ConvertKilometersToMiles(double kilometers)
    {
        double milesPerKilometer = 0.621371;
        return kilometers * milesPerKilometer;
    }

    public static double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        double dLat = ToRadians(lat2 - lat1);
        double dLon = ToRadians(lon2 - lon1);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c;
    }
    
    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static double ToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }
}