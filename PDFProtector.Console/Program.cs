using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace PDFProtector.Console
{
    public class Program
    {
        static int Main(string[] args)
        {
            if(args.Length < 2)
            {
                System.Console.WriteLine("Please run it with parameters, e.g.");
                System.Console.WriteLine("\t{0} ..\\..\\abc.pdf 111", Environment.GetCommandLineArgs());
                return 1;
            }

            var path = args[0];
            var password = args[1];

            return AddPassword(path, password);
        }

        private static int AddPassword(string path, string password)
        {
            path = Path.GetFullPath(path);

            if (!File.Exists(path))
            {
                System.Console.WriteLine("File does not exists: {0}", path);
                return 1;
            }

            string outputPath = string.Format("{0}{1}{2}-encrypted{3}"
                , Path.GetDirectoryName(path)
                , Path.DirectorySeparatorChar
                , Path.GetFileNameWithoutExtension(path)
                , Path.GetExtension(path));

            //System.Console.WriteLine(outputPath);
            if(File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            PdfDocument document = null;

            try
            {
                document = PdfReader.Open(path);
                var securitySettings = document.SecuritySettings;

                securitySettings.UserPassword = password;
                //securitySettings.OwnerPassword = options.OwnerPassword;

                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = false;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = false;
                securitySettings.PermitPrint = true;

                document.Save(outputPath);

                document.Close();

                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
            finally
            {
                document?.Close();

                System.Console.WriteLine($"File {path} has been processed");
            }
        }
    }
}
