using System;
using System.IO;
using System.ServiceProcess;


namespace serviceVesonPrepis
{
    public partial class ServiceVesconPrepis : ServiceBase
    {
        FileSystemWatcher fsw;
        public ServiceVesconPrepis()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("serviceVesconPrepis sa spustila " + DateTime.Now);
            string path = @"C:\Users\janik\OneDrive\PC\prepis";
            try
            {
                fsw = new FileSystemWatcher(path);
                fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                WriteToFile("Pocuvam na : " + path);
                fsw.Created += new FileSystemEventHandler(OnCreated);
                fsw.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                WriteToFile("Error: serviceVesconPrepis ma zlu cestu k suboru: " + DateTime.Now);
            }

        }

        protected override void OnStop()
        {
            WriteToFile("serviceVesonPrepis sa stopla : " + DateTime.Now);
        }

        protected override void OnShutdown()
        {
            WriteToFile("serviceVesonPrepis spadla RESTARTUJTE JU : " + DateTime.Now);
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
           
            string destination = e.Name;
            string jedinecny_kod = "!!!!!";
            string[] words = destination.Split('.');
            string new_file = @"C:\Users\janik\OneDrive\PC\hotovo\" + words[0] + jedinecny_kod+ "." + words[1];
            //Directory.Move(e.FullPath, @"C:\Users\janik\OneDrive\PC\prepis\" + words[0] + jedinecny_kod + "." + words[1]);
            if (File.Exists(e.FullPath))
            {
                try { 
                File.Copy(e.FullPath, new_file, true);
                    try
                    {
                        File.Delete(e.FullPath);
                    }
                    catch (Exception)
                    {
                        WriteToFile("WARNING: Subor sa nepodarilo vymazat " + e.FullPath);
                    }
                }
                catch
                {
                    WriteToFile("WARNING: Nepodarilo sa skopirovat a pridelit nove meno suboru" + e.FullPath +"Meno noveho suboru" + words[0]+jedinecny_kod+"."+words[1]);
                }
            }
            //  WriteToFile("Prepisujem subor: " + e.Name + ", pridavam jedinecny kod: " + jedinecny_kod);
        }


        public static void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
