using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Util;

public class GenerateReleaseIndexManager
{
    public enum IndexType
    {
        Merge,
        Min
    }

    public IndexType indexType;

    public void Run(IndexType indexType)
    {
        this.indexType = indexType;

        string indexTmp = null;
        switch(indexType)
        {
            case IndexType.Merge:
                indexTmp = Setting.Options.binCache + "/index-merge.html";
                break;
            case IndexType.Min:
                indexTmp = Setting.Options.binCache + "/index-min.html";
                break;
        }

        string indexSave = null;

        switch (indexType)
        {
            case IndexType.Merge:
                indexSave = Setting.Options.binRelease + "/index-merge.html";
                break;
            case IndexType.Min:
                indexSave = Setting.Options.binRelease + "/index-min.html";
                break;
        }

        string indexHtml = Setting.Options.binRelease + "/index.html";

        if(File.Exists(indexHtml))
        {
            if(File.ReadAllLines(indexHtml).Length > 100)
            {
                string indexBak = Setting.Options.binRelease + "/index-src.html";
                PathHelper.CheckPath(indexBak);
                if (File.Exists(indexBak))
                    File.Delete(indexBak);
                File.Copy(indexHtml, indexBak);
            }

        }


        string text = File.ReadAllText(indexTmp);


        Lexer lexer = new Lexer(text);
        Parser parser = new Parser(lexer);
        NodeList htmlNodes = parser.Parse(new NodeClassFilter(typeof(ScriptTag)));

        List<string> folders = new List<string> ();




        for (int i = 0; i < htmlNodes.Count; i++)
        {
            var node = htmlNodes[i];
            ScriptTag script = node as ScriptTag;

            if (script != null)
            {

                string key = script.GetAttribute("src").Trim();
                key = Path.GetDirectoryName(key);
                if(folders.IndexOf(key) == -1)
                {
                    Console.WriteLine("Folder:" + key);
                    folders.Add(key);
                }
            }
        }


        List<string> list = new List<string>();
        foreach(string folder in folders)
        {
            string path = Setting.Options.binRelease + "/" + folder;
            if (!Directory.Exists(path))
            {

                Console.WriteLine($"不存在目录 {path}");
                continue;
            }

            string[] files = Directory.GetFiles(path, "*.js");
            list.AddRange(files);
        }

        Dictionary<string, long> dateDict = new Dictionary<string, long>();
        Dictionary<string, string> dict = new Dictionary<string, string>();
        foreach(string path in list)
        {
            FileInfo fileInfo =  new FileInfo(path);
            long time = fileInfo.LastWriteTime.ToFileTime();
            string js = path.Replace("\\", "/").Replace(Setting.Options.binRelease + "/", "");
            string key = js;
            key = key.Substring(0, key.Length - 11) + ".js";
            if(dateDict.ContainsKey(key))
            {
                if(time > dateDict[key])
                {
                    dateDict[key] = time;
                    dict[key] = js;
                }
            }
            else
            {
                dateDict.Add(key, time);
                dict.Add(key, js);
            }
        }


        for (int i = 0; i < htmlNodes.Count; i++)
        {
            var node = htmlNodes[i];
            ScriptTag script = node as ScriptTag;

            if (script != null)
            {

                Console.WriteLine(script.GetAttribute("src"));
                string key = script.GetAttribute("src").Trim();
                if(dict.ContainsKey(key))
                    text = text.Replace(key, dict[key]);
            }
        }

        File.WriteAllText(indexSave, text);
        File.WriteAllText(indexHtml, text);

    }








}