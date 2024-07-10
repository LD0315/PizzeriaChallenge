

namespace LOR.Interview.PizzeriaChallenge.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Welcome to LOR Pizzeria! Please select the store location: Brisbane OR Sydney OR Gold Coast");
            var storeLocation = System.Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(storeLocation))
            {
                System.Console.WriteLine("No store location provided. Please try again...");
                return;
            }
            
            var store = StoreFactory.GetStore(storeLocation);
            if (store == null)
            {
                System.Console.WriteLine("Invalid store location. Please try again...");
                return;
            }

            store.DisplayMenu();

            System.Console.WriteLine("How many pizzas would you like to order?");
            if (!int.TryParse(System.Console.ReadLine(), out int pizzaCount) || pizzaCount < 1)
            {
                System.Console.WriteLine("Invalid number of pizzas. Please try again...");
                return;
            }

            var order = new Order();

            for (int i = 0; i < pizzaCount; i++)
            {
                System.Console.WriteLine("What can I get you?");
                var pizzaType = System.Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(pizzaType))
                {
                    System.Console.WriteLine("No pizza type provided.");
                    continue;
                }

                var pizza = store.OrderPizza(pizzaType);

                if (pizza != null)
                {
                    order.AddPizza(pizza);
                }
                else
                {
                    System.Console.WriteLine($"Sorry, we don't have {pizzaType} pizza.");
                }
            }

            if (order.HasPizzas())
            {
                order.PreparePizzas();
                System.Console.WriteLine("\nYour order is ready!");
                order.PrintReceipt();
            }
            else
            {
                System.Console.WriteLine("\nNo pizzas ordered.");
            }
        }
    }

    public static class StoreFactory
    {
        public static Store? GetStore(string location)
        {
            return location.ToLower() switch
            {
                "brisbane" => new BrisbaneStore(),
                "sydney" => new SydneyStore(),
                "gold coast" => new GoldCoastStore(),
                _ => null,
            };
        }
    }
    public abstract class Store
    {
        protected List<Pizza> Menu = new List<Pizza>();

        public void DisplayMenu()
        {
            System.Console.WriteLine("MENU");
            foreach (var pizza in Menu)
            {
                System.Console.WriteLine($"{pizza.Name} - {string.Join(", ", pizza.Ingredients)} - {pizza.BasePrice} AUD");
            }
        }

        public Pizza? OrderPizza(string pizzaName)
        {
            return Menu.Find(p => p.Name.Equals(pizzaName, StringComparison.OrdinalIgnoreCase));
        }
    }

    public class BrisbaneStore : Store
    {
        public BrisbaneStore()
        {
            Menu.Add(new Pizza("Capriciosa", new List<string> { "mushrooms", "cheese", "ham", "mozzarella" }, 20));
            Menu.Add(new Pizza("Florenza", new List<string> { "olives", "pastrami", "mozzarella", "onion" }, 21));
            Menu.Add(new Pizza("Margherita", new List<string> { "mozzarella", "onion", "garlic", "oregano" }, 22));
        }
    }

    public class SydneyStore : Store
    {
        public SydneyStore()
        {
            Menu.Add(new Pizza("Capriciosa", new List<string> { "mushrooms", "cheese", "ham", "mozzarella" }, 30));
            Menu.Add(new Pizza("Inferno", new List<string> { "chili peppers", "mozzarella", "chicken", "cheese" }, 31));
        }
    }

    public class GoldCoastStore : Store
    {
        public GoldCoastStore()
        {
            Menu.Add(new Pizza("Capriciosa", new List<string> { "mushrooms", "cheese", "ham", "mozarella" }, 25));
            Menu.Add(new Pizza("Pepperoni", new List<string> { "salami", "mozzarella cheese", "crispy meat", "tomato" }, 27));
            Menu.Add(new Pizza("Super Supreme", new List<string> { "smoked chorizo", "onion", "capsicum", "mozzarella" }, 28));
        }
    }

    public class Pizza
    {
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public decimal BasePrice { get; set; }

        public Pizza(string name, List<string> ingredients, decimal basePrice)
        {
            Name = name;
            Ingredients = ingredients;
            BasePrice = basePrice;
        }

        public void Prepare()
        {
            System.Console.WriteLine(new string('-', 30));
            System.Console.WriteLine("Preparing " + Name + "...");
            System.Console.Write("Adding ");
            foreach (var ingredient in Ingredients)
            {
                System.Console.Write(ingredient + " ");
            }
            System.Console.WriteLine();
        }

        public void Bake()
        {
            System.Console.WriteLine("Baking pizza for " + (Name == "Margherita" ? 15 : 30) + " minutes at 200 degrees...");
        }

        public void Cut()
        {
            System.Console.WriteLine("Cutting pizza into " + (Name == "Florenza" ? 6 : 8) + " slices...");
        }

        public void Box()
        {
            System.Console.WriteLine("Putting pizza into a nice box...");
        }
    }

    public class Order
    {
        private List<Pizza> Pizzas { get; set; } = new List<Pizza>();
        private decimal TotalPrice { get; set; } = 0;

        public void AddPizza(Pizza pizza)
        {
            Pizzas.Add(pizza);
            TotalPrice += pizza.BasePrice;
        }

        public bool HasPizzas()
        {
            return Pizzas.Count > 0;
        }

        public void PreparePizzas()
        {
            foreach (var pizza in Pizzas)
            {
                pizza.Prepare();
                pizza.Bake();
                pizza.Cut();
                pizza.Box();
            }
        }

        public void PrintReceipt()
        {
            System.Console.WriteLine("Total price: " + TotalPrice + " AUD");
        }
    }
}
