using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Game game = new Game();
        game.Start();
    }
}

class Room
{
    public string Name { get; }
    public string Description { get; }
    public Dictionary<string, Room> Exits { get; } = new Dictionary<string, Room>();
    public List<Item> Items { get; } = new List<Item>();

    public Room(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Describe()
    {
        Console.WriteLine($"{Name}: {Description}");
        if (Items.Count > 0)
        {
            Console.WriteLine("You see:");
            foreach (var item in Items)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
        if (Exits.Count > 0)
        {
            Console.WriteLine("Exits:");
            foreach (var direction in Exits.Keys)
            {
                Console.WriteLine($"- {direction}");
            }
        }
    }
}

class Item
{
    public string Name { get; }
    public string Description { get; }

    public Item(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void Inspect()
    {
        Console.WriteLine(Description);
    }
}

class Game
{
    private Room currentRoom;
    private Dictionary<string, Room> rooms = new Dictionary<string, Room>();
    private List<Item> inventory = new List<Item>();
    private const int InventoryLimit = 10;

    public Game()
    {
        Room foyer = new Room("Foyer", "A dimly lit entrance hall with a dusty chandelier.");
        Room library = new Room("Library", "Shelves filled with ancient books cover the walls.");
        Room diningHall = new Room("Dining Hall", "A long table sits beneath a cracked stained-glass window.");

        // Foyer Items
        foyer.Items.AddRange(new List<Item>
        {
            new Item("Umbrella", "A black umbrella, slightly damp."),
            new Item("Coat Rack", "An ornate wooden rack with a single coat hanging."),
            new Item("Welcome Mat", "A mat that says 'WELCOME' in faded letters."),
            new Item("Dusty Mirror", "A tall mirror covered in a layer of dust."),
            new Item("Old Key", "A small brass key with intricate carvings."),
            new Item("Candle", "A candle that has almost melted down to the base."),
            new Item("Letter", "A sealed letter addressed to no one in particular."),
            new Item("Hat", "A stylish fedora that has seen better days."),
            new Item("Boots", "A pair of muddy boots."),
            new Item("Painting", "A portrait of a stern-looking gentleman."),
            new Item("Vase", "A cracked vase with dried flowers.")
        });

        // Library Items
        library.Items.AddRange(new List<Item>
        {
            new Item("Ancient Book", "An old leather-bound book with strange symbols on its cover."),
            new Item("Magnifying Glass", "A brass-handled magnifying glass."),
            new Item("Reading Lamp", "A small green-shaded lamp flickering slightly."),
            new Item("Scroll", "A rolled-up parchment with illegible text."),
            new Item("Feather Pen", "A black feathered pen with a silver tip."),
            new Item("Notebook", "A notebook filled with cryptic notes."),
            new Item("Globe", "A dusty globe, slightly faded."),
            new Item("Telescope", "A short telescope aimed at the ceiling."),
            new Item("Bookshelf", "A massive bookshelf, some books seem loose."),
            new Item("Ink Bottle", "A half-full bottle of blue ink."),
            new Item("Rug", "A finely woven rug, frayed at the edges.")
        });

        // Dining Hall Items
        diningHall.Items.AddRange(new List<Item>
        {
            new Item("Silver Spoon", "A slightly tarnished silver spoon."),
            new Item("Tablecloth", "A white cloth with a red wine stain in the center."),
            new Item("Candle Holder", "A heavy iron candle holder with wax drippings."),
            new Item("Goblet", "A golden goblet with jewels around the rim.")
        });

        // Connect Rooms
        foyer.Exits["north"] = library;
        library.Exits["south"] = foyer;

        foyer.Exits["west"] = diningHall;
        diningHall.Exits["east"] = foyer;

        rooms["Foyer"] = foyer;
        rooms["Library"] = library;
        rooms["Dining Hall"] = diningHall;

        currentRoom = foyer;
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine("Welcome to the adventure game!");
        currentRoom.Describe();
        while (true)
        {
            Console.Write("\n> ");
            string input = Console.ReadLine().Trim().ToLower();
            Console.Clear();
            if (!ProcessCommand(input)) break;
        }
    }

    private bool ProcessCommand(string command)
    {
        if (command == "look")
        {
            currentRoom.Describe();
        }
        else if (command.StartsWith("inspect "))
        {
            string itemName = command.Substring(8);
            Item item = currentRoom.Items.Find(i => i.Name.ToLower() == itemName);
            if (item == null)
            {
                item = inventory.Find(i => i.Name.ToLower() == itemName);
            }
            if (item != null)
            {
                item.Inspect();
            }
            else
            {
                Console.WriteLine("There is no such item here or in your inventory.");
            }
        }
        else if (command.StartsWith("take "))
        {
            string itemName = command.Substring(5);
            TakeItem(itemName);
        }
        else if (command.StartsWith("drop "))
        {
            string itemName = command.Substring(5);
            DropItem(itemName);
        }
        else if (command == "inventory")
        {
            ShowInventory();
        }
        else if (command == "quit")
        {
            Console.Write("Are you sure you want to quit? (yes/no): ");
            string response = Console.ReadLine().Trim().ToLower();
            return response != "yes";
        }
        else if (IsDirection(command))
        {
            Move(command);
        }
        else
        {
            Console.WriteLine("Unknown command.");
        }
        return true;
    }

    private bool IsDirection(string command)
    {
        return command == "north" || command == "south" || command == "east" || command == "west" || command == "up" || command == "down";
    }

    private void Move(string direction)
    {
        if (currentRoom.Exits.ContainsKey(direction))
        {
            currentRoom = currentRoom.Exits[direction];
            Console.WriteLine($"You move {direction}.");
            currentRoom.Describe();
        }
        else
        {
            Console.WriteLine("You can't go that way.");
        }
    }

    private void TakeItem(string itemName)
    {
        if (inventory.Count >= InventoryLimit)
        {
            Console.WriteLine("Your inventory is full. You can't carry more items.");
            return;
        }

        Item item = currentRoom.Items.Find(i => i.Name.ToLower() == itemName);
        if (item != null)
        {
            currentRoom.Items.Remove(item);
            inventory.Add(item);
            Console.WriteLine($"You take the {item.Name}.");
        }
        else
        {
            Console.WriteLine("There is no such item here.");
        }
    }

    private void DropItem(string itemName)
    {
        Item item = inventory.Find(i => i.Name.ToLower() == itemName);
        if (item != null)
        {
            inventory.Remove(item);
            currentRoom.Items.Add(item);
            Console.WriteLine($"You drop the {item.Name}.");
        }
        else
        {
            Console.WriteLine("You don't have that item.");
        }
    }

    private void ShowInventory()
    {
        if (inventory.Count == 0)
        {
            Console.WriteLine("You are not carrying anything.");
        }
        else
        {
            Console.WriteLine("You are carrying:");
            foreach (var item in inventory)
            {
                Console.WriteLine($"- {item.Name}");
            }
        }
    }
}
