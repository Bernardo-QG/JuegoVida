﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace JuegoVida
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>

	public partial class MainWindow : Window
	{
		bool[,] _Universo, _UniversoAntiguo;
		int _tamaño = 50;
	
		DispatcherTimer Timer = new DispatcherTimer();
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
			

			bwObj.DoWork += new DoWorkEventHandler(bwObj_DoWork);
			bwObj.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);


			Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
			Timer.Tick += (s, a) =>
			{
				
				if (bwObj.IsBusy != true)
				{
					bwObj.RunWorkerAsync();
				}
				
			};

			gridPlay.MouseDown += (s, e) => { _setUniverso(); Timer.Start(); };
			gridPause.MouseDown += (s, e) => { Timer.Stop(); };
			gridRestart.MouseDown += (s, e) => { _CleanUniverso(); _VerUniverso(); Timer.Stop(); };
			gridClose.MouseDown += (s, e) => { Application.Current.Shutdown(); };
		}


		BackgroundWorker bwObj = new BackgroundWorker();
		private void bwObj_DoWork(object sender, DoWorkEventArgs e)
		{
			_Vida();
		}
		private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (!(e.Error == null))
				MessageBox.Show("Error: " + e.Error.Message);
			else
			{
				_VerUniverso();
			}
		}

		private void _InicializandoUniverso()
		{
			int _idGrid = 0;
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

					gridJuego.Children.Add(new _Celula(gridJuego.Children.Count, new Point(j, i),  _Universo[i, j]));
					_idGrid++;
				}

			}
			_UniversoAntiguo = (bool[,])_Universo.Clone();

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
					for (int i = y-1; i <= y+1; i++)
					{
						for (int j = x-1; j <= x+1; j++)
						{
							//Limites
							if ((i >= 0 && i < _tamaño) && (j >= 0 && j < _tamaño))
							{								
								if (!_UniversoAntiguo[i, j])
									poblacion++;
							}
						}
					}
															
					//Primera Regla
					if (!_Universo[y, x])
					{
						poblacion--;
						if ((poblacion < 2 || poblacion > 3))
							_Universo[y, x] = true;
					}
					//Segunda Regla
					else
						if (poblacion == 3)
							_Universo[y, x] = false;
				}
				
			}			
		}

		private void _VerUniverso() {

			foreach (_Celula item in gridJuego.Children)
			{
				item._muerto =  _Universo[(int)item._cordenada.X,(int)item._cordenada.Y];
				item._setCelula();
			}
		}

		private void _setUniverso() {

			foreach (_Celula item in gridJuego.Children)
			{
					_Universo[(int)item._cordenada.X,(int)item._cordenada.Y] = item._muerto ;
			}

		}

		private void _CleanUniverso()
		{

			for (int i = 0; i < _tamaño; i++)
				for (int j = 0; j < _tamaño; j++)
					_Universo[i, j] = true;

		}
			

	}

	
	public class _Celula : Grid
	{
		public Point _cordenada;
		public bool _muerto;
				
		public _Celula(int _idGrid, Point _cordenada, bool _muerto) {
			this._cordenada = _cordenada;		
			this._muerto = _muerto;
			Name = "c" + _idGrid.ToString();
			Tag = _cordenada;
			Background = Brushes.WhiteSmoke;
			Grid.SetRow(this, (int)_cordenada.X);
			Grid.SetColumn(this, (int)_cordenada.Y);
			_Eventos();
		}
		private void _Eventos() {
			MouseDown += (s, e) => {
				
				//Console.WriteLine(_cordenada.X + ", " + _cordenada.Y+" => "+_muerto+",  "+Name);
				if (_muerto)
				{
					Background = new SolidColorBrush(Color.FromRgb(83, 0, 167)); _muerto = false;

				}
				else
				{
					Background = Brushes.WhiteSmoke; _muerto = true;
				}
				
			};
		}

		public void _setCelula() {
			if (!_muerto)			
				Background = new SolidColorBrush(Color.FromRgb(83, 0, 167)); 
			else
				Background = Brushes.WhiteSmoke; 
			
		}
	}

	
}
