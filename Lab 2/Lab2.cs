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
            Console.WriteLine("The list must have at least 2 elements to calculate the expression.");
            return 0;
        }
        List<Node> nodes = new List<Node>();
        Node current = head;
        while (current != null)
        {
            nodes.Add(current);
            current = current.Next;
        }

        int count = nodes.Count;
        long product = 1;
        for (int i = 0; i < count - 1; i++)
        {
            long ak = nodes[i].Value;
            long akPlus1 = nodes[i + 1].Value;
            long aOpposite = nodes[count - 1 - i].Value;

            long termSum = ak + akPlus1 + (2 * aOpposite);
            product *= termSum;
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
            Console.WriteLine("Do you want to clear the list? (y/n)");
            string input = Console.ReadLine();
            if (input.ToLower() == "y")
            {
                list.Clear();
                list.Print();
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }
}



        
        
       

    
    
    
       


    
    






    
    


