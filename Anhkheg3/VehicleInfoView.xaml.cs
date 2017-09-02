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
	public sealed partial class VehicleInfoView : Page
	{
		Vehicle SelectedVehicle;

		public VehicleInfoView()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter == null)
			{
				SelectedVehicle = null;
				CommandLabel.Text = "Add New Vehicle";
			}
			else
			{
				SelectedVehicle = e.Parameter as Vehicle;
				VehicleName.Text = SelectedVehicle.Name;
				VehicleYear.Text = SelectedVehicle.Year.ToString();
				VehicleMake.Text = SelectedVehicle.Make;
				VehicleModel.Text = SelectedVehicle.Model;
				StartingMileage.Text = SelectedVehicle.StartingMileage.ToString();
				CommandLabel.Text = "Edit " + SelectedVehicle.Name;
			}
			base.OnNavigatedTo(e);
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			// Go back to the main page.
			this.Frame.Navigate(typeof(MainPage));
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			using (var db = new DbSchema())
			{
				if (SelectedVehicle == null)
				{
					SelectedVehicle = new Vehicle
					{
						Name = VehicleName.Text,
						Year = Convert.ToInt32(VehicleYear.Text),
						Make = VehicleMake.Text,
						Model = VehicleModel.Text,
						StartingMileage = Convert.ToInt32(StartingMileage.Text)
					};

					db.Vehicles.Add(SelectedVehicle);
				}
				else
				{
					// Grab the info.
					SelectedVehicle.Name = VehicleName.Text;
					SelectedVehicle.Year = Convert.ToInt32(VehicleYear.Text);
					SelectedVehicle.Make = VehicleMake.Text;
					SelectedVehicle.Model = VehicleModel.Text;
					SelectedVehicle.StartingMileage = Convert.ToInt32(StartingMileage.Text);

					// Mark the entry as having been modified.
					// Note that it is not worth trying to figure out if the
					// user really changed something or not.
					db.Entry(SelectedVehicle).State = EntityState.Modified;
				}

				db.SaveChanges();
			}

			// Go back to the main page.
			this.Frame.Navigate(typeof(MainPage));
		}
	}
}
