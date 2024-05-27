
using System.Device.Gpio;
using System.Device.Pwm;


namespace RegulationLib.Models;

/// <summary>
/// Represents a pump with a PWM channel type Grundfos upm3  
/// </summary>
public class Pump
{
    private const int FREQUENCY = 400;
    private const int INITIAL_SPEED = 50;
    private int? _currentSpeed;
    private bool _isRunning = false;
    private PwmChannel? _pwmChannel;
    public int Id { get; init; }
    static public Dictionary<int, Pump> Pumps = new ();

    public string Name { get; init; }

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
    }

    public int CurrentSpeed
    {
        get
        {
            if (_currentSpeed is null) throw new NullReferenceException("Current speed is null");
            return (int)_currentSpeed;
        }
    }

    public Pump(int id, string name, int GpioPin)
    {
        Id = id;
        Name = name;
        InitializePwmChannel(GpioPin);
        Pumps.Add(id, this);
    }
    public void SetSpeed(int speed)
    {
        speed = speed > 100 ? 100 : speed < 0 ? 0 : speed;
        _currentSpeed = speed;
        if (_pwmChannel is null) throw new NullReferenceException("PWM channel is null");
        _pwmChannel.DutyCycle = speed / 100.0;
    }
    public void Start()
    {
        if (_pwmChannel is null) throw new NullReferenceException("PWM channel is null");
        _pwmChannel.Start();
        _isRunning = true;
    }
    public void Stop()
    {
        if (_pwmChannel is null) throw new NullReferenceException("PWM channel is null");
        _pwmChannel.Stop();
        _isRunning = false;
    }
    public string Status()
    {
        return $"Pump {Id} is running at {_currentSpeed}%.";
    }

    /// <summary>
    /// Define the PWM channel based on the GPIO pin
    /// </summary>
    /// <param name="GpioPin">The GPIO number, must be 12, 13, 18 or 19</param> 
    /// <returns>A tuple containing the PWM channel and the PWM channel number</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Tuple<int, int> definePwmChannel(int GpioPin)
    {
        switch (GpioPin)
        {
            case 12:
                return new Tuple<int, int>(0, 0);
            case 13:
                return new Tuple<int, int>(0, 1);
            case 18:
                return new Tuple<int, int>(1, 0);
            case 19:
                return new Tuple<int, int>(1, 1);
            default:
                throw new ArgumentOutOfRangeException("Invalid GPIO pin.");
        }
    }

    private void InitializePwmChannel(int GpioPin)
    {
        Tuple<int, int> pwmChannel = definePwmChannel(GpioPin);
        _pwmChannel = PwmChannel.Create(pwmChannel.Item1, pwmChannel.Item2, 400, INITIAL_SPEED / 100.0);
    }
}