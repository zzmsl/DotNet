
namespace DotNet.Diagnostics
{
    public class Process
    {
        public static void KillByName(string processName)
        {
            System.Diagnostics.Process[] ps
                = System.Diagnostics.Process.GetProcessesByName(processName);

            foreach (System.Diagnostics.Process p in ps)
            {
                //HasExited：如果 Process 组件引用的操作系统进程已终止，则为 true；否则为 false。
                if (!p.HasExited)
                {
                    try { p.Kill(); }
                    catch { return; }
                }
            }
        }

        /// <summary>
        /// 是否在运行
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningByName(string processName)
        {
            System.Diagnostics.Process[] ps
                = System.Diagnostics.Process.GetProcessesByName(processName);

            foreach (System.Diagnostics.Process p in ps)
            { return true; }

            return false;
        }

        /// <summary>  
        /// 执行DOS命令，返回DOS命令的输出  
        /// </summary>  
        /// <param name="dosCommand">dos命令</param>  
        /// <param name="seconds">等待命令执行的时间（单位：秒），  
        /// 如果设定为0，则无限等待</param>  
        /// <returns>返回DOS命令的输出</returns>  
        public static string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串  
            if (command == null || command.Equals("")) return output;

            System.Diagnostics.Process process
                = new System.Diagnostics.Process();//创建进程对象  

            System.Diagnostics.ProcessStartInfo startInfo
                = new System.Diagnostics.ProcessStartInfo();

            startInfo.FileName = "cmd.exe";//设定需要执行的命令
            startInfo.WorkingDirectory = @"C:\Windows\System32";
            startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出  
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动  
            startInfo.RedirectStandardInput = false;//不重定向输入  
            startInfo.RedirectStandardOutput = true; //重定向输出  
            startInfo.CreateNoWindow = true;//不创建窗口  
            process.StartInfo = startInfo;

            try
            {
                if (process.Start())//开始进程  
                {
                    if (seconds == 0)
                        process.WaitForExit();//这里无限等待进程结束  
                    else
                        process.WaitForExit(seconds * 1000); //等待进程结束，等待时间为指定的毫秒
                    output = process.StandardOutput.ReadToEnd();//读取进程的输出  
                }
            }
            catch
            { }
            finally
            { if (process != null) process.Close(); }
            return output;
        }

        /// <summary>
        /// 直接运行指定路径的exe文件，如果已经运行，则不运行；没有运行，则运行。即单例
        /// </summary>
        /// <param name="path">D:\DotNet\bin\Debug\dotnet.exe</param>
        public static void Execute(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            int i = path.LastIndexOf('\\') + 1;
            string name = path.Substring(i, path.Length - i - ".exe".Length);
            string workingDirectory = path.Substring(0, i);
            bool isRunning = false;

            System.Diagnostics.Process[] ps = System.Diagnostics.Process.GetProcessesByName(name);
            foreach (System.Diagnostics.Process p in ps)
            {
                //HasExited：如果 Process 组件引用的操作系统进程已终止，则为 true；否则为 false。
                if (!p.HasExited)
                {
                    isRunning = true;
                }
            }

            if (!isRunning)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = workingDirectory;//设置启动路径
                p.StartInfo.FileName = name;//设置启动文件名
                p.Start();
            }
        }
    }


}
