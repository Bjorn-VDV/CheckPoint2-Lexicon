using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace CheckPoint2
{
    class AllCategories
    {
        public List<string> Options { get; set; }

        List<string> options = new List<string>{ "What Category does your product belong to?",
            "Clothes",
            "Electronics"};

        public AllCategories()
        {
            options.Add("Show all lists ");
            Options = options;
        }
        public AllCategories(string added)
        {
            options.Add(added);
            Options = options;
        }
    }

    class Clothes
    {
        public Clothes(string productName, int price)
        {
            ProductName = productName;
            Price = price;
        }

        public string ProductName { get; set; }
        public int Price { get; set; }
    }

    class Electronics
    {
        public Electronics(string productName, int price)
        {
            ProductName = productName;
            Price = price;
        }

        public string ProductName { get; set; }
        public int Price { get; set; }
    }

    class Merged
    {
        public Merged(string category, string productName, int price)
        {
            Category = category;
            ProductName = productName;
            Price = price;
        }

        public string Category { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
    }

    internal class Checkpoint2_Björn
    {
        static int Menu()
        {
        Retry:
            // Instantiating variables
            int i = 0;
            AllCategories allCategories = new AllCategories();
            List<string> options = allCategories.Options;
            ConsoleKeyInfo choice;

            Console.WriteLine("You will be requested to choose a category, then add the name of your product, and finally the price thereof.\n");

            // Printing options + counter for numbers!
            foreach (string s in options)
            {
                if (i == 0) Console.WriteLine(s);
                else if (options[i] != null) Console.WriteLine($"{i}) {s}");
                i++;
            }
            Console.WriteLine(" -- Press Q to exit --");

            // Choice return based on input. Q is quit. If not a number, retry
            choice = Console.ReadKey(true);
            if (Char.IsDigit(choice.KeyChar) && choice.KeyChar != 48 && choice.KeyChar < options.Count + 48)
            {
                return Convert.ToInt32(choice.KeyChar - 48);
            }
            else if (choice.Key == ConsoleKey.Q) { Environment.Exit(0); return 0; }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White; Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("This was not a valid option.");
                Console.ResetColor();
                goto Retry;
            }
        }

        static int SubMenu()
        {
        // Menu to return a choice back to the Main method
        NumbersAreHard:
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Would you like the list to be SPLIT (1), or MERGED (2)? Write \"1 OR 2\" below\n");
            Console.ResetColor();
            ConsoleKeyInfo number = Console.ReadKey(true);
            try { Convert.ToInt32(number.KeyChar); }
            catch
            {
                InvalidOption("\nPlease input a valid number.");
                goto NumbersAreHard;
            }
            int i = (Convert.ToInt32((number.KeyChar) - 48));
            return i;
        }

        static bool ReturnOrNot()
        {
            Console.Write("\n──────────────────────────────────────────────────────");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nPress enter if you want to go back to the menu and add more items, otherwise quit the program.");
            Console.ResetColor();

            // readkey. Enter = return true, otherwise close program
            ConsoleKeyInfo choice = Console.ReadKey();
            Console.Clear();
            if (choice.Key == ConsoleKey.Enter) return true; else return false;
        }

        static (bool, string) FindItem()
        {
            // Prompt yes or no question, return bool true or false
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.WriteLine("Do you want to search through your list? Y/N");
            Console.ResetColor();
            ConsoleKeyInfo input = Console.ReadKey(true);

            // This code could be a lot shorter, but I decided to write it out to make it easier to read
            bool answer = (input.Key == ConsoleKey.Y) ? true : false;
            string searchFor = "empty lmao";

            if (answer)
            {
                Console.Write("What would you like to search for within the list? Please write: ");
                searchFor = Console.ReadLine();
            }
            return (answer, searchFor);
        }

        static void InputList(int listChoice, List<Clothes> clothes, List<Electronics> electronics)
        {
            // Preparation
            string addingName = string.Empty;
            if (listChoice == 1) addingName = "clothes";
            else addingName = "electronics";

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Press enter to add a new {addingName}, or 'Q' to exit.\n");
                Console.ResetColor();

                // Input. If Q, break. Otherwise, continue writing!
                ConsoleKeyInfo quitOrNot = Console.ReadKey(true);
                if (quitOrNot.Key != ConsoleKey.Q)
                {
                    // ask for name + input name
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"\nYou are adding new {addingName}.");
                    Console.ResetColor();
                    Console.Write($"Please input your productname: ");
                    string name = Console.ReadLine();

                // ask for price + input price. Unless price != number, then ask again + error!
                NumbersAreHard:
                    Console.Write($"Please add a price to {name}: ");
                    string priceString = Console.ReadLine();
                    try { Convert.ToInt32(priceString); }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Prices can only be numbers. Try again.");
                        Console.ResetColor();
                        goto NumbersAreHard;
                    }
                    int price = Convert.ToInt32(priceString);

                    // Add result, prompt success, repeat
                    if (listChoice == 1) clothes.Add(new Clothes(name, price));
                    else electronics.Add(new Electronics(name, price));
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"You have successfully added {name} to {addingName}.");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }
            while (true);

            // Sort all the lists
            List<Electronics> priceSortEl = electronics.OrderBy(x => x.Price).ToList();
            List<Clothes> priceSortCl = clothes.OrderBy(x => x.Price).ToList();

            // Print the lists depending on initial choice
            if (listChoice == 1)
            {
                foreach (Clothes c in priceSortCl)
                {
                    Console.WriteLine((c.ProductName).PadRight(15) + c.Price);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Sum of prices".PadRight(15) + clothes.Sum(clothes => clothes.Price).ToString());
                Console.ResetColor();
            }
            else
            {
                foreach (Electronics e in priceSortEl)
                {
                    Console.WriteLine(e.ProductName.PadRight(15) + e.Price);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Sum of prices".PadRight(15) + electronics.Sum(electronics => electronics.Price).ToString());
                Console.ResetColor();
            }
        }

        static void ShowAllTog(List<Clothes> clothes, List<Electronics> electronics)
        {
            // Merge clothes and electronics to one list
            List<Merged> merged = new List<Merged>();
            foreach (Electronics e in electronics) { merged.Add(new Merged("Electronics", e.ProductName, e.Price)); }
            foreach (Clothes c in clothes) { merged.Add(new Merged("Clothes", c.ProductName, c.Price)); }

            // Sort list and set variables needed find function
            List<Merged> mergeSort = merged.OrderBy(x => x.Price).ToList();
            bool yesNo = true;
            string searchWhat = string.Empty;
            bool seenThis = false;

            // Do loop to check if person wants to search or not. If yes, repeat, otherwise break
            do
            {
                if (yesNo)
                {
                    // Clear console. Search for index of searched item. This starts as an empty string for the first (clean) print
                    Console.Clear();
                    int highlight = mergeSort.FindIndex(x => x.ProductName.ToLower().Contains(searchWhat.ToLower()));
                    int counter = 0;

                    // Counter goes up by one each time. When it matches with index of searched item, highlight it
                    // seenThis is used for a first print. Sets to true after first printing
                    foreach (Merged m in mergeSort)
                    {
                        if (counter == highlight & seenThis)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine((m.Category).PadRight(15) + (m.ProductName).PadRight(15) + m.Price);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine((m.Category).PadRight(15) + (m.ProductName).PadRight(15) + m.Price);
                        }
                        counter++;
                    }

                    // Finally printing the sum of all items
                    int summedMerge = mergeSort.Sum(x => x.Price);
                    Console.WriteLine("───────────────────────────────────");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("SUMMED".PadRight(30) + summedMerge);
                    Console.ResetColor();

                    // This checks whether the item you searched for exists or not. Won't run on first print
                    if (highlight == -1 && seenThis)
                    {
                        InvalidOption("There was no result for your querry");
                    }

                    // Now we ask if you want to find an item or not, and then rerun the entire code. It will break immediately of yesNo = false
                    (yesNo, searchWhat) = FindItem();
                    seenThis = true;
                }

                // Person doesn't want to search. Therefore break loop
                else break;
            }
            while (true);
        }

        static void ShowAllSplit(List<Clothes> clothes, List<Electronics> electronics)
        {
            // Retrieve list, Sort list, Sum list
            List<Electronics> priceSortEl = electronics.OrderBy(x => x.Price).ToList();
            List<Clothes> priceSortCl = clothes.OrderBy(x => x.Price).ToList();
            int elPrices = priceSortEl.Sum(electronics => electronics.Price);
            int clPrices = priceSortCl.Sum(clothes => clothes.Price);

            // Print electronics
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Electronics\n──────────────────────────────────────────────────────");
            Console.ResetColor();
            foreach (Electronics e in priceSortEl)
            {
                Console.WriteLine((e.ProductName).PadRight(15) + e.Price);
            }

            // Print Clothes
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nClothes\n──────────────────────────────────────────────────────");
            Console.ResetColor();
            foreach (Clothes c in priceSortCl)
            {
                Console.WriteLine((c.ProductName).PadRight(15) + c.Price);
            }

            // Print prices added together
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nSum of all Prices\n──────────────────────────────────────────────────────\n" +
                "".PadRight(15) + (elPrices + clPrices));
            Console.ResetColor();
        }

        static void InvalidOption(string s)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            // Instantiating lists
            List<Clothes> clothes = new List<Clothes>();
            List<Electronics> electronics = new List<Electronics>();

            // Setting a few example items
            clothes.AddRange(new List<Clothes>
            {
                new Clothes("Shirt",1500),
                new Clothes("Vest",2000),
                new Clothes("Pants",1800)
            });
            electronics.AddRange(new List<Electronics>
            {
                new Electronics("Mobile",1800),
                new Electronics("HD-600",2990),
                new Electronics("Mouse",1650)
            });


            // Start at clean menu. Open menu and take back choice
            Console.Clear();
        BackToMenu:
            int i = Menu();

            // Go to inputlist if 1 or 2, Show list if 3, otherwise retry
            if (i == 1 || i == 2)
            {
                InputList(i, clothes, electronics);
            }

            // Submenu to determine whether to show the list merged, or split
            else if (i == 3)
            {
                i = SubMenu();
                if (i == 2) ShowAllTog(clothes, electronics);
                else if (i == 1) ShowAllSplit(clothes, electronics);
                else
                {
                    Console.Clear();
                    InvalidOption("This was not a valid choice.");
                    goto BackToMenu;
                }
            }

            // Invalid option, return to menu
            else
            {
                Console.Clear();
                InvalidOption("This was not a valid choice.");
                goto BackToMenu;
            }

            // Whether to return to main menu, or quit the app
            bool yesOrNo = ReturnOrNot();
            if (yesOrNo) goto BackToMenu;
            else Environment.Exit(0);
        }
    }
}
