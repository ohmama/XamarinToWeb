using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace WebController
{
	public class LoginPageCS : ContentPage
	{

		IDictionary<Entry, Entry> pwdNext = new Dictionary<Entry, Entry>();
		private YangDb _database;
		bool alertLock;


		Entry pwd1 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};
		Entry pwd2 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};

		Entry pwd3 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};
		Entry pwd4 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};
        Label warning = new Label();
		public LoginPageCS()
		{

			//InitializeComponent();
			_database = new YangDb();
			alertLock = false;
            Title = "Login";			
            Grid grid = new Grid
			{
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = 200,
				HorizontalOptions = LayoutOptions.Center,
			};
			//grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			//grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			//grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			//grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			//grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.RowDefinitions.Add(new RowDefinition { Height = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });


			pwd1.TextChanged += Entry_TextChanged;
			pwd2.TextChanged += Entry_TextChanged;
			pwd3.TextChanged += Entry_TextChanged;
			pwd4.TextChanged += Last_LastEntry;

			pwdNext.Add(pwd1, pwd2);
			pwdNext.Add(pwd2, pwd3);
			pwdNext.Add(pwd3, pwd4);


			grid.Children.Add(pwd1, 0, 0);
			grid.Children.Add(pwd2, 1, 0);
			grid.Children.Add(pwd3, 2, 0);
			grid.Children.Add(pwd4, 3, 0);

			// login button
			var LoginButton = new Button
			{
				Text = "Login"
			};
			var RegisterButton = new Button
			{
				Text = "Register"
			};
			LoginButton.Clicked += Login_CheckAsync;
			RegisterButton.Clicked += Register_ClickAsync;

			RelativeLayout mainStackLayOut = new RelativeLayout
			{
				BackgroundColor = Color.Yellow,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				//Orientation = StackOrientation.Vertical
			};

			Func<View, View, double> getSwitchWidth = (parent, self) => self.Measure(parent.Width, parent.Height).Request.Width;
			Func<View, View, double> getSwitchHeight = (parent, self) => self.Measure(parent.Width, parent.Height).Request.Height;
			mainStackLayOut.Children.Add(grid,
				Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, grid) / 2),
				Constraint.RelativeToParent((parent) => parent.Height / 2 - getSwitchHeight(parent, grid) * 2)
			);


            warning.Text = "";
            warning.TextColor = Color.Red;
            mainStackLayOut.Children.Add(warning,
                                         Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, warning) / 2),
				Constraint.RelativeToParent((parent) => parent.Height / 2 - getSwitchHeight(parent, grid) -  100)
			);

			mainStackLayOut.Children.Add(LoginButton,
										 Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, LoginButton) / 2),
										 Constraint.RelativeToParent(parent => parent.Height / 2));
			mainStackLayOut.Children.Add(RegisterButton,
										 Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, RegisterButton) / 2),
				Constraint.RelativeToParent((parent) => parent.Height - 100)
			);

			this.Content = mainStackLayOut;
            //pwd1.Focus();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pwd1.Focus();
        }


		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
            if (e.NewTextValue.Equals(e.OldTextValue) || (string.IsNullOrWhiteSpace(e.NewTextValue) && (string.IsNullOrWhiteSpace(e.OldTextValue))))
                return;
			Entry thisBlock = ((Entry)sender);
			Entry next = new Entry();
			pwdNext.TryGetValue(thisBlock, out next);
			if (next != null)
				next.Focus();

			if (string.IsNullOrEmpty(thisBlock.Text) || !Utils.IsNumeric(thisBlock.Text))
			{
				thisBlock.Text = "";
			}
			else
			{
				if (e.NewTextValue.Length > 1)
				{
					// new text is surfix the old value
					if (e.NewTextValue.IndexOf(e.OldTextValue, StringComparison.Ordinal) == 0)
					{
						thisBlock.Text = e.NewTextValue.Substring(1, 1);
						Debug.WriteLine(e.NewTextValue);
					}
					else
					{
						thisBlock.Text = thisBlock.Text.Remove(1);
					}
				}
			}
		}

		// last block entry event
		void Last_LastEntry(object sender, TextChangedEventArgs e)
		{
			Entry_TextChanged(sender, e);
			Login_CheckAsync(sender, e);
		}

		// click LOGIN event
		async void Login_CheckAsync(object sender, EventArgs e)
		{
			var isSuccess = false;

			if (!(string.IsNullOrEmpty(pwd1.Text) || string.IsNullOrEmpty(pwd2.Text) || string.IsNullOrEmpty(pwd3.Text) || string.IsNullOrEmpty(pwd4.Text)))
			{
				if (Utils.IsNumeric(pwd1.Text) && Utils.IsNumeric(pwd2.Text) && Utils.IsNumeric(pwd3.Text) && Utils.IsNumeric(pwd4.Text))
				{
					String pin = pwd1.Text + pwd2.Text + pwd3.Text + pwd4.Text;
					var users = _database.GetUsers();
					// todo how go check usr
					foreach (var user in users)
					{
						if (user.Pin.Equals(pin))
						{
							App.IsUserLoggedIn = true;
							isSuccess = true;
                            // set username to application
                            App.UserEntity = user;
							//Navigation.InsertPageBefore(new MainPageCS(), Navigation.NavigationStack.First());
							//await Navigation.PopToRootAsync();
							// grab the paths
							App.PathList = _database.GetPaths(user.ID);
							foreach (var item in App.PathList)
							{
								string ret = "User ID:" + item.UserID + ",Name: " + item.Name + ",Path: " + item.Path;
								Debug.WriteLine(ret);
							}
							//App.Current.MainPage = new NavigationPage(new MainPageCS());
							Application.Current.MainPage = new MainPageCS();
                            break;
							
						}
						else
						{
                            //warning.Text = "Pin not match";
							Debug.WriteLine("Pin not match");
						}
					}
				}
				else
				{
					Debug.WriteLine("Pin only support number");
				}
				Debug.WriteLine("contain empty");
			}

			if (!isSuccess && !alertLock)
			{
				alertLock = true;
				pwd1.Text = "";
				pwd2.Text = "";
				pwd3.Text = "";
				pwd4.Text = "";

				Debug.WriteLine("fail to login");
				await DisplayAlert("Failed", "Please try again", "Ok");
			}
			pwd1.Focus();

		}

		// click REGISTER
		async void Register_ClickAsync(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterPageCS(this));
		}
	}
}
