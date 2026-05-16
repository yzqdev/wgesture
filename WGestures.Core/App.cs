using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace WGestures.Core;

[Serializable]
public abstract class AbstractApp
{
    public string Name { get; set; }

    public GestureIntentDict GestureIntents { get; set; }
    /// <summary>
    /// 是否启用手势
    /// </summary>
    public bool IsGesturingEnabled { get; set; }


    protected AbstractApp()
    {
        Name = "Noname";
        GestureIntents = new GestureIntentDict();
        IsGesturingEnabled = true;
    }

    public virtual GestureIntent Find(Gesture key)
    {
        // Console.WriteLine("Find("+key.ToString()+")");
        GestureIntents.TryGetValue(key, out GestureIntent val);
        //if(val!=null)Console.WriteLine("Found? "+val.ToString());
        return val;
    }

    public virtual void Add(GestureIntent intent)
    {
        GestureIntents.Add(intent.Gesture,intent);
    }

    public virtual void Remove(GestureIntent intent)
    {
        GestureIntents.Remove(intent);
    }

    public virtual void Import(AbstractApp from)
    {
        IsGesturingEnabled = from.IsGesturingEnabled;
        Name = from.Name;
        ImportGestures(from);
    }

    public virtual void ImportGestures(AbstractApp from)
    {
        foreach (var kv in from.GestureIntents)
        {
            GestureIntents.AddOrReplace(kv.Value);
        }
    }
}

[/*JsonArray, */Serializable]
public class GestureIntentDict : Dictionary<Gesture, GestureIntent>
{

    public GestureIntentDict(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public GestureIntentDict() { }


    public void Add(GestureIntent intent)
    {
        Add(intent.Gesture,intent);
    }

    public void Remove(GestureIntent intent)
    {
        Remove(intent.Gesture);
    }

    public void AddOrReplace(GestureIntent intent)
    {
        this[intent.Gesture] = intent;
    }

    public void Import(GestureIntentDict from)
    {
        foreach (var kv in from)
        {
            this[kv.Key] = kv.Value;
        }
    }

    public void Import(IEnumerable<GestureIntent> from, bool replace=false)
    {
        if(replace) Clear();

        foreach (var i in from)
        {
            this[i.Gesture] = i;
        }
    }
}

/// <summary>
/// 用可执行文件路径来代表的应用程序
/// </summary>
/// 
[Serializable]
public class ExeApp : AbstractApp
{
    /// <summary>
    /// 是否继承全局手势
    /// </summary>
    public bool InheritGlobalGestures { get; set; }
    /// <summary>
    /// exe路径
    /// </summary>
    public string ExecutablePath { get; set; }
    /// <summary>
    /// 新增：是否使用正则表达式来匹配路径
    /// </summary>
    public bool UseRegex { get; set; }
  
    /// <summary>
    /// 新增：验证当前活跃窗口的路径是否匹配当前配置
    /// </summary>
    /// <param name="currentActivePath">当前系统捕获到的活动进程路径</param>
    public bool IsMatch(string currentActivePath)
    {
        if (string.IsNullOrEmpty(currentActivePath)) return false;

        if (UseRegex)
        {
            try
            {
                // 使用不区分大小写的正则匹配
                return Regex.IsMatch(currentActivePath, ExecutablePath, RegexOptions.IgnoreCase);
            }
            catch (ArgumentException)
            {
                // 防止用户写错正则表达式导致程序崩溃
                return false;
            }
        }

        // 默认的普通路径匹配（全小写比对）
        return currentActivePath.ToLower() == ExecutablePath.ToLower();
    }
    public override void Import(AbstractApp @from)
    {
        var asExeApp = @from as ExeApp;
        if (asExeApp != null)
        {
            InheritGlobalGestures = asExeApp.InheritGlobalGestures;
            this.UseRegex = asExeApp.UseRegex;
        }

        base.Import(@from);
    }

    public ExeApp()
    {
        InheritGlobalGestures = true;
        UseRegex = false;
    }
}

/// <summary>
/// 表示全局有效的特殊应用
/// </summary>
[Serializable]
public class GlobalApp : AbstractApp
{
    public GlobalApp()
    {
        Name = "(Global)";
    }
}