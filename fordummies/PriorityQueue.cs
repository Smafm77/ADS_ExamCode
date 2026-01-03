using System.Text;
public enum MaxMin
{
    Max,
    Min
}
public interface IElementWithKey
{
    public int Key { get; }
    public int ChangeableKey { get; set; }
    public Action ChangingAction { set; }
}
public class MaxMinPriorityQueue<T> where T : IElementWithKey
{
    private T[] _daten;
    private int _size; //Anzahl der Elemente im Heap
    private MaxMin _mode;

    public MaxMinPriorityQueue(int size, MaxMin auswahl)
    {
        _daten = new T[size];
        _size = 0;
        _mode = auswahl;
    }
    public T[] Daten => _daten;
    public int Size => _size; //Weil so in Aufgabenblatt 10 gefordert
    public MaxMin Mode => _mode;
    private void Swap(int i, int j)
    {
        (Daten[i], Daten[j]) = (Daten[j], Daten[i]); //Tatsächlicher Tausch
        Daten[i].ChangingAction = CreateChangingKeyAction(i); //einmal bescheid sagen, dass ein tausch statt fand
        Daten[j].ChangingAction = CreateChangingKeyAction(j);
    }
    private bool Better(int i, int j)
    {
        return (Mode == MaxMin.Max && Greater(Daten[i], Daten[j])) || (Mode == MaxMin.Min && Less(Daten[i], Daten[j]));
        //Je nachdem ob mode Max/Min wird geguckt ob es Greater/Less ist und true/false zurück geworfen
    }
    public bool Less(T object1, T object2) => object1.ChangeableKey < object2.ChangeableKey;

    public bool Greater(T object1, T object2) => object1.ChangeableKey > object2.ChangeableKey;

    public void Heapify(int index)
    {
        while (index < Size)
        {
            int best = index; //"Bester" Knoten -> Größter bei Max, Kleinster bei Min
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            if (left < Size) //Wenn linker Knoten existiert, prüfe ob "besser" als best
            {
                if (Better(left, best))
                {
                    best = left;
                }
            }

            if (right < Size)
            {
                if (Better(right, best))
                {
                    best = right;
                }
            }

            if (best == index)
                break;

            Swap(index, best);
            index = best;
        }
    }
    public T First()
    {
        if (Size == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        return Daten[0];
    }
    public T ExtractFirst()
    {
        if (Size == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        T result = Daten[0];
        _size--; //Letzes Element logisch entfernen (Größe verringern) Size hat halt kein Setter also direkt aufs Feld
        if (Size > 0)
        {
            Daten[0] = Daten[Size]; //root aktualisieren
            Daten[0].ChangingAction = CreateChangingKeyAction(0); //Auch hier wird pos geändert
            Heapify(0); //sortieren
        }
        return result;
    }
    public void Insert(T element)
    {
        if (Size == Daten.Length)
        {
            throw new InvalidOperationException("PriorityQueue ist voll.");
        }
        int i = Size;
        Daten[i] = element;
        _size++;
        Daten[i].ChangingAction = CreateChangingKeyAction(i);
        IncreaseKey(i);
    }
    private void IncreaseKey(int position)
    {
        while (position > 0) //Bubble up
        {
            int parent = (position - 1) / 2;
            if (Better(position, parent))
            {
                Swap(position, parent);
                position = parent;
            }
            else
            {
                break;
            }
        }
    }
    private Action CreateChangingKeyAction(int position)
    {
        return () => IncreaseKey(position);
    }
    override public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < Size; i++)
        {
            sb.Append(Daten[i].ChangeableKey).Append(" ");
        }
        return sb.ToString().TrimEnd(' ');
    }
}
public class MaxPriorityQueue<T> : MaxMinPriorityQueue<T> where T : IElementWithKey
{
    public MaxPriorityQueue(int size) : base(size, MaxMin.Max) // Aufruf des Parent-Constructors mit MaxMin = Max
    {
    }
    public T Maximum() => First();
}
public class MinPriorityQueue<T> : MaxMinPriorityQueue<T> where T : IElementWithKey
{
    public MinPriorityQueue(int size) : base(size, MaxMin.Min)
    {
    }
    public T Minimum() => First();
}