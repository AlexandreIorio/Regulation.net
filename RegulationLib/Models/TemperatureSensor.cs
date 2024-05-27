using System;
using System.Device.Gpio;
using nanoFramework.Device.OneWire;
using Iot.Device.Ds18b20;

public class TemperatureSensor
{
    private OneWireHost _oneWire;
    private Ds18b20 _sensor;

    public TemperatureSensor(int pin)
    {
        _oneWire = new OneWireHost();
        _sensor = new Ds18b20(_oneWire, null, false, TemperatureResolution.VeryHigh);
        if (_sensor.Initialize())
        {
            Console.WriteLine($"Is sensor parasite powered?:{_sensor.IsParasitePowered}");
            string devAddrStr = "";
            foreach (var addrByte in _sensor.Address)
            {
                devAddrStr += addrByte.ToString("X2");
            }

            Console.WriteLine($"Sensor address:{devAddrStr}");
        }
    }

    public void ReadTemperature()
    {
        
         if (!_sensor.TryReadTemperature(out var temperature))
             {
                 Console.WriteLine("Can't read!");
             }
             else
             {
                 Console.WriteLine($"Temperature: {temperature.DegreesCelsius.ToString("F")}\u00B0C");
                //  return temperature.DegreesCelsius;
             }
    }
}
