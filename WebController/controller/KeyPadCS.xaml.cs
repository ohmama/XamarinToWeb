using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace WebController
{
    public partial class KeyPadCS : ContentPage
    {
        Button btnRegister;

        public KeyPadCS()
        {
            InitializeComponent();
            Title = "Login";

            btnRegister = this.FindByName<Button>("");
        }

		// click REGISTER
		async void Register_ClickAsync(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterPageCS(this));
		}
    }
}
