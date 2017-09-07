using System;
using Xamarin.Forms;
using System.Diagnostics;

namespace WebController
{
	public class MainPageCS : MasterDetailPage
	{
		public MasterPageCS masterPage;

		public MainPageCS ()
		{
			masterPage = new MasterPageCS ();
			Master = masterPage;
            Detail = new NavigationPage (new HomePageCS ());

			masterPage.ListView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform.Equals("Android"))
			{
				Master.Icon = "swap.png";
			}
			else if (Device.RuntimePlatform.Equals("iOS"))
			{
                Debug.WriteLine($"Onplatform: {Device.RuntimePlatform}");
			}
			else if (Device.RuntimePlatform.Equals("Windows"))
			{
				Debug.WriteLine($"Onplatform: {Device.RuntimePlatform}");
			}

			

		}

        async void OnItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;

			if (item != null) {
                if (item.TargetType == typeof(LoginPageCS))
                {
					var answer = await DisplayAlert("Logout", "Do you want to logout?", "Yes", "No");
		            if(answer==true){
		                App.IsUserLoggedIn = false;
                        //Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                        Application.Current.MainPage = new NavigationPage(new LoginPageCS());
    					masterPage.ListView.SelectedItem = null;
    					IsPresented = false;
					 }
				}else{
					if(item.TargetType == typeof(HomePageCS)){
                        HomePageCS.currentPath = item.PathEntiry;
                    }else{
                        HomePageCS.currentPath = null;
                    }
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
					masterPage.ListView.SelectedItem = null;
					IsPresented = false;
                }
				
			}
		}

        public void refresh(){
            if (masterPage != null)
                masterPage.refresh();
        }
	}
}
