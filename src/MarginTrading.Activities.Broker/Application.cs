﻿using System;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.MarginTrading.Activities.Contracts.Models;
using Lykke.MarginTrading.BrokerBase;
using Lykke.MarginTrading.BrokerBase.Models;
using Lykke.MarginTrading.BrokerBase.Settings;
using Lykke.SlackNotifications;
using MarginTrading.Activities.Core.Domain;
using MarginTrading.Activities.Core.Extensions;
using MarginTrading.Activities.Core.Repositories;

namespace MarginTrading.Activities.Broker
{
    public class Application : BrokerApplicationBase<NewActivityEvent>
    {
        private readonly IActivitiesRepository _activitiesRepository;
        private readonly ILog _log;
        private readonly Settings _settings;

        public Application(
            IActivitiesRepository activitiesRepository,
            ILog logger,
            Settings settings, 
            CurrentApplicationInfo applicationInfo,
            ISlackNotificationsSender slackNotificationsSender) 
        : base(logger, slackNotificationsSender, applicationInfo, MessageFormat.MessagePack)
        {
            _activitiesRepository = activitiesRepository;
            _log = logger;
            _settings = settings;
        }

        protected override BrokerSettingsBase Settings => _settings;
        protected override string ExchangeName => _settings.RabbitMqQueues.Activities.ExchangeName;
        protected override string RoutingKey => "NewActivityEvent";
        
        protected override Task HandleMessage(NewActivityEvent e)
        {
            var contract = e.Activity;
            
            var activity = new Activity(contract.Id, contract.AccountId, contract.Instrument, contract.EventSourceId,
                contract.Timestamp, contract.Event.ToType<ActivityType>(), contract.DescriptionAttributes, contract.RelatedIds);

            return Task.Run(async () =>
            {
                try
                {
                    await _activitiesRepository.AddAsync(activity);
                }
                catch (Exception ex)
                {
                    await _log.WriteErrorAsync(nameof(Broker), nameof(HandleMessage),
                        activity.ToJson(), ex);
                }
            });
        }
    }
}





















