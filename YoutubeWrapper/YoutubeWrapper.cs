using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using static Google.Apis.YouTube.v3.VideosResource;
using Video = YPlayer.Models.Video;

namespace YPlayer.YoutubeUtility
{
    static class YoutubeWrapper
    {
        const string C_API_KEY = "AIzaSyBqySyGatkWh-mfV5Vc_lyZLjyacmFieDA";
        const string C_APPLICATION_NAME = "YPlayer";

        static YouTubeService youTubeServiceMain = new (new BaseClientService.Initializer() {
            ApiKey = C_API_KEY,
            ApplicationName = C_APPLICATION_NAME
        });

        public static async Task<Video[]> GetTrendingVideosAsync() => await GetVideoListAsync(ListRequest.ChartEnum.MostPopular, 50, "RU");

        static async Task<Video[]> GetVideoListAsync(ListRequest.ChartEnum chart, int maxResults, string regionCode)
        {
            ListRequest list = youTubeServiceMain.Videos.List(new string[] { "snippet", "contentDetails", "statistics" });

            list.Chart = chart;
            list.MaxResults = maxResults;
            list.RegionCode = regionCode;

            VideoListResponse response = await list.ExecuteAsync();

            if (response is null || response.Items is null)
                return Array.Empty<Video>();

            return await Video.FromResponse(response);
        }

        public static async Task<(string, ThumbnailDetails)[]> GetChannelsThumbnailsAsync(string[] channelIds)
        {
            ChannelsResource.ListRequest list = youTubeServiceMain.Channels.List("snippet");
            list.Id = channelIds;
            ChannelListResponse response = await list.ExecuteAsync();
            return response.Items.Select(x => (x.Id, x.Snippet.Thumbnails)).ToArray();
        }
        
        public static Thumbnail GetSmallThumbnail(this ThumbnailDetails thumbnails) => thumbnails.Standard ?? thumbnails.Medium ?? thumbnails.High ?? thumbnails.Maxres ?? thumbnails.Default__;
        public static Thumbnail GetMaxThumbnail(this ThumbnailDetails thumbnails) => thumbnails.Maxres ?? thumbnails.High ?? thumbnails.Medium ?? thumbnails.Standard ?? thumbnails.Default__;
    }
}
