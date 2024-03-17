using System.IO.Pipes;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Reflection;

namespace TestAutomation
{
    public static class TestExecutionScreenRecorder
    {
        public const string screenRecorderName = "TestExecutionScreenRecorder";
        static Process screenRecorderProcess = null;

        /// <summary>
        /// Starting Screen Recording Application
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        public static void StartApplication()
        {
            if (IsApplicationRunning)
            {
                Console.WriteLine($"{nameof(TestExecutionScreenRecorder)}.{nameof(StartApplication)} - already running");
                return;
            }
            else { StopApplication(); }
              

                var pathToRecorder = Path.Combine(nameof(TestExecutionScreenRecorder) + ".exe");
                
                ProcessStartInfo psi = new ProcessStartInfo()
                {
                    FileName = pathToRecorder,
                    UseShellExecute = false,
                    RedirectStandardError = true
                };

                screenRecorderProcess = new Process();
                ConcurrentQueue<string> stdErrQ = new ConcurrentQueue<string>();
                screenRecorderProcess.ErrorDataReceived += (sender, args) => stdErrQ.Enqueue(args.Data);
                screenRecorderProcess.StartInfo = psi;
                screenRecorderProcess.Start();
                screenRecorderProcess.BeginErrorReadLine();
                Task.Yield();
                if (!(ExecuteCommand("Status") == "Ready"))
                    throw new ApplicationException("Cannot start Screen Recorder Executable");
          
        }

        public static void StopApplication()
        {
            Console.WriteLine("Stopping Screen Recording Application");

            if (IsApplicationRunning)
            {
                ExecuteCommand("Exit");
            }
            else
            {
                Console.WriteLine("Application is NOT running - won't call \"Exit\"");
            }
            Task.Delay(1000).Wait();
            try
            {
                screenRecorderProcess.Kill();
            }
            catch (Exception) { }
            screenRecorderProcess = null;
            Console.WriteLine("Screen Recording Application should be stopped by now.");
        }

        public static bool IsApplicationRunning
        {
            get
            {
                try
                {
                    ExecuteCommand("Status");
                    return true;
                }
                catch (TimeoutException)
                {
                    Console.WriteLine("TimeoutException: Screen recorder is not running.");
                    return false;
                }
                catch (TargetInvocationException)
                {
                    Console.WriteLine("TargetInvocationException: Screen recorder is not running.");
                    return false;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Caught exception in IsApplicationRunning: {e}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Starting Screen Recording with file name
        /// </summary>
        /// <param name="fileName">Filename of the recorded xesc file</param>
        /// <returns>True if 'Success' else false</returns>
        public static bool StartRecording(string fileName)
        {
            bool result = false;

            Console.WriteLine($"Starting Screen Recording with file name of \"{fileName}\".", () =>
            {
                fileName = Path.Combine(
                Path.GetDirectoryName(fileName),
                "Video_" + Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("-HHmmss") + ".xesc");

                if (!IsApplicationRunning)
                    StartApplication();

                string answer = "";
                for (int i = 0; (i < 4) && (!answer.StartsWith("Success")); i++)
                {
                    try
                    {
                        answer = ExecuteCommand("Start " + fileName, TimeSpan.FromSeconds(5));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("(suspended)exception while Starting a recording throug sending a command: " + ex.Message);
                        answer = "";
                        Console.WriteLine($"Attempt {i} for Start recording will be made.");
                    }
                    Task.Delay(1000).Wait();
                }

                if (string.IsNullOrEmpty(answer))
                {
                    Console.WriteLine("Cannot start Screen Recording!");
                }
                else
                {
                    Console.WriteLine("Screen Recording started successfully.");
                    result = answer.StartsWith("Success");
                }
            });

            return result;
        }

        public static void StopRecording(bool deleteRecording)
        {
            Console.WriteLine($"Stopping recording ({deleteRecording}) ");
            try
            {
                string answer = ExecuteCommand("Stop " + deleteRecording.ToString(), TimeSpan.FromSeconds(60));
                if (answer == "Success")
                {
                    Console.WriteLine("Screen Recording stopped");
                }
                else
                {
                    Console.WriteLine($"Execution of the stop command returned \"{answer}\"");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("(suspended)exception while Stopping the recording: " + ex.Message);
            }
        }

        private static string ExecuteCommand(string command, TimeSpan? timeOut = null)
        {
            Console.WriteLine($"{nameof(ExecuteCommand)}({command})");
            NamedPipeClientStream stream = new NamedPipeClientStream("Milestone.TA.ScreenRecorderPipe");
            Console.WriteLine("Got stream");
            stream.Connect(timeOut == null ? 5000 : timeOut.Value.Milliseconds);
            Console.WriteLine("stream connected");
            try
            {
                while (!stream.CanWrite)
                {
                    Console.WriteLine("Waiting for stream to be writable");
                    Task.Yield();
                    Task.Delay(1000).Wait();
                }

                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                writer.WriteLine(command);
                writer.Flush();

                while (!stream.CanRead)
                {
                    Console.WriteLine("Waiting for stream to be readable");
                    Task.Yield();
                    Task.Delay(1000).Wait();
                }

                return reader.ReadLine(); //returning null is fine here
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
