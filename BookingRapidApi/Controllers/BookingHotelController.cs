using BookingRapidApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BookingRapidApi.Controllers
{
    public class BookingHotelController : Controller
    {
        private readonly string _apiKey = "Your Api";
        private readonly string _apiHost = "booking-com15.p.rapidapi.com";

        [HttpGet]
        public IActionResult Index()
        {
            return View(new BookingHotelViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(
            string p,
            string dateIn,
            string dateOut,
            int guest,
            int children
        )
        {
            if (string.IsNullOrWhiteSpace(p) || string.IsNullOrWhiteSpace(dateIn) || string.IsNullOrWhiteSpace(dateOut) || guest <= 0)
            {
                ModelState.AddModelError("", "Lütfen tüm gerekli alanları doldurun ve yetişkin sayısını 1 veya daha fazla girin.");
                return View(new BookingHotelViewModel());
            }

            string destId = string.Empty;
            string childrenAgeString = "";

            // Location Search Request
            using (var client = new HttpClient())
            {
                var locationRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://{_apiHost}/api/v1/hotels/searchDestination?query={Uri.EscapeDataString(p)}&locale=en-us"),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };
                using (var locationResponse = await client.SendAsync(locationRequest))
                {
                    locationResponse.EnsureSuccessStatusCode();
                    var locationBody = await locationResponse.Content.ReadAsStringAsync();
                    var locationData = JsonConvert.DeserializeObject<LocationSearchViewModel>(locationBody);

                    var primaryLocation = locationData?.data?.FirstOrDefault(d => d.type == "ci" && d.name.Equals(p, StringComparison.OrdinalIgnoreCase));

                    if (primaryLocation == null)
                    {
                        primaryLocation = locationData?.data?.FirstOrDefault(d => !string.IsNullOrEmpty(d.dest_id));
                    }

                    if (primaryLocation != null)
                    {
                        destId = primaryLocation.dest_id;
                    }
                    else
                    {
                        ModelState.AddModelError("", $"'{p}' şehri için konum bulunamadı. Lütfen başka bir şehir deneyin.");
                        return View(new BookingHotelViewModel());
                    }
                }
            }

            if (children > 0)
            {
                childrenAgeString = "&children_age=" + string.Join("%2C", Enumerable.Repeat("0", children));
            }

            BookingHotelViewModel bookingData = new BookingHotelViewModel();

            // Hotel Search Request
            using (var client = new HttpClient())
            {
                var hotelSearchRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://{_apiHost}/api/v1/hotels/searchHotels?" +
                                         $"dest_id={destId}&search_type=CITY&arrival_date={dateIn}&departure_date={dateOut}" +
                                         $"&adults={guest}{childrenAgeString}&room_qty=1&page_number=1&units=metric" +
                                         $"&temperature_unit=c&languagecode=en-us&currency_code=EUR&location=US"),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };
                using (var hotelSearchResponse = await client.SendAsync(hotelSearchRequest))
                {
                    hotelSearchResponse.EnsureSuccessStatusCode();
                    var hotelSearchBody = await hotelSearchResponse.Content.ReadAsStringAsync();
                    bookingData = JsonConvert.DeserializeObject<BookingHotelViewModel>(hotelSearchBody);
                }
            }

            if (bookingData != null && bookingData.data?.hotels != null)
            {
                foreach (var hotel in bookingData.data.hotels)
                {
                    hotel.property.checkinDate = dateIn;
                    hotel.property.checkoutDate = dateOut;
                }
            }

            return View(bookingData);
        }

        [HttpGet]
        public async Task<IActionResult> HotelDetails(string hotelId, string checkinDate, string departureDate)
        {
            if (string.IsNullOrEmpty(hotelId) || string.IsNullOrEmpty(checkinDate) || string.IsNullOrEmpty(departureDate))
            {
                return RedirectToAction("Index");
            }

            var viewModel = new BookingHotelDetailsViewModel
            {
                HotelId = hotelId,
                CheckinDate = checkinDate,
                CheckoutDate = departureDate,
                PhotoUrls = new List<string>(),
                Amenities = new List<string>()
            };

            // Hotel Details Request
            using (var client = new HttpClient())
            {
                string detailsApiUrl = $"https://{_apiHost}/api/v1/hotels/getHotelDetails?" +
                                       $"hotel_id={hotelId}&" +
                                       $"arrival_date={checkinDate}&" +
                                       $"departure_date={departureDate}&" +
                                       "adults=1&" +
                                       "children_age=1%2C17&" +
                                       "room_qty=1&" +
                                       "units=metric&" +
                                       "temperature_unit=c&" +
                                       "languagecode=en-us&" +
                                       "currency_code=EUR";

                var detailsRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(detailsApiUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };
                using (var detailsResponse = await client.SendAsync(detailsRequest))
                {
                    detailsResponse.EnsureSuccessStatusCode();
                    var detailsBody = await detailsResponse.Content.ReadAsStringAsync();
                    var detailsApiResponse = JsonConvert.DeserializeObject<BookingHotelDetailsApiResponse>(detailsBody);

                    if (detailsApiResponse != null && detailsApiResponse.status && detailsApiResponse.data != null)
                    {
                        var data = detailsApiResponse.data;
                        viewModel.HotelName = data.hotel_name;
                        viewModel.Address = data.address;
                        viewModel.City = data.city;
                        viewModel.Country = data.country_trans;
                        viewModel.ReviewCount = data.review_nr;
                        viewModel.Currency = data.currency_code;
                        viewModel.Description = data.hotel_text?.content;

                        if (data.product_price_breakdown?.gross_amount != null)
                        {
                            viewModel.Price = (decimal)data.product_price_breakdown.gross_amount.value;
                            viewModel.Currency = data.product_price_breakdown.gross_amount.currency;
                        }

                        viewModel.ReviewScore = data.review_score;
                        viewModel.ReviewScoreWord = data.review_score_word;

                        viewModel.BreakfastReviewScoreWord = data.breakfast_review_score?.review_score_word;
                        viewModel.WifiRating = data.wifi_review_score?.rating ?? 0.0;

                        if (data.facilities_block?.facilities != null)
                        {
                            viewModel.Amenities.AddRange(data.facilities_block.facilities.Select(f => f.name));
                        }

                        if (data.rooms?.AdditionalRooms != null)
                        {
                            foreach (var roomDetail in data.rooms.AdditionalRooms.Values)
                            {
                                if (roomDetail.photos != null)
                                {
                                    foreach (var photo in roomDetail.photos)
                                    {
                                        if (!string.IsNullOrEmpty(photo.url_max1280))
                                        {
                                            viewModel.PhotoUrls.Add(photo.url_max1280);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            using (var client = new HttpClient())
            {
                string photosApiUrl = $"https://{_apiHost}/api/v1/hotels/getHotelPhotos?hotel_id={hotelId}";
                var photosRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(photosApiUrl),
                    Headers =
                    {
                        { "x-rapidapi-key", _apiKey },
                        { "x-rapidapi-host", _apiHost },
                    },
                };
                using (var photosResponse = await client.SendAsync(photosRequest))
                {
                    photosResponse.EnsureSuccessStatusCode();
                    var photosBody = await photosResponse.Content.ReadAsStringAsync();
                    var photosApiResponse = JsonConvert.DeserializeObject<HotelPhotoApiResponse>(photosBody);

                    if (photosApiResponse != null && photosApiResponse.status && photosApiResponse.data != null)
                    {
                        foreach (var photo in photosApiResponse.data)
                        {
                            if (!string.IsNullOrEmpty(photo.url) && !viewModel.PhotoUrls.Contains(photo.url))
                            {
                                viewModel.PhotoUrls.Add(photo.url);
                            }
                        }
                    }
                }
            }

            return View(viewModel);
        }
    }
}