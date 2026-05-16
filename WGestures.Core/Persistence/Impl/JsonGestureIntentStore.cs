using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using WGestures.Core.Commands;


namespace WGestures.Core.Persistence.Impl;

public class JsonGestureIntentStore : IGestureIntentStore {
    public string FileVersion { get; set; }
    public Dictionary<string, ExeApp> Apps { get; set; }
    public GlobalApp GlobalApp { get; set; }
 
    public AbstractCommand[] HotCornerCommands { get; set; } //4 corners + 4 edges

    private string jsonPath;
    private JsonSerializer ser = new JsonSerializer();

    private JsonGestureIntentStore() { }

    public JsonGestureIntentStore(string jsonPath, string fileVersion)
    {
        FileVersion = fileVersion;
        this.jsonPath = jsonPath;
        SetupSerializer();

        if (File.Exists(jsonPath))
        {
            Deserialize();
        }
        else
        {
            Apps = new Dictionary<string, ExeApp>();
            GlobalApp = new GlobalApp();

            HotCornerCommands = new AbstractCommand[8]; //4 corners + 4 edges
        }
    }

    public JsonGestureIntentStore(Stream stream, bool closeStream, string fileVersion)
    {
        FileVersion = fileVersion;
        SetupSerializer();
        Deserialize(stream, closeStream);
    }

    private void Deserialize(Stream stream, bool closeStream)
    {
        if (stream == null || !stream.CanRead) throw new ArgumentException("stream");
        try
        {
            using (var txtReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(txtReader))
            {
                /*var ser = new JsonSerializer();
                ser.Formatting = Formatting.None;
                ser.TypeNameHandling = TypeNameHandling.Auto;

                if (FileVersion.Equals("1"))
                {
                    ser.Converters.Add(new GestureIntentConverter_V1());

                }
                else// if (FileVersion.Equals("2"))
                {
                    ser.Converters.Add(new GestureIntentConverter());

                }*/
                var result = ser.Deserialize<SerializeWrapper>(jsonReader);

                FileVersion = result.FileVersion;
                GlobalApp = result.Global;
                //Apps = result.Apps;
              
                Apps = new Dictionary<string, ExeApp>();

                //to lower
                foreach (var a in result.Apps.Values)
                {
                    a.ExecutablePath = a.ExecutablePath.ToLower();
                    Apps.Add(a.ExecutablePath, a);
                }

                //convert old version GestureButton Value ( 0->1, 1->2)
                if (FileVersion == "1" || FileVersion == "2")
                {
                    var globalIntents = GlobalApp.GestureIntents.Values.ToArray();
                    GlobalApp.GestureIntents.Clear();

                    foreach (var gestIntent in globalIntents)
                    {
                        gestIntent.Gesture.GestureButton += 1;
                        GlobalApp.GestureIntents.Add(gestIntent);
                    }

                    foreach (var app in Apps.Values)
                    {
                        var intents = app.GestureIntents.Values.ToArray();
                        app.GestureIntents.Clear();

                        foreach (var gestIntent in intents)
                        {
                            gestIntent.Gesture.GestureButton += 1;
                            app.GestureIntents.Add(gestIntent);
                        }
                    }
                }

                HotCornerCommands = new AbstractCommand[8];
                Array.Copy(result.HotCornerCommands, HotCornerCommands, result.HotCornerCommands.Length);
                //HotCornerCommands = result.HotCornerCommands;
            }
        }
        finally
        {
            if (closeStream) stream.Dispose();
        }

        //todo: 完全在独立domain中加载json.net?
        /*var deserializeDomain = AppDomain.CreateDomain("jsonDeserialize");
        deserializeDomain.UnhandledException += (sender, args) => { throw new IOException(args.ExceptionObject.ToString()); };
        deserializeDomain.DomainUnload += (sender, args) =>
        {
            Console.WriteLine("deserializeDomain Unloaded");
        };
        var wrapperRef = (ISerializeWrapper)deserializeDomain.CreateInstanceAndUnwrap("SerializeWrapper", "SerializeWrapper.SerializeWrapper");

        wrapperRef.DeserializeFromStream(stream, FileVersion, closeStream);

        GlobalApp = wrapperRef.Global;
        Apps = wrapperRef.Apps;

        wrapperRef = null;

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        AppDomain.Unload(deserializeDomain);
        deserializeDomain = null;

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();*/
    }

    private void Serialize()
    {
        using (var fs = new StreamWriter(jsonPath))
        {
            using (var writer = new JsonTextWriter(fs))
            {
                
                    ser.Serialize(writer,
                        new SerializeWrapper()
                        {
                            Apps = Apps,
                            FileVersion = FileVersion,
                            Global = GlobalApp,
                            HotCornerCommands = HotCornerCommands
                        });
              

                
            }
        }

        //todo: 完全在独立domain中加载json.net?
        /*var serializeDomain = AppDomain.CreateDomain("jsonDeserialize");
        serializeDomain.UnhandledException += (sender, args) => { throw new IOException(args.ExceptionObject.ToString()); };

        serializeDomain.DomainUnload += (sender, args) =>
        {
            Console.WriteLine("serializeDomain Unloaded");
        };
        var wrapperRef = (ISerializeWrapper)serializeDomain.CreateInstanceAndUnwrap("SerializeWrapper", "SerializeWrapper.SerializeWrapper");

        wrapperRef.FileVersion = FileVersion;
        wrapperRef.Apps = Apps;
        wrapperRef.Global = GlobalApp;
        wrapperRef.SerializeTo(jsonPath);

        wrapperRef = null;

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();

        AppDomain.Unload(serializeDomain);
        serializeDomain = null;

        GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        GC.WaitForPendingFinalizers();*/
    }

