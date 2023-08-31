foreach (var arg in args)
{
    Console.WriteLine(arg);
}
// I
/*
Random r = new Random();
if (args.Length < 3)
{
    Console.WriteLine("Welcome to the magic eight ball \n What is your question?");
    string question = Console.ReadLine();
    int ans = r.Next(3);
    string answer;
    if(ans == 0)
    {
        answer = "Nuh uh";
    }
    else
    {
        answer = "Yup";
    }
    Console.WriteLine($"The answer to your question: {question}, is: {answer}");

}
else
{
    var message = args[1];
    var amount = int.Parse(args[2]);
    foreach (int i in Enumerable.Range(1, amount))
    {
        var wait = r.Next(100, 1000);
        await Task.Delay(wait);
        Console.Write(message + " " + i + " ");
        
    }
}
*/

//Bjørnekode


/*
if (args.Length < 3)
{
    Console.WriteLine("Needs 3 arguments.");
}
if (args[0] == "say")
{
    var message = args[1];
    var frequency = int.Parse(args[2]);
    var j = 1.0;
    foreach (var i in Enumerable.Range(1, frequency))
    {
        var wait = (int)(4000 / j);
        Console.WriteLine(message + " ");
        Thread.Sleep(wait);
        await Task.Delay(TimeSpan.FromMilliseconds(wait)); // does same as previous line
        j = j * 1.2;
    }
}
*/