using System;

using Xamarin.Forms;

namespace MasterDetailPageNavigation
{
    public class WebViewCS : ContentPage
    {
        public WebViewCS()
        {
			var browser = new WebView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Source = "http://xamarin.com"
			};
			this.Content = browser;
        }
    }
}

