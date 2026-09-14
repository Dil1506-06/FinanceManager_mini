using ExpenseManager;

class Program{
    static readonly string DataFile = "expenses.csv";
    static void Main(){
        List<Expense> expenses = FileManager.Load(DataFile);

        while(true){
            Console.Clear();
            Console.WriteLine("=== МЕНЕДЖЕР РАСХОДОВ ===");
            Console.WriteLine("1. Добавить расход");
            Console.WriteLine("2. Показать все расходы");
            Console.WriteLine("3. Показать общую сумму");
            Console.WriteLine("4. Расходы по категориям");
            Console.WriteLine("5. Удалить расход");
            Console.WriteLine("0. Выход");
            Console.Write("\nВыберите действие: ");

            switch (Console.ReadLine()){
                case "1": AddExpense(expenses); break;
                case "2": ShowExpenses(expenses); break;
                case "3": ShowTotal(expenses); break;
                case "4": ShowByCategories(expenses); break;
                case "5": DeleteExpense(expenses); break;
                case "0": FileManager.Save(DataFile, expenses); return;
                default: Pause("Неверный пункт меню."); break;
            }
        }
    }

    static void AddExpense(List<Expense> expenses){
        Console.Clear();
        Console.WriteLine("=== ДОБАВЛЕНИЕ РАСХОДА ===");

        Console.Write("Название: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Категория: ");
        string category = Console.ReadLine() ?? "";

        Console.Write("Сумма: ");
        if(!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0){
            Pause("Некорректная сумма.");
            return;
        }
        Console.Write("Дата (дд.мм.гггг) или Enter для сегодняшней: ");
        string dateInput = Console.ReadLine() ?? "";

        DateTime date;
        if(string.IsNullOrWhiteSpace(dateInput)){
            date = DateTime.Now;
        }
        else if(!DateTime.TryParse(dateInput, out date)){
            Pause("Некорректная дата.");
            return;
        }
        Console.Write("Описание: ");
        string description = Console.ReadLine() ?? "";

        expenses.Add(new Expense(name, category, amount, date, description));
        FileManager.Save(DataFile, expenses);

        Pause("Расход успешно добавлен.");
    }

    static void ShowExpenses(List<Expense> expenses){
        Console.Clear();
        Console.WriteLine("=== ВСЕ РАСХОДЫ ===");

        if(expenses.Count == 0){
            Pause("Расходов пока нет.");
            return;
        }
        for (int i = 0; i < expenses.Count; i++){
            Expense e = expenses[i];

            Console.WriteLine($"{i + 1}. {e.Name} | {e.Category} | " + $"{e.Amount:F2} | {e.Date:dd.MM.yyyy} | {e.Description}");
        }
        Pause();
    }

    static void ShowTotal(List<Expense> expenses){
        Console.Clear();

        decimal total = expenses.Sum(e => e.Amount);

        Console.WriteLine("=== ОБЩАЯ СУММА ===");
        Console.WriteLine($"Всего расходов: {total:F2}");

        Pause();
    }

    static void ShowByCategories(List<Expense> expenses){
        Console.Clear();
        Console.WriteLine("=== РАСХОДЫ ПО КАТЕГОРИЯМ ===");
        if(expenses.Count == 0){
            Pause("Расходов пока нет.");
            return;
        }
        var groups = expenses.GroupBy(e => e.Category).OrderByDescending(g => g.Sum(e => e.Amount));

        foreach (var group in groups){
            Console.WriteLine($"{group.Key}: {group.Sum(e => e.Amount):F2}");
        }
        Pause();
    }
    static void DeleteExpense(List<Expense> expenses){
        Console.Clear();
        Console.WriteLine("=== УДАЛЕНИЕ РАСХОДА ===");
        if(expenses.Count == 0){
            Pause("Расходов нет.");
            return;
        }
        for(int i = 0; i < expenses.Count; i++){
            Console.WriteLine($"{i + 1}. {expenses[i].Name} — {expenses[i].Amount:F2}");
        }
        Console.Write("\nВведите номер расхода: ");
        if(!int.TryParse(Console.ReadLine(), out int number) || number < 1 || number > expenses.Count){
            Pause("Некорректный номер.");
            return;
        }
        expenses.RemoveAt(number - 1);
        FileManager.Save(DataFile, expenses);

        Pause("Расход удалён.");
    }
    
    static void Pause(string message = "Нажмите Enter для продолжения."){
        Console.WriteLine($"\n{message}");
        Console.ReadLine();
    }


}