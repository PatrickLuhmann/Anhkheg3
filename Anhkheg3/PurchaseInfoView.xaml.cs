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
		Vehicle SelectedVehicle;
		Purchase SelectedPurchase;

		public PurchaseInfoView()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter is Vehicle)
			{
				SelectedPurchase = null;
				SelectedVehicle = e.Parameter as Vehicle;
				CommandLabel.Text = "Add New Fuel Purchase For " + SelectedVehicle.Name;
			}
			else
			{
				SelectedPurchase = e.Parameter as Purchase;
				SelectedVehicle = SelectedPurchase.Vehicle;
				Date.Date = SelectedPurchase.Date;
				Gallons.Text = SelectedPurchase.Gallons.ToString();
				Cost.Text = SelectedPurchase.Cost.ToString();
				Trip.Text = SelectedPurchase.Trip.ToString();
				Odometer.Text = SelectedPurchase.Odometer.ToString();
				CommandLabel.Text = "Edit Purchase For " + SelectedVehicle.Name + " On " + SelectedPurchase.Date.ToString();
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
					// This is required but I don't know why. If I try to do
					// the Add directly on SelectedVehicle, the database doesn't
					// get modified at all. It is as if the Vehicle objects in the
					// list view are not the same as the Vehicle objects in the database;
					// something is missing that allows the database code to function
					// as expected.
					Vehicle tgt = db.Vehicles.Include(v => v.Purchases).Single(v => v.ID == SelectedVehicle.ID);
					//					Vehicle tgt = db.Vehicles.Find(SelectedVehicle.ID);
					//SelectedPurchase.Vehicle = tgt;
					tgt.Purchases.Add(SelectedPurchase);

					//db.Purchases.Add(SelectedPurchase);
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
			this.Frame.GoBack();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			// Go back to the main page.
			this.Frame.GoBack();
		}
	}
}
