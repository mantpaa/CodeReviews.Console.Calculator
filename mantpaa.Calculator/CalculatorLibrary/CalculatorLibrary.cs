using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter writer;
        List<OperationHistory> operationsHistory;
        private int runCount;
        private const string RunCountFile = "runcount.txt";
        public Calculator()
        {
            runCount = 1;
            if (File.Exists(RunCountFile))
            {
                string readString =  File.ReadAllText(RunCountFile);

                if (int.TryParse(readString, out runCount))
                {
                    runCount++;
                }
            }

            File.WriteAllText(RunCountFile, runCount.ToString());

            StreamWriter logFile = File.CreateText("calculator.log");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();

            operationsHistory = new List<OperationHistory>();
        }

        public void SelectOperation()
        {
            double[] operands = new double[] {0,0};
            bool isValid = false;
            Console.WriteLine("Valid operations (+,-,*,/,^,sqrt,10x,sin,cos,tan,arcsin,arccos,arctan)");
            Console.Write("Your option: ");
            string readResult = Console.ReadLine();
            if (readResult == null) readResult = ""; // meh solution (avoid null dereference)
            
            while (isValid == false)
            {
                isValid = true; // avoid setting it to true in every accepted case, only set to false in default case
                
                switch(readResult.ToLower().Trim())
                {
                    case "+":
                        operands = GetOperands("addition", 2);
                        break;
                    case "-":
                        operands = GetOperands("subtraction", 2);
                        break;
                    case "*":
                        operands = GetOperands("multiplication", 2);
                        break;
                    case "/":
                        operands = GetOperands("division", 2);
                        break;
                    case "^":
                        operands = GetOperands("power", 2);
                        break;
                    case "sqrt":
                        operands = GetOperands("sqrt", 1);
                        break;
                    case "10x":
                        operands = GetOperands("10x", 1);
                        break;
                    case "sin":
                        operands = GetOperands("sin", 1);
                        break;
                    case "cos":
                        operands = GetOperands("cos", 1);
                        break;
                    case "tan":
                        operands = GetOperands("tan", 1);
                        break;
                    case "arcsin":
                        operands = GetOperands("arcsin", 1);
                        break;
                    case "arccos":
                        operands = GetOperands("arccos", 1);
                        break;
                    case "arctan":
                        operands = GetOperands("arctan", 1);
                        break;
                    default:
                        Console.WriteLine("Invalid operator " +  readResult + ", try again: ");
                        isValid = false;
                        readResult = Console.ReadLine();
                        if (readResult == null) readResult = "";
                        break;
                }
            }

            double resultOperation = DoOperation(operands, readResult);
            Console.WriteLine("Result of operation: " + readResult + " : " + resultOperation);
        }

        public double DoOperation(double [] operands, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.

            writer.WriteStartObject();
            for (int i = 0; i<operands.Count(); i++)
            {
                writer.WritePropertyName("Operand"+(i+1));
                writer.WriteValue(operands[i]);
            }
            
            writer.WritePropertyName("Operation");

            // Use a switch statement to do the math.
            switch (op)
            {
                case "+":
                    result = operands[0] + operands[1];
                    writer.WriteValue("Addition");
                    operationsHistory.Add(new OperationHistory(string.Format("{0} + {1}", operands[0], operands[1]), result));
                    break;
                case "-":
                    result = operands[0] - operands[1];
                    writer.WriteValue("Subtraction");
                    operationsHistory.Add(new OperationHistory(string.Format("{0} + {1}", operands[0], operands[1]), result));
                    break;
                case "*":
                    result = operands[0] * operands[1];
                    writer.WriteValue("Multiplication");
                    operationsHistory.Add(new OperationHistory(string.Format("{0} + {1}", operands[0], operands[1]), result));
                    break;
                case "/":
                    result = operands[0] / operands[1];
                    writer.WriteValue("Division");
                    operationsHistory.Add(new OperationHistory(string.Format("{0} + {1}", operands[0], operands[1]), result));
                    break;
                case "^":
                    result = Math.Pow(operands[0], operands[1]);
                    writer.WriteValue("Power");
                    operationsHistory.Add(new OperationHistory(string.Format("{0} ^ {1}", operands[0], operands[1]), result));
                    break;
                case "sqrt":
                    result = Math.Sqrt(operands[0]);
                    writer.WriteValue("Sqrt");
                    operationsHistory.Add(new OperationHistory(string.Format("sqrt({0})", operands[0]), result));
                    break;
                case "10x":
                    result = Math.Pow(10, operands[0]);
                    writer.WriteValue("Power");
                    operationsHistory.Add(new OperationHistory(string.Format("10 ^ {0}", operands[0]), result));
                    break;
                case "sin":
                    result = Math.Sin(ToRadians(operands[0]));
                    writer.WriteValue("sin");
                    operationsHistory.Add(new OperationHistory(string.Format("sin({0})", operands[0]), result));
                    break;
                case "cos":
                    result = Math.Cos(ToRadians(operands[0]));
                    writer.WriteValue("cos");
                    operationsHistory.Add(new OperationHistory(string.Format("cos({0})", operands[0]), result));
                    break;
                case "tan":
                    result = Math.Tan(ToRadians(operands[0]));
                    writer.WriteValue("tan");
                    operationsHistory.Add(new OperationHistory(string.Format("tan({0})", operands[0]), result));
                    break;
                case "arcsin":
                    result = ToDegrees(Math.Asin(operands[0]));
                    writer.WriteValue("Asin");
                    operationsHistory.Add(new OperationHistory(string.Format("asin({0})", operands[0]), result));
                    break;
                case "arccos":
                    result = ToDegrees(Math.Acos(operands[0]));
                    writer.WriteValue("Acos");
                    operationsHistory.Add(new OperationHistory(string.Format("acos({0})", operands[0]), result));
                    break;
                case "arctan":
                    result = ToDegrees(Math.Atan(operands[0]));
                    writer.WriteValue("Atan");
                    operationsHistory.Add(new OperationHistory(string.Format("atan({0})", operands[0]), result));
                    break;

                // Return text for an incorrect option entry.
                default:
                    writer.WriteValue("Error, default clause encountered, input was: " +op);
                    break;
            }

            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();
            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        public int GetTimesRun()
        {
            return runCount;
        }

        private double[] GetOperands(string operName, int operandsNeeded)
        {
            double[] operandsResult = new double[operandsNeeded];
            Console.WriteLine($"Operation {operName} requires {operandsNeeded} operands.");
            for (int i = 0; i < operandsNeeded; i++)
            {
                string readResult = "";
                double readValue = 0;
                bool isValid = false;

                while (!isValid)
                {
                    Console.Write($"Operand {i + 1} - Enter a number, or write 'hist', to select a value from previous calculations: ");
                    readResult = Console.ReadLine();
                    
                    if (readResult == "hist")
                    {
                        readResult = ReadResultFromHist();
                    }
                    
                    // value is either direct or from hist (or cancelled from hist meaning empty)
                    if (double.TryParse(readResult, out readValue))
                    {
                        operandsResult[i] = readValue;
                        isValid = true;
                    }
                }          
                operandsResult[i] = readValue;
            }

            return operandsResult;
        }

        private string ReadResultFromHist()
        {
            // show hist
            DisplayHistory();
            // query user for result to use
            Console.WriteLine("Select index of equation to use it, or select b to go back.");
            string readResult = Console.ReadLine();
            string returnValue = "";
            bool isValid = false;

            while (!isValid)
            {
                int indexChoice = -1;
                if (readResult == "b")
                {
                    isValid = true;
                    returnValue = "";
                }               
                else if (!int.TryParse(readResult, out indexChoice))
                {
                    Console.WriteLine("Could not interpret input as an index, try again: ");
                    readResult = Console.ReadLine();
                }
                else if (indexChoice >= operationsHistory.Count || indexChoice < 0)
                {
                    Console.WriteLine("Index chosen is out of bounds, try again:");
                    readResult = Console.ReadLine();
                }
                else
                {
                    returnValue = operationsHistory[indexChoice].Result.ToString();
                    isValid = true;
                }
            }
           
            return returnValue;
        }

        public void DisplayHistory()
        {
            int count = 0;
            Console.WriteLine("Equations\t\tResult");
            foreach (var calc in operationsHistory)
            {
                Console.WriteLine("[" + count + "]" + calc.Equation + "\t=\t" + calc.Result);
                count++;
            }
        }

        public void DeleteHistory()
        {
            operationsHistory.Clear();
        }

        static double ToDegrees(double radians) => radians * 180.0 / Math.PI;
        static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
    }

    public class OperationHistory
    {
        public string Equation { get; }
        public double Result { get; }

        public OperationHistory(string equation, double result)
        {
            Equation = equation;
            Result = result;
        }
    }
}