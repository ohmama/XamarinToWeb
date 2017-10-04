using System;
using System.Collections.Generic;
using FormsPinView.PCL;
using Xamarin.Forms;
using System.Diagnostics;

namespace WebController
{
    public partial class PinLoginCS:ContentPage
    {
        public PinLoginCS()
        {
            InitializeComponent();
        }

		public PinViewModel PinViewModel { get; private set; } = new PinViewModel
		{
			TargetPinLength = 4,
			ValidatorFunc = (arg) =>
			{
                //TODO Check entered pin
				return true;
			}
		};

    }
}
