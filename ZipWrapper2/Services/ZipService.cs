using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ZipWrapper2.Services
{
    public class ZipService
    {
        private const string SevenZipPath = @"""C:\Program Files\7-Zip\7z.exe"""; // Adjust the path to where 7-Zip is installed

        public string ZipFile(string filePath, string password)
        {
            string zipFilePath = Path.ChangeExtension(filePath, ".zip");
            string arguments = $"a \"{zipFilePath}\" \"{filePath}\"";

            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -p{password} -mem=AES256";
            }

            return RunProcess(arguments);
        }

        public string UnzipFile(string zipFilePath, string password)
        {
            string extractPath = Path.GetDirectoryName(zipFilePath);
            string arguments = $"x \"{zipFilePath}\" -o\"{extractPath}\"";

            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -p{password}";
            }
            Console.WriteLine($"Command: {SevenZipPath} {arguments}");
            return RunProcess(arguments);
        }

        private string RunProcess1(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = SevenZipPath,
                Arguments = arguments,
                // RedirectStandardOutput = true,
                // RedirectStandardError = true, // Capture error messages as well
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();


                process.WaitForExit();
                // Read the standard output and error streams
                string output = "";//process.StandardOutput.ReadToEnd();
                string error = "";//process.StandardError.ReadToEnd();

                // Combine output and error messages
                if (!string.IsNullOrEmpty(error))
                {
                    output += Environment.NewLine + "Error: " + error;
                }

                return output;
            }
        }
        private string RunProcess(string arguments)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = SevenZipPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                // Use asynchronous reading to prevent hanging
                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                // Set a timeout for the process
                if (!process.WaitForExit(10000)) // Timeout after 10 seconds
                {
                    process.Kill(); // Terminate the process if it hangs
                    return "The operation timed out. Please check if the file is password-protected or if the input is valid.";
                }

                // Ensure the tasks are completed
                string output = outputTask.Result;
                string error = errorTask.Result;

                // Log the output and error for debugging
                Console.WriteLine("Output: " + output);
                Console.WriteLine("Error: " + error);

                // Check for specific errors (e.g., password prompt)
                if (!string.IsNullOrEmpty(error) && error.Contains("Wrong password"))
                {
                    return "The provided password is incorrect. Please try again.";
                }
                else if (!string.IsNullOrEmpty(error))
                {
                    return "An error occurred: " + error;
                }

                // Return the output if no errors occurred
                return string.IsNullOrEmpty(output) ? "Operation completed successfully." : output;
            }
        }

    }
    
}