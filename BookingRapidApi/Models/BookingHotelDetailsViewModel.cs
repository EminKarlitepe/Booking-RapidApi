using System.Collections.Generic;

namespace BookingRapidApi.Models
{
    public class BookingHotelDetailsApiResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public int hotel_id { get; set; }
        public string hotel_name { get; set; }
        public string url { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country_trans { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int review_nr { get; set; }
        public Product_Price_Breakdown product_price_breakdown { get; set; }
        public Hotel_Text hotel_text { get; set; }
        public Rooms rooms { get; set; }
        public Facilities_Block facilities_block { get; set; }
        public string arrival_date { get; set; }
        public string departure_date { get; set; }
        public string currency_code { get; set; }
        public Breakfast_Review_Score breakfast_review_score { get; set; }
        public Wifi_Review_Score wifi_review_score { get; set; }
        public double? review_score { get; set; } // Hata düzeltmesi eklendi
        public string review_score_word { get; set; } // Hata düzeltmesi eklendi
    }

    public class Product_Price_Breakdown
    {
        public Gross_Amount gross_amount { get; set; }
    }

    public class Gross_Amount
    {
        public float value { get; set; }
        public string currency { get; set; }
    }

    public class Hotel_Text
    {
        public string content { get; set; } // Hata düzeltmesi eklendi
    }

    public class Rooms
    {
        public Dictionary<string, RoomDetail> AdditionalRooms { get; set; }
    }

    public class RoomDetail
    {
        public Photo[] photos { get; set; }
        public Facility1[] facilities { get; set; }
        public string description { get; set; }
    }

    public class Photo
    {
        public string url_max1280 { get; set; }
    }

    public class Facility1
    {
        public string name { get; set; }
    }

    public class Facilities_Block
    {
        public Facility[] facilities { get; set; }
    }

    public class Facility
    {
        public string name { get; set; }
    }

    public class Breakfast_Review_Score
    {
        public string review_score_word { get; set; }
        public int review_score { get; set; }
    }

    public class Wifi_Review_Score
    {
        public float rating { get; set; }
    }

    public class BookingHotelDetailsViewModel
    {
        public string HotelId { get; set; }
        public string HotelName { get; set; }
        public string Description { get; set; }
        public List<string> PhotoUrls { get; set; } = new List<string>();
        public double? ReviewScore { get; set; }
        public string ReviewScoreWord { get; set; }
        public int ReviewCount { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public List<string> Amenities { get; set; } = new List<string>();
        public string CheckinDate { get; set; }
        public string CheckoutDate { get; set; }
        public string BreakfastReviewScoreWord { get; set; }
        public double WifiRating { get; set; }
    }
}