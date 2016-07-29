using System;
using System.IO;
using System.Threading;
using CommandLine;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Security;

namespace PDFProtector.Console
{
    public class Program
    {
        private static bool isRunning;

        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args).MapResult(Run, _ => 1);
        }

        private static int Run(Options options)
        {
            var fileSystemWatcher = new FileSystemWatcher(options.MonitorPath, "*.pdf")
            {
                IncludeSubdirectories = false,
                NotifyFilter = NotifyFilters.FileName |
                               NotifyFilters.DirectoryName |
                               NotifyFilters.LastWrite
            };

            System.Console.WriteLine($"Monitoring {options.MonitorPath}...");

            foreach (var file in Directory.GetFiles(options.MonitorPath, "*.pdf"))
            {
                ProtectDocument(file, options);
            }

            fileSystemWatcher.Created += (s, e) => OnFileCreated(e, options);
            fileSystemWatcher.EnableRaisingEvents = true;
            isRunning = true;

            while (isRunning)
            {
                Thread.Sleep(1000);
            }

            return 0;
        }

        private static void OnFileCreated(FileSystemEventArgs eventArgs, Options options)
        {
            if (!eventArgs.ChangeType.HasFlag(WatcherChangeTypes.Created))
            {
                return;
            }

            ProtectDocument(eventArgs.FullPath, options);
        }

        private static void ProtectDocument(string path, Options options)
        {
            var filename = Path.GetFileName(path);
            if (string.IsNullOrEmpty(filename))
            {
                System.Console.WriteLine($"Failed to get filename from {path}");
                return;
            }

            System.Console.WriteLine($"Processing file {path}");

            var outputPath = Path.Combine(options.OutputPath, filename);
            if (File.Exists(outputPath))
            {
                System.Console.WriteLine($"Output file already exists: {outputPath}");
            }

            PdfDocument document = null;

            try
            {
                document = PdfReader.Open(path);
                var securitySettings = document.SecuritySettings;

                securitySettings.UserPassword = options.UserPassword;
                securitySettings.OwnerPassword = options.OwnerPassword;

                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = false;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = false;
                securitySettings.PermitPrint = false;

                document.Save(outputPath);

                document.Close();

                if (options.DeleteOriginal)
                {
                    System.Console.WriteLine($"Deleting unprotected source file {path}");
                    File.Delete(path);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                document?.Close();

                System.Console.WriteLine($"File {path} has been processed");
            }
        }
    }
}
