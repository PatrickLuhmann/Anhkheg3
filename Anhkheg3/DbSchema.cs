using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Anhkheg3
{
    class DbSchema : DbContext
    {
		public DbSet<Vehicle> Vehicles { get; set; }
		public DbSet<Purchase> Purchases { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder ob)
		{
			ob.UseSqlite("Data Source=purchases.db");
		}
	}

	public class Vehicle
	{
		public int ID { get; set; }

		public string Name { get; set; }
		public int Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public int StartingMileage { get; set; }
		//TODO: Something for the image here

		public List<Purchase> Purchases { get; set; } = new List<Purchase>();
	}

	public class Purchase
	{
		public int ID { get; set; }

		public DateTime Date { get; set; }
		public decimal Gallons { get; set; }
		public decimal Cost { get; set; }
		public decimal Trip { get; set; }
		public int Odometer { get; set; }

		public Vehicle Vehicle;
	}
}
