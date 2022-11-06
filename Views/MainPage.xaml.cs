using YPlayer.ViewModels;

namespace YPlayer;

public partial class MainPage : ContentPage
{

	public MainPage(MainPageViewModel model)
	{
		InitializeComponent();
        BindingContext = model;
    }
}