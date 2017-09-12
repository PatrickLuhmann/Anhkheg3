using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.EntityFrameworkCore;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Anhkheg3
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PurchaseInfoView : Page
	{
		Purchase SelectedPurchase;

		public PurchaseInfoView()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter == null)
			{
				SelectedPurchase = null;
				CommandLabel.Text = "Add New Fuel Purchase";
			}
			else
			{
				SelectedPurchase = e.Parameter as Purchase;
				Date.Date = SelectedPurchase.Date;
				Gallons.Text = SelectedPurchase.Gallons.ToString();
				Cost.Text = SelectedPurchase.Cost.ToString();
				Trip.Text = SelectedPurchase.Trip.ToString();
				Odometer.Text = SelectedPurchase.Odometer.ToString();
				CommandLabel.Text = "Edit Purchase On " + SelectedPurchase.Date.ToString();
			}
			base.OnNavigatedTo(e);
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			using (var db = new DbSchema())
			{
				if (SelectedPurchase == null)
				{
					DateTimeOffset tempDate = (DateTimeOffset)Date.Date;
					SelectedPurchase = new Purchase
					{
						Date = tempDate.Date,
						Gallons = Convert.ToDecimal(Gallons.Text),
						Cost = Convert.ToDecimal(Cost.Text),
						Trip = Convert.ToDecimal(Trip.Text),
						Odometer = Convert.ToInt32(Odometer.Text)
					};

					db.Purchases.Add(SelectedPurchase);
				}
				else
				{
					// Grab the info.
					DateTimeOffset tempDate = (DateTimeOffset)Date.Date;
					SelectedPurchase.Date = tempDate.Date;
					SelectedPurchase.Gallons = Convert.ToDecimal(Gallons.Text);
					SelectedPurchase.Cost = Convert.ToDecimal(Cost.Text);
					SelectedPurchase.Trip = Convert.ToDecimal(Trip.Text);
					SelectedPurchase.Odometer = Convert.ToInt32(Odometer.Text);

					// Mark the entry as having been modified.
					// Note that it is not worth trying to figure out if the
					// user really changed something or not.
					db.Entry(SelectedPurchase).State = EntityState.Modified;
				}

				db.SaveChanges();
			}

			// Go back to the main page.
			this.Frame.Navigate(typeof(MainPage));
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			// Go back to the main page.
			this.Frame.Navigate(typeof(MainPage));
		}
	}
}
