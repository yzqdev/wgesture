using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WGestures.Common.Config;
using WGestures.Core.Persistence;

namespace WGestures.App.Migrate;

internal class ConfigAndGestures
{
    public PlistConfig Config { get; private set; }

    public JsonGestureIntentStore GestureIntentStore { get; private set; }

    public ConfigAndGestures(PlistConfig config, JsonGestureIntentStore gestures)
    {
        Config = config;
        GestureIntentStore = gestures;
    }
}
