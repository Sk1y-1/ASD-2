using System;
using System.Collections.Generic;
namespace Lab2;

public class Node
{
    public int Value { get; set; }
    public Node Next { get; set; }

    public Node (int value)
    {
        Value = value;
        Next = null;
        Console.WriteLine($"[CREATED]   Node({value}) | addr: {GetHashCode()}");
    }
    ~Node()
    {
        Console.WriteLine($"[DESTROYED] Node({Value}) | addr: {GetHashCode()}");
    }
}
public class LinkList {
        private Node head;
        public Node Head => head;

    public void Create(int n)
    {
        if (n <= 0) 
        return;
    
    head = null;
    Node tail = null;
    Console.WriteLine($"Enter {n} element of the list:");
    for (int i = 0; i < n; i++)
    {
     Console.WriteLine($"Value [{i+1}]:");
     if (int.TryParse(Console.ReadLine(), out int value))
     {
     
                    
    
        Node newNode = new Node(value);
        if (head == null)
                {
                    head = newNode;
                    tail = head;
                }
                else
                {
                    tail.Next = newNode;
                    tail = newNode;
                }
    }
    
    else
                {
                    Console.WriteLine("Invalid input. Please enter an integer.");
                    i--;
                }
            }
        }
        public void AddLast(int value)
        {
            Node newNode = new Node(value);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
        }

        public void Remove(int value)
        {
            if (head == null) return;

            if (head.Value == value)
            {
                head = head.Next;
                return;
            }
            Node prev = head;
            Node current = head.Next;
            while (current != null)
            {
                if (current.Value == value)
                {
                    prev.Next = current.Next;
                    return;
                }
                prev = current;
                current = current.Next;
            }
        }

        public void Print()
        {
            Console.WriteLine("Elements of the list:");
            Node current = head;
            while (current != null)
            {
                Console.WriteLine($"{current.Value}");
                current = current.Next;
            }
            Console.WriteLine("null");
        }
        public void Clear()
        {
            head = null;
            Console.WriteLine("List cleared.");
        }
public long CalculateExpression()
{
    if (head == null || head.Next == null)
    {
        Console.WriteLine("The list must have at least 2 elements.");
        return 0;
    }

    List<long> values = new List<long>();
    Node current = head;
    while (current != null)
    {
        values.Add(current.Value);
        current = current.Next;
    }

    int n = values.Count; 
    long product = 1;
    for (int i = 0; i < n - 1; i++)
    {
        long ak       = values[i];         
        long akNext   = values[i + 1];     
        long akOpp    = values[n - 1 - i]; 
        long term = ak + akNext + 2 * akOpp;
        product *= term;
    }

    return product;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        LinkList list = new LinkList();
        Console.WriteLine("Enter the number of elements in the list:");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            list.Create(n);
            list.Print();
            if (n >= 2)
                {
                    long result = list.CalculateExpression();
                    Console.WriteLine($"The result of the expression is: {result}");
                }
                Console.WriteLine("Enter value to add:");
                if (int.TryParse(Console.ReadLine(), out int addVal))
                {
                    list.AddLast(addVal);
                    list.Print();
}

                    Console.WriteLine("Enter value to remove:");
                    if (int.TryParse(Console.ReadLine(), out int removeVal))
                {
                    list.Remove(removeVal);
                    list.Print();
}
            Console.WriteLine("Do you want to clear the list? (y/n)");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                list.Clear();
                GC.Collect();
                GC.WaitForPendingFinalizers();
                list.Print();
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
}



        
    


    
    






    
    


