using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OpenHardwareMonitor.Hardware;

namespace BugsReporterServer
{
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }

    public static class StatusHelpers
    {
        public static List<string> GetCPUTemperatureData()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);

            List<string> result = new List<string>();

            var cpuHardware = computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.CPU);

            if (cpuHardware != null)
            {
                var data = cpuHardware.Sensors.Where(x => x.SensorType == SensorType.Load).ToList();

                for (int i = 0; i < cpuHardware.Sensors.Length; ++i)
                {
                    if (cpuHardware.Sensors[i].SensorType == SensorType.Load)
                    {
                        result.Add(cpuHardware.Sensors[i].Name + ": " + cpuHardware.Sensors[i].Value.Value.ToString("0.00") + "%");
                    }
                    else if (cpuHardware.Sensors[i].SensorType == SensorType.Temperature)
                    {
                        result.Add(cpuHardware.Sensors[i].Name + ": " + cpuHardware.Sensors[i].Value + "°C");
                    }
                }
            }

            var ramHardware = computer.Hardware.FirstOrDefault(x => x.HardwareType == HardwareType.RAM);

            if (ramHardware != null)
            {
                var data = ramHardware.Sensors.Where(x => x.SensorType == SensorType.Data).ToList();

                var used = data.FirstOrDefault(x => x.Name.Contains("Used"));
                var available = data.FirstOrDefault(x => x.Name.Contains("Available"));

                if (used != null && available != null)
                    result.Add("Available RAM: " + used.Value.Value.ToString("0.000") + " GB" + "/" + (used.Value + available.Value).Value.ToString("0.000") + " GB");
            }

            return result;
        }
    }
}