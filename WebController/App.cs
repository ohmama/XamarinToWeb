using Xamarin.Forms;
using System.Collections.Generic;

namespace WebController
{
	public class App : Application
	{
		public static bool IsUserLoggedIn { get; set; }
		public static UserEntity UserEntity { get; set; }
		public static List<PathEntity> PathList { get; set; }

		public App ()
		{
			
			MainPage = new NavigationPage(new LoginPageCS());
			
			//MainPage = new LoginPageCS();

			//if (!IsUserLoggedIn)
			//{
			//	MainPage = new NavigationPage(new LoginPage());
			//}
			//else
			//{
			//	MainPage = new NavigationPage(new LoginNavigation.MainPage());
			//}
		}



		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

