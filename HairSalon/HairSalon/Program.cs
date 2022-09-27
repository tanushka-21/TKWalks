using System;
namespace HairSalon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            double total = 0;
            var services = new[]
            {
                new Service{Name="Haircut_Long",Price=200,ServiceType=ServiceType.Normal},
                new Service{Name="Haircut_Short",Price=400,ServiceType=ServiceType.Normal},
                new Service{Name="Hair_coloring", Price=900,ServiceType=ServiceType.Normal},
                new Service{Name="Hair_Straigning",Price=3500,ServiceType=ServiceType.Premium},
                new Service{Name="Antioxident_Facial",Price=600,ServiceType=ServiceType.Normal},
                new Service{Name="Acne_reducing_facial", Price=800,ServiceType=ServiceType.Normal},
                new Service{Name="Skin_Lightening_Facial", Price=500,ServiceType=ServiceType.Normal}
            };

            var map1 = new Dictionary<string, int>();
            var map2 = new Dictionary<string, ServiceType>();
            for (int i = 0; i < services.Length; i++)
            {
                map1[services[i].Name] = services[i].Price;
            }
            for (int i = 0; i < services.Length; i++)
            {
                map2[services[i].Name] = services[i].ServiceType;
            }


            var list = new List<string>();
            while (true)
            {
                var serviceType = Console.ReadLine();
                if (serviceType != null)
                {
                    list.Add(serviceType);
                    if (serviceType.Equals("Weekday") || serviceType.Equals("Weekend"))
                        break;
                }

            }
            var last = list[list.Count - 1];
            list.RemoveAt(list.Count-1);
            foreach (var service in list)
            {
                if (map2[service].Equals(ServiceType.Normal))
                    total += map1[service] - (map1[service] * 0.05);
                else
                    total += map1[service] - (map1[service] * 0.02);
            }

            if (total > 1500)
            {
                if (last.Equals("Weekday"))
                {
                    total = total - total * 0.10;
                }
                else
                {
                    total = total - total * 0.05;
                }
            }

            Console.WriteLine(total);
        }
    }
}
