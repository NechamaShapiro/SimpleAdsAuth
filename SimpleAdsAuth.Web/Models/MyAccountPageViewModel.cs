using SimpleAdsAuth.Data;

namespace SimpleAdsAuth.Web.Models
{
    public class MyAccountPageViewModel
    {
        public List<Ad> MyAds { get; set; }
        public User MyUserInfo { get; set; }
    }
}
