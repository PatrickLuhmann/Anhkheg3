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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Anhkheg3
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class VehicleInfoView : Page
	{
		public VehicleInfoView()
		{
			this.InitializeComponent();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			// TODO: Discard any info, or does it just go away on its own?

			// Go back to the main page.
			this.Frame.Navigate(typeof(MainPage));
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			using (var db = new DbSchema())
			{
				var vehicle = new Vehicle
				{
					Name = VehicleName.Text,
					Year = Convert.ToInt32(VehicleYear.Text),
					Make = VehicleMake.Text,
					Model = VehicleModel.Text,
					StartingMileage = Convert.ToInt32(StartingMileage.Text)
				};

				db.Vehicles.Add(vehicle);
				db.SaveChanges();
			}
		}
	}
}
