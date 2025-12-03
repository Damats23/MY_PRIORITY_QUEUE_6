using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_PRIORITY_QUEUE_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("                                             ПРОВЕРКА РЕАЛИЗАЦИИ ОЧЕРЕДИ С ПРИОРИТЕТОМ");

            Console.WriteLine("1. Создаем очередь и добавляем элементы:");
            MyPriorityQueue<int> queue = new MyPriorityQueue<int>();
            queue.Add(5);
            queue.Add(10);
            queue.Add(3);
            queue.Add(7);
            queue.Add(15);
            queue.PrintQueue();

            Console.WriteLine("\n2. Максимальный элемент:");
            Console.WriteLine("Результат: " + queue.Peek());

            Console.WriteLine("\n3. Извлечение максимального элемента:");
            Console.WriteLine("Извлечен: " + queue.Poll());
            queue.PrintQueue();

            Console.WriteLine("\n4. Проверка присутствия элемента:");
            Console.WriteLine("Содержит 10? " + queue.Contains(10));
            Console.WriteLine("Содержит 666? " + queue.Contains(666));


            Console.WriteLine("\n5. Максимальный элемент (с исключением если пусто):");
            Console.WriteLine("Результат: " + queue.Element());

            Console.WriteLine("\n6. Извлекаем все элементы:");
            while (!queue.IsEmpty())
            {
                Console.WriteLine("Извлечен: " + queue.Poll());
            }

            Console.WriteLine("\n7. Проверяем пустую очередь:");
            Console.WriteLine("Очередь пуста? " + queue.IsEmpty());
            Console.WriteLine("Максимум пустой очереди: " + queue.Peek());

            Console.WriteLine("\n8. Создаем очередь из массива [20, 5, 30, 8]:");
            int[] array = { 20, 5, 30, 8 };
            MyPriorityQueue<int> queue2 = new MyPriorityQueue<int>(array);
            queue2.PrintQueue();

            Console.ReadLine();
            Console.WriteLine("\nНажмите любую клавишу, чтобы закрыть");
        }
    }
}
