namespace ExpenseManager;

public class Expense{
    public string Name{get; set;}
    public string Category{get; set;}
    public decimal Amount{get; set;}
    public DateTime Date{get; set;}
    public string Description{get; set;}

    public Expense(string name, string category, decimal amount, DateTime date, string description){
            Name = name;
            Category = category;
            Amount = amount;
            Date = date;
            Description = description;
        }
}