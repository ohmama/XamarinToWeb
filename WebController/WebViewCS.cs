using System;

using Xamarin.Forms;

namespace WebController
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

