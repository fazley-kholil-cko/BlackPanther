using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Request.Validator
{
    public static class AppSettings
    {
        static IConfigurationRoot Configuration;
        static IConfigurationBuilder Builder;

        static AppSettings()
        {
            Builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = Builder.Build();
        }
        
        public static string RedisConnection => Configuration["appSettings:RedisConnection"];

        public static string KafkaConnection => Configuration["appSettings:KafkaConnection"];

        public static string KafkaInTopicName => Configuration["appSettings:KafkaInTopicName"];

        public static string KafkaOutTopicName => Configuration["appSettings:KafkaOutTopicName"];

        public static string kafkaOutErrorTopicName => Configuration["appSettings:kafkaOutErrorTopicName"];

        public static string KafkaGroupName => Configuration["appSettings:KafkaGroupName"];
    }
}