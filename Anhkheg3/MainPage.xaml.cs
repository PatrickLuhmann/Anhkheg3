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
			List<int> someInts = new List<int>();
			someInts.Add(1);
			Vehicles.ItemsSource = someInts;

			List<Vehicle> tempVehicles = new List<Vehicle>();
			Vehicle v1;
			v1 = new Vehicle
			{
				Name = "Pumpkinmobile",
				Year = 1978,
				Make = "Pontiac",
				Model = "Bonneville"
			};
			tempVehicles.Add(v1);
			v1 = new Vehicle
			{
				Name = "The Beast",
				Year = 2017,
				Make = "Tesla",
				Model = "Model X"
			};
			tempVehicles.Add(v1);
			Vehicles.ItemsSource = tempVehicles;
		}
	}
}
