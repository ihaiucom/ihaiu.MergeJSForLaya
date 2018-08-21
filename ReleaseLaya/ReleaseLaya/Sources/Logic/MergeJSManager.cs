using System;
using System.Collections.Generic;
using System.Text;
using Winista.Text.HtmlParser;
using Winista.Text.HtmlParser.Lex;
using Winista.Text.HtmlParser.Util;
using System.IO;
using Winista.Text.HtmlParser.Tags;
using Winista.Text.HtmlParser.Filters;
using Winista.Text.HtmlParser.Nodes;

public class MergeJSManager : LayaProjectManager
{

    List<MergeJSData> list = new List<MergeJSData>();
    public MergeJSManager(string projectPath): base(projectPath)
    {
       
    }

    public void LoadProject()
    {
        ReadIndex();
        MergeJS();
    }

    private void ReadIndex()
    {
        string text = File.ReadAllText(indexPath);
        Lexer lexer = new Lexer(text);
        Parser parser = new Parser(lexer);
        NodeList htmlNodes = parser.Parse(new OrFilter(new NodeClassFilter(typeof(RemarkNode)), new NodeClassFilter(typeof(ScriptTag)) ));

        MergeJSData mergeJSData = null;

        for (int i = 0; i < htmlNodes.Count; i++)
        {
            var node = htmlNodes[i];
            ScriptTag script = node as ScriptTag;
            RemarkNode remark = node as RemarkNode;

            if(remark != null)
            {
                string remarkText = remark.GetText();
                if(remarkText.StartsWith("MergeJSBegin"))
                {
                    mergeJSData = new MergeJSData();
                    mergeJSData.name = remarkText.Split(":")[1].Trim();
                    list.Add(mergeJSData);
                }
                else if(remarkText.StartsWith("MergeJSEnd"))
                {
                    mergeJSData = null;
                }
            }

            if (script != null)
            {
                if(mergeJSData != null)
                {
                    mergeJSData.AddPath(script.GetAttribute("src"));
                }
            }

        }
    }

    private void MergeJS()
    {
        StringWriter sw = new StringWriter();
        bool isFirst = true;
        foreach(MergeJSData item in list)
        {
            item.Merge(isFirst);
            item.Save();
            isFirst = false;

            sw.WriteLine(item.sw.ToString());
        }

        string path = Setting.MergeRoot + "/all.merge.js";
        File.WriteAllText(path, sw.ToString());

    }

    private void ReadNodeList(NodeList htmlNodes, int level = 0)
    {
        string padding = "";
        for(int i = 0; i <  level; i ++)
        {
            padding += " -- ";
        }
        for (int i = 0; i < htmlNodes.Count; i++)
        {
            var node = htmlNodes[i];


            Console.WriteLine(padding + " " + i + " " + node.GetType());

            var nodeList = node as NodeList;
            if(nodeList != null)
            {
                ReadNodeList(nodeList, level + 1);
            }

            var composite = node as CompositeTag;
            if (composite != null)
            {
                ReadCompositeTag(composite, level + 1);
            }
        }
    }


    private void ReadCompositeTag(CompositeTag htmlNodes, int level = 0)
    {
        string padding = "";
        for (int i = 0; i < level; i++)
        {
            padding += "--";
        }
        for (int i = 0; i < htmlNodes.ChildCount; i++)
        {
            var node = htmlNodes.GetChild(i);


            Console.WriteLine(padding + " " + i + " " + node.GetType());

            var nodeList = node as NodeList;
            if (nodeList != null)
            {
                ReadNodeList(nodeList, level + 1);
            }

            var composite = node as CompositeTag;
            if (composite != null)
            {
                ReadCompositeTag(composite, level + 1);
            }
        }
    }

}