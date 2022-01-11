using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace WGestures.Common.Config.Impl
{
    public enum plistType {
        Auto, Binary, Xml
    }
    public class PlistConfig  
    {
        //public string FileVersion
        //{
        //    get { 
        //        return Get<string>("$$FileVersion", null); 
        //    }
        //    set
        //    {
        //        Set("$$FileVersion",value);
        //    }
        //}
        public ConfigData Dict { get; set; }

        public string PlistPath { get; set; }

        /// <summary>
        /// 创建一个空的Config
        /// </summary>
        public PlistConfig(){}

        /// <summary>
        /// 创建并指定要加载或保存的plist文件位置
        /// </summary>
        /// <param name="plistPath">Plist path.</param>
        public PlistConfig(string plistPath)
        {
            PlistPath = plistPath;
            Load();
        }

        public PlistConfig(Stream stream, bool closeStream)
        {
            Load(stream,closeStream);
        }

        private void Load()
        {
            if (PlistPath == null)
                throw new InvalidOperationException("未指定需要加载的plist文件路径");
            if (!File.Exists(PlistPath))
            {
                return;
            }
            
            StreamReader reader = new StreamReader(PlistPath);
            Dict=JsonConvert.DeserializeObject<ConfigData>(reader.ReadToEnd());
            //Dict = (Dictionary<string, object>)Plist.readPlist(PlistPath);
        }
        
        private void Load(Stream stream, bool closeStream = false)
        {
            if(stream == null || !stream.CanRead) throw new ArgumentException("stream");
 
            try
            {
                StreamReader reader = new StreamReader(PlistPath);
                Dict = JsonConvert.DeserializeObject<ConfigData>(reader.ReadToEnd());
                
            }
            catch (Exception)
            {
                if (closeStream && stream != null) stream.Close();
                throw;
            }
                

            

        }

        public   void Save()
        {
            if (PlistPath == null)
                throw new InvalidOperationException("未指定需要保存到的plist文件路径(PlistPath属性)");

            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(PlistPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, Dict);
                 
            }
            //Plist.writeXml(Dict, PlistPath);
        }

        public void Import(PlistConfig config)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(PlistPath))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, config.Dict);

            }
        }

        public void Import(PlistConfig config1, PlistConfig config2)
        {
            throw new NotImplementedException();
        }
    }

}
