using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HEAP
{
    public class Heap<T>
    {
        private List<T> elem;
        private readonly IComparer<T> comparer;
        private int capacity;

        public Heap(IComparer<T> comparer = null) : this(0, comparer) { }
        public Heap(int initialCapacity, IComparer<T> comparer = null)
        {
            this.capacity = initialCapacity > 0 ? initialCapacity : 0;
            this.elem = capacity > 0 ? new List<T>(capacity) : new List<T>();
            this.comparer = comparer ?? Comparer<T>.Default;
        }
        public Heap(T[] array, IComparer<T> comparer = null)
        {
            this.comparer = comparer ?? Comparer<T>.Default;
            this.elem = new List<T>(array);
            this.capacity = elem.Capacity;
            BuildHeap();
        }

        public int CountElem => elem.Count;
        public bool IsEmpty => elem.Count == 0;
        private void Swap(int i, int j)
        {
            T temp = elem[i];
            elem[i] = elem[j];
            elem[j] = temp;
        }

        private int ParentIndex(int index) => (index - 1) / 2;
        private int LeftChildIndex(int index) => 2 * index + 1;
        private int RightChildIndex(int index) => 2 * index + 2;

        private bool HasParent(int index) => index > 0;
        private bool HasLeftChild(int index) => LeftChildIndex(index) < elem.Count;
        private bool HasRightChild(int index) => RightChildIndex(index) < elem.Count;


        private void HeapifyDown(int index)
        {
            int current = index;

            while (HasLeftChild(current))
            {
                int largestChildIndex = LeftChildIndex(current);

                if (HasRightChild(current))
                {
                    int rightIndex = RightChildIndex(current);

                    if (comparer.Compare(elem[rightIndex], elem[largestChildIndex]) > 0)
                    {
                        largestChildIndex = rightIndex;
                    }
                }
                if (comparer.Compare(elem[current], elem[largestChildIndex]) >= 0)
                {
                    break;
                }

                Swap(current, largestChildIndex);
                current = largestChildIndex;
            }
        }

        private void HeapifyUp(int index)
        {
            int current = index;

            while (HasParent(current))
            {
                int parentIdx = ParentIndex(current);

                if (comparer.Compare(elem[current], elem[parentIdx]) <= 0)
                {
                    break;
                }

                Swap(current, parentIdx);
                current = parentIdx;
            }
        }

        private void BuildHeap()
        {
            for (int i = elem.Count / 2 - 1; i >= 0; i--)
            {
                HeapifyDown(i);
            }
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Ошибка: Куча пуста");

            return elem[0];
        }

        public T ExtractTop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Ошибка: Куча пуста");

            T max = elem[0];
            elem[0] = elem[elem.Count - 1];
            elem.RemoveAt(elem.Count - 1);

            if (!IsEmpty)
                HeapifyDown(0);
            return max;
        }

        public void ChangeKey(int index, T newValue)
        {
            if (index < 0 || index >= elem.Count)
                throw new ArgumentOutOfRangeException("Ошибка: Неверный индекс");

            if (comparer.Compare(newValue, elem[index]) < 0)
                throw new ArgumentException("Ошибка: Значение меньше текущего");

            elem[index] = newValue;
            HeapifyUp(index);
        }

        public void Insert(T item)
        {
            elem.Add(item);
            HeapifyUp(elem.Count - 1);
        }

        public Heap<T> Merge(Heap<T> otherHeap)
        {
            if (otherHeap == null)
                throw new ArgumentNullException("Ошибка: Другая куча пуста");

            List<T> allElements = new List<T>(elem);
            allElements.AddRange(otherHeap.elem);

            Heap<T> resultHeap = new Heap<T>(this.comparer);
            resultHeap.elem = allElements;
            resultHeap.capacity = Math.Max(this.capacity, otherHeap.capacity);
            resultHeap.BuildHeap();

            return resultHeap;
        }

        public void Print()
        {
            for (int i = 0; i < elem.Count; i++)
                Console.Write(elem[i] + " ");

            Console.WriteLine();
        }

        public T GetElement(int index)
        {
            if (index < 0 || index >= elem.Count)
                throw new ArgumentOutOfRangeException("Ошибка: Неверный индекс");

            return elem[index];
        }
        //-----------------------------------------------------------
        public bool Contains(T item)
        {
            return elem.Contains(item);
        }

        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= CountElem)
                return false;

            ReplaceAndReheap(index);
            return true;
        }

        public void ReplaceAndReheap(int index)
        {
            if (index < 0 || index >= CountElem)
                throw new ArgumentOutOfRangeException(nameof(index));

            int lastIndex = CountElem - 1;
            elem[index] = elem[lastIndex];
            elem.RemoveAt(lastIndex);

            if (index < CountElem)
            {
                if (index > 0 && comparer.Compare(elem[index], elem[ParentIndex(index)]) > 0)
                    HeapifyUp(index);
                else
                    HeapifyDown(index);
            }
        }

        public void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > capacity)
            {
                capacity = minCapacity;
                if (elem.Capacity < minCapacity)
                    elem.Capacity = minCapacity;
            }
        }

        public T[] ToArray()
        {
            return elem.ToArray();
        }

    }
}

