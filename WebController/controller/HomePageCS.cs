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

            var command = new Command<string>(o => pp(o));

            var settings = new ToolbarItem
            {
                Text = "Add",
                Command = command,
				CommandParameter = currentPath,
			};


            ToolbarItems.Add(settings);

            browser = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //if (App.UserEntity.Url)

                Source = App.UserEntity.Url
            };

            Debug.WriteLine("path is " + App.UserEntity.Url + currentPath);
            this.Content = browser;
        }

        private void pp(string o)
        {
            Debug.WriteLine("currentPath is " + currentPath);
        }



		 

		protected override void OnAppearing()
		{
			base.OnAppearing();
            browser.Source = App.UserEntity.Url;
		}

    }
}

