namespace ExpenseManager;

public static class FileManager{
    public static void Save(string fileName, List<Expense> expenses){
        using StreamWriter writer = new(fileName, false);

        foreach(Expense expense in expenses){
            string line = 
                $"{Escape(expense.Name)};" +
                $"{Escape(expense.Category)};" +
                $"{expense.Amount};" +
                $"{expense.Date:yyyy-MM-dd};" +
                $"{Escape(expense.Description)}";
            writer.WriteLine(line);
        }
    }

    public static List<Expense> Load(string fileName){
        List<Expense> expenses = new();

        if(!File.Exists(fileName)) return expenses;

        foreach(string line in File.ReadAllLines(fileName)){
            if(string.IsNullOrWhiteSpace(line)) continue;
            string[] parts = line.Split(';');

            if(parts.Length != 5) continue;
            if(!decimal.TryParse(parts[2], out decimal amount)) continue;
            if(!DateTime.TryParse(parts[3], out DateTime date)) continue;
            expenses.Add(new Expense(parts[0], parts[1], amount, date, parts[4]));
        }
        return expenses;
    }
    
    static string Escape(string value){
        return value.Replace(";", ",");
    }
}