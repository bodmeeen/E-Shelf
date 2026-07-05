using System.Text;
using SReader;
using SReader.Services;

// активує підтримку Windows-1251 для коректного читання fb2
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Console.Write("Enter the path to the book : ");

string inputPath = Console.ReadLine();
inputPath = inputPath.Trim('\'', '"');

string bookText = BookParser.ReadAnyBook(inputPath);
PageSeparator.Separator(bookText, inputPath);
