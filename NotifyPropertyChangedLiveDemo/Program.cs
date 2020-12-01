using System;
using System.ComponentModel;

namespace NotifyPropertyChangedLiveDemo
{
  static class Program
  {
    static void Main()
    {
      var carModel = new CarModel
      {
        Model = "MyCar",
        NumberOfDoors = 4,
        SpeedKmPerHour = 200
      };

      Console.Write("Got ");
      PrintCarDetails();

      carModel.PropertyChanged += OnPropertyChanged();

      Console.WriteLine();
      Console.WriteLine("Updating to racing model...");
      carModel.Model = "Racing " + carModel.Model;
      carModel.NumberOfDoors = 2;
      
      while (carModel.SpeedKmPerHour < 250)
      {
        Console.WriteLine();
        Console.WriteLine("Vrrrrrr...");
        carModel.SpeedUp();
      }

      Console.WriteLine();
      Console.WriteLine("You got speeding ticket");
      Console.WriteLine("GAME OVER");

      carModel.PropertyChanged -= OnPropertyChanged();
      return;


      PropertyChangedEventHandler OnPropertyChanged()
      {
        return (sender, args) =>
        {
          Console.WriteLine($"Property '{args.PropertyName}' changed");
          PrintCarDetails();
        };
      }

      void PrintCarDetails()
        => Console.WriteLine($"Car {{ Model: {carModel.Model},"
                             + $" Doors: {carModel.NumberOfDoors},"
                             + $" Speed: {carModel.SpeedKmPerHour:N0} km/h,  }}");
    }
  }
}