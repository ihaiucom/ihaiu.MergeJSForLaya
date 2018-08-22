using CommandLine;
using System;
using System.IO;


public class CmdType
{
    // 合并js
    public const string mergejs = "mergejs";
    // 压缩 js
    public const string autominjs = "autominjs";
    // 压缩 js 读配置
    public const string minjs = "minjs";
    // 生成index-merge
    public const string generateindexmerge = "generateindexmerge";
    // 生成index-min
    public const string generateindexmin = "generateindexmin";
}

public class Setting
{
    public static Options Options { get; set; }
    public static string cmd = CmdType.mergejs;

    public static void Init(string[] args)
    {
        bool useSetting = args.Length == 0;
        foreach (string op in args)
        {
            if (op.StartsWith("--optionSetting"))
            {
                useSetting = true;
                break;
            }
        }

        Parse(args);

        if(!File.Exists(Options.optionSetting))
        {
            Options.Save(Options.optionSetting);
        }


        cmd = Options.cmd;
        if (string.IsNullOrEmpty(cmd))
        {
            cmd = CmdType.mergejs;
        }

        //if (useSetting)
        {
            Options = Options.Load(Options.optionSetting);
        }
    }


    public static void Parse(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
            .WithParsed(options =>
            {
                Options = options;
            });
    }




    public static string MergeRoot
    {
        get
        {
            return Options.binCache + "/js-merge";
        }
    }

    public static string MinRoot
    {
        get
        {
            return Options.binCache + "/js-min";
        }
    }


}