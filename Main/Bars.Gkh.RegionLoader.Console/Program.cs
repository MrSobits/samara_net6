namespace Bars.Gkh.RegionLoader.Console
{
    using System;
    using Base;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Вы не передали ни одного параметра для работы приложения!");
                Console.ReadLine();
                return;
            }

            

            var autoMode = false;
            var region = string.Empty;
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                switch (arg)
                {
                    case "-auto":
                        autoMode = true;
                        break;
                    case "-region":
                        if (i + 1 < args.Length)
                        {
                            region = args[i + 1];
                        }
                        break;
                }
            }

            var regionManager = new RegionLoadManager();

            try
            {
                regionManager.InitializeLoader();

                if (regionManager.Regions.Contains(regionManager.Region)
                    || string.IsNullOrEmpty(regionManager.Region))
                { 
                    Console.WriteLine("Конфигуратор был успешно инициализирован. " +
                                     (string.IsNullOrEmpty(regionManager.Region)
                                         ? "Вы можете выбрать регион"
                                         : "Текущий регион сборки - "
                                           + regionManager.Region));
                    
                }
                else
                {
                    Console.WriteLine("Инициализация конфигуратора завершилась с предупреждением: Неизвестный регион");
                    Console.WriteLine("Регион заданный в файле сборки отстутствует в modules.json");
                }

                if (!regionManager.Regions.Contains(region))
                {
                    Console.WriteLine("Сведений о регионе \"" + region + "\" не обнаружено в конфигурационном файле регионов modules.json");
                    if (!autoMode)
                    {
                        Console.ReadLine();
                    }
                    return;
                }

                Console.WriteLine("Установка региона \"" + region + "\"...");

                regionManager.SetRegion(region);

                Console.WriteLine("Регион успешно установлен. Сборка теперь формируется для региона \"" + region + "\"");
            }
            catch (Exception e)
            {
                Console.WriteLine("При инициализации лоадера произошла ошибка\r\n" + e);
            }

            if (!autoMode)
            {
                Console.ReadLine();
            }
        }
    }
}
