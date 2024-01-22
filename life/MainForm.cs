//This code created by (AdiXaK) Карницкий Вячеслав
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace life
{
	public partial class MainForm : Form
	{
		//настройка цветов и создания переменных  и масивов которые будут использоваться в программе
		private Graphics graphics;//обьект класса графикс
		private int resolution;// переменная отвечающия за разрешение
		private bool[,] Field;//массив игровго поля
		private int rows;//строки
		private	int cols;//столбцы
		static KnownColor co = KnownColor.Black;//цвет мертвых клеток
		Color bcol = Color.FromKnownColor(co);
		static KnownColor c = KnownColor.Cornsilk;//цвет живых клеток
		Color fcol = Color.FromKnownColor(c);
		
		public MainForm()
		{
			InitializeComponent();
		}
		private void StartGame()
		{
			//название метода говорит само за себя 
			//метод производит настройку стартовых значений переменных , запуска таймера , и стартовой генерации клеток , а также первой прорисовки игрового поля
			// этот метод вызывается при нажатии зеленой кнопки - ButtonStart
			if (timer1.Enabled) {
				return;
			}
			ButtonStart.Enabled = true;
			numericUpDown1.Enabled = false;
			numericUpDown2.Enabled = false;
			buttonColor.Enabled = false;
			buttonfcolor.Enabled = false;
			resolution = (int)numericUpDown1.Value;
			rows = pictureBox1.Height / resolution;
			cols = pictureBox1.Width / resolution;
			Field = new bool[cols, rows];
			Random random = new Random();
			for (int x = 0; x < cols; x++) {
				for (int y = 0; y < rows; y++) {
					Field[x, y] = random.Next((int)numericUpDown2.Value) == 0;
				}
			}
			pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			graphics = Graphics.FromImage(pictureBox1.Image);
			timer1.Start();
		}
		private void NextGeneration()
		{
			graphics.Clear(bcol);
			var NewField = new bool[cols, rows];
			for (int x = 0; x < cols; x++) {
				for (int y = 0; y < rows; y++) {
					var NeighbourCount = CountNeighbour(x, y);
					var HasLife = Field[x, y];
					if (!HasLife && NeighbourCount == 3) {
						NewField[x, y] = true;
					} else if (HasLife && (NeighbourCount < 2 || NeighbourCount > 3)) {
						NewField[x, y] = false;
					} else {
						NewField[x, y] = Field[x, y];
					}
					if (HasLife) {
						graphics.FillEllipse(new SolidBrush(fcol), x * resolution, y * resolution, resolution, resolution);
					}
				}
				
			}
			Field = NewField;
			pictureBox1.Refresh();
		}
		private void StopGame()
		{
			if (!timer1.Enabled) {
				return;
			}
			timer1.Stop();
			numericUpDown1.Enabled = true;
			numericUpDown2.Enabled = true;
			ButtonStart.Enabled = true;
			buttonfcolor.Enabled = true;
			buttonColor.Enabled = true;
		}
		private int CountNeighbour(int x, int y)
		{
			int count = 0;
			for (int i = -1; i < 2; i++) {
				for (int j = -1; j < 2; j++) {
					var col = (x + i + cols) % cols;
					var row	= (y + j + rows) % rows;
					var IsSelfChecking = col == x && row == y;
					var hasLife = Field[col, row];
					if (hasLife && !IsSelfChecking) {
						count++;
					}
				}
			}
			return count;
		}
		private void UpdatePictcherBox()
		{
			pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
			graphics = Graphics.FromImage(pictureBox1.Image);
		}
		void Timer1Tick(object sender, EventArgs e)
		{
			UpdatePictcherBox();
			NextGeneration();
			StartGame();	
		}
		void ButtonStartClick(object sender, EventArgs e)
		{
			StartGame(); // при нажатии на зеленую кнопку будет выхван метод StartGame();
		}
		void ButtonStopClick(object sender, EventArgs e)
		{
			StopGame();
		}
		void ButtonColorClick(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK) {
				bcol = c.Color;
			}
		}
		void ВыходToolStripMenuItemClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		private void SetFieldFromMouse(MouseEventArgs e, bool setValue)
		{
			if (!timer1.Enabled) {
				return;
			}

			if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right) {
				var x = e.Location.X / resolution;
				var y = e.Location.Y / resolution;
				var validMousePassed = ValidMousePosition(x, y);
				if (validMousePassed) {
					Field[x, y] = setValue;
				}
			}
		}

		private void PictureBox1MouseMove(object sender, MouseEventArgs e)
		{
			SetFieldFromMouse(e, true);
		}

		private void PictureBox1MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left) {
				SetFieldFromMouse(e, e.Button == MouseButtons.Left);
			}
		}
		private bool ValidMousePosition(int x, int y)
		{
			return x >= 0 && y >= 0 && x < cols && y < rows;
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			ColorDialog c = new ColorDialog();
			if (c.ShowDialog() == DialogResult.OK) {
				fcol = c.Color;
			}
		}
		
		void ОПрограммеToolStripMenuItemClick(object sender, EventArgs e)
		{
			About a = new About();
			a.ShowDialog();
		}
	}
}