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
using System.IO;
using Gtk;
using Gdk;
using System.Collections.Generic;


using Mono.Unix;
// Path
using System.Threading;
using System.Text;
using System.Diagnostics ;


namespace xPLduinoManager
{

	//Classe Customer
	//Classe permettant de gerer le widget customer
	[System.ComponentModel.ToolboxItem(true)]
	public partial class CustomerEdit : Gtk.Bin
	{
		public Mono.TextEditor.TextEditor TextEditor1;
		public Mono.TextEditor.TextEditor TextEditor2;
        public Mono.TextEditor.TextEditorOptions OptionsTextEditor1;	
		public Mono.TextEditor.TextEditorOptions OptionsTextEditor2;	
		
		public DataManagement datamanagement;
		public Int32 CustomerId;
		public Int32 ProjectId;
		public Int32 Project_Id;
		public Int32 NodeId;
		public Param param;
		public MainWindow mainwindow;
	
		public int PositionInTextView;
		public int PositionLineInTextView;

		public Gtk.MenuItem MenuItemTextEditor;
		
		public CustomerEdit (DataManagement _datamanagement, Param _param, Int32 _CustomerId, MainWindow _mainwindow)
		{
			this.Build ();
			this.CustomerId = _CustomerId;
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.mainwindow = _mainwindow;
			
			InitWidget();
		
			TextEditor1.FocusOutEvent += OnTextEditor1FocusOutEvent;
			TextEditor1.ButtonReleaseEvent += OnTextEditor1ButtonReleaseEvent;
			TextEditor1.KeyReleaseEvent += OnTextEditor1NoteKeyReleaseEvent;
		
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == _CustomerId)
						{
							datamanagement.CurrentProjectId = pro.Project_Id;
							Project_Id = pro.Project_Id;
						}
					}    
				}
			}			
		}
		
		
//##################### WIDGET LOCAL #################################
		
		//Fonction InitWidget
		//Fonction permettant d'initialiser le widget
		public void InitWidget()
		{
			hpaned2.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
			ReturnId();
			InitTextEditor1("text/x-csharp");
			InitTextEditor2("text/x-csharp");
			InitialiseMenu();				
		}
			
		//Fonction UpdateHpaned
		//Fonction permettant de mettre à jour les Hpaned
		public void UpdateHpaned()
		{
			hpaned2.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
		}
		
		//Fonction UpdateWidget
		//Fonction permettant de mettre à jour le widget	
		public void UpdateWidget()
		{	
			foreach(Project pro in datamanagement.ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == CustomerId)
						{
							TextEditor1.Text = cus.CustomerData;
						}
					}
				}
			}	
			
			TextEditor2.Document.Text = datamanagement.ReplaceData(TextEditor1.Document.Text,ProjectId,NodeId,CustomerId);
			datamanagement.AddElementInListMenuTextEditor();
			InitialiseMenu();
		}
		
