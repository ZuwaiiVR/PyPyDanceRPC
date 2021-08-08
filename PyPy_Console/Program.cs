using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using DiscordRPC.Logging;


namespace PyPy_Console
{
    class Program
    {
        public static DiscordRpcClient client;
        static string ver = "v1.0a";
        static string[] logo = {
" ######          ######          ######                                       ######   ######    #####  ",
" #     #  #   #  #     #  #   #  #     #    ##    #    #   ####   ######      #     #  #     #  #     # ",
" #     #   # #   #     #   # #   #     #   #  #   ##   #  #    #  #           #     #  #     #  #       ",
" ######     #    ######     #    #     #  #    #  # #  #  #       #####       ######   ######   #       ",
" #          #    #          #    #     #  ######  #  # #  #       #           #   #    #        #       ",
" #          #    #          #    #     #  #    #  #   ##  #    #  #           #    #   #        #     # ",
" #          #    #          #    ######   #    #  #    #   ####   ######      #     #  #         #####  "};

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            client.Dispose();


        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            foreach (var a in logo)
            {
                Console.WriteLine(a);
            }
            Console.WriteLine("");
            Console.WriteLine("PyPyDance Discord Rich Presence " + ver);
            Console.WriteLine("Join the discord! http://jd.pypy.moe/discord");
            Console.WriteLine();
            Console.WriteLine("Please add the command line args  \"VRChat.exe --enable-sdk-log-levels\" to let the magic work");
            Console.WriteLine();
  
            string logpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"Low\VRChat\VRChat";
            string localpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var directory = new DirectoryInfo(logpath);
            
        
            //config file
            bool save = false;
            bool mylogolol = false;
            bool rpc_ = true;
            string[] configlayout = { "DiscordRPCEnabled=","LogSongsToFile=","Logo=" };
            string[] configlayoutsave = { "DiscordRPCEnabled=true", "LogSongsToFile=false","Logo=true" };
            if (!File.Exists(localpath + "/conf.ini"))
            {
                //create file
                using (StreamWriter conf = File.CreateText(localpath + "/conf.ini")) //same here // Actual Youtube DL Commandline Args
                {
                    foreach (var a in configlayoutsave) conf.WriteLine(a);
                }
                Console.WriteLine("No config file, creating..");
            }
            string[] config_read = System.IO.File.ReadAllLines(localpath + "/conf.ini");
            foreach (var a in config_read)
            {
                Console.WriteLine(a);
                if (a.Contains(configlayout[0]))
                {
                    try
                    {
                        rpc_ = bool.Parse(a.Substring(configlayout[0].Length));
                    }
                    catch
                    {
                    }
                }
                if (a.Contains(configlayout[1]))
                {
                    try
                    {
                        save = bool.Parse(a.Substring(configlayout[1].Length));
                    }
                    catch
                    {
                    }
                }
                if (a.Contains(configlayout[2]))
                {
                    try
                    {
                        mylogolol = bool.Parse(a.Substring(configlayout[2].Length));
                    }
                    catch
                    {
                    }
                }
            }
            //
            if (rpc_) Initialize();
            while (true)
            {
                var logfile = directory.GetFiles("output_log_*.txt", SearchOption.TopDirectoryOnly).OrderByDescending(f => f.LastWriteTime).First();
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine(logfile.Name);
                Console.WriteLine();
               
                pypymagic(logpath + "\\" + logfile.Name, save, mylogolol, rpc_);
            }
            //

        }


        public static  void Initialize()
        { 
            //client = new DiscordRpcClient("872555415186071552");
            client = new DiscordRpcClient(System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String("ODcyNTU1NDE1MTg2MDcxNTUy")));
          
