﻿// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Common.Log;
using Lykke.Common;
using Lykke.Common.Chaos;
using Lykke.SettingsReader;
using MarginTrading.Activities.Core.Repositories;
using MarginTrading.Activities.Core.Settings;
using MarginTrading.Activities.SqlRepositories;
using Microsoft.Extensions.Internal;

namespace MarginTrading.Activities.Producer.Modules
{
    internal class ActivitiesModule : Module
    {
        private readonly IReloadingManager<AppSettings> _settings;
        private readonly ILog _log;

        public ActivitiesModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings.Nested(s => s.ActivitiesProducer)).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.ActivitiesProducer).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.ActivitiesProducer.Db).SingleInstance();
            
            builder.RegisterInstance(_log).As<ILog>().SingleInstance();
            
            builder.RegisterType<ThreadSwitcherToNewTask>()
                .As<IThreadSwitcher>()
                .SingleInstance();
            
            builder.RegisterChaosKitty(_settings.CurrentValue.ActivitiesProducer.ChaosKitty);
        }
    }
}