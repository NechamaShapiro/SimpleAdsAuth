using SimpleAdsAuth.Data;

namespace SimpleAdsAuth.Web.Models
{
    public class HomePageViewModel
    {
        public List<Ad> Ads { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
