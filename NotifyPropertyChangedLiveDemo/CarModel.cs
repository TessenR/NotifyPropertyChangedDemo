using System.ComponentModel;

namespace NotifyPropertyChangedLiveDemo
{
  public class CarModel : INotifyPropertyChanged
  {
    private double SpeedKmPerHourBackingField;
    private int NumberOfDoorsBackingField;
    private string ModelBackingField = "";

    public void SpeedUp() => SpeedKmPerHour *= 1.1;

    public double SpeedKmPerHour
    {
      get => SpeedKmPerHourBackingField;
      set 
      {
        SpeedKmPerHourBackingField = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SpeedKmPerHour)));
      }
    }

    public int NumberOfDoors
    {
      get => NumberOfDoorsBackingField;
      set 
      {
        NumberOfDoorsBackingField = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfDoors)));
      }
    }

    public string Model
    {
      get => ModelBackingField;
      set 
      {
        ModelBackingField = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Model)));
      }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}