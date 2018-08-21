using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        //注册EncodeProvider
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);



        Setting.Init(args);


        switch (Setting.cmd)
        {
            // 合并js
            case CmdType.mergejs:
                mergeJSManager();
                break;
            // 压缩 js
            case CmdType.minjs:
                new MinJSManager().ConfigToMin();
                break;
            // 压缩 js
            case CmdType.autominjs:
                new MinJSManager().AutoToMin();
                break;
        }

        Console.WriteLine("完成!");

        if (!Setting.Options.autoEnd)
            Console.Read();
    }

    static void mergeJSManager()
    {
        MergeJSManager mergeJS = new MergeJSManager(Setting.Options.layaProject);
        mergeJS.LoadProject();
    }

}