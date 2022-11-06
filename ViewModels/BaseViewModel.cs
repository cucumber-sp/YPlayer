namespace YPlayer.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty, NotifyPropertyChangedFor(nameof(IsNotBusy))] bool isBusy;
        public bool IsNotBusy => !isBusy;

        [ObservableProperty] string title;
    }
}
