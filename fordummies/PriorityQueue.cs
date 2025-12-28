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
    private int _count; //Anzahl der Elemente im Heap
    private MaxMin _mode;
 
    public MaxMinPriorityQueue(int size, MaxMin auswahl)
    {
        _daten = new T[size];
        _count = 0;
        _mode = auswahl;
    }
    public T[] Daten => _daten;
    public int Count => _count; //hat mir besser gefallen weil der Konstruktor eine size hat und das ust was anderes
    public int Size => _count; //Weil so in Aufgabenblatt 10 gefordert. Ich könnte auch Count zu Size umbenennen aber eh... vielleicht später
    public MaxMin Mode => _mode;

    private void Swap(int i, int j)
    {
        (_daten[i], _daten[j]) = (_daten[j], _daten[i]); //Tatsächlicher Tausch
        _daten[i].ChangingAction = CreateChangingKeyAction(i); //einmal bescheid sagen, dass ein tausch statt fand
        _daten[j].ChangingAction = CreateChangingKeyAction(j);
    }
    private bool Better(int i, int j)
    {
        return (_mode == MaxMin.Max && Greater(_daten[i], _daten[j])) || (_mode == MaxMin.Min && Less(_daten[i], _daten[j]));
        //Je nachdem ob mode Max/Min wird geguckt ob es Greater/Less ist und true/false zurück geworfen
    }
    public bool Less(T object1, T object2) => object1.ChangeableKey < object2.ChangeableKey;

    public bool Greater(T object1, T object2) => object1.ChangeableKey > object2.ChangeableKey;

    public void Heapify(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int best = index; //"Bester" Knoten -> Größter bei Max, Kleinster bei Min

            if (left < _count) //Wenn linker Knoten existiert, prüfe ob "besser" als best
            {
                if (Better(left, best))
                {
                    best = left;
                }
            }

            if (right < _count)
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
        if (_count == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        return Daten[0];
    }
    public T ExtractFirst()
    {
        if (_count == 0)
        {
            throw new InvalidOperationException("PriorityQueue ist leer");
        }
        T result = _daten[0];
        _count--; //Letzes Element logisch entfernen (Größe verringern)

        if (_count > 0)
        {
            _daten[0] = _daten[_count]; //root aktualisieren
            _daten[0].ChangingAction = CreateChangingKeyAction(0); //Auch hier wird pos geändert
            Heapify(0); //sortieren
        }
        return result;
    }
    public void Insert(T element)
    {
        if (_count == _daten.Length)
        {
            throw new InvalidOperationException("PriorityQueue ist voll.");
        }
        int i = _count;
        _daten[i] = element;
        _count++;

        _daten[i].ChangingAction = CreateChangingKeyAction(i);

        IncreaseKey(i);
    }
    private void IncreaseKey(int pos)
    {
        while (pos > 0) //Bubble up
        {
            int parent = (pos - 1) / 2;

            if (Better(pos, parent))
            {
                Swap(pos, parent);
                pos = parent;
            }
            else
                break;
        }
    }
    private Action CreateChangingKeyAction(int pos)
    {
        return () => IncreaseKey(pos);
    }
    override public string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _count; i++)
        {
            sb.Append(_daten[i].ChangeableKey).Append(" ");
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
