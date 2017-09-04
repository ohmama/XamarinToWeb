using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace WebController
{
    public class HomePageCS : ContentPage
    {
        WebView browser;
        public static string currentPath = "";

        public HomePageCS()
        {
			Title = "WebView";
			browser = new WebView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
                //if (App.UserEntity.Url)
                Source = App.UserEntity.Url
			};
            Debug.WriteLine("path is "+ App.UserEntity.Url + "/"+currentPath);
			this.Content = browser;
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            browser.Source = App.UserEntity.Url;
		}

    }
}

