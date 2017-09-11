using System.Diagnostics;
using Xamarin.Forms;

namespace WebController
{
    public class HomePageCS : ContentPage
    {
        public WebView browser;
        public static HomePathEntiry currentPath;
        public bool isLoaded;

        ActivityIndicator LoadingSpinner = new ActivityIndicator
        {
            Color = Color.Black,
            IsVisible = false,
            IsRunning = false
		};


		public HomePageCS()
        {
            Title = "WebView";
            isLoaded = false;

            // if has parent property, add GO TO PARENT link
            if(currentPath!=null && currentPath.Parent !=null && !currentPath.Parent.Equals(PathItemUI.NO_PARENT)){
                var command = new Command<HomePathEntiry>(o => OnParentClick(o));
				var settings = new ToolbarItem
				{
					Text = "Parent",
					Command = command,
					CommandParameter = currentPath,
				};
				ToolbarItems.Add(settings);
			}
            //AbsoluteLayout
            browser = new WebView
            {
                Source = combineUrlWithLogin(App.UserEntity.Url)
            };
            browser.Navigating += webOnNavigating;
            browser.Navigated += webOnEndNavigating;
			AbsoluteLayout.SetLayoutFlags(browser, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(browser,new Rectangle(0,0,1,1));
            AbsoluteLayout.SetLayoutFlags(LoadingSpinner, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(LoadingSpinner, new Rectangle(0, 0, 1, 1));

            if(currentPath == null || currentPath.Path == null){
				Debug.WriteLine("path is " + App.UserEntity.Url);
			}else{
				Debug.WriteLine("path is " + App.UserEntity.Url + currentPath == null ? "" : currentPath.Path);
			}

            this.Content = new AbsoluteLayout
            {
                Padding = new Thickness(0, 0, 0, 0),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = {
					browser,
					LoadingSpinner,
                }
            };
        }

        private void OnParentClick(HomePathEntiry o)
        {
            Debug.WriteLine("currentPath is " + currentPath.Path);
        }

		protected override void OnAppearing()
		{
			base.OnAppearing();
            //browser.Source = App.UserEntity.Url;
		}

        private string combineUrlWithLogin(string url){
            string retUrl = "https://" + App.UserEntity.Name + ":" + App.UserEntity.Password + "@" + Utils.cutHttpstr(url) + "/";
            Debug.WriteLine("request url is " + retUrl);
            return retUrl;
        }

		private string combineUrl(string url)
		{
			string retUrl = "https://" + Utils.cutHttpstr(url) + "/";
			Debug.WriteLine("request url is " + retUrl);
			return retUrl;
		}

		void webOnNavigating(object sender, WebNavigatingEventArgs e)
		{
			//LoadingLabel.IsVisible = true;
            LoadingSpinner.IsVisible = true;
			LoadingSpinner.IsRunning = true;

			Debug.WriteLine("on webOnNavigating");

		}

		void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
            //LoadingLabel.IsVisible = false;

            if(!isLoaded)
                browser.Source = combineUrl(App.UserEntity.Url);
            isLoaded = true;
            LoadingSpinner.IsVisible = false;
            LoadingSpinner.IsRunning = false;
            Debug.WriteLine("on end webOnEndNavigating");
		}
    }



    public class HomePathEntiry{
        public string Path;
        public string Parent;

        public HomePathEntiry(string path, string parent){
            this.Path = path;
            this.Parent = parent;
        }
    }
}

