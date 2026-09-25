using System;
using Newtonsoft.Json.Serialization;

namespace WGestures.Core.Persistence;

/// <summary>
/// 手势配置数据（gestures.wg2 / 导入文件）以 TypeNameHandling 持久化 $type，
/// 历史数据中的程序集名为 WGestures.Core / WGestures.Common / NativeMultiFileArchiveLib。
/// 项目合并后这些类型都位于主程序集 WGestures 中，反序列化时需重映射旧程序集名。
/// </summary>
class LegacyAssemblySerializationBinder : DefaultSerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        if (assemblyName == "WGestures.Core"
            || assemblyName == "WGestures.Common"
            || assemblyName == "NativeMultiFileArchiveLib")
        {
            assemblyName = "WGestures";
        }

        return base.BindToType(assemblyName, typeName);
    }
}
