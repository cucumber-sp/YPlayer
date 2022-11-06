using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace YPlayer.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        public RangeObservableCollection<Video> Videos { get; } = new();

        public MainPageViewModel()
        {
            Title = "YPlayer";
        }

        [RelayCommand]
        async Task LoadTrendingVideosAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            Video[] result = await YoutubeWrapper.GetTrendingVideosAsync();

            Videos.Clear();
            Videos.AddRange(result);

            IsBusy = false;
        }
    }
}
