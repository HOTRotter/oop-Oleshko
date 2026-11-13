using Laba2.Models;

namespace Laba2.Logic
{
    public class Drawing
    {
        private readonly List<Shape> _shapes = new();

        public event Action? OnChanged;

        public IReadOnlyList<Shape> Shapes => _shapes;

        public void Add(Shape shape) { _shapes.Add(shape); OnChanged?.Invoke(); }
        public void Remove(Shape shape) { _shapes.Remove(shape); OnChanged?.Invoke(); }
        public void Clear() { _shapes.Clear(); OnChanged?.Invoke(); }

        public void Draw(Graphics g)
        {
            foreach (var s in _shapes) s.Draw(g);
        }

        public Shape? FindShapeAt(Point p)
        {
            for (int i = _shapes.Count - 1; i >= 0; i--)
                if (_shapes[i].Contains(p)) return _shapes[i];
            return null;
        }

        public void SetShapes(IEnumerable<Shape> shapes)
        {
            _shapes.Clear();
            _shapes.AddRange(shapes);
            OnChanged?.Invoke();
        }
    }
}
