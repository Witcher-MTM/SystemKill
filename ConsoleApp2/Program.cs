using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool check = false;
          
            List<string> Session = new List<string>();
            string whiteList = "ConsoleApp2\nexplorer\nMicrosoft\nDesktop\nsystem\ndevenv";
            RegistryKey registry = Registry.CurrentUser.CreateSubKey("WhiteList");
            registry.SetValue("WhiteProc", whiteList);
            for (int i = 0; i <args.Length; i++)
            {
                Session.Add(args[i].ToLower());
            }
            

            for (int i = 0; i < Session.Count; i++)
            {
                if (Session[i].ToLower().Contains("last"))
                {
                    if (File.Exists("Processes.txt"))
                    {
                        Session.AddRange(File.ReadAllText("Processes.txt").Split("\n"));
                        Console.WriteLine("added last session");
                        File.Delete("Processes.txt");
                    }
                    else
                    {
                        Console.WriteLine("You haven't last session");
                        check = true;
                        break;
                    }
                    try
                    {
                        Session.Remove("last");
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            if (!check)
            {
                Task.Factory.StartNew(() => {

                    Console.WriteLine("exit-exit\nclose - exit with save");
                    string dispose = Console.ReadLine();
                    if (dispose.ToLower() == "exit")
                    {
                        Environment.Exit(0);
                    }
                    if (dispose.ToLower() == "close")
                    {
                        File.Delete("Processes.txt");
                        foreach (var item in Session)
                        {
                            File.AppendAllText("Processes.txt", item + "\n");
                        }
                        Environment.Exit(0);
                    }

                });
            }
           









            if (!check)
            {
                while (true)
                {

                    if (Session.Count > 0)
                    {

                        foreach (var x in Session)
                        {
                            foreach (var z in registry.GetValue("WhiteProc").ToString().Split("\n"))
                            {
                                if (x.ToLower().Contains(z.ToLower()))
                                {
                                    Console.WriteLine("You cant destroy this process!");
                                    check = true;
                                    break;

                                }
                            }
                            if (check)
                            {
                                break;
                            }
                            Thread.Sleep(2000);
                            Process.GetProcessesByName(x.ToString()).ToList().ForEach(item => item.Kill());
                        }

                    }
                    if (check)
                    {
                        break;
                    }

                }
            }
           
            



        }
    }
}
