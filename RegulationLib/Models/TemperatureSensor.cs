using System.Diagnostics;

public class TemperatureSensor
{

    public int Id { get; init; }
    private string Address { get; init; }
    
    private const string _baseAddress = "/sys/bus/w1/devices/";
    public string Name { get; init; }

    public static Dictionary<int, TemperatureSensor> Sensors = new();

    public TemperatureSensor(int id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
        Sensors.Add(id, this);       
    }

    public double? GetTemperature()
    {
         // Configurer les informations de démarrage du processus
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            FileName = "/bin/bash",
            Arguments = $"-c \"cat {_baseAddress}{Address}/w1_slave\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        double? temperature;
        // Créer et démarrer le processus
        using (Process process = new Process())
        {
            process.StartInfo = startInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            if (!string.IsNullOrEmpty(error))
            {
                Console.WriteLine(error);
                return null;
            }
            string crc = output.Substring(output.IndexOf("crc=") + 4, 3);
            if (crc.Contains("NO"))
            {
                Console.WriteLine("CRC error");
                return null;
            }

            string temperatureStr = output.Substring(output.IndexOf("t=") + 2);
            temperature = double.Parse(temperatureStr) / 1000;
        }
        return temperature;
    }
}
