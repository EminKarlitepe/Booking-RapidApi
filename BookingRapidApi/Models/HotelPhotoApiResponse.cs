namespace BookingRapidApi.Models
{
    public class HotelPhotoApiResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public long timestamp { get; set; }
        public List<HotelPhotoData> data { get; set; }
    }

    public class HotelPhotoData
    {
        public int id { get; set; }
        public string url { get; set; }
    }
}