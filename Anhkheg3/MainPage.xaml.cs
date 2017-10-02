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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Anhkheg3
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		static int CurrentVehicleIndex = -1;

		public MainPage()
		{
			this.InitializeComponent();
			System.Diagnostics.Debug.WriteLine("Exit: MainPage() constructor");
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Enter: OnNavigatedTo");
			using (var db = new DbSchema())
			{
				// Show list of all vehicles
				Vehicles.ItemsSource = db.Vehicles.ToList();

				// Show purchases for selected vehicle, if there is one
				if (CurrentVehicleIndex != -1)
					Vehicles.SelectedIndex = CurrentVehicleIndex;
			}

			base.OnNavigatedTo(e);
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			// No parameter because we want a new vehicle to be created.
			this.Frame.Navigate(typeof(VehicleInfoView));
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			if (Vehicles.SelectedIndex != -1)
				// Pass the current vehicle to the vehicle info view via parameter.
				this.Frame.Navigate(typeof(VehicleInfoView), Vehicles.SelectedItem as Vehicle);
		}

		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			Vehicle veh = Vehicles.SelectedItem as Vehicle;
			if (veh != null)
			{
				using (var db = new DbSchema())
				{
					db.Vehicles.Remove(veh);
					db.SaveChanges();
					Vehicles.ItemsSource = db.Vehicles.ToList();
				}

				Vehicles.SelectedIndex = -1;
			}
		}

		private void AddPurchse_Click(object sender, RoutedEventArgs e)
		{
			if (Vehicles.SelectedIndex != -1)
				this.Frame.Navigate(typeof(PurchaseInfoView), Vehicles.SelectedItem as Vehicle);
		}

		private void EditPurchase_Click(object sender, RoutedEventArgs e)
		{
			if (Purchases.SelectedIndex != -1)
			{
				PurchaseSummary p = Purchases.SelectedItem as PurchaseSummary;
				int purId = p.Id;
				this.Frame.Navigate(typeof(PurchaseInfoView), purId);
			}
		}

		private void DeletePurchase_Click(object sender, RoutedEventArgs e)
		{
			Purchase item = (Purchase)Purchases.SelectedItem;
			if (item != null)
			{
				using (var db = new DbSchema())
				{
					db.Purchases.Remove(item);
					db.SaveChanges();
				}

				Purchases.ItemsSource = GetPurchasesForVehicle(Vehicles.SelectedItem as Vehicle);
				Purchases2.ItemsSource = GetPurchasesForVehicle(Vehicles.SelectedItem as Vehicle);
			}
		}

		private List<Purchase> GetPurchasesForVehicle(Vehicle veh)
		{
			List<Purchase> purchList;
			using (var db = new DbSchema())
			{
				purchList = db.Purchases.Where(p => p.Vehicle == veh).ToList();
			}
			return purchList;
		}

		private List<PurchaseSummary> GetPurchaseSummariesForVehicle(Vehicle veh)
		{
			List<Purchase> rawPurchases;
			using (var db = new DbSchema())
			{
				rawPurchases = db.Purchases.Where(p => p.Vehicle == veh).ToList();
			}
			List<PurchaseSummary> purchList = new List<PurchaseSummary>();

			foreach (var rp in rawPurchases)
			{
				PurchaseSummary newPurch = new PurchaseSummary();
				newPurch.Date = rp.Date.ToString("yyyy-MM-dd");
				newPurch.Gallons = rp.Gallons;
				newPurch.Cost = rp.Cost.ToString("F");
				newPurch.Mpg = Math.Round(rp.Trip / rp.Gallons, 1);
				newPurch.Dpg = Math.Round(rp.Cost / rp.Gallons, 2);
				newPurch.Id = rp.ID;
				purchList.Add(newPurch);
			}
			return purchList;
		}

		private void Vehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("Enter: Vehicles_SelectionChanged");

			CurrentVehicleIndex = Vehicles.SelectedIndex;
			if (CurrentVehicleIndex == -1)
			{
				HeaderText.Text = "";
				NumPurchases.Text = "";
				Purchases.ItemsSource = null;
				Purchases2.ItemsSource = null;
			}
			else
			{
				using (var db = new DbSchema())
				{
					NumPurchasesGlobal.Text = "Global purchases: " + db.Purchases.Count().ToString();
				}
				Vehicle selVehicle = Vehicles.SelectedItem as Vehicle;
				HeaderText.Text = "Purchases For " + selVehicle.Name;
				NumPurchases.Text = "This vehicle has " + selVehicle.Purchases.Count.ToString() + " fuel purchases";
				Purchases.ItemsSource = GetPurchaseSummariesForVehicle(selVehicle);
				Purchases2.ItemsSource = GetPurchasesForVehicle(selVehicle);
			}
		}
	}

	public class PurchaseSummary
	{
		public string Date { get; set; }
		public decimal Gallons { get; set; }
		public string Cost { get; set; }
		public decimal Mpg { get; set; }
		public decimal Dpg { get; set; }
		public int Id { get; set; } // lower case because this is not a true database object
	}
}
