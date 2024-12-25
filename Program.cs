using System;
using System.Collections.Generic;

// Вектор, расширяемый массив, лежащий в основе стека
public class MyVector<T>
{
    private T[] elementData;  // Массив для хранения элементов
    private int elementCount; // Количество элементов в массиве
    private int capacityIncrement; // Размер приращения емкости массива

    public MyVector(int capacityIncrement = 0)
    {
        elementData = new T[16]; // Начальная емкость массива
        elementCount = 0; 
        this.capacityIncrement = capacityIncrement;
    }

    // Добавление элемента в массив
    public void Add(T e)
    {
        if (elementCount == elementData.Length) // Если массив заполнен
        {
            // Увеличиваем размер массива
            int newCapacity = capacityIncrement == 0 
                ? elementCount * 2 + 1 
                : elementCount + capacityIncrement + 1;

            T[] array = new T[newCapacity];
            for (int i = 0; i < elementCount; i++) array[i] = elementData[i];
            elementData = array;
        }
        elementData[elementCount++] = e; // Добавляем элемент
    }

    // Удаление элемента по индексу
    public void Remove(int index)
    {
        if (index >= 0 && index < elementCount)
        {
            for (int j = index; j < elementCount - 1; j++) 
                elementData[j] = elementData[j + 1];
            elementCount--;
        }
    }

    // Получение элемента по индексу
    public T Get(int index)
    {
        return elementData[index];
    }

    // Поиск элемента
    public int IndexOf(object o)
    {
        for (int i = 0; i < elementCount; i++)
        {
            if (elementData[i].Equals(o))
            {
                return i;
            }
        }
        return -1;
    }
}

// Реализация стека на основе вектора
public class MyStack<T> : MyVector<T>
{
    private int _size = -1; // Индекс верхнего элемента стека

    public MyStack(int capacityIncrement = 0) : base(capacityIncrement) { }

    // Добавление элемента в стек
    public void Push(T item)
    {
        Add(item); // Используем метод добавления из MyVector
        _size++;
    }

    // Удаление и возвращение верхнего элемента стека
    public T Pop()
    {
        if (_size == -1) // Проверка на пустой стек
            throw new InvalidOperationException("Стек пуст.");
        
        T item = Get(_size); // Получаем верхний элемент
        Remove(_size);       // Удаляем его
        _size--;
        return item;
    }

    // Проверка, пуст ли стек
    public bool IsEmpty()
    {
        return _size == -1;
    }

    // Просмотр верхнего элемента без удаления
    public T Peek()
    {
        if (_size == -1)
            throw new InvalidOperationException("Стек пуст.");
        
        return Get(_size);
    }

    public int Search(T item)
    {
        return IndexOf(item);
    }
}




public class RPNCalculator
{
    // Словарь для бинарных операторов и их реализации
    private Dictionary<string, Func<double, double, double>> _operators = new Dictionary<string, Func<double, double, double>>()
    {
        { "+", (x, y) => x + y }, 
        { "-", (x, y) => x - y }, 
        { "*", (x, y) => x * y }, 
        { "/", (x, y) => x / y }, 
        { "^", (x, y) => Math.Pow(x, y) },
        { "mod", (x, y) => x % y },
        { "min", (x, y) => Math.Min(x, y) },
        { "max", (x, y) => Math.Max(x, y) }
    };

    // Словарь для унарных операторов
    private Dictionary<string, Func<double, double>> _unaryOperators = new Dictionary<string, Func<double, double>>()
    {
        { "sqrt", x => Math.Sqrt(x) },
        { "sin", x => Math.Sin(x) },
        { "cos", x => Math.Cos(x) },
        { "tan", x => Math.Tan(x) },
        { "log", x => Math.Log(x) },
        { "log10", x => Math.Log10(x) }
    };

    // Основной метод вычисления
    public double Calculate(string expression)
    {
        MyStack<double> stack = new MyStack<double>();

        // Обработка каждого токена выражения
        foreach (string token in expression.Split(' '))
        {
            if (double.TryParse(token, out double operand)) // Если токен — число
            {
                stack.Push(operand);
            }
            else if (_unaryOperators.ContainsKey(token)) // Если токен — унарный оператор
            {
                double unaryOperators = stack.Pop();
                stack.Push(_unaryOperators[token](operand));
            }
            else if (_operators.ContainsKey(token)) // Если токен — бинарный оператор
            {
                double operand2 = stack.Pop();
                double operand1 = stack.Pop();
                stack.Push(_operators[token](operand1, operand2));
            }
        }

        // Возвращаем итоговый результат
        return stack.Pop();
    }
}


  class Program
{
    static void Main(string[] args)
    {
        RPNCalculator calculator = new RPNCalculator();

        Console.Write("Введите выражение в обратной польской нотации: ");
        string expression = Console.ReadLine();

        try
        {
            double result = calculator.Calculate(expression); // Вычисление результата
            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}