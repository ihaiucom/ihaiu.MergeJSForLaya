using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class MergeJSData
{
    public string name;
    private Dictionary<string, bool> pathDict = new Dictionary<string, bool>();
    public List<string> paths = new List<string>();
    private StringWriter sw;

    string replaceCode = @"var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();";

    public MergeJSData()
    {

    }



    public void AddPath(string path)
    {
        path = path.Trim();
        if (string.IsNullOrEmpty(path))
            return;

        if(!pathDict.ContainsKey(path))
        {
            pathDict.Add(path, true);
            paths.Add(path);
        }
    }

    public void Merge(bool isFirst = false)
    {
        sw = new StringWriter();
        if(isFirst)
        {
            sw.WriteLine(replaceCode);
        }
        string srcConfigPath;
        string outConfigPath;
        string content;
        bool isReplace = false;
        foreach (string item in paths)
        {
            string path = Setting.Options.layaProject + "/bin/" + item;
            if(File.Exists(path))
            {

                content = File.ReadAllText(path);
                isReplace = false;
                if (item.StartsWith("js/Config/ConfigExtends/"))
                {
                    string configName = Path.GetFileNameWithoutExtension(item);
                    outConfigPath = Setting.Options.configOut + "/ConfigExtends/" + configName + ".ts";
                    srcConfigPath = Setting.Options.layaProject + "/src/Config/ConfigExtends/" + configName + ".ts";
                    if (File.Exists(outConfigPath) && File.Exists(srcConfigPath))
                    {
                        if(File.ReadAllText(outConfigPath).Equals(File.ReadAllText(srcConfigPath)))
                        {
                            isReplace = true;
                            content = $"configs.{configName} = configs.{configName}Struct;";
                            Console.WriteLine(content);
                            sw.WriteLine(content);
                        }
                    }
                }
                else if(item.StartsWith("js/Config/ReaderExtends/"))
                {
                    string configName = Path.GetFileNameWithoutExtension(item);
                    outConfigPath = Setting.Options.configOut + "/ReaderExtends/" + configName + ".ts";
                    srcConfigPath = Setting.Options.layaProject + "/src/Config/ReaderExtends/" + configName + ".ts";
                    if (File.Exists(outConfigPath) && File.Exists(srcConfigPath))
                    {
                        if (File.ReadAllText(outConfigPath).Equals(File.ReadAllText(srcConfigPath)))
                        {
                            isReplace = true;
                            content = $"configs.{configName} = configs.{configName}Struct;";
                            Console.WriteLine(content);
                            sw.WriteLine(content);
                        }
                    }
                }
                //else if (item.StartsWith("js/fgui/Extends/"))
                //{
                //    string fguiCodePath = item.Replace("js/fgui/Extends/", "").Replace(".js", ".ts");
                //    string configName = fguiCodePath.Replace(".ts", "").Replace("/", ".");
                //    outConfigPath = Setting.Options.fguiCodeOut + "/Extends/" + fguiCodePath;
                //    srcConfigPath = Setting.Options.layaProject + "/src/fgui/Extends/" + fguiCodePath;
                //    if (File.Exists(outConfigPath) && File.Exists(srcConfigPath))
                //    {
                //        if (File.ReadAllText(outConfigPath).Equals(File.ReadAllText(srcConfigPath)))
                //        {
                //            isReplace = true;
                //            content = $"fgui.{configName} = fgui.{configName}Struct;";
                //            Console.WriteLine(content);
                //            sw.WriteLine(content);
                //        }
                //    }
                //}

                if (!isReplace)
                {
                    content = content.Replace(replaceCode, string.Empty);
                    sw.WriteLine(content);
                }


            }
        }
    }

    public void Save()
    {
        string path = Setting.MergeRoot + "/" + name + ".merge.js";
        PathHelper.CheckPath(path);
        File.WriteAllText(path, sw.ToString());

        path = Setting.MergeRoot + "/" + name + ".merge.txt";
        File.WriteAllText(path, String.Join("\r\n", paths));
    }
}