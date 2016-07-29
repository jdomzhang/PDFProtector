using CommandLine;

namespace PDFProtector.Console
{
    public class Options
    {
        [Option(HelpText = "Should the original source file be deleted after a protected version has been created?")]
        public bool DeleteOriginal { get; set; }

        [Option(HelpText = "The folder to monitor for new PDF files.")]
        public string MonitorPath { get; set; }

        [Option(HelpText = "The owner password for all incoming PDF files.")]
        public string OwnerPassword { get; set; }

        [Option(HelpText = "The folder to write the protected PDF files.")]
        public string OutputPath { get; set; }

        [Option(HelpText = "The user password for all incoming PDF files.")]
        public string UserPassword { get; set; }

    }
}