using Laba2.IO;
using Laba2.Logic;
using Laba2.Models;
using Laba2.Patterns;

namespace Laba2
{
    public partial class Form1 : Form
    {
        private readonly Drawing _drawing = new();
        private readonly History _history = new();

        private string _currentTool = "LineShape";
        private Color _currentColor = Color.Black;
        private int _currentWidth = 2;

        private Shape? _currentShape;
        private Shape? _selectedShape;
        private bool _isDragging;
        private Point _lastMouse;

        public Form1()
        {
            InitializeComponent();

            _drawing.OnChanged += () => pictureBox1.Invalidate();
            pictureBox1.TabStop = true;

            numWidth.Value = 2;

            btnLine.Click += (_, _) => _currentTool = "LineShape";
            btnRect.Click += (_, _) => _currentTool = "RectShape";
            btnEllipse.Click += (_, _) => _currentTool = "EllipseShape";
            btnPencil.Click += (_, _) => _currentTool = "PencilShape";

            btnColor.Click += (_, _) =>
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                    _currentColor = colorDialog1.Color;
            };

            numWidth.ValueChanged += (_, _) => _currentWidth = (int)numWidth.Value;

            btnUndo.Click += (_, _) => _history.Undo();
            btnRedo.Click += (_, _) => _history.Redo();
            btnClear.Click += (_, _) => _history.Do(new ClearCommand(_drawing));

            btnSave.Click += BtnSave_Click;
            btnLoad.Click += BtnLoad_Click;
            btnExportPng.Click += BtnExportPng_Click;

            pictureBox1.Paint += PictureBox_Paint;
            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;

            KeyDown += Form1_KeyDown;
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            _drawing.Draw(e.Graphics);

            if (_selectedShape != null)
            {
                var b = _selectedShape.Bounds();
                b.Inflate(4, 4);

                using var pen = new Pen(Color.DodgerBlue, 1);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                e.Graphics.DrawRectangle(pen, b);
            }
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            pictureBox1.Focus();
            if (e.Button != MouseButtons.Left) return;

            var hit = _drawing.FindShapeAt(e.Location);
            if (hit != null)
            {
                _selectedShape = hit;
                _isDragging = true;
                _lastMouse = e.Location;
                pictureBox1.Invalidate();
                return;
            }

            _currentShape = ShapeFactory.Create(_currentTool, e.Location, _currentColor, _currentWidth);
            _history.Do(new AddShapeCommand(_drawing, _currentShape));

            _selectedShape = null;
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging && _selectedShape != null)
            {
                var delta = new Point(e.X - _lastMouse.X, e.Y - _lastMouse.Y);
                _selectedShape.Move(delta);
                _lastMouse = e.Location;
                pictureBox1.Invalidate();
                return;
            }

            if (_currentShape != null)
            {
                _currentShape.MoveEnd(e.Location);
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            _isDragging = false;
            _currentShape = null;
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                _history.Undo();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.Y)
            {
                _history.Redo();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Delete && _selectedShape != null)
            {
                _history.Do(new RemoveShapeCommand(_drawing, _selectedShape));
                _selectedShape = null;
                pictureBox1.Invalidate();
                e.Handled = true;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json",
                FileName = "drawing.json"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try { JsonIO.Save(dlg.FileName, _drawing.Shapes); }
                catch (Exception ex) { MessageBox.Show("Ошибка сохранения: " + ex.Message); }
            }
        }

        private void BtnLoad_Click(object? sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog { Filter = "JSON files (*.json)|*.json" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var shapes = JsonIO.Load(dlg.FileName);
                    _drawing.SetShapes(shapes);
                    _history.Clear();
                    _selectedShape = null;
                }
                catch (Exception ex) { MessageBox.Show("Ошибка загрузки: " + ex.Message); }
            }
        }

        private void BtnExportPng_Click(object? sender, EventArgs e)
        {
            using var dlg = new SaveFileDialog
            {
                Filter = "PNG файлы (*.png)|*.png",
                FileName = "my_drawing.png"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                bmp.Save(dlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                MessageBox.Show("Рисунок успешно сохранен в PNG!", "Успех");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }
    }
}