            // client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Connected! user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                
             Console.WriteLine("Discord RPC Update!");
            };
            client.Initialize();
           
        }



        public static void pypymagic(string filePath,bool saving,bool logo,bool discord)
        {
            string logpath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"Low\VRChat\VRChat";
            string localpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var directory = new DirectoryInfo(logpath);
            var initialFileSize = new FileInfo(filePath).Length;
            var lastReadLength = initialFileSize - 1024;
            if (lastReadLength < 0) lastReadLength = 0;
            DateTime dt = new DateTime(); //current length
            DateTime idletime = new DateTime(); //idle time
            bool has_time = false ;
            bool clear_presence = false;
            string logo_ = "";
            if (logo)
            {
                logo_ = "melogo";
            } else
            {
                logo_ = "pylol";
            }
            while (true)
            {
                try
                {
                    var logfile = directory.GetFiles("output_log_*.txt", SearchOption.TopDirectoryOnly).OrderByDescending(f => f.LastWriteTime).First();
                    if (logfile.Name != filePath.Substring(filePath.LastIndexOf("\\")+1))
                    {
                        Console.WriteLine("New Logfile Found, restarting..");
                        break;
                    }
                    if (clear_presence)
                    {
                        if (DateTime.UtcNow > idletime)
                        {
                            if (discord)
                            {
                                clear_presence = false;
                                client.ClearPresence();
                                Console.WriteLine("Clear Presence due idle");
                            }
                        }
                    }
                    if (has_time == true)
                    {
                        //
                        if (DateTime.UtcNow > dt + TimeSpan.FromSeconds(5))//change to idle after 5sec song finished 
                        {
                            has_time = false;
                            idletime = DateTime.UtcNow + TimeSpan.FromSeconds(30); //clear presence after 30sec after idle
                            clear_presence = true;
                            if (discord)
                            {
                                Console.WriteLine("Idle");
                                client.SetPresence(new RichPresence()
                                {
                                    Details = "Idle",
                                    State = "",

                                    Assets = new Assets()
                                    {
                                        LargeImageKey = "pypy_",
                                        LargeImageText = "PyPy Dance Rich thing by Zuwaii- Available soon on Github",
                                        SmallImageKey = logo_
                                    },
                                });
                            }
                        }
                    }
                    var fileSize = new FileInfo(filePath).Length;
                    if (fileSize > lastReadLength)
                    {
                        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            fs.Seek(lastReadLength, SeekOrigin.Begin);
                            var buffer = new byte[1024];
                            while (true)
                            {
                                var bytesRead = fs.Read(buffer, 0, buffer.Length);
                                lastReadLength += bytesRead;
                                if (bytesRead == 0)
                                    break;
                 
                                var text = Encoding.GetEncoding(65001).GetString(buffer, 0, bytesRead);

                                if (text.Contains("X] VideoPlay(PyPyDance)") )
                                {
                                    string abc = text.Substring(text.IndexOf("X] VideoPlay(PyPyDance)") + 23);
                                    abc = abc.Substring(0, abc.IndexOf(")\"")+2);
                                    string[] splitthiscrap = abc.Split(',');
                                    try
                                    {
                                        int cnt = 0;
                                        string rawname = "";
                                        foreach (var a in splitthiscrap)
                                        {
                                            cnt++;
                                            if (cnt > 3) rawname = rawname + a;
                                        }

                                        string url = "";
                                        if (splitthiscrap[0].Contains("https://jd.pypy.moe/api/v1/videos/"))
                                        {
                                            url = splitthiscrap[0].Replace("\"", "").Replace("https://jd.pypy.moe/api/v1/videos/", "http://youtu.be/").Replace(".mp4", "");
                                        }
                                        else
                                        {
                                            url = splitthiscrap[0].Replace("\"", "");
                                        }

                                        string name = rawname.Substring(rawname.LastIndexOf("(")).Replace("(", "").Replace(")", "");
                                        name = name.Substring(0, name.IndexOf("\""));
                                        string song = rawname.Substring(1, rawname.LastIndexOf("(") - 2);

                                        dt = DateTime.UtcNow + TimeSpan.FromSeconds((long)float.Parse(splitthiscrap[2], System.Globalization.CultureInfo.InvariantCulture));
                                        has_time = true;
                                        clear_presence = false;
                                        
                                        //create link for the big button in discord to youtube
                                        if (splitthiscrap[0].Contains("https://jd.pypy.moe/api/v1/videos/"))
                                        {
                                            Console.WriteLine(song + " | Requested by " + name);
                                        }
                                        else
                                        {
                                            Console.WriteLine(url + " | Requested by " + name);
                                        }
                                        if (discord)
                                        {
                                            client.SetPresence(new RichPresence()
                                            {
                                                Details = song,
                                                State = "Requested by " + name,
                                                Timestamps = new Timestamps()
                                                {
                                                    Start = DateTime.UtcNow + TimeSpan.FromSeconds((long)float.Parse(splitthiscrap[1], System.Globalization.CultureInfo.InvariantCulture)),
                                                    End = DateTime.UtcNow + TimeSpan.FromSeconds((long)float.Parse(splitthiscrap[2], System.Globalization.CultureInfo.InvariantCulture))
                                                },
                                                Assets = new Assets()
                                                {
                                                    LargeImageKey = "pypy_",
                                                    LargeImageText = "PyPy Dance Rich thing by Zuwaii- Available soon on Github",
                                                    SmallImageKey = logo_
                                                },
                                                Buttons = new Button[]
                                                {
                                                new Button() { Label = "Watch On Youtube", Url = url }
                                                }
                                                
                                            });
                                        }
                                        if (saving)
                                        {
                                            using (StreamWriter sw2 = File.AppendText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"/pypylog.txt"))
                                            {
                                                DateTime dta = DateTime.Now;
                                                if (!splitthiscrap[0].Contains("https://jd.pypy.moe/api/v1/videos/"))
                                                {
                                                    sw2.WriteLine(dta.ToString(System.Globalization.CultureInfo.InvariantCulture) + " | " + url + " (" + name + ")");
                                                }
                                                else
                                                {
                                                    sw2.WriteLine(dta.ToString(System.Globalization.CultureInfo.InvariantCulture) + " | " + song + " (" + name + ")");
                                                }
                                            }
                                        }
                                    }

                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("[Error] " + ex.Message + "--" + ex.InnerException);
                                    }
                                }                                 
                            }
                        }
                    }
                }
                catch { }
                Thread.Sleep(1000);//poll
            }
        }

    }
}
