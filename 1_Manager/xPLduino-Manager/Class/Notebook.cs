/*
    xPLduino - Manager
    Copyright (C) 2014  VOGEL Ludovic - xplduino@gmail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using Gtk;
using Gdk;
using System.Collections.Generic;

namespace xPLduinoManager
{
	//Classe Notebook
	//Classe permettant de sauvegarder les notebook afficher permettant de faire des mise à jour dans ceux ci
	public class Notebook
	{
		public MainWindow mainwindow;
		
		public string SelectType;
		public string SelectID;
		
		public Widget widget;
		public string title;
		
		public HBox TabLayout;
		public Gtk.Image ImgLayout;
		public string ImgName;
		
		public bool Display;
		
		public Notebook (string _Title,Widget _Widget,string _SelectType, string _SelectID, string _ImgName, MainWindow _mainwindow)
		{
			this.mainwindow = _mainwindow;
			this.widget = _Widget;
			this.title = _Title;
			this.SelectType = _SelectType;
			this.SelectID = _SelectID;
			this.ImgName = _ImgName;
			
			CreateLayout(title);
			Display = false;
		}
		
		public void CreateLayout(string _title)
		{
			
			TabLayout = new HBox();	 //Création d'un nouveau header box permettant de contenir le label et le bouton de fermeture
			
			ImgLayout = global::Gtk.Image.LoadFromResource(ImgName);

			Label TabLabelTitle = new Label("  " + _title + " ");  // Création d'un nouveau label contenant le titre que nous avons passé en paramètre
			
			Gtk.Image CloseImg = new Gtk.Image(Stetic.IconLoader.LoadIcon(mainwindow, mainwindow.param.ParamP("TabIconClose"), Gtk.IconSize.Menu));			
			Button TabCloseButton = new Button(CloseImg); //Creation d'un nouveau bouton de fermeture
			
			TabCloseButton.Relief = ReliefStyle.None; // Ajustement d'un paramètre de relief pour le bouton
			TabCloseButton.HeightRequest = 20;
			TabCloseButton.FocusOnClick = false;	 	
			TabCloseButton.Clicked += delegate //Lorsque nous cliquons sur le boutons, nous détruison l'onglet
									  { 
										widget.Destroy(); //On détruit le widget
										mainwindow.UpdateListNoteBook(widget);//Nous appelons la fonction permettant de détruire de la listenotebook l'élément qui va bien
										mainwindow.DeleteWidgetInTab(widget);//Nous appelons la fonction permettant de détruire le widget stocket dans une liste
									  }; 
			 	
			TabLayout.PackStart(ImgLayout);
			TabLayout.PackStart(TabLabelTitle); // On implémente le label dans le header box
			TabLayout.PackStart(TabCloseButton); //Même chose pour le bouton
			TabLayout.ShowAll(); //On affiche le tous			
			
		}
	}
}

