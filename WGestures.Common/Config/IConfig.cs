using System;
using System.Collections;
using System.Collections.Generic;
using WGestures.Common.Config.Impl;

namespace WGestures.Common.Config
{
    public interface IConfig : IEnumerable<KeyValuePair<string, object>>
    {
        T Get<T>(string key);
        T Get<T>(string key, T defaultValue);
        void Set<T>(string key, T value);
        void Save();

        void Import(  PlistConfig from);
    }
}
