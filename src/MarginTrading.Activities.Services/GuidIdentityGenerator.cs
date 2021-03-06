// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

using System;
using MarginTrading.Activities.Services.Abstractions;

namespace MarginTrading.Activities.Services
{
    public class GuidIdentityGenerator : IIdentityGenerator
    {
        public string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}