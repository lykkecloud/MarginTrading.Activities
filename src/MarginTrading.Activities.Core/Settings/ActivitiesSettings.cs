﻿using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.SettingsReader.Attributes;

namespace MarginTrading.Activities.Core.Settings
{
    [UsedImplicitly]
    public class ActivitiesSettings
    {
        public DbSettings Db { get; set; }

        [Optional, CanBeNull]
        public ChaosSettings ChaosKitty { get; set; }
        
        [Optional]
        public bool UseSerilog { get; set; }
        
        public CqrsSettings Cqrs { get; set; }
        
        public ConsumersSettings Consumers { get; set; }
        
        public SettingsServiceClient SettingsServiceClient { get; set; }
    }
}