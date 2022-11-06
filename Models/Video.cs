using CommunityToolkit.Maui.Alerts;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Maui.ApplicationModel.DataTransfer;

namespace YPlayer.Models
{
    public struct Video
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailSmall { get; set; }
        public string ThumbnailMax { get; set; }
        public string VideoId { get; set; }
        public string ChannelTitle { get; set; }
        public string ChannelId { get; set; }
        public string ChannelThumbnail { get; set; }
        public string PublishedAt { get; set; }
        public string Duration { get; set; }
        public string ViewCount { get; set; }
        public string LikeCount { get; set; }
        public string DislikeCount { get; set; }

        public static async Task<Video[]> FromResponse(VideoListResponse response)
        {
            Video[] result = new Video[response.Items.Count];
            
            for (int i = 0; i < response.Items.Count; i++)
            {
                Google.Apis.YouTube.v3.Data.Video youtubeVideo = response.Items[i];
                VideoSnippet snippet = youtubeVideo.Snippet;
                VideoContentDetails contentDetails = youtubeVideo.ContentDetails;
                VideoStatistics statistics = youtubeVideo.Statistics;

                result[i] = new Video()
                {
                    Title = snippet.Title,
                    Description = snippet.Description,
                    ThumbnailSmall = snippet.Thumbnails.GetSmallThumbnail().Url,
                    ThumbnailMax = snippet.Thumbnails.GetMaxThumbnail().Url,
                    VideoId = youtubeVideo.Id,
                    ChannelTitle = snippet.ChannelTitle,
                    ChannelId = snippet.ChannelId,
                    PublishedAt = snippet.PublishedAt.ToString(),
                    Duration = contentDetails.Duration,
                    ViewCount = statistics.ViewCount.ToString(),
                    LikeCount = statistics.LikeCount.ToString(),
                    DislikeCount = statistics.LikeCount.ToString()
                };
            }
            var thumbnails = await YoutubeWrapper.GetChannelsThumbnailsAsync(result.Select(x => x.ChannelId).ToArray());

            for (int i = 0; i < thumbnails.Length; i++)
            {
                var result1 = thumbnails.FirstOrDefault(x => x.Item1 == result[i].ChannelId);
                if (result1 == default((string, ThumbnailDetails)))
                    result[i].ChannelThumbnail = "thumbnail_default.png";
                else
                    result[i].ChannelThumbnail = string.IsNullOrWhiteSpace(result1.Item2.Default__.Url) ? "thumbnail_default.png" : result1.Item2.Default__.Url;
            }
            
            return result;
        }
    }
}
