﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Incident.Core.Configuration;

public class UserSettings
{
    [JsonProperty("userName")]
    [ConfigurationKeyName("userName")]
    public string UserName { get; set; } = string.Empty;
}