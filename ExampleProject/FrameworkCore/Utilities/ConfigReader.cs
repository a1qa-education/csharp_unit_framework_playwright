using Microsoft.Extensions.Configuration;
using System.IO;

namespace FrameworkCore.Utilities
{
    public static class ConfigReader
    {
        private static readonly IConfiguration FrameworkConfig;
        private static readonly IConfiguration TestDataConfig;

        static ConfigReader()
        {
            string basePath = Directory.GetCurrentDirectory();

            FrameworkConfig = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Configuration/frameworkSettings.json", optional: false, reloadOnChange: true)
                .Build();

            TestDataConfig = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("Configuration/testData.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Generic method to extract values from frameworkSettings.json
        public static T GetFrameworkSetting<T>(string key)
        {
            return FrameworkConfig.GetValue<T>(key)!;
        }

        // Generic method to extract values from testData.json
        public static T GetTestData<T>(string key)
        {
            return TestDataConfig.GetValue<T>(key)!;
        }

        // Methods to bind entire JSON objects to C# classes/records if required
        public static IConfigurationSection GetFrameworkSection(string sectionName) => FrameworkConfig.GetSection(sectionName)!;
        public static IConfigurationSection GetTestDataSection(string sectionName) => TestDataConfig.GetSection(sectionName)!;
    }
}