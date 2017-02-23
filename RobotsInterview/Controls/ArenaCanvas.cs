using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RobotsInterview.Controls
{
	public class ArenaCanvas : Canvas
	{
		public ArenaCanvas()
			: base()
		{
			SizeChanged += ArenaCanvas_SizeChanged;
		}

		void ArenaCanvas_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{
			DrawArena();
		}

		bool isDrawn;

		private void DrawArena()
		{
			
			var width = this.ActualWidth;
			var height = this.ActualHeight;
			if (width == 0 || height == 0) return;
			var xspace = width / 10;
			var yspace = height / 10;
			if (!isDrawn)
			{
				for (int i = 0; i <=10; i++)
				{
					Line l = new Line();

					l.X1 = xspace * i;
					l.Y1 = 0;

					l.X2 = xspace * i;
					l.Y2 = height;

					l.Stroke = Brushes.LightGray;
					l.StrokeThickness = 2;
					Children.Add(l);
					
				}
				for (int i = 0; i <= 10; i++)
				{
					Line l2 = new Line();

					l2.X1 = 0;
					l2.Y1 = yspace * i;

					l2.X2 = width;
					l2.Y2 = yspace * i;

					l2.Stroke = Brushes.LightGray;
					l2.StrokeThickness = 2;
					Children.Add(l2);

					
				}
				isDrawn = true;
			}
			
		}
	}
}
