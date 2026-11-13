using System.Drawing;

namespace Laba2.Models
{
    public abstract class Shape
    {
        public Point Start { get; protected set; }
        public Point End { get; protected set; }
        public Color Color { get; set; }
        public int Width { get; set; }

        public abstract string Kind { get; }

        protected Shape(Point start, Color color, int width)
        {
            Start = start;
            End = start;
            Color = color;
            Width = width;
        }

        public virtual void MoveEnd(Point p) => End = p;

        public virtual void Move(Point delta)
        {
            Start = new Point(Start.X + delta.X, Start.Y + delta.Y);
            End = new Point(End.X + delta.X, End.Y + delta.Y);
        }

        
        public Rectangle Bounds() => new Rectangle(
            Math.Min(Start.X, End.X),
            Math.Min(Start.Y, End.Y),
            Math.Abs(End.X - Start.X),
            Math.Abs(End.Y - Start.Y));

        public abstract void Draw(Graphics g);
        public abstract bool Contains(Point p);
    }
}
