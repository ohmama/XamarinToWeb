using System.Diagnostics;
using Xamarin.Forms;

namespace WebController
{
    public class HomePageCS : ContentPage
    {
        public WebView browser;
        public static HomePathEntity currentPath;
        public bool isLoaded;

        ActivityIndicator LoadingSpinner = new ActivityIndicator
        {
            Color = Color.Gray,
            IsVisible = true,
            IsRunning = true
		};


		public HomePageCS()
        {

			Title = "WebView";
            isLoaded = false;

            string loadingUrl = loginUrl();


			//string loadUrl = combineUrlWithLogin(App.UserEntity.Url);
			// if has parent property, add GO TO PARENT link
			if(currentPath!=null && currentPath.Parent !=null && !currentPath.Parent.Equals(PathItemUI.NO_PARENT)){
                var command = new Command<HomePathEntity>(o => OnParentClick());
				var settings = new ToolbarItem
				{
					Text = "Parent",
					Command = command,
					CommandParameter = currentPath,
				};
				ToolbarItems.Add(settings);
			}
            // if this is the tab click, then goto tag page
            if (currentPath!=null && !string.IsNullOrEmpty(currentPath.Path)){
                loadingUrl = loginUrl() + currentPath.Path + "/";
			}
			Debug.WriteLine("first request path is " + loadingUrl);

			//AbsoluteLayout
			browser = new WebView
            {
                Source = loadingUrl
            };
            //browser.Navigating += webOnNavigating;
            browser.Navigated += webOnEndNavigating;
			AbsoluteLayout.SetLayoutFlags(browser, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(browser,new Rectangle(0,0,1,1));
			AbsoluteLayout.SetLayoutFlags(LoadingSpinner, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(LoadingSpinner, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));



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

        private void OnParentClick()
        {
             
            Debug.WriteLine("loading parent path is " + baseUrl() + currentPath.Parent + "/");
            browser.Source = baseUrl() + currentPath.Parent + "/";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
            //browser.Source = App.UserEntity.Url;
		}

        private string loginUrl(){
            return "https://" + App.UserEntity.Name + ":" + App.UserEntity.Password + "@" + Utils.cutHttpstr(App.UserEntity.Url) + "/";
        }

		private string baseUrl()
		{
            return Utils.formatToHttpsUrl(App.UserEntity.Url);
		}

		void webOnNavigating(object sender, WebNavigatingEventArgs e)
		{
            LoadingSpinner.IsVisible = true;
			LoadingSpinner.IsRunning = true;
		}

		void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
            // have to load it again, otherwise page will not show up correctly
            if(!isLoaded){
                string loadingPath = baseUrl();
                if (currentPath != null){
                    loadingPath = baseUrl() + currentPath.Path + "/";
				}
				Debug.WriteLine("webOnEndNavigating request url is " + loadingPath);
				browser.Source = loadingPath;
			}
            isLoaded = true;
            LoadingSpinner.IsVisible = false;
            LoadingSpinner.IsRunning = false;
		}
    }



    public class HomePathEntity{
        public string Path;
        public string Parent;

        public HomePathEntity(string path, string parent){
            this.Path = path;
            this.Parent = parent;
        }
    }
}

