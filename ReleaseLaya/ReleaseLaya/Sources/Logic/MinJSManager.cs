using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public  class MinJSManager
{
    public void ConfigToMin()
    {
        foreach(OptionsMinConfig config in Setting.Options.minConfigs)
        {
            Upload(config.paths, config.savePath);
        }
    }

    public void AutoToMin()
    {
        string[] files = Directory.GetFiles(Setting.MergeRoot, "*.merge.js");
        string savePath;
        foreach (string path in files)
        {
            savePath = Setting.MinRoot + "/" + Path.GetFileName(path).Replace(".merge.js", ".min.js" );
            Upload(new string[] { path }, savePath);
        }



        savePath = Setting.MinRoot + "/all.min.js";
        Upload(files, savePath);

    }


    public void Upload(string[] paths, string savePath)
    {
        string url = "http://tool.oschina.net/action/jscompress/multi_js_compress";
        var httpUpload = new HttpUpload();
        httpUpload.SetFieldValue("linebreak", "500000");
        httpUpload.SetFieldValue("munge", Setting.Options.minJsMunge.ToString());

        for(int i = 0; i <  paths.Length; i ++)
        {
            string path = paths[i];
            string fieldName = "file";
            if (i > 0)
                fieldName += (i + 1);


            byte[] fileBytepdf = File.ReadAllBytes(path);
            httpUpload.SetFieldValue(fieldName, fieldName, "application/x-javascript", fileBytepdf);

        }

        string responStr = "";
        bool result = httpUpload.Upload(url, out responStr);
        if(result)
        {
            PathHelper.CheckPath(savePath);
            File.WriteAllText(savePath, responStr);
        }
        else
        {
            Console.WriteLine("[Error]" + responStr);
        }
        Console.WriteLine((result ? "成功" : "失败") + "  " + savePath);
    }
}