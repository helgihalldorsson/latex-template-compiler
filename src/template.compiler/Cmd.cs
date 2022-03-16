using System.Diagnostics;

namespace template.compiler
{
    public class Cmd
    {
        private string output = "";
        private Process cmd = null;
        
        public string Run(string workingDirectory, string argument)
        {
            output = "";
            cmd = new Process();
            cmd.StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            cmd.OutputDataReceived += ReadOutputData;
            cmd.Start();
            cmd.BeginOutputReadLine();
            cmd.StandardInput.WriteLine(argument);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            cmd.CancelOutputRead();
            cmd.Close();
            return output;
        }

        private void ReadOutputData(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                if(cmd != null)
                {
                    cmd.CancelOutputRead();
                }
                return;
            }
            output += e.Data + "\n";
        }
    }
}
