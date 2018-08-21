using System;
using System.Collections.Generic;
using System.Text;

public class LayaProjectManager
{
    protected string projectPath;

    protected string binPath
    {
        get
        {
            return projectPath + "/bin";
        }
    }

    protected string indexPath
    {
        get
        {
            return binPath + "/index.html";
        }
    }

    public LayaProjectManager(string projectPath)
    {
        this.projectPath = projectPath;
    }
}