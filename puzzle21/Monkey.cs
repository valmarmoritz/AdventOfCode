// See https://aka.ms/new-console-template for more information

internal class Monkey
{
    public string Name { get; set; }
    public string Task { get; set; }
    public char Operation { get; set; }
    public long Number { get; set; }
    public string FirstInput { get; set; }
    public string SecondInput { get; set; }
    public void Yell(Dictionary<string, long> yelledNumbers)
    {
        if (Operation == '=')
        {
            yelledNumbers[Name] = Number;
            return;
        }

        if (yelledNumbers.ContainsKey(FirstInput) && yelledNumbers.ContainsKey(SecondInput))
        {
            long y = 0;
            long first_op = yelledNumbers.First(x => x.Key == FirstInput).Value;
            long sec_op = yelledNumbers.First(x => x.Key == SecondInput).Value;

            switch (Operation)
            {
                case '+':
                    if (first_op != 0 && sec_op != 0)
                    {
                        y = first_op + sec_op;
                    }
                    break;
                case '-':
                    if (first_op != 0 && sec_op != 0)
                    {
                        y = first_op - sec_op;
                    }
                    break;
                case '*':
                    if (first_op != 0 && sec_op != 0)
                    {
                        y = first_op * sec_op;
                    }
                    break;
                case '/':
                    if (first_op != 0 && sec_op != 0)
                    {
                        y = first_op / sec_op;
                    }
                    break;
                default:
                    break;
            }

            yelledNumbers[Name] = y;
        }
        else
        {
            return;
        }
    }
}