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
		int CurrentVehicleIndex = -1;

		public MainPage()
		{
			this.InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			using (var db = new DbSchema())
			{
				var list = db.Vehicles.Include(v => v.Purchases).ToList();
				//List<Vehicle> list = db.Vehicles.ToList();
				Vehicles.ItemsSource = list;

				// TODO: Not global list, just those for selected vehicle.
				if (CurrentVehicleIndex == -1)
				{
					List<Purchase> purchList = db.Purchases.ToList();
					Purchases.ItemsSource = purchList;
					NumPurchasesGlobal.Text = "Global number of puchases: " + purchList.Count.ToString();
				}
				else
				{
					List<Purchase> purchList = db.Purchases.ToList();
					Purchases.ItemsSource = purchList;
					NumPurchasesGlobal.Text = "Global number of puchases: " + purchList.Count.ToString();
				}
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

		}

		private void AddPurchse_Click(object sender, RoutedEventArgs e)
		{
			if (Vehicles.SelectedIndex != -1)
				this.Frame.Navigate(typeof(PurchaseInfoView), Vehicles.SelectedItem as Vehicle);
		}

		private void EditPurchase_Click(object sender, RoutedEventArgs e)
		{
			if (Purchases.SelectedIndex != -1)
				this.Frame.Navigate(typeof(PurchaseInfoView), Purchases.SelectedItem as Purchase);
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

					Purchases.ItemsSource = db.Purchases.ToList(); // TODO: This is wrong
				}
			}
		}

		private List<Purchase> GetPurchasesForVehicle(Vehicle veh)
		{
			List<Purchase> purchList;
			using (var db = new DbSchema())
			{
				Vehicle tgt = db.Vehicles.Find(veh.ID);
				var temp1 = db.Purchases.Where(p => p.Vehicle == tgt);
				purchList = temp1.ToList();
			}
			return purchList;
		}

		private void Vehicles_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CurrentVehicleIndex = Vehicles.SelectedIndex;
			Vehicle selVehicle = Vehicles.SelectedItem as Vehicle;
			HeaderText.Text = "Purchases For " + selVehicle.Name;
			NumPurchases.Text = "This vehicle has " + selVehicle.Purchases.Count.ToString() + " fuel purchases";
#if false

			using (var db = new DbSchema())
			{
				Vehicle tgt = db.Vehicles.Find(selVehicle.ID);
				//var temp1 = db.Purchases.Where(p => p.Vehicle == null);
				var temp1 = db.Purchases.Where(p => p.Vehicle == tgt);
				List<Purchase> purchList = temp1.ToList();
				//List<Purchase> purchList = db.Purchases.ToList();
				Purchases.ItemsSource = purchList;
			}
#else
			Purchases.ItemsSource = GetPurchasesForVehicle(selVehicle);
#endif
		}


	}
}
