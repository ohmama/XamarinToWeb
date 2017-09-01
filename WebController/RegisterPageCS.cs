using System;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;

namespace WebController
{
	public class RegisterPageCS : ContentPage
	{
		public YangDb _database;

		Entry usernameEntry, passwordEntry, pingEntry, urlEntry;
		Label messageLabel;

		public RegisterPageCS(LoginPageCS parent)
		{
			_database = new YangDb();
			messageLabel = new Label();
			usernameEntry = new Entry
			{
				Placeholder = "Input username",
                Margin = new Thickness(20, 5)
			};
			passwordEntry = new Entry
			{
				Placeholder = "Input password",
				IsPassword = true,
                Margin = new Thickness(20, 5)
			};
			pingEntry = new Entry()
			{
				Placeholder = "Input 4 digit pin",
				Keyboard = Keyboard.Numeric,
                Margin = new Thickness(20, 5)
			};
			pingEntry.TextChanged += Entry_TextChanged;
			urlEntry = new Entry()
			{
				Placeholder = "Input your prefered URL",
                Margin = new Thickness(20, 5)
			};

			var signUpButton = new Button
			{
				Text = "Sign Up"
			};
			signUpButton.Clicked += OnSignUpButtonClicked;

			Title = "Sign Up";

			Content = new StackLayout
			{
                VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
                    new Label { Text = "Username", Margin= new Thickness(20,5) },
					usernameEntry,
					new Label { Text = "Password", Margin= new Thickness(20,5) },
					passwordEntry,
					new Label { Text = "4 Digital Ping", Margin= new Thickness(20,5) },
					pingEntry,
					new Label { Text = "URL", Margin= new Thickness(20,5) },
					urlEntry,
					signUpButton,
					messageLabel
				}
			};
		}

        // register click event
		async void OnSignUpButtonClicked(object sender, EventArgs e)
		{
			var user = new UserEntity()
			{
				Name = usernameEntry.Text,
				Password = passwordEntry.Text,
				Pin = pingEntry.Text,
				Url = urlEntry.Text
			};

			// Sign up logic goes here
			var signUpSucceeded = AreDetailsValid(user);
			if (signUpSucceeded)
			{
				var rootPage = Navigation.NavigationStack.FirstOrDefault();

				_database.AddUser(user);
				int number = _database.GetUsers().Count();
				Debug.WriteLine("we have " + number + "Users");
				if (rootPage != null)
				{
					App.IsUserLoggedIn = true;
                    Navigation.InsertPageBefore(new LoginPageCS(), Navigation.NavigationStack.First());
					await Navigation.PopToRootAsync();
				}
			}
			else
			{
				messageLabel.Text = "Sign up failed";
			}
		}

		bool AreDetailsValid(UserEntity user)
		{
			return (!string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Pin));
		}

		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			Entry thisBlock = ((Entry)sender);
			String pingStr = e.NewTextValue;

			Debug.WriteLine(Utils.IsNumeric(pingStr));
			if (pingStr == null || pingStr.Trim().Length == 0)
			{

			}
			else
			{
				//if(!Utils.IsNumeric(pingStr.Substring((pingStr.Length - 1), 1)))
				if (!Utils.IsNumeric(pingStr))
				{
					thisBlock.Text = e.OldTextValue;
				}
				else if (pingStr.Length > 4)
				{
					thisBlock.Text = pingStr.Remove(4);
				}
			}
		}


	}
}
