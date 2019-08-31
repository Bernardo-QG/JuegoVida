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
		int _tamaño=30;
		List<Grid> listaGrid = new List<Grid>();
		public MainWindow()
		{
			InitializeComponent();
			
			_InicializandoAplicacion();
			_InicializandoUniverso();
			_Ver();



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

			gridPlay.MouseDown += (s, e) => { _setUniverso(); _Ver(); _Vida(); _Ver();  _VerUniverso();  _Ver(); };
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

					gridJuego.Children.Add(new _Celula(_idGrid, new Point(j, i),txtXY, _Universo[i,j],this));
					_idGrid++;
				}			
				
			}
			_UniversoAntiguo = (bool[,])_Universo.Clone();

		}
	
		private void _Vida() {
			Console.WriteLine("$$$$$$$$$$$  Vida $$$$$$$$$$$$$");

			_UniversoAntiguo = (bool[,])_Universo.Clone();
						
			
			

			byte poblacion;
			/* Universo */
			for (int y = 0; y < _tamaño; y++)
			{
				for (int x = 0; x < _tamaño; x++)
				{
					poblacion = 0;
					Console.WriteLine("Cordenada"+ x +", "+y);
					/* Vecinos de una celula*/
					for (int i = y-1; i <= y+1; i++)
					{
						for (int j = x-1; j <= x+1; j++)
						{

							//Console.ReadLine();
							if ((i >= 0 && i < _tamaño) && (j >= 0 && j < _tamaño))
							{
								Console.WriteLine(" i" + i + " j" + j + " p" + poblacion + " u" + _UniversoAntiguo[i, j]);
								//Console.ReadLine();
								if (!_UniversoAntiguo[i, j])
									poblacion++;
							}

						}
					}

					Console.WriteLine(" Poblacion "+poblacion+" ");
		
						if (poblacion == 2 || poblacion == 3)
							_Universo[y, x] = false;
						else
							_Universo[y, x] = true;
					}
				Console.WriteLine();
			}
			Console.WriteLine("$$$$$$$$$$$$$$$$$$$$$$$$$$$$");

		}

		private void _VerUniverso() {

			foreach (_Celula item in gridJuego.Children)
			{
				//MessageBox.Show(item.Name);
				item._muerto =  _Universo[(int)item._cordenada.X,(int)item._cordenada.Y];
				item._setCelula();
			}

		}

		private void _setUniverso() {

			foreach (_Celula item in gridJuego.Children)
			{
				//MessageBox.Show(item.Name);
				_Universo[(int)item._cordenada.X,(int)item._cordenada.Y] = item._muerto ;
		
			}

		}

		public void _Ver()
		{
			Console.WriteLine("%%%%%%%%%%%%     VER      %%%%%%%%%%%%%%%");
			for (int i = 0; i < _tamaño; i++)
			{
				for (int j = 0; j < _tamaño; j++)
				{
					Console.Write(_UniversoAntiguo[i, j]+" ");

				}
				Console.WriteLine();
			}
			Console.WriteLine(".................................");

			for (int i = 0; i < _tamaño; i++)
			{
				for (int j = 0; j < _tamaño; j++)
				{
					Console.Write(_Universo[i, j] + " ");

				}
				Console.WriteLine();
			}
			Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
		}

	}

	public class _Celula : Grid
	{
		public Point _cordenada;
		public bool _muerto;
	
		TextBlock _tb;
		MainWindow m;

		bool[,] _Universo;
		public _Celula(int _idGrid, Point _cordenada,TextBlock _tb, bool _muerto, MainWindow m) {
			this._cordenada = _cordenada;
			this._tb = _tb;
		
			this.m = m;
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
				//m._Ver();
				if (_muerto)
				{
					Background = Brushes.LightGreen; _muerto = false;

				}
				else
				{
					Background = Brushes.WhiteSmoke; _muerto = true;
				}
				
			};
		}

		public void _setCelula() {
			if (!_muerto)
			{
				Background = Brushes.LightGreen;
			}
			else
			{
				Background = Brushes.WhiteSmoke; 
			}
		}
	}

	
}
