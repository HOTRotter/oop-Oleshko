using System.Drawing;
using Laba2.Models;

namespace Laba2.Patterns
{
    public static class ShapeFactory
    {
        public static readonly Dictionary<string, Func<Point, Color, int, Shape>> Kinds = new()
        {
            ["LineShape"] = (p, c, w) => new LineShape(p, c, w),
            ["RectShape"] = (p, c, w) => new RectShape(p, c, w),
            ["EllipseShape"] = (p, c, w) => new EllipseShape(p, c, w),
            ["PencilShape"] = (p, c, w) => new PencilShape(p, c, w),
        };

        public static Shape Create(string kind, Point start, Color color, int width)
            => Kinds[kind](start, color, width);
    }
}
