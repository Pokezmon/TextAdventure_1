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

    public Game()
    {
        Room room1 = new Room("Foyer", "A dimly lit entrance hall with a dusty chandelier.");
        Room room2 = new Room("Library", "Shelves filled with ancient books cover the walls.");
        Item book = new Item("Ancient Book", "An old leather-bound book with strange symbols on its cover.");
        room2.Items.Add(book);

        room1.Exits["north"] = room2;
        room2.Exits["south"] = room1;

        rooms["Foyer"] = room1;
        rooms["Library"] = room2;

        currentRoom = room1;
    }

    public void Start()
    {
        Console.WriteLine("Welcome to the adventure game!");
        currentRoom.Describe();
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine().Trim().ToLower();
            ProcessCommand(input);
        }
    }

    private void ProcessCommand(string command)
    {
        if (command == "look")
        {
            currentRoom.Describe();
        }
        else if (command.StartsWith("inspect "))
        {
            string itemName = command.Substring(8);
            Item item = currentRoom.Items.Find(i => i.Name.ToLower() == itemName);
            if (item != null)
            {
                item.Inspect();
            }
            else
            {
                Console.WriteLine("There is no such item here.");
            }
        }
        else if (IsDirection(command))
        {
            Move(command);
        }
        else
        {
            Console.WriteLine("Unknown command.");
        }
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
}
