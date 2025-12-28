using Core.IO;
using Mafia2Tool.Forms;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Toolkit.Forms;
using Utils.Language;
using Utils.Settings;

namespace Mafia2Tool
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            StartForm splash = new StartForm();
            splash.Show();
            Application.DoEvents();
            Thread.Sleep(3000);

            ToolkitAssemblyLoadContext.SetupLoadContext();
            ToolkitExceptionHandler.Initialise();

            if (args.Length > 0)
            {
                CheckINIExists();
                ToolkitSettings.ReadINI();
                ProcessCommandArguments(args);
                splash.Close();
                return;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            CheckINIExists();
            ToolkitSettings.ReadINI();

            GameStorage.Instance.InitStorage();
            Language.ReadLanguageXML();
        
            splash.Close();
            splash.Dispose();

            if (ToolkitSettings.SkipGameSelector)
            {
                GameStorage.Instance.SetSelectedGameByIndex(ToolkitSettings.DefaultGame);
                OpenGameExplorer();
                return;
            }

            GameSelector selector = new GameSelector();
            if (selector.ShowDialog() == DialogResult.OK)
            {
                selector.Dispose();
                OpenGameExplorer();
            }
        }


        private static void ProcessCommandArguments(string[] Args)
        {
            Cursor.Current = Cursors.WaitCursor;
            if(Args[0].Equals("-gt"))
            {
                GamesEnumerator GameType = (GamesEnumerator)Enum.Parse(typeof(GamesEnumerator), Args[1]);

                if(Args[2].Equals("-SDSPack"))
                {
                    string SDSPath = Args[3];
                    string ExportPath = Args[4];

                    FileInfo SDSFileInfo = new FileInfo(SDSPath);
                    FileSDS SDSFile = new FileSDS(SDSFileInfo);
                    SDSFile.SaveSDSWithCustomFolder(GameType, ExportPath);
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private static void OpenGameExplorer()
        {
            GameExplorer explorer = new GameExplorer();
            explorer.ShowDialog();
            explorer.Dispose();
        }

        private static void CheckINIExists()
        {
            string PathToIni = Path.Combine(Application.ExecutablePath, "MafiaToolkit.ini");
            if (!File.Exists(PathToIni))
            {
                new IniFile();
            }

        }
    }

    public static class ToolkitAssemblyLoadContext
    {
        private static bool bAppliedCallback = false;

        public static void SetupLoadContext()
        {
            if (!bAppliedCallback)
            {
                AssemblyLoadContext.Default.Resolving += Default_Resolving;
                bAppliedCallback = true;
            }
        }

        private static System.Reflection.Assembly Default_Resolving(AssemblyLoadContext ALC, System.Reflection.AssemblyName AssemblyName)
        {
            string probeSetting = AppContext.GetData("SubdirectoriesToProbe") as string;
            if (string.IsNullOrEmpty(probeSetting))
            {
                return null;
            }

            foreach (string subdirectory in probeSetting.Split(';'))
            {
                string pathMaybe = Path.Combine(AppContext.BaseDirectory, subdirectory, $"{AssemblyName.Name}.dll");
                if (File.Exists(pathMaybe))
                {
                    return ALC.LoadFromAssemblyPath(pathMaybe);
                }
            }

            return null;
        }
    }

    public static class ToolkitExceptionHandler
    {
        public static void Initialise()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                ToolkitExceptionHandler.ShowExceptionForm((Exception)e.ExceptionObject);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                ToolkitExceptionHandler.ShowExceptionForm(e.Exception);
            }
        }

        private static void ShowExceptionForm(Exception InException)
        {
            ExceptionForm Form = new ExceptionForm();
            Form.ShowException(InException);

            DialogResult Result = Form.ShowDialog();
            if (Result == DialogResult.No)
            {
                Application.Exit();
            }
        }
    }
}
