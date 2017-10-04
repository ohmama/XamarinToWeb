using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebController
{
    // develope page is duplicate of Home Page. But do not use the Windows Authentication
	public class DeveloperPageCS : ContentPage
	{
		private YangDb _database;

		public WebView browser;
		//public static HomePathEntity currentPathStr;
		public bool isLoaded;

		ActivityIndicator LoadingSpinner = new ActivityIndicator
		{
			Color = Color.Gray,
			IsVisible = true,
			IsRunning = true
		};


        public void outputData(){
			_database = new YangDb();
			var users = _database.GetUsers();
			foreach (var user in users)
			{
				Debug.WriteLine("user name is " + user.Name
								+ ", pwd is " + user.Password + ", pin is " + user.Pin
								+ ", url is " + user.Url);
			}
        }

		public DeveloperPageCS()
		{
            outputData();

			Title = "WebView";
			isLoaded = false;

			// if has parent property, add GO TO PARENT link
			if (HomePageCS.currentPath != null && HomePageCS.currentPath.Parent != null && !HomePageCS.currentPath.Parent.Equals(PathItemUI.NO_PARENT))
			{
				var command = new Command<HomePathEntity>(o => OnParentClick(o));
				var settings = new ToolbarItem
				{
					Text = "Parent",
					Command = command,
					CommandParameter = HomePageCS.currentPath,
				};
				ToolbarItems.Add(settings);
			}
			//AbsoluteLayout
			browser = new WebView
			{
				Source = combineUrlWithLogin(App.UserEntity.Url)
			};
			//browser.Navigating += webOnNavigating;
			browser.Navigated += webOnEndNavigating;
			AbsoluteLayout.SetLayoutFlags(browser, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(browser, new Rectangle(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(LoadingSpinner, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(LoadingSpinner, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

			if (HomePageCS.currentPath == null || HomePageCS.currentPath.Path == null)
			{
				Debug.WriteLine("path is " + App.UserEntity.Url);
			}
			else
			{
				Debug.WriteLine("path is " + App.UserEntity.Url + HomePageCS.currentPath == null ? "" : HomePageCS.currentPath.Path);
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

		private void OnParentClick(HomePathEntity o)
		{
			Debug.WriteLine("currentPath is " + HomePageCS.currentPath.Path);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private string combineUrlWithLogin(string url)
		{
			string retUrl = "https://" + App.UserEntity.Name + ":" + App.UserEntity.Password + "@" + Utils.cutHttpstr(url) + "/";
			Debug.WriteLine("request url is " + retUrl);
			Debug.WriteLine("username is " + App.UserEntity.Name);
			Debug.WriteLine("password is " + App.UserEntity.Password);
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
			LoadingSpinner.IsVisible = true;
			LoadingSpinner.IsRunning = true;

			Debug.WriteLine("on webOnNavigating");

		}

		void webOnEndNavigating(object sender, WebNavigatedEventArgs e)
		{
			if (!isLoaded)
				browser.Source = combineUrl(App.UserEntity.Url);
			isLoaded = true;
			LoadingSpinner.IsVisible = false;
			LoadingSpinner.IsRunning = false;
			Debug.WriteLine("on end webOnEndNavigating");
		}

		public void Refresh()
		{
		}
	}

}