//##################### EDITEUR DE TEXTE #############################		
		
		//Fonction InitTextEditor1
		//Fonction permettant d'initialiser l'éditeur de texte de gauche
		public void InitTextEditor1(string mimetype)
		{
			TextEditor1 = new MyTextEditor();
			OptionsTextEditor1 = new Mono.TextEditor.TextEditorOptions();
			
            if (mimetype != null)
                TextEditor1.Document.MimeType = mimetype;
           
			ScrolledWindowTextEdit1.Add(TextEditor1);
			
			OptionsTextEditor1.ShowInvalidLines = false;
            OptionsTextEditor1.ShowLineNumberMargin = true;
            OptionsTextEditor1.TabsToSpaces = true;
            OptionsTextEditor1.HighlightCaretLine = true;
            OptionsTextEditor1.HighlightMatchingBracket = true;
            OptionsTextEditor1.OverrideDocumentEolMarker = true;
            OptionsTextEditor1.DefaultEolMarker = "\n";
            OptionsTextEditor1.ShowIconMargin = true;
            OptionsTextEditor1.ShowFoldMargin = true;			
			
			TextEditor1.Options = OptionsTextEditor1;
			
			foreach(Project pro in datamanagement.ListProject)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == CustomerId)
						{
							TextEditor1.Text = cus.CustomerData;
						}
					}
				}
			}	
		}
		
		//Fonction InitTextEditor2
		//Fonction permettant d'initialiser l'éditeur de texte de droite		
		public void InitTextEditor2(string mimetype)
		{
			TextEditor2 = new MyTextEditor();
			OptionsTextEditor2 = new Mono.TextEditor.TextEditorOptions();
			
            if (mimetype != null)
                TextEditor2.Document.MimeType = mimetype;
           
			ScrolledWindowTextEdit2.Add(TextEditor2);
			
			OptionsTextEditor2.ShowInvalidLines = false;
            OptionsTextEditor2.ShowLineNumberMargin = true;
            OptionsTextEditor2.TabsToSpaces = true;
            OptionsTextEditor2.HighlightCaretLine = true;
            OptionsTextEditor2.HighlightMatchingBracket = true;
            OptionsTextEditor2.OverrideDocumentEolMarker = true;
            OptionsTextEditor2.DefaultEolMarker = "\n";
            OptionsTextEditor2.ShowIconMargin = false;
            OptionsTextEditor2.ShowFoldMargin = false;	
			
			TextEditor2.Options = OptionsTextEditor2;
			TextEditor2.Sensitive = true;
			
			TextEditor2.Document.Text = datamanagement.ReplaceData(TextEditor1.Document.Text,ProjectId,NodeId,CustomerId);
		}	
		
			//Classe MyTextEditor
			//Classe permettant la création d'un éditeur de texte
			public class MyTextEditor : Mono.TextEditor.TextEditor 
			{
		        public MyTextEditor() : base() 
				{		
		        }
	        	public MyTextEditor(Mono.TextEditor.Document doc, Mono.TextEditor.ITextEditorOptions options) : base(doc, options) 
				{
	        	}
		       // protected override void OnDragDataReceived (Gdk.DragContext context, Int32 x, Int32 y, Gtk.SelectionData args, UInt32 info, UInt32 time_)
			   //{	
			   //}
			}	
		
			//Fonction OnTextEditor1NoteKeyReleaseEvent
			//Permet de faire des actions lors d'un appuie sur une touche
			[GLib.ConnectBefore]
			private void OnTextEditor1NoteKeyReleaseEvent (object o, Gtk.KeyReleaseEventArgs args)
			{
				TextEditor2.Document.Text = datamanagement.ReplaceData(TextEditor1.Document.Text,ProjectId,NodeId,CustomerId);
			}		
		
			//Fonction OnTextEditor1FocusOutEvent
			//Fonction permettant de d'enregistrer les données lorsque nous quittons la fenêtre
			[GLib.ConnectBefore]
			private void OnTextEditor1FocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				datamanagement.ModifyCustomer(TextEditor1.Document.Text,param.ParamI("MoCu_ChoiceData"),CustomerId);	
			}
		
				//Fonction SaveCurrentData
				//Fonction permettant de faire une sauvegarde du travail en cours lors d'un enregistrement
				public void SaveCurrentData()
				{
					datamanagement.ModifyCustomer(TextEditor1.Document.Text,param.ParamI("MoCu_ChoiceData"),CustomerId);
				}
		
			//Fonction OnTextEditor1ButtonReleaseEvent
			//Fonction permettant de faire des actions lors d'un clic de souris
			[GLib.ConnectBefore]
			private void OnTextEditor1ButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
			{
			/*
				if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
				{			
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",true);
					if(TextEditor1.SelectedText != "" && TextEditor1.SelectedText != null)
					{
						Search(datamanagement.ReplaceData(TextEditor1.Document.Text,ProjectId,NodeId,CustomerId),false);
					}
				}
				else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
				{	
					MenuItemTextEditor.Submenu.ShowAll();
					((Gtk.Menu)MenuItemTextEditor.Submenu).Popup();
				}*/
			}
			
			//Fonction InitialiseMenu
			//Fonction permettant d'iniitaliser le menu que nous verrons apparaitre lors d'un clic droit de menu
			private void InitialiseMenu()
			{
				MenuItemTextEditor = new Gtk.MenuItem("test");
				MenuItemTextEditor.Submenu = new Gtk.Menu();
			
				foreach(MenuTextEditor mte in datamanagement.ListMenuTextEditor)
				{
					if(mte.MenuTextEditor_Column == 1)
					{
						if(mte.MenuTextEditor_Name != "SeparatorMenuItem")
						{								
							Gtk.MenuItem SubMenuLevel1 = new Gtk.MenuItem(mte.MenuTextEditor_Name);	
							((Gtk.Menu)MenuItemTextEditor.Submenu).Add(SubMenuLevel1);	
							if(mte.MenuTextEditor_ToolTip != "")
							{
							SubMenuLevel1.TooltipText = mte.MenuTextEditor_ToolTip;	
							}
						
							if(!mte.MenuTextEditor_Active)
							{
								SubMenuLevel1.Submenu = new Gtk.Menu();
							}
							else
							{
								var MTE_Texte = mte.MenuTextEditor_Text;
								SubMenuLevel1.Activated += delegate {WriteDataTest(MTE_Texte);};
							}						
						
							foreach(MenuTextEditor mte_1 in datamanagement.ListMenuTextEditor)
							{	
								if(mte_1.MenuTextEditor_Column == 2 && mte_1.MenuTextEditor_IdParent == mte.MenuTextEditor_Id && !mte.MenuTextEditor_Active)
								{	
									if(mte_1.MenuTextEditor_Name != "SeparatorMenuItem")
									{								
										Gtk.MenuItem SubMenuLevel2 = new Gtk.MenuItem(mte_1.MenuTextEditor_Name);
										((Gtk.Menu)SubMenuLevel1.Submenu).Add(SubMenuLevel2);	
										if(mte_1.MenuTextEditor_ToolTip != "")
										{								
											SubMenuLevel2.TooltipText = mte_1.MenuTextEditor_ToolTip;
										}
									
										if(!mte_1.MenuTextEditor_Active)
										{
											SubMenuLevel2.Submenu = new Gtk.Menu();
										}	
										else
										{
											var MTE_Texte = mte_1.MenuTextEditor_Text;
											SubMenuLevel2.ButtonPressEvent += delegate {WriteDataTest(MTE_Texte);};
										}									
									
										foreach(MenuTextEditor mte_2 in datamanagement.ListMenuTextEditor)
										{
											if(mte_2.MenuTextEditor_Column == 3 && mte_2.MenuTextEditor_IdParent == mte_1.MenuTextEditor_Id && !mte_1.MenuTextEditor_Active)
											{
												if(mte_2.MenuTextEditor_Name != "SeparatorMenuItem")
												{
													Gtk.MenuItem SubMenuLevel3 = new Gtk.MenuItem(mte_2.MenuTextEditor_Name);
													if(mte_2.MenuTextEditor_ToolTip != "")
													{
													SubMenuLevel3.TooltipText = mte_2.MenuTextEditor_ToolTip;
													}
													((Gtk.Menu)SubMenuLevel2.Submenu).Add(SubMenuLevel3);	
															
													if(!mte_2.MenuTextEditor_Active)
													{
														SubMenuLevel3.Submenu = new Gtk.Menu();
													}	
													else
													{
														var MTE_Texte = mte_2.MenuTextEditor_Text;
														SubMenuLevel3.ButtonPressEvent += delegate {WriteDataTest(MTE_Texte);};
													}
												}
												else
												{
													Gtk.SeparatorMenuItem Separator = new Gtk.SeparatorMenuItem();
													((Gtk.Menu)SubMenuLevel2.Submenu).Add(Separator);
												}
											}
										}
									}
									else
									{
										Gtk.SeparatorMenuItem Separator = new Gtk.SeparatorMenuItem();
										((Gtk.Menu)SubMenuLevel1.Submenu).Add(Separator);
									}									
								}
							}
						}
						else
						{
							Gtk.SeparatorMenuItem Separator = new Gtk.SeparatorMenuItem();
							((Gtk.Menu)MenuItemTextEditor.Submenu).Add(Separator);
						}								
					}
				}			
			}
		
			//Fonction WriteDataTest
			//Fonction permettant d'écrire les datas que nous avons sélectionné
			public void WriteDataTest (string ReadValueType)
			{
				var data = TextEditor1.GetTextEditorData();	
	        	var curLine = TextEditor1.GetLine (data.Caret.Line);				
				PositionInTextView = curLine.Offset + data.Caret.Column - 1;
				PositionLineInTextView = data.Caret.Line;
			
				TextEditor1.Insert(PositionInTextView,ReadValueType);
			
				datamanagement.ModifyCustomer(TextEditor1.Document.Text,param.ParamI("MoCu_ChoiceData"),CustomerId);	
				foreach(Project pro in datamanagement.ListProject)
				{
					foreach(Node node in pro.ReturnListNode())
					{
						foreach(Customer cus in node.ReturnListCustomer())
						{
							if(cus.CustomerId == CustomerId)
							{
								TextEditor1.Text = cus.CustomerData;
							}
						}
					}
				}	
				
				TextEditor2.Document.Text = datamanagement.ReplaceData(TextEditor1.Document.Text,ProjectId,NodeId,CustomerId);	
				TextEditor1.SetSelectLines(PositionLineInTextView,PositionLineInTextView);	
			}
		
			//Fonction Search
			//Fonction permettant de faire une recherche de texte dans le texteeditor
		    Mono.TextEditor.SearchResult Search(string s, bool from_selection_start) {
				
		            // internal to handle next/next with more text
		            int offset;
		            Mono.TextEditor.SearchResult search_result = null;
		            TextEditor2.SearchPattern = s;
		            var selection_range = TextEditor2.SelectionRange;
		            if (selection_range != null) {
		                if (from_selection_start) {
		                    offset = selection_range.Offset;
		                } else {
		                    offset = selection_range.Offset + selection_range.Length;
		                }
		            } else {
		                offset = TextEditor1.Caret.Offset;
		            }
		            search_result = TextEditor2.SearchForward(offset);
		            if (search_result != null) {
		                offset = search_result.Offset;
		                int length = search_result.Length;
		                TextEditor2.Caret.Offset = offset + length;
		                TextEditor2.SetSelection(offset, offset + length);
		                TextEditor2.ScrollToCaret();
		            }
		            return search_result;
		        }		
		
        public void SelectLine(int lineno) {
            var data = TextEditor1.GetTextEditorData();
            var curLine = TextEditor1.GetLine (data.Caret.Line);
    	    TextEditor1.SetSelection(lineno, 1, lineno, System.Math.Min (curLine.EditableLength, System.Math.Max (0, curLine.Length)) + 1);
        }		
				
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Customer cus in node.ReturnListCustomer())
					{
						if(cus.CustomerId == CustomerId)
						{
							return true;
						}
					}
				}
			}
			return false;
		}	
	
		public void ReturnId()
		{
			foreach(Project Pro in datamanagement.ListProject)
			{
				foreach(Node nod in Pro.ReturnListNode())
				{
					foreach(Customer cus in nod.ReturnListCustomer())
					{
						if(cus.CustomerId == CustomerId)
						{
							ProjectId = Pro.Project_Id;
							NodeId = nod.Node_Id;
						}
					}
				}
			}   			
		}
				
	}
}