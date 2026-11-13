using Laba2.Logic;
using Laba2.Models;

namespace Laba2.Patterns
{

    public class AddShapeCommand : ICommand
    {
        private readonly Drawing _drawing;
        private readonly Shape _shape;

        public AddShapeCommand(Drawing drawing, Shape shape)
        {
            _drawing = drawing;
            _shape = shape;
        }

        public void Execute() => _drawing.Add(_shape);
        public void Undo() => _drawing.Remove(_shape);
    }

    public class RemoveShapeCommand : ICommand
    {
        private readonly Drawing _drawing;
        private readonly Shape _shape;

        public RemoveShapeCommand(Drawing drawing, Shape shape)
        {
            _drawing = drawing;
            _shape = shape;
        }

        public void Execute() => _drawing.Remove(_shape);
        public void Undo() => _drawing.Add(_shape);
    }

    public class ClearCommand : ICommand
    {
        private readonly Drawing _drawing;
        private readonly List<Shape> _old;

        public ClearCommand(Drawing drawing)
        {
            _drawing = drawing;
            _old = drawing.Shapes.ToList();
        }

        public void Execute() => _drawing.Clear();
        public void Undo() => _drawing.SetShapes(_old);
    }
}
