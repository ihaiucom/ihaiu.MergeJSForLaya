using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommandLine;

public class Options
{
    // 运行完，是否自动关闭cmd
    [Option("autoEnd", Required = false, Default = true)]
    public bool autoEnd { get; set; }

    // 命令
    [Option("cmd", Required = false, Default = "mergejs")]
    public string cmd { get; set; }

    // 启动参数设置 配置路径
    [Option("optionSetting", Required = false, Default = "./ReleaseLaya.json")]
    public string optionSetting { get; set; }

    // laya项目路径
    [Option("layaProject", Required = false, Default = "E:/zengfeng/GamePF/Gidea-PF-Client/GamePF")]
    public string layaProject { get; set; }

    // laya项目发布过度路径
    [Option("binCache", Required = false, Default = "E:/wamp/www/GamePF/bin-cache")]
    public string binCache { get; set; }

    // laya项目发布路径
    [Option("binRelease", Required = false, Default = "E:/wamp/www/GamePF/bin-release")]
    public string binRelease { get; set; }


    // 配置生成输出目录
    [Option("configOut", Required = false, Default = "E:/zengfeng/GamePF/ConfigOut/Client/Config")]
    public string configOut { get; set; }


    // 配置fgui代码输出目录
    [Option("fguiCodeOut", Required = false, Default = "E:/zengfeng/GamePF/FairyGUICode/TS")]
    public string fguiCodeOut { get; set; }


    //  min js 是否混淆
    [Option("minJsMunge", Required = false, Default = false)]
    public bool minJsMunge { get; set; }

    public OptionsMinConfig[] minConfigs = new OptionsMinConfig[] { new OptionsMinConfig() { paths = new string[] { "E:/wamp/www/GamePF/bin-cache/js-merge/GameLaunch.merge.js", "E:/wamp/www/GamePF/bin-cache/js-merge/Game.merge.js" }, savePath="E:/wamp/www/GamePF/bin-release/js-min/all.min.js" },
                            new OptionsMinConfig() { paths = new string[] { "E:/wamp/www/GamePF/bin-cache/js-merge/GameLaunch.merge.js" }, savePath="E:/wamp/www/GamePF/bin-release/js-min/GameLaunch.min.js" },
                            new OptionsMinConfig() { paths = new string[] { "E:/wamp/www/GamePF/bin-cache/js-merge/Game.merge.js" }, savePath="E:/wamp/www/GamePF/bin-release/js-min/Game.min.js" }};



    public void Save(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./ReleaseLaya.json";

        string json = JsonHelper.ToJsonType(this);
        File.WriteAllText(path, json);
    }

    public static Options Load(string path = null)
    {
        if (string.IsNullOrEmpty(path))
            path = "./ReleaseLaya.json";

        string json = File.ReadAllText(path);
        Options options = JsonHelper.FromJson<Options>(json);
        return options;
    }
}


public class OptionsMinConfig
{
    public string[] paths;
    public string savePath;


}