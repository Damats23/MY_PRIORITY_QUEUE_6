using HEAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MY_PRIORITY_QUEUE_6
{
    public class MyPriorityQueue<T>
    {
        private Heap<T> queue;
        private int size;
        private int capacity;
        private IComparer<T> comparer;   

        private const int DEFAULT_CAPACITY = 11;

        // 1. 
        public MyPriorityQueue()
        {
            queue = new Heap<T>(comparer);
            size = 0;
            capacity = DEFAULT_CAPACITY;
            comparer = Comparer<T>.Default;
        }

        // 2.
        public MyPriorityQueue(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array), "Ошибка: Массив не может быть null");
            
            queue = new Heap<T>(array, comparer);
            size = array.Length;
            capacity = Math.Max(array.Length, DEFAULT_CAPACITY);
            comparer = Comparer<T>.Default;
        }

        // 3.
        public MyPriorityQueue(int initialCapacity)
        {
            if (initialCapacity < 1)
                throw new ArgumentException("Ошибка: Ёмкость должна быть больше нуля.");

            queue = new Heap<T>(comparer);
            size = 0;
            capacity = initialCapacity;
            comparer = Comparer<T>.Default;
        }

        // 4.
        public MyPriorityQueue(int initialCapacity, IComparer<T> comparator)
        {
            if (initialCapacity < 1)
                throw new ArgumentException("Ошибка: Ёмкость должна быть больше нуля.");

            comparer = comparator ?? Comparer<T>.Default;
            queue = new Heap<T>(comparer);
            capacity = initialCapacity;
            size = 0;

        }

        // 5.
        public MyPriorityQueue(MyPriorityQueue<T> c)
        {
            if (c == null)
                throw new ArgumentNullException("Ошибка: Очередь пуста.");

            comparer = c.comparer;
            capacity = c.capacity;
            size = c.size;

            T[] elements = new T[size];
            for (int i = 0; i < size; i++)
            {
                elements[i] = c.queue.GetElement(i);
            }
            queue = new Heap<T>(elements, comparer);
            
        }

        // 6.
        public void Add(T e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e), "Ошибка: Элемент не может быть null");

            EnsureCapacity();
            queue.Insert(e);
            size++;
        }

        // 7.
        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Ошибка: Массив не может быть null");

            EnsureCapacity(a.Length);
            foreach (var item in a)
            {
                queue.Insert(item);
                size++;
            }
        }

        // 8.
        public void Clear()
        {
            queue = new Heap<T>(capacity, comparer);
            size = 0;
        }

        // 9.
        public bool Contains(object o)
        {
            if (o is T value)
            {
                for (int i = 0; i < size; i++)
                {
                    if (comparer.Compare(queue.GetElement(i), value) == 0)
                        return true;
                }
            }
            return false;
        }

        // 10.
        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Ошибка: Массив не может быть null");

            foreach (var item in a)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        // 11.
        public bool IsEmpty()
        {
            return size == 0;
        }

        // 12.
        public bool Remove(object o)
        {
            if (!(o is T value))
                return false;

            for (int i = 0; i < size; i++)
            {
                if (comparer.Compare(queue.GetElement(i), value) == 0)
                {
                    queue.ReplaceAndReheap(i);
                    size--;
                    return true;
                }
            }
            return false;
        }

        // 13.
        public bool RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Ошибка: Массив не может быть пустым" );

            bool modified = false;
            foreach (var item in a)
            {
                if (Remove(item))
                    modified = true;
            }
            return modified;
        }

        // 14.
        public bool RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a), "Ошибка: Массив не может быть пустым");

            HashSet<T> allowed = new HashSet<T>(a);
            List<T> retained = new List<T>();

            for (int i = 0; i < size; i++)
            {
                T element = queue.GetElement(i);
                if (allowed.Contains(element))
                    retained.Add(element);
            }

            if (retained.Count == size)
                return false;

            queue = new Heap<T>(retained.ToArray(), comparer);
            size = retained.Count;

            if (size < capacity / 2 && capacity > DEFAULT_CAPACITY)
                capacity = Math.Max(size, DEFAULT_CAPACITY);

            return true;
        }

        // 15.
        public int Size()
        {
            return size;
        }

        // 16. 
        public object[] ToArray()
        {
            object[] arr = new object[size];
            for (int i = 0; i < size; i++)
                arr[i] = queue.GetElement(i);
            return arr;
        }

        // 17.
        public T[] ToArray(T[] a)
        {
            if (a == null || a.Length < size)
                a = new T[size];

            for (int i = 0; i < size; i++)
                a[i] = queue.GetElement(i);

            return a;
        }

        // 18.
        public T Element()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Ошибка: Очередь пуста.");

            return queue.Peek();
        }

        // 19.
        public bool Offer(T obj)
        {
            try
            {
                Add(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // 20.
        public T Peek()
        {
            return IsEmpty() ? default(T) : queue.Peek();
        }

        // 21.
        public T Poll()
        {
            if (IsEmpty())
                return default(T);

            size--;
            return queue.ExtractTop();
        }

        private void EnsureCapacity(int additional = 1)
        {
            int required = size + additional;
            if (required > capacity)
            {
                int newCapacity;
                if (capacity < 64)
                    newCapacity = capacity + 2;
                else
                    newCapacity = capacity + capacity / 2;

                if (newCapacity < required)
                    newCapacity = required;

                capacity = newCapacity;
                queue.EnsureCapacity(capacity);
            }
        }

        public int GetCapacity()
        {
            return capacity;
        }

        public void PrintQueue()
        {
            Console.Write("Очередь с приоритетами (размер: {0}, емкость: {1}): ", size, capacity);
            queue.Print();
        }

        public void TrimToSize()
        {
            capacity = size > 0 ? size : DEFAULT_CAPACITY;
            queue.EnsureCapacity(capacity);
        }
    }
}