    private void Deserialize()
    {
        using (var file = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
        {
            Deserialize(file, false);
        }
    }

    private void SetupSerializer()
    {
        ser.Formatting = Formatting.None;
        ser.TypeNameHandling = TypeNameHandling.Auto;

        if (FileVersion.Equals("1"))
        {
            ser.Converters.Add(new GestureIntentConverter_V1());
        }
        else //if (FileVersion.Equals("2"))
        {
            ser.Converters.Add(new GestureIntentConverter());
        }
    }


    public bool TryGetExeApp(string key, out ExeApp found)
    {
        // 【新增拦截】如果当前有任何程序处于全屏状态（如玩游戏、看电影），直接关掉手势识别，返回 false
        if (WGestures.Common.OsSpecific.Windows.ScreenHelper.IsAnyAppFullScreen())
        {
            found = null;
            return false;
        }

        // 1. 优先采用老逻辑：如果是普通路径，直接从字典通过 Key 秒杀（效率最高）
        if (Apps.TryGetValue(key.ToLower(), out found))
        {
            return true;
        }

        // 2. 如果字典没找到，说明当前进程可能需要走正则匹配
        // 遍历所有开启了 UseRegex 的配置
        foreach (var app in Apps.Values)
        {
            if (app.UseRegex && app.IsMatch(key))
            {
                found = app;
                return true;
            }
        }

        found = null;
        return false;
    }

    public ExeApp GetExeApp(string key)
    {
        return Apps[key.ToLower()];
    }


    public void Remove(string key)
    {
        Apps.Remove(key.ToLower());
    }

    public void Remove(ExeApp app)
    {
        Remove(app.ExecutablePath.ToLower());
    }

    public void Add(ExeApp app)
    {
        app.ExecutablePath = app.ExecutablePath.ToLower();
        Apps.Add(app.ExecutablePath, app);
    }

    public void Save()
    {
        Serialize();
    }

    public JsonGestureIntentStore Clone()
    {
        //fixme: dummy impl
        var ret = new JsonGestureIntentStore();
        ret.GlobalApp = GlobalApp;
        ret.Apps = Apps;
        ret.FileVersion = FileVersion;
        ret.jsonPath = jsonPath;
        ret.HotCornerCommands = HotCornerCommands;
        return ret;
    }
    /// <summary>
    /// 导入另一个配置并返回一个全新的 Store 实例。
    /// 采用深拷贝技术，绝对不会修改当前实例（原对象）和源实例的数据。
    /// </summary>
    /// <param name="from">要导入的源配置</param>
    /// <param name="replace">是否采用替换模式</param>
    /// <returns>包含合并后数据的全新 JsonGestureIntentStore 实例</returns>
    public JsonGestureIntentStore ImportAndReturnNew(JsonGestureIntentStore from, bool replace = false)
    {
        if (from == null) return this; // 如果源为空，直接返回自己（或者根据需要返回当前实例的深拷贝）

        JsonGestureIntentStore clonedStore;

        // 1. 使用 MemoryStream 和 Newtonsoft.Json 对当前对象进行完美的深拷贝
        using (var ms = new MemoryStream())
        {
            using (var sw = new StreamWriter(ms, System.Text.Encoding.UTF8, 1024, true))
            using (var writer = new JsonTextWriter(sw))
            {
                // 序列化当前对象的状态
                ser.Serialize(writer, new SerializeWrapper()
                {
                    Apps = this.Apps,
                    FileVersion = this.FileVersion,
                    Global = this.GlobalApp,
                    HotCornerCommands = this.HotCornerCommands,
                  
                });
            }

            // 重置流指针，准备读取
            ms.Position = 0;

            // 调用你现有的流构造函数，在完全隔离的内存中生成新实例
            clonedStore = new JsonGestureIntentStore(ms, false, this.FileVersion);
            clonedStore.jsonPath = this.jsonPath;
        }

        // 2. 在这个确认安全的克隆体上执行导入逻辑
        clonedStore.Import(from, replace);

        // 3. 返回这个干净的、合并后的新对象
        return clonedStore;
    }
    public void Import(JsonGestureIntentStore from, bool replace = false)
    {
        if (from == null) return;

        if (replace)
        {
            GlobalApp.GestureIntents.Clear();
            GlobalApp.IsGesturingEnabled = from.GlobalApp.IsGesturingEnabled;
            Apps.Clear();
            HotCornerCommands = from.HotCornerCommands;
        }
        else
        {
            for (var i = 0; i < from.HotCornerCommands.Length; i++)
            {
                if (from.HotCornerCommands[i] != null)
                {
                    HotCornerCommands[i] = from.HotCornerCommands[i];
                }
            }
        }

        GlobalApp.ImportGestures(from.GlobalApp);

        foreach (var kv in from.Apps)
        {
            ExeApp appInSelf;
            //如果应用程序已经在列表中，则合并手势
            if (TryGetExeApp(kv.Key.ToLower(), out appInSelf))
            {
                appInSelf.ImportGestures(kv.Value);
                appInSelf.IsGesturingEnabled = appInSelf.IsGesturingEnabled && kv.Value.IsGesturingEnabled;
            }
            else //否则将app添加到列表中
            {
                var defaultApp = kv.Value;
                // 我们可以用一个自定义逻辑标记它，或者如果 ExeApp 无法修改，
                // 可以在 Store 内部维护一个私有的 List<string> _defaultAppKeys 来专门记账。
                Add(defaultApp);
            }
        }
    }

    public IEnumerator<ExeApp> GetEnumerator()
    {
        return Apps.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    internal class SerializeWrapper {
        public string FileVersion { get; set; }
        public Dictionary<string, ExeApp> Apps { get; set; }
        public GlobalApp Global { get; set; }
     
        public AbstractCommand[] HotCornerCommands { get; set; } = new AbstractCommand[8];
    }

    internal class GestureIntentConverter : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dict = value as GestureIntentDict;
            serializer.Serialize(writer, dict.Values.ToList());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var dict = new GestureIntentDict();
            var list = serializer.Deserialize<List<GestureIntent>>(reader);
            foreach (var i in list)
            {
                dict.Add(i);
            }

            return dict;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GestureIntentDict);
        }
    }

    //.json
    internal class GestureIntentConverter_V1 : JsonConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dict = value as GestureIntentDict;
            serializer.Serialize(writer, dict.Values.ToList());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var dict = new GestureIntentDict();
            var list = serializer.Deserialize<List<KeyValuePair<Gesture, GestureIntent>>>(reader);
            foreach (var i in list)
            {
                Debug.WriteLine("Add Gesture: " + i.Value.Gesture);
                dict.Add(i.Value);
            }

            return dict;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(GestureIntentDict);
        }
    }
    /*
    public interface ISerializeWrapper
    {
         string FileVersion { get; set; }
         Dictionary<string, ExeApp> Apps { get; set; }
         GlobalApp Global { get; set; }

         void DeserilizeFromFile(string filename, string version);
        void DeserializeFromStream(Stream s, string version, bool close = false);
         void SerializeTo(string fileName);
    }*/
}