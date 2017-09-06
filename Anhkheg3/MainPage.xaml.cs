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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Anhkheg3
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
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
				List<Vehicle> list = db.Vehicles.ToList();
				Vehicles.ItemsSource = list;
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
	}
}
