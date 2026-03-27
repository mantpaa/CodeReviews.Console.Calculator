using CalculatorLibrary;

namespace CalculatorProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;
            Calculator calculator = new Calculator();

            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");
            Console.WriteLine("This program has executed " + calculator.GetTimesRun() +" times.");

            while (!endApp)
            {
                string? readUserInput;
                bool validInput = false;
                DisplayMainMenu();
                readUserInput = Console.ReadLine();
                while (!validInput)
                {
                    if (readUserInput == null) readUserInput = ""; // avoid null dereference in switch
                    
                    validInput = true; // instead of updating it every valid case, we now only set it to false in  the default case
                    switch (readUserInput.ToLower().Trim())
                    {
                        case "1":
                            calculator.SelectOperation();
                            break;
                        case "2":
                            calculator.DisplayHistory();
                            Console.WriteLine();
                            break;
                        case "3":
                            calculator.DeleteHistory();
                            Console.WriteLine("History deleted...");
                            break;
                        case "4":
                            Console.Clear();
                            break;
                        case "5":
                            endApp = true;
                            break;
                        default:
                            validInput = false;
                            Console.WriteLine("Unknown command, try again: ");
                            readUserInput = Console.ReadLine();
                            break;
                    }
                }
            }

            // Add call to close the JSON writer before return
            calculator.Finish();
            return;
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("Enter a number to select your option.");
            Console.WriteLine("1. Do a math operation");
            Console.WriteLine("2. See history");
            Console.WriteLine("3. Delete history");
            Console.WriteLine("4. Clear screen");
            Console.WriteLine("5. exit");
            Console.Write("Your option: ");
        }
    }
}