# PDFProtector
This sample code demostrates how to use `PDFsharp` to add password protection to an existing PDF file.

## Debug
Pull all the source code to your local machine, use Visual Studio to open the solution, run it in Debug mode. It will encrypt the `abc.pdf` file on the root folder with the password `111`.

## Run
`PDFProtector.Console.exe ..\..\abc.pdf 111`

`..\..\abc.pdf` is the test PDF file, `111` is the password to protect that PDF file.

After it ran successfully, it will generate an password-protected PDF file on the same folder as the original file. In this case, it will generate `abc-encrypted.pdf` file.
