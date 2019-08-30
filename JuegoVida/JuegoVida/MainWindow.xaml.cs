using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JuegoVida
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	
	public partial class MainWindow : Window
	{
		bool[,] _Universo, _UniversoAntiguo;
		int _tamaño=10;
		List<Grid> listaGrid = new List<Grid>();
		public MainWindow()
		{
			InitializeComponent();
			
			_InicializandoAplicacion();
			_InicializandoUniverso();

			
			
		}

		private void _InicializandoAplicacion() {
			Application.Current.MainWindow = this;

			this.SizeChanged += (s, e) =>
			{
				Application.Current.MainWindow.Height = Application.Current.MainWindow.Width;
			};
			
			gridAlfa.MouseDown += (s, e) =>
			{
				DragMove();
			};

			gridPlay.MouseDown += (s, e) => { _Vida(); _VerUniverso(); };
			gridPause.MouseDown += (s, e) => {  };
			gridRestart.MouseDown += (s, e) => {  };
			gridClose.MouseDown += (s, e) => {	Application.Current.Shutdown();	};
		}

		private void _InicializandoUniverso()
		{
			int _idGrid=0;
			_Universo = new bool[_tamaño, _tamaño];
			_UniversoAntiguo = new bool[_tamaño, _tamaño];
			for (int i = 0; i < _tamaño; i++)
			{
				gridJuego.RowDefinitions.Add(new RowDefinition());
				gridJuego.ColumnDefinitions.Add(new ColumnDefinition());
			}

			for (int i = 0; i < _tamaño; i++)
			{
				for (int j = 0; j < _tamaño; j++)
				{
					_Universo[i, j] = true;

					gridJuego.Children.Add(new _Celula(_idGrid, new Point(j, i),txtXY, _Universo[i,j]));
					_idGrid++;
				}			
				
			}

		}
	
		private void _Vida() {

			
			_UniversoAntiguo = (bool[,])_Universo.Clone();

			byte poblacion;
			/* Universo */
			for (int y = 0; y < _tamaño; y++)
			{
				for (int x = 0; x < _tamaño; x++)
				{
					poblacion = 0;
					/* Vecinos de una celula*/
					for (int i = y-1; i < y+1; i++)
					{
						for (int j = x-1; j < x+1; j++)
						{
							if ((i >= 0 & i < gridJuego.ActualHeight) & (j >= 0 & j < gridJuego.ActualWidth))
								if (_UniversoAntiguo[i, j] == true)
									poblacion++;

						}
					}

						
		
						if (poblacion == 2 || poblacion == 3)
							_Universo[y, x] = true;
						else
						_Universo[y, x] = false;
					}				
			}

		}

		private void _VerUniverso() {

			foreach (_Celula item in gridJuego.Children)
			{
				//MessageBox.Show(item.Name);
				item._muerto =  _Universo[(int)item._cordenada.X,(int)item._cordenada.Y];
				item._setCelula();
			}

		}

	}

	public class _Celula : Grid
	{
		public Point _cordenada;
		public bool _muerto;
	
		TextBlock _tb;

	
		public _Celula(int _idGrid, Point _cordenada,TextBlock _tb, bool _muerto) {
			this._cordenada = _cordenada;
			this._tb = _tb;
			this._muerto = _muerto;
			Name = "cell" + _idGrid.ToString();
			Tag = _cordenada;
			Background = Brushes.WhiteSmoke;
			Grid.SetRow(this, (int)_cordenada.X);
			Grid.SetColumn(this, (int)_cordenada.Y);
			_Eventos();
		}
		private void _Eventos() {
			MouseDown += (s, e) => {
				
				_tb.Text=_cordenada.X + ", " + _cordenada.Y+" => "+_muerto+", n: "+Name;
				_setCelula();
			};
		}

		public void _setCelula() {
			if (_muerto)
			{
				Background = Brushes.LightGreen; _muerto = false;
			}
			else
			{
				Background = Brushes.WhiteSmoke; _muerto = true;
			}
		}
	}

	
}
