using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ZipWrapper2.Services
{
    public class ZipService
    {
        private readonly string _sevenZipPath;
        private readonly int _defaultTimeout;

        public ZipService()
        {
            // Load configuration from appsettings.json
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(configPath))
            {
                string configJson = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);

                _sevenZipPath = config.ContainsKey("SevenZipPath") ? config["SevenZipPath"].ToString() : @"C:\Program Files\7-Zip\7z.exe";
                _defaultTimeout = config.ContainsKey("DefaultTimeout") ? Convert.ToInt32(config["DefaultTimeout"]) : 10000;
            }
            else
            {
                // Fallback to defaults if configuration file is missing
                _sevenZipPath = @"C:\Program Files\7-Zip\7z.exe";
                _defaultTimeout = 10000;
            }
        }

        public string ZipFile(string filePath, string password)
        {
            string directory = Path.GetDirectoryName(filePath);
            string zipFilePath = Path.Combine(directory, Path.GetFileNameWithoutExtension(filePath) + ".zip");
            string arguments = $"a \"{zipFilePath}\" \"{filePath}\"";

            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -p{password} -mem=AES256";
            }

            return RunProcess(arguments);
        }

        public string ZipMultipleFiles(IEnumerable<string> filePaths, string password)
        {
            // Ensure the list of files is not empty
            if (filePaths == null || !filePaths.Any())
            {
                return "No files provided to zip.";
            }

            // Get the directory of the first file
            string firstFilePath = filePaths.First();
            string directory = Path.GetDirectoryName(firstFilePath);

            // Build the zip file name with the full path
            string zipName = Path.Combine(directory, Path.GetFileNameWithoutExtension(firstFilePath) + ".zip");

            // Build the arguments for zipping multiple files
            string arguments = $"a \"{zipName}\"";

            foreach (string filePath in filePaths)
            {
                arguments += $" \"{filePath}\"";
            }

            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -p{password} -mem=AES256";
            }

            // Run the process and return the result
            return RunProcess(arguments);
        }

        public string ZipFolder(string folderPath, string password)
        {
            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                return "The specified folder does not exist.";
            }

            // Build the zip file name with the full path
            string zipFilePath = Path.Combine(Path.GetDirectoryName(folderPath), Path.GetFileName(folderPath) + ".zip");

            // Build the arguments for zipping the folder
            string arguments = $"a \"{zipFilePath}\" \"{folderPath}\\*\"";

            if (!string.IsNullOrEmpty(password))
            {
                arguments += $" -p{password} -mem=AES256";
            }

            // Run the process and return the result
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

            return RunProcess(arguments);
        }

        private string RunProcess(string arguments)
        {
            Console.WriteLine($"Command: {_sevenZipPath} {arguments}");
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = _sevenZipPath,
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
                if (!process.WaitForExit(_defaultTimeout)) // Use the timeout from configuration
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