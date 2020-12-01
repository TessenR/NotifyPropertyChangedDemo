using System.ComponentModel;

namespace NotifyPropertyChangedLiveDemo
{
  public partial class CarModel : INotifyPropertyChanged
  {
    private double SpeedKmPerHourBackingField;
    private int NumberOfDoorsBackingField;
    private string ModelBackingField = "";

    public void SpeedUp() => SpeedKmPerHour *= 1.1;
  }
}