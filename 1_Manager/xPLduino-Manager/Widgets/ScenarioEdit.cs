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


namespace xPLduinoManager
{
	[System.ComponentModel.ToolboxItem(true)]
	public partial class ScenarioEdit : Gtk.Bin
	{
		public DataManagement datamanagement;
		public Int32 ScenarioId;
		public Int32 Project_Id;
		public Param param;
		public MainWindow mainwindow;
		public Preference pref;
		
		public bool KeyboardOK = true;
		
		public Mono.TextEditor.TextEditor TextEditorScenario;
		public Mono.TextEditor.TextEditor TextEditorFunction;
        public Mono.TextEditor.TextEditorOptions OptionsTextEditorScenario;	
		public Mono.TextEditor.TextEditorOptions OptionsTextEditorFunction;	
		
		//Variable TreeView
		public global::Gtk.TreeViewColumn VariableNameColumn;
		public global::Gtk.TreeViewColumn VariableTypeColumn;
		public global::Gtk.TreeViewColumn VariableInitalValueColumn;
		public global::Gtk.TreeViewColumn VariableNoteColumn;
		public global::Gtk.TreeViewColumn VariableIdColumn;
		public global::Gtk.TreeViewColumn VariableEmptyColumn;		
		
		public global::Gtk.CellRendererText VariableNameCell;
		public global::Gtk.CellRendererCombo VariableTypeCell;
		public global::Gtk.CellRendererText VariableInitalValueCell;
		public global::Gtk.CellRendererText VariableNoteCell;
		public global::Gtk.CellRendererText VariableIdCell;		
		
		public global::Gtk.TreeStore VariableListStore;	
		public List<string> ListTypeVariable;
		public Gtk.ListStore ListStoreTypeVariable;
		public Gtk.ListStore ListStoreTypeVariableFunction;
		
		public global::Gtk.TreeIter IterChild;
		public global::Gtk.TreeModel TreeModelChildTreeView;		
		
		public Gtk.MenuItem MenuItemVariable;
		public global::Gtk.TreeIter IterProject;
		public global::Gtk.TreeModel TreeModelVariableTreeView;	
		
		//Function Treeview
		public global::Gtk.TreeViewColumn FunctionNameColumn;
		public global::Gtk.TreeViewColumn FunctionTypeColumn;
		public global::Gtk.TreeViewColumn FunctionNoteColumn;
		public global::Gtk.TreeViewColumn FunctionIdColumn;
		public global::Gtk.TreeViewColumn FunctionEmptyColumn;		
		
		public global::Gtk.CellRendererText FunctionNameCell;
		public global::Gtk.CellRendererCombo FunctionTypeCell;
		public global::Gtk.CellRendererText FunctionNoteCell;
		public global::Gtk.CellRendererText FunctionIdCell;	
		
		public global::Gtk.TreeStore FunctionListStore;	
		
		//Function Treeview args
		public global::Gtk.TreeViewColumn FunctionArgsNumberColumn;
		public global::Gtk.TreeViewColumn FunctionArgsTypeColumn;
		public global::Gtk.TreeViewColumn FunctionArgsNameColumn;
		
		public global::Gtk.CellRendererText FunctionArgsNumberCell;
		public global::Gtk.CellRendererCombo FunctionArgsTypeCell;
		public global::Gtk.CellRendererText FunctionArgsNameCell;	
		
		public global::Gtk.TreeStore FunctionArgsListStore;	
		public string MemorisationIdFunction = "";
		
		public Gtk.MenuItem MenuItemTextEditorScenario;
		public Gtk.MenuItem MenuItemTextEditorFunction;
		
		public List<string> ValuesPreferenceAction;
		
		public ScenarioEdit (DataManagement _datamanagement, Param _param, Int32 _ScenarioId, MainWindow _mainwindow, Preference _pref)
		{
			this.Build ();
			this.ScenarioId = _ScenarioId;
			this.datamanagement = _datamanagement;
			this.param = _param;
			this.mainwindow = _mainwindow;
			this.pref = _pref;
			
			InitWidget();
	
			TextEditorScenario.FocusOutEvent += OnTextEditorScenarioFocusOutEvent;
			TextEditorScenario.ButtonReleaseEvent += OnTextEditorScenarioButtonReleaseEvent;
			TextEditorScenario.ButtonPressEvent += OnTextEditorScenarioButtonPressEvent;
			
			TextEditorFunction.FocusOutEvent += OnTextEditorFunctionFocusOutEvent;
			TextEditorFunction.ButtonReleaseEvent += OnTextEditorFunctionButtonReleaseEvent;
			
			//Permet de retourner l'id du projet
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == _ScenarioId)
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
			ListTypeVariable = new List<string>();
			ListTypeVariable.Add("void");
			ListTypeVariable.Add("bool");
			ListTypeVariable.Add("byte");
			ListTypeVariable.Add("uint8_t");
			ListTypeVariable.Add("uint16_t");
			
			ListStoreTypeVariable = new Gtk.ListStore(typeof (string));
			ListStoreTypeVariableFunction = new Gtk.ListStore(typeof (string));
			
			bool OneShoot = false;
			foreach(string Typevar in ListTypeVariable)
			{
				if(OneShoot)
				{
					ListStoreTypeVariable.AppendValues(Typevar);
				}
				OneShoot = true;
				
				ListStoreTypeVariableFunction.AppendValues(Typevar);
			}
		
			hpaned2.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
			hpaned3.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
			hpaned4.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 4 ;
			InitTextEditorScenario("text/x-csharp");
			InitTextEditorFunction("text/x-csharp");	
			TextEditorFunction.Sensitive = false;
			InitVariableTreeview();
			UpdateVariableTreeview();
			InitFunctionTreeview();
			UpdateFunctionTreeview();
			InitFunctionArgumentTreeview();
			
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							datamanagement.AddElementInListMenuTextEditorScenario(node.Node_Id,ScenarioId);
						}
					}
				}
			}			
			
			InitialiseMenuScenario();
			
			ValuesPreferenceAction = new List<string>(){pref.LIGToggleActionName, 		
											 		    pref.LIGTuneActionName, 
											 		    pref.LIGStopActionName,
												 	    pref.SWIClicActionName,
											 		    pref.SWIDoubleClicActionName,
											 		    pref.SWIOnActionName,
											 		    pref.SWIOnFmActionName,
											 		    pref.SWIOffActionName,
											 		    pref.SWIOffFmActionName,
											 		    pref.SHUOpenActionName,
											 		    pref.SHUCloseActionName,
											 		    pref.SHUStopActionName
														};
			
			UpdateVpaned(70);
		}
			
		//Fonction UpdateHpaned
		//Fonction permettant de mettre à jour les Hpaned
		public void UpdateHpaned()
		{
			hpaned2.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
			hpaned3.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 2 ;
			hpaned4.Position = datamanagement.mainwindow.ReturnHpanedPosition() / 4 ;
			
		}
		
		//Fonction UpdateVpaned
		//Fonction peremettant de mettre à jour vpaned3
		public void UpdateVpaned(int _ValueVpaned)
		{
			vpaned3.Position = datamanagement.mainwindow.ReturnVpanedPosition() * _ValueVpaned / 100 ;
		}
		
		//Fonction UpdateWidget
		//Fonction permettant de mettre à jour le widget	
		public void UpdateWidget()
		{	
			ValuesPreferenceAction.Clear();
			ValuesPreferenceAction = new List<string>(){pref.LIGToggleActionName, 		
											 		    pref.LIGTuneActionName, 
											 		    pref.LIGStopActionName,
														pref.LIGSetActionName,
												 	    pref.SWIClicActionName,
											 		    pref.SWIDoubleClicActionName,
											 		    pref.SWIOnActionName,
											 		    pref.SWIOnFmActionName,
											 		    pref.SWIOffActionName,
											 		    pref.SWIOffFmActionName,
											 		    pref.SHUOpenActionName,
											 		    pref.SHUCloseActionName,
											 		    pref.SHUStopActionName,
														pref.SHUToggleActionName,
														pref.TEMPGetValue,
														};			
			
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							TextEditorScenario.Text = sce.ScenarioData;
							datamanagement.AddElementInListMenuTextEditorScenario(node.Node_Id,ScenarioId);
							if(MemorisationIdFunction != "")
							{
								foreach(Function fun in sce.ReturnListFunction())
								{
									if(fun.FunctionId == Convert.ToInt32(MemorisationIdFunction))
									{
										datamanagement.AddElementInListMenuTextEditorFunction(node.Node_Id,ScenarioId,fun.FunctionId);
										InitialiseMenuFunction();
									}
								}
							}
						}
					}
				}
			}
			UpdateVariableTreeview();
			UpdateFunctionTreeview();
			UpdateFunctionArgumentTreeview();
			

			
			InitialiseMenuScenario();
			InitialiseMenuFunction();
		}	
		
//##################### EDITEUR DE TEXTE #############################		
		
		//Fonction InitTextEditorScenario
		//Fonction permettant d'initialiser l'éditeur de texte de gauche
		public void InitTextEditorScenario(string mimetype)
		{
			TextEditorScenario = new MyTextEditor();
			OptionsTextEditorScenario = new Mono.TextEditor.TextEditorOptions();
			
            if (mimetype != null)
                TextEditorScenario.Document.MimeType = mimetype;
           
			ScrolledWindowScenario.Add(TextEditorScenario);
			
			OptionsTextEditorScenario.ShowInvalidLines = false;
            OptionsTextEditorScenario.ShowLineNumberMargin = true;
            OptionsTextEditorScenario.TabsToSpaces = true;
            OptionsTextEditorScenario.HighlightCaretLine = true;
            OptionsTextEditorScenario.HighlightMatchingBracket = true;
            OptionsTextEditorScenario.OverrideDocumentEolMarker = true;
            OptionsTextEditorScenario.DefaultEolMarker = "\n";
            OptionsTextEditorScenario.ShowIconMargin = false;
            OptionsTextEditorScenario.ShowFoldMargin = false;			
			
			TextEditorScenario.Options = OptionsTextEditorScenario;
			
			foreach(Project pro in datamanagement.ListProject.Values)
			{
				foreach(Node node in pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							TextEditorScenario.Text = sce.ScenarioData;
						}
					}
				}
			}	
		}
		
		//Fonction InitTextEditorFunction
		//Fonction permettant d'initialiser l'éditeur de texte de droite		
		public void InitTextEditorFunction(string mimetype)
		{
			TextEditorFunction = new MyTextEditor();
			OptionsTextEditorFunction = new Mono.TextEditor.TextEditorOptions();
			
            if (mimetype != null)
                TextEditorFunction.Document.MimeType = mimetype;
           
			ScrolledWindowFonction.Add(TextEditorFunction);
			
			OptionsTextEditorFunction.ShowInvalidLines = false;
            OptionsTextEditorFunction.ShowLineNumberMargin = true;
            OptionsTextEditorFunction.TabsToSpaces = true;
            OptionsTextEditorFunction.HighlightCaretLine = true;
            OptionsTextEditorFunction.HighlightMatchingBracket = true;
            OptionsTextEditorFunction.OverrideDocumentEolMarker = true;
            OptionsTextEditorFunction.DefaultEolMarker = "\n";
            OptionsTextEditorFunction.ShowIconMargin = false;
            OptionsTextEditorFunction.ShowFoldMargin = false;	
			
			TextEditorFunction.Options = OptionsTextEditorFunction;
			TextEditorFunction.Sensitive = true;	
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
		
			//Fonction OnTextEditorScenarioFocusOutEvent
			//Fonction permettant faire des actions lors de la perte du focus du TextEditorScenario
			[GLib.ConnectBefore]
			private void OnTextEditorScenarioFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{						
				SaveCurrentDataScenario();
			}
		
				//Fonction SaveCurrentDataScenario
				//Fonction permettant de faire une sauvegarde du travail en cours lors d'un enregistrement
				public void SaveCurrentDataScenario()
				{
					if(TextEditorScenario.Length < 2)
					{
						TextEditorScenario.Text = "//\n"+ TextEditorScenario.Text;
					}
					
					if(TextEditorScenario.Text[0].ToString() != "/" && TextEditorScenario.Text[1].ToString() != "/")
					{
						TextEditorScenario.Text = "//\n"+ TextEditorScenario.Text;
					}			
					
					if(TextEditorScenario.Text[TextEditorScenario.Length-1].ToString() != "\n")
					{
						TextEditorScenario.Text = TextEditorScenario.Text + "\n";
					}	
	
					foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
					{
						foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
						{
							foreach(Scenario sce in node.ReturnListScenario())
							{
								if(sce.ScenarioId == ScenarioId)
								{
									foreach(Variable vari in sce.ReturnListVariable())
									{
										AddSpace(TextEditorScenario,vari.VariableName);
									}
									foreach(Function fun in sce.ReturnListFunction())
									{
										AddSpace(TextEditorScenario,fun.FunctionName);
									}							
								}
							}
							foreach(Instance ins in node.ReturnListInstance())
							{
								AddSpace(TextEditorScenario,ins.Instance_Name);
							}
						
							foreach(Network net in node.ReturnListNetwork())
							{
								foreach(Board boa in net.ReturnListBoard())
								{
									AddSpace(TextEditorScenario,boa.Board_Name);
								}
							}
						}
					}		
					foreach(string VPA in ValuesPreferenceAction)
					{
						AddSpace(TextEditorScenario,VPA);
					}
					datamanagement.ModifyScenario(TextEditorScenario.Text,param.ParamI("MoSc_ChoiceData"),ScenarioId);			
				}		
		
			//Fonction OnTextEditorScenarioButtonReleaseEvent
			//Fonction permettant de détecter un clic de souris sur le TextEditorScenario
			[GLib.ConnectBefore]
			private void OnTextEditorScenarioButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
			{
				if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
				{				
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",true);
			
					if(TextEditorScenario.SelectedText != null)
					{
						foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
						{
							foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
							{
								foreach(Scenario sce in node.ReturnListScenario())
								{
									if(sce.ScenarioId == ScenarioId)
									{
										foreach(Function fun in sce.ReturnListFunction())
										{
											if(TextEditorScenario.SelectedText == fun.FunctionName)
											{
												MemorisationIdFunction = fun.FunctionId.ToString();
												UpdateFunctionArgumentTreeview();	
											
												string Args1 = "";
												string Args2 = "";
												string Args3 = "";
												string Args4 = "";
												string Args5 = "";
												string Args6 = "";
											
												if(fun.FunctionNameArg1 != "")
												{
													Args1 = fun.FunctionTypeArg1;
												}
												if(fun.FunctionNameArg2 != "")
												{
													Args2 = "," + fun.FunctionTypeArg2;
												}
												if(fun.FunctionNameArg3 != "")
												{
													Args3 = "," + fun.FunctionTypeArg3;
												}
												if(fun.FunctionNameArg4 != "")
												{
													Args4 = "," + fun.FunctionTypeArg4;
												}
												if(fun.FunctionNameArg5 != "")
												{
													Args5 = "," + fun.FunctionTypeArg5;
												}
												if(fun.FunctionNameArg6 != "")
												{
													Args6 = "," + fun.FunctionTypeArg6;
												}													
											
												TextEditorScenario.TooltipText = fun.FunctionName + "(" + Args1 + Args2 + Args3 + Args4 + Args5 + Args6 + ")";
											}
										}
									
										foreach(Variable vari in sce.ReturnListVariable())
										{
											if(TextEditorScenario.SelectedText == vari.VariableName)
											{
												TextEditorScenario.TooltipText = vari.VariableType + " = " + vari.VariableDefaultValue;
											}
										}
									}
								}
							}
						}						
					}
				}
				else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
				{	
					MenuItemTextEditorScenario.Submenu.ShowAll();
					((Gtk.Menu)MenuItemTextEditorScenario.Submenu).Popup();
				}				
			}
		
				//Fonction InitialiseMenuScenario
				//Fonction permettant d'iniitaliser le menu que nous verrons apparaitre lors d'un clic droit de menu
				private void InitialiseMenuScenario()
				{
					MenuItemTextEditorScenario = new Gtk.MenuItem("test");
					MenuItemTextEditorScenario.Submenu = new Gtk.Menu();
				
					foreach(MenuTextEditorScenario mte in datamanagement.ListMenuTextEditorScenario)
					{
						if(mte.MenuTextEditor_Column == 1)
						{
							if(mte.MenuTextEditor_Name != "SeparatorMenuItem")
							{								
								Gtk.MenuItem SubMenuLevel1 = new Gtk.MenuItem(mte.MenuTextEditor_Name);	
								((Gtk.Menu)MenuItemTextEditorScenario.Submenu).Add(SubMenuLevel1);	
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
									SubMenuLevel1.Activated += delegate {WriteDataTestScenario(MTE_Texte);};
								}						
							
								foreach(MenuTextEditorScenario mte_1 in datamanagement.ListMenuTextEditorScenario)
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
												SubMenuLevel2.ButtonPressEvent += delegate {WriteDataTestScenario(MTE_Texte);};
											}									
										
											foreach(MenuTextEditorScenario mte_2 in datamanagement.ListMenuTextEditorScenario)
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
															SubMenuLevel3.ButtonPressEvent += delegate {WriteDataTestScenario(MTE_Texte);};
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
								((Gtk.Menu)MenuItemTextEditorScenario.Submenu).Add(Separator);
							}								
						}
					}			
				}
			
				//Fonction WriteDataTestScenario
				//Fonction permettant d'écrire les datas que nous avons sélectionné
				public void WriteDataTestScenario (string ReadValueType)
				{
					int PositionInTextView;
					var data = TextEditorScenario.GetTextEditorData();	
		        	var curLine = TextEditorScenario.GetLine (data.Caret.Line);				
					PositionInTextView = curLine.Offset + data.Caret.Column - 1;
					TextEditorScenario.Insert(PositionInTextView,ReadValueType);
				}
		
			//Fonction OnTextEditorScenarioButtonPressEvent
			//Fonction permettant de détecter un clic de sourie sur le texteditor
			[GLib.ConnectBefore]
			private void OnTextEditorScenarioButtonPressEvent (object o, Gtk.ButtonPressEventArgs args)
			{

			}
		
	
		
			//Fonction OnTextEditorFunctionFocusOutEvent
			//Fonction permettant faire des actions lors de la perte du focus du TextEditorFunction
			[GLib.ConnectBefore]
			private void OnTextEditorFunctionFocusOutEvent (object o, Gtk.FocusOutEventArgs args)
			{
				SaveCurrentDataFunction();
			}		
		
				//Fonction SaveCurrentDataFunction
				//Fonction permettant de faire une sauvegarde du travail en cours lors d'un enregistrement
				public void SaveCurrentDataFunction()
				{	
					if(MemorisationIdFunction != "")
					{
						if(TextEditorFunction.Length == 0)
						{
							TextEditorFunction.Text = "//\n"+ TextEditorFunction.Text;
						}
						
						if(TextEditorFunction.Text[0].ToString() != "/" && TextEditorFunction.Text[1].ToString() != "/")
						{
							TextEditorFunction.Text = "//\n"+ TextEditorFunction.Text;
						}			
						
						if(TextEditorFunction.Text[TextEditorFunction.Length-1].ToString() != "\n")
						{
							TextEditorFunction.Text = TextEditorFunction.Text + "\n";
						}	
		
						foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
						{
							foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
							{
								foreach(Scenario sce in node.ReturnListScenario())
								{
									if(sce.ScenarioId == ScenarioId)
									{
										foreach(Variable vari in sce.ReturnListVariable())
										{
											AddSpace(TextEditorFunction, vari.VariableName);
										}			
									
										foreach(Function fun in sce.ReturnListFunction())
										{				
											if(fun.FunctionId == Convert.ToInt32(MemorisationIdFunction))
											{
												if(fun.FunctionNameArg1 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg1);  
												}
												if(fun.FunctionNameArg2 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg2);  
												}
												if(fun.FunctionNameArg3 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg3);  
												}
												if(fun.FunctionNameArg4 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg4);  
												}
												if(fun.FunctionNameArg5 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg5);  
												}
												if(fun.FunctionNameArg6 != "")
												{
													AddSpace(TextEditorFunction, fun.FunctionNameArg6);  
												}									
											}
										}
									}
								}
								foreach(Instance ins in node.ReturnListInstance())
								{
									AddSpace(TextEditorFunction,ins.Instance_Name);
								}	
								foreach(Network net in node.ReturnListNetwork())
								{
									foreach(Board boa in net.ReturnListBoard())
									{
										AddSpace(TextEditorScenario,boa.Board_Name);
									}
								}						
							}
						}	
						foreach(string VPA in ValuesPreferenceAction)
						{
							AddSpace(TextEditorFunction,VPA);
						}			
						datamanagement.ModifyFunction(TextEditorFunction.Text,param.ParamI("MoFu_ChoiceData"),Convert.ToInt32(MemorisationIdFunction));	
					}
				}
		
			//Fonction OnTextEditorFunctionButtonReleaseEvent
			//Fonction permettant de détecter un clic de souris sur le TextEditorScenario
			[GLib.ConnectBefore]
			private void OnTextEditorFunctionButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
			{
				if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
				{				
					mainwindow.UndoRedoInactif("undo",true);
					mainwindow.UndoRedoInactif("redo",true);
				}
				else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
				{	
					MenuItemTextEditorFunction.Submenu.ShowAll();
					((Gtk.Menu)MenuItemTextEditorFunction.Submenu).Popup();
				}				
			}
		
				//Fonction InitialiseMenuFunction
				//Fonction permettant d'iniitaliser le menu que nous verrons apparaitre lors d'un clic droit de menu
				private void InitialiseMenuFunction()
				{
					MenuItemTextEditorFunction = new Gtk.MenuItem("test");
					MenuItemTextEditorFunction.Submenu = new Gtk.Menu();
				
					foreach(MenuTextEditorFunction mte in datamanagement.ListMenuTextEditorFunction)
					{
						if(mte.MenuTextEditor_Column == 1)
						{
							if(mte.MenuTextEditor_Name != "SeparatorMenuItem")
							{								
								Gtk.MenuItem SubMenuLevel1 = new Gtk.MenuItem(mte.MenuTextEditor_Name);	
								((Gtk.Menu)MenuItemTextEditorFunction.Submenu).Add(SubMenuLevel1);	
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
									SubMenuLevel1.Activated += delegate {WriteDataTestFunction(MTE_Texte);};
								}						
							
								foreach(MenuTextEditorFunction mte_1 in datamanagement.ListMenuTextEditorFunction)
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
												SubMenuLevel2.ButtonPressEvent += delegate {WriteDataTestFunction(MTE_Texte);};
											}									
										
											foreach(MenuTextEditorFunction mte_2 in datamanagement.ListMenuTextEditorFunction)
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
															SubMenuLevel3.ButtonPressEvent += delegate {WriteDataTestFunction(MTE_Texte);};
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
								((Gtk.Menu)MenuItemTextEditorFunction.Submenu).Add(Separator);
							}								
						}
					}			
				}		
		
				//Fonction WriteDataTestFunction
				//Fonction permettant d'écrire les datas que nous avons sélectionné
				public void WriteDataTestFunction (string ReadValueType)
				{
					int PositionInTextView;
					var data = TextEditorFunction.GetTextEditorData();	
		        	var curLine = TextEditorFunction.GetLine (data.Caret.Line);				
					PositionInTextView = curLine.Offset + data.Caret.Column - 1;
					TextEditorFunction.Insert(PositionInTextView,ReadValueType);
				}				
		
			//Fonction AddSpace
			//Fonction permettant d'ajouter un espace avant et apres un nom de variable
			public void AddSpace(Mono.TextEditor.TextEditor te, string _Name)
			{
			
				int MemoryOffset = 0;
				bool Flag = false;
				int LastLetter;	
				int FirstLetter;
			
				te.SearchPattern = _Name;
			
				Mono.TextEditor.SearchResult search_result = null;
				search_result = te.SearchForward(MemoryOffset);
				if(search_result != null)
				{
					while(!Flag)
					{
						if(search_result != null)
						{				
							if(search_result.EndOffset > MemoryOffset)
							{
								LastLetter = te.Text[search_result.EndOffset];
								FirstLetter = te.Text[search_result.Offset-1];
								
								if(LastLetter < 32 || (LastLetter > 32 && LastLetter < 48) || (LastLetter > 57 && LastLetter < 65) || (LastLetter > 90 && LastLetter < 95) || (LastLetter > 95 && LastLetter < 97) || LastLetter > 122)
								{
									te.Text = te.Text.Insert (search_result.EndOffset," ");
								}	
							
								if(FirstLetter < 32 || (FirstLetter > 32 && FirstLetter < 48) || (FirstLetter > 57 && FirstLetter < 65) || (FirstLetter > 90 && FirstLetter < 95) || (FirstLetter > 95 && FirstLetter < 97) || FirstLetter > 122)
								{
									te.Text = te.Text.Insert (search_result.Offset," ");
								}													
							
								MemoryOffset = search_result.EndOffset + 1;
							}
							else
							{
								Flag = true;
							}
							search_result = te.SearchForward(MemoryOffset);	
						}
						else
						{
							Flag = true;
						}
					}
				}			
			}
		
//##################### VARIABLE TREEVIEW #######################################		
		
		//Fonction InitVariableTreeview
		//Fonction permettant d'initialier l'arbre des varaibles
		public void InitVariableTreeview()
		{
			TreeviewVariable.EnableGridLines = TreeViewGridLines.Both;
			TreeviewVariable.TooltipText = param.ParamT("SEW_TooltipVariableTreeView");
			
			//Ajout des colonnes dans le TreeviewVariable
			VariableNameColumn = new Gtk.TreeViewColumn();
			VariableTypeColumn = new Gtk.TreeViewColumn();
			VariableInitalValueColumn = new Gtk.TreeViewColumn();
			VariableNoteColumn = new Gtk.TreeViewColumn();
			VariableIdColumn = new Gtk.TreeViewColumn();
			VariableEmptyColumn = new Gtk.TreeViewColumn();		
			
			//Visibilité des colonnes
			VariableIdColumn.Visible = false;
			
			//Nous donnons des titre au colonnes
			VariableNameColumn.Title = param.ParamT("SEW_TVVariable_Name");
			VariableTypeColumn.Title = param.ParamT("SEW_TVVariable_Type");
			VariableInitalValueColumn.Title = param.ParamT("SEW_TVVariable_DefaultValue");
			VariableNoteColumn.Title = param.ParamT("SEW_TVVariable_Note");
			
			//On redimensionne les colonnes
			VariableNameColumn.Resizable = true;
			VariableTypeColumn.Resizable = true;
			VariableInitalValueColumn.Resizable = true;
			VariableNoteColumn.Resizable = true;		
				
			//Création des cellules
			VariableNameCell = new Gtk.CellRendererText ();
			VariableTypeCell = new Gtk.CellRendererCombo ();
			VariableInitalValueCell = new Gtk.CellRendererText ();
			VariableNoteCell = new Gtk.CellRendererText ();
			VariableIdCell = new Gtk.CellRendererText ();				
	
			VariableTypeCell.Model = ListStoreTypeVariable;
			VariableTypeCell.TextColumn = 0;
			
			//On rend éditable les cellules
			VariableNameCell.Editable = true;
			VariableTypeCell.Editable = true;
			VariableInitalValueCell.Editable = true;
			VariableNoteCell.Editable = true;
			
			VariableNameCell.Edited += VariableNameCell_Edited;
			VariableTypeCell.Edited += VariableTypeCell_Edited;
			VariableInitalValueCell.Edited += VariableInitalValueCell_Edited;
			VariableNoteCell.Edited += VariableNoteCell_Edited;			
			
			//On associe des cellule au colonne
			VariableNameColumn.PackStart(VariableNameCell, true);
			VariableTypeColumn.PackStart (VariableTypeCell, true);
			VariableInitalValueColumn.PackStart(VariableInitalValueCell, true);
			VariableNoteColumn.PackStart (VariableNoteCell, true);	
			VariableIdColumn.PackStart (VariableIdCell, true);
			
			//Ajout des colonnes dans TreeviewVariable
			TreeviewVariable.AppendColumn (VariableNameColumn);
			TreeviewVariable.AppendColumn (VariableTypeColumn);
			TreeviewVariable.AppendColumn (VariableInitalValueColumn);			
			TreeviewVariable.AppendColumn (VariableNoteColumn);
			TreeviewVariable.AppendColumn (VariableIdColumn);	
			TreeviewVariable.AppendColumn (VariableEmptyColumn);
			
			//Ajout des attibuts à chaque colonne
			VariableNameColumn.AddAttribute(VariableNameCell,"text",param.ParamI("SEW_TVVariable_PositionName"));
			VariableTypeColumn.AddAttribute (VariableTypeCell, "text", param.ParamI("SEW_TVVariable_PositionType"));		
			VariableInitalValueColumn.AddAttribute(VariableInitalValueCell,"text",param.ParamI("SEW_TVVariable_PositionDefaultValue"));
			VariableNoteColumn.AddAttribute (VariableNoteCell, "text", param.ParamI("SEW_TVVariable_PositionNote"));	
			VariableIdColumn.AddAttribute (VariableIdCell, "text", param.ParamI("SEW_TVVariable_PositionId"));	
		
			//Création d'un nouveau store, param : texte Type, texte Value
			VariableListStore = new Gtk.TreeStore(typeof (string),typeof (string), typeof (string), typeof (string), typeof (string));
			TreeviewVariable.Model = VariableListStore;
			TreeviewVariable.ShowAll();
		}
		
		//Fonction UpdateVariableTreeview
		//Fonction permettant de mettre à jour le treeview
		public void UpdateVariableTreeview()
		{
			VariableListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							foreach(Variable vari in sce.ReturnListVariable())
							{
								VariableListStore.AppendValues(vari.VariableName,vari.VariableType,vari.VariableDefaultValue.ToString(),vari.VariableNote,vari.VariableId.ToString());
							}
						}
					}
				}
			}
		}
		
			//Fonction VariableNameCell_Edited
			//Fonction permettant de modifier un nom de variable
			void VariableNameCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	
			
				TreeSelection selection = TreeviewVariable.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionId")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionName")); //Nous retournons l'id du noeud selectionné
				}	
			
				string NewText = datamanagement.ReturnCorrectName(args.NewText);
			
				if(NewText != NameSelected)
				{
					if(NewText == "")
					{
						mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameVariableEmpty");
						return;				
					}
				
				foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
					{
						foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
						{
							foreach(Scenario sce in node.ReturnListScenario())
							{
								if(sce.ScenarioId == ScenarioId)
								{
									foreach(Variable vari in sce.ReturnListVariable())
									{
										if(vari.VariableName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameVariableExist");
											return;
										}
									}
									foreach(Function fun in sce.ReturnListFunction())
									{
										if(fun.FunctionName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameVariableExistInFunction");
											return;										
										}
									}
								}
							}
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(NewText == ins.Instance_Name)
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInInstance");
									return;								
								}
							}
						}
					}
				
					for(int i=0;i<ValuesPreferenceAction.Count;i++)
					{
						if(NewText == ValuesPreferenceAction[i])
						{
							mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInActionName");
							return;
						}
					}
				
					if(IdSelected != "")
					{
						datamanagement.ModifyVariable(NewText,param.ParamI("MoVa_ChoiceName"),Convert.ToInt32(IdSelected));
						UpdateVariableTreeview();						
					}
				}
				return;
			}
		
			//Fonction VariableTypeCell_Edited
			//Fonction permettant la modification du type de variable
			void VariableTypeCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				
				TreeSelection selection = TreeviewVariable.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionId")); //Nous retournons l'id du noeud selectionné
				}	
				
				if(IdSelected != "")
				{
					datamanagement.ModifyVariable(args.NewText,param.ParamI("MoVa_ChoiceType"),Convert.ToInt32(IdSelected));
					UpdateVariableTreeview();	
				}
			}
			
			//Fonction VariableInitalValueCell_Edited
			//Fonction permettant de modifier la valeur initial
			void VariableInitalValueCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				
				TreeSelection selection = TreeviewVariable.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionId")); //Nous retournons l'id du noeud selectionné
				}	
				
				if(IdSelected != "")
				{
					datamanagement.ModifyVariable(args.NewText,param.ParamI("MoVa_ChoiceValue"),Convert.ToInt32(IdSelected));
					UpdateVariableTreeview();		
				}
			}
		
			//Fonction VariableNoteCell_Edited
			//Fonctione permettant de modifier les commentaires d'une variables
			void VariableNoteCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NoteSelected = "";	
			
				TreeSelection selection = TreeviewVariable.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionId")); //Nous retournons l'id du noeud selectionné
					NoteSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVVariable_PositionNote"));
				}	
				
				if(args.NewText != NoteSelected && IdSelected != "")
				{
					datamanagement.ModifyVariable(args.NewText,param.ParamI("MoVa_ChoiceNote"),Convert.ToInt32(IdSelected));
					UpdateVariableTreeview();					
				}
			}
		
		//Fonction OnTreeviewVariableButtonReleaseEvent
		//Fonction permettant de détecter un clic de souris
		protected void OnTreeviewVariableButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
			{			
				
			}
			else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
			{	
				string IdVariable = "0";
				string NameVariable = "";
					
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelVariableTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdVariable = (string) TreeModelVariableTreeView.GetValue (IterProject, param.ParamI("SEW_TVVariable_PositionId")); //Nous mettons la valeur de la 1ere cellule dans un string (Texte)
					NameVariable = (string) TreeModelVariableTreeView.GetValue (IterProject, param.ParamI("SEW_TVVariable_PositionName"));
				}
			
			
				MenuItemVariable = new Gtk.MenuItem("test");
				MenuItemVariable.Submenu = new Gtk.Menu();
			
				if(Convert.ToInt32(IdVariable) != 0)
				{
					Gtk.MenuItem MenuDelete = new Gtk.MenuItem(param.ParamT("SEW_DeleteVariable"));
					((Gtk.Menu)MenuItemVariable.Submenu).Add(MenuDelete);
					MenuDelete.Activated += delegate {DeleteVariable(NameVariable,Convert.ToInt32(IdVariable));};
					
					Gtk.SeparatorMenuItem Separator = new Gtk.SeparatorMenuItem();
					((Gtk.Menu)MenuItemVariable.Submenu).Add(Separator);
				}
			
				Gtk.MenuItem SubMenuLevel1 = new Gtk.MenuItem(param.ParamT("SEW_MenuNewVariable"));
				((Gtk.Menu)MenuItemVariable.Submenu).Add(SubMenuLevel1);	
				SubMenuLevel1.Submenu = new Gtk.Menu();
			
				bool OneShoot = false;
				foreach(string Typevar in ListTypeVariable)
				{
					if(OneShoot)
					{
						var TypevarText = Typevar;
						Gtk.MenuItem SubMenuLevel2 = new Gtk.MenuItem(Typevar.Replace("_","-"));		
						SubMenuLevel2.ButtonPressEvent += delegate {AddNewVariable(TypevarText);};
						((Gtk.Menu)SubMenuLevel1.Submenu).Add(SubMenuLevel2);
					}
					OneShoot = true;
				}
			
				MenuItemVariable.Submenu.ShowAll();
				((Gtk.Menu)MenuItemVariable.Submenu).Popup();					
			}			
		}		
			
			//Fonction AddNewVariable
			//Fonction permettant d'ajouter une variable à la suite d'un clic droit
			public void AddNewVariable(string TypeVar)
			{
				datamanagement.AddVariableInScenario(datamanagement.ReturnNewNameVariable(param.ParamT("SEW_DefaultNameVariable"),ScenarioId),TypeVar,ScenarioId);
			}
			
			//Fonction DeleteVariable
			//Fonction permettant d'ouvrir un popup pour supprimer une variable
			public void DeleteVariable (string NameVar, Int32 IdVar)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)	
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (mainwindow, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteVariable") + NameVar + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteVariableInScenario(IdVar);
					UpdateVariableTreeview(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue	
					TextEditorScenario.Text = TextEditorScenario.Text.Replace(" " + NameVar + " ", " " + param.ParamT("SEW_VariableEmpty") + " ");
					datamanagement.ModifyScenario(TextEditorScenario.Text,param.ParamI("MoSc_ChoiceData"),ScenarioId);				
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal				
			}
		       		
//####################### FUNCTION TREEVIEW #####################################
		
		//Fonction InitFunctionTreeview
		//Fonction permettant d'initialier l'arbre des fonctions
		public void InitFunctionTreeview()
		{
			TreeviewFunction.EnableGridLines = TreeViewGridLines.Both;
			TreeviewFunction.TooltipText = param.ParamT("SEW_TooltipFunctionTreeView");
			
			//Ajout des colonnes dans le TreeviewVariable
			FunctionNameColumn = new Gtk.TreeViewColumn();
			FunctionTypeColumn = new Gtk.TreeViewColumn();
			FunctionNoteColumn = new Gtk.TreeViewColumn();
			FunctionIdColumn = new Gtk.TreeViewColumn();
			FunctionEmptyColumn = new Gtk.TreeViewColumn();
			
			//Visibilité des colonnes
			FunctionIdColumn.Visible = false;
			
			//Nous donnons des titre au colonnes
			FunctionNameColumn.Title = param.ParamT("SEW_TVFunction_Name");
			FunctionTypeColumn.Title = param.ParamT("SEW_TVFunction_Type");
			FunctionNoteColumn.Title = param.ParamT("SEW_TVFunction_Note");		
		
			//On redimensionne les colonnes
			FunctionNameColumn.Resizable = true;
			FunctionTypeColumn.Resizable = true;
			FunctionNoteColumn.Resizable = true;
			
			//Création des cellules
			FunctionNameCell = new Gtk.CellRendererText ();
			FunctionTypeCell = new Gtk.CellRendererCombo ();
			FunctionNoteCell = new Gtk.CellRendererText ();
			FunctionIdCell = new Gtk.CellRendererText ();
			
			FunctionTypeCell.Model = ListStoreTypeVariableFunction;
			FunctionTypeCell.TextColumn = 0;	
			
			//On rend éditable les cellules
			FunctionNameCell.Editable = true;
			FunctionTypeCell.Editable = true;
			FunctionNoteCell.Editable = true;
			FunctionIdCell.Editable = true;	
			
			FunctionNameCell.Edited += FunctionNameCell_Edited;
			FunctionTypeCell.Edited += FunctionTypeCell_Edited;
			FunctionNoteCell.Edited += FunctionNoteCell_Edited;
			
			//On associe des cellule au colonne
			FunctionNameColumn.PackStart(FunctionNameCell, true);
			FunctionTypeColumn.PackStart (FunctionTypeCell, true);
			FunctionNoteColumn.PackStart(FunctionNoteCell, true);
			FunctionIdColumn.PackStart (FunctionIdCell, true);			
				
			//Ajout des colonnes dans TreeviewVariable
			TreeviewFunction.AppendColumn (FunctionNameColumn);
			TreeviewFunction.AppendColumn (FunctionTypeColumn);	
			TreeviewFunction.AppendColumn (FunctionNoteColumn);
			TreeviewFunction.AppendColumn (FunctionIdColumn);	
			TreeviewFunction.AppendColumn (FunctionEmptyColumn);		
			
			//Ajout des attibuts à chaque colonne
			FunctionNameColumn.AddAttribute(FunctionNameCell,"text",param.ParamI("SEW_TVFunction_PositionName"));
			FunctionTypeColumn.AddAttribute (FunctionTypeCell, "text", param.ParamI("SEW_TVFunction_PositionType"));		
			FunctionNoteColumn.AddAttribute(FunctionNoteCell,"text",param.ParamI("SEW_TVFunction_PositionNote"));
			FunctionIdColumn.AddAttribute (FunctionIdCell, "text", param.ParamI("SEW_TVFunction_PositionId"));			
			
			//Création d'un nouveau store, param : texte Type, texte Value
			FunctionListStore = new Gtk.TreeStore(typeof (string),typeof (string), typeof (string), typeof (string));
			TreeviewFunction.Model = FunctionListStore;
			TreeviewFunction.ShowAll();			
		}
		
		//Fonction UpdateFunctionTreeview
		//Fonction permettant de mettre à jour le treeview des fonctions
		public void UpdateFunctionTreeview()
		{
			FunctionListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							foreach(Function fun in sce.ReturnListFunction())
							{
								FunctionListStore.AppendValues(fun.FunctionName,fun.FunctionTypeReturn,fun.FunctionNote,fun.FunctionId.ToString());
							}
						}
					}
				}
			}			
		}
		
			//Fonction FunctionNameCell_Edited
			//Fonction permettant de modifier un nom de variable
			void FunctionNameCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NameSelected = "";	
			
				TreeSelection selection = TreeviewFunction.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionId")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionName")); //Nous retournons l'id du noeud selectionné
				}
			
				string NewText = datamanagement.ReturnCorrectName(args.NewText);
			
				if(NewText != NameSelected)
				{
					if(NewText == "")
					{
						mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameVariableEmpty");
						return;				
					}
				
				foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
					{
						foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
						{
							foreach(Scenario sce in node.ReturnListScenario())
							{
								if(sce.ScenarioId == ScenarioId)
								{
									foreach(Function fun in sce.ReturnListFunction())
									{
										if(fun.FunctionName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameFunctionExist");
											return;
										}
									}
									foreach(Variable vari in sce.ReturnListVariable())
									{
										if(vari.VariableName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameFunctionExistInVariable");
											return;										
										}
									}
								}
							}
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(NewText == ins.Instance_Name)
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInInstance");
									return;								
								}
							}						
						}
					}
				
					for(int i=0;i<ValuesPreferenceAction.Count;i++)
					{
						if(NewText == ValuesPreferenceAction[i])
						{
							mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInActionName");
							return;
						}
					}				
				
					if(IdSelected != "")
					{
						datamanagement.ModifyFunction(NewText,param.ParamI("MoFu_ChoiceName"),Convert.ToInt32(IdSelected));		
						UpdateFunctionTreeview();			
					}
				}
				return;			
			}
		
			//Fonction FunctionTypeCell_Edited
			//Fonction permettant de modifier un type d'argument
			void FunctionTypeCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string TypeSelected ="";
			
				TreeSelection selection = TreeviewFunction.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionId")); //Nous retournons l'id du noeud selectionné
					TypeSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionType"));
				}			
			
				if(args.NewText != TypeSelected && IdSelected != "")
				{
					datamanagement.ModifyFunction(args.NewText,param.ParamI("MoFu_ChoiceType"),Convert.ToInt32(IdSelected));
					UpdateFunctionTreeview();		
				}
			}
		
			//Fonction FunctionNoteCell_Edited
			//Fonction permettant de modifier les commettaires d'une fonction
			void FunctionNoteCell_Edited (object o, EditedArgs args)
			{
				string IdSelected = "";		//variable permettant de stocker l'id selectionné
				string NoteSelected = "";
			
				TreeSelection selection = TreeviewFunction.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionId")); //Nous retournons l'id du noeud selectionné
					NoteSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunction_PositionNote"));
				}					
			
				if(NoteSelected != args.NewText && IdSelected != "")
				{
					datamanagement.ModifyFunction(args.NewText,param.ParamI("MoFu_ChoiceNote"),Convert.ToInt32(IdSelected));
					UpdateFunctionTreeview();				
				}
			}
		
		//Function OnTreeviewFunctionButtonReleaseEvent
		//Fonction permettant de faire des actions lorsque nous faisons un clic dans le treeview des fonctions
		protected void OnTreeviewFunctionButtonReleaseEvent (object o, Gtk.ButtonReleaseEventArgs args)
		{
			if(args.Event.Button == param.ParamI("LeftClic")) //Si le bouton cliquer est le clic gauche
			{			
				MemorisationIdFunction = "";
				string IdFunctionRead = "";
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelVariableTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdFunctionRead = (string) TreeModelVariableTreeView.GetValue (IterProject, param.ParamI("SEW_TVFunction_PositionId")); //Nous mettons la valeur de la 1ere cellule dans un string (Texte)
				}	
				
				if(IdFunctionRead != "")
				{
					MemorisationIdFunction = IdFunctionRead;
				}
				
				foreach(Project Pro in datamanagement.ListProject.Values)
				{	
					foreach(Node node in Pro.ReturnListNode())
					{
						foreach(Scenario sce in node.ReturnListScenario())
						{	
							foreach(Function fun in sce.ReturnListFunction())
							{
								if(MemorisationIdFunction != "")
								{
									if(fun.FunctionId == Convert.ToInt32(MemorisationIdFunction))
									{
										datamanagement.AddElementInListMenuTextEditorFunction(node.Node_Id,ScenarioId,Convert.ToInt32(MemorisationIdFunction));
										InitialiseMenuFunction();
									}
								}
							}
						}
					}
				}
				
				UpdateFunctionArgumentTreeview();
				
				if(MemorisationIdFunction != "")
				{
					TextEditorFunction.Sensitive = true;
				}
				else
				{
					TextEditorFunction.Sensitive = false;
					TextEditorFunction.Text = "";
				}
			}
			else if(args.Event.Button == param.ParamI("RightClic")) //Si le bouton cliquer est le clic droit
			{	
				string IdFunction = "0";
				string NameFunction = "";
					
				TreeSelection selection = (o as TreeView).Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelVariableTreeView, out IterProject)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					IdFunction = (string) TreeModelVariableTreeView.GetValue (IterProject, param.ParamI("SEW_TVFunction_PositionId")); //Nous mettons la valeur de la 1ere cellule dans un string (Texte)
					NameFunction = (string) TreeModelVariableTreeView.GetValue (IterProject, param.ParamI("SEW_TVFunction_PositionName"));
				}
			
			
				MenuItemVariable = new Gtk.MenuItem("test");
				MenuItemVariable.Submenu = new Gtk.Menu();
			
				if(Convert.ToInt32(IdFunction) != 0)
				{
					Gtk.MenuItem MenuDelete = new Gtk.MenuItem(param.ParamT("SEW_DeleteFunction"));
					((Gtk.Menu)MenuItemVariable.Submenu).Add(MenuDelete);
					MenuDelete.Activated += delegate {DeleteFunction(NameFunction,Convert.ToInt32(IdFunction));};
					
					Gtk.SeparatorMenuItem Separator = new Gtk.SeparatorMenuItem();
					((Gtk.Menu)MenuItemVariable.Submenu).Add(Separator);
				}
			
				Gtk.MenuItem SubMenuLevel1 = new Gtk.MenuItem(param.ParamT("SEW_MenuNewFunction"));
				((Gtk.Menu)MenuItemVariable.Submenu).Add(SubMenuLevel1);	
				SubMenuLevel1.Submenu = new Gtk.Menu();
				
				foreach(string Typevar in ListTypeVariable)
				{
					var TypevarText = Typevar;
					Gtk.MenuItem SubMenuLevel2 = new Gtk.MenuItem(Typevar.Replace("_","-"));		
					SubMenuLevel2.ButtonPressEvent += delegate {AddNewFunction(TypevarText);};
					((Gtk.Menu)SubMenuLevel1.Submenu).Add(SubMenuLevel2);
				}
			
				MenuItemVariable.Submenu.ShowAll();
				((Gtk.Menu)MenuItemVariable.Submenu).Popup();					
			}					
		}	
		
			//Fonction AddNewFunction
			//Fonction permettant d'ajouter une fonction
			public void AddNewFunction (string TypeVar)
			{
				datamanagement.AddFunctionInScenario(datamanagement.ReturnNewNameFunction(param.ParamT("SEW_DefaultNameFunction"),ScenarioId),TypeVar,ScenarioId);
			}
		
			//Fonction DeleteFunction
			//Fonction permettant de supprimer une fonction
			public void DeleteFunction (string NameFun, Int32 IdVar)
			{
				this.Sensitive = false; //Permet d'incativer la fenêtre principal
				if(pref.BeepOnDelete)	
					System.Media.SystemSounds.Question.Play(); //Avertir du popup
				//Boite de dialogue permettant de valider la suppression d'un réseau
				MessageDialog message = new MessageDialog (mainwindow, 
	                                      			  	   DialogFlags.DestroyWithParent,
		                              				       MessageType.Question, 
	                                                       ButtonsType.YesNo, param.ParamT("QuestionDeleteFunction") + NameFun + " ?");
	            
				ResponseType messageresult = (ResponseType)message.Run (); //On lance la box
			
				if (messageresult == ResponseType.Yes) //On regarde le résultat et si Oui
				{
					datamanagement.DeleteFunctionInScenario(IdVar);
					UpdateVariableTreeview(); //On met  jour l'explorer treeview
					message.Destroy(); //On détruit la boite de dialogue	
					TextEditorScenario.Text = TextEditorScenario.Text.Replace(" " + NameFun + " ", " " + param.ParamT("SEW_FunctionEmpty") + " ");
					datamanagement.ModifyScenario(TextEditorScenario.Text,param.ParamI("MoSc_ChoiceData"),ScenarioId);	
					TextEditorFunction.Text = "";
					TextEditorFunction.Sensitive = false;
				}
				else
				{
					message.Destroy(); //On détruit la boite de dialogue
				}
				this.Sensitive = true; //Permet d'activer la fenêtre principal					
			}
				
//######################## FUNCTION ARGUMENTS TREEVIEW ################################
		
		//Fonction InitFunctionArgumentTreeview
		//Fonction permettant d'initialiser le treeview contenant les arguments des fonctions
		public void InitFunctionArgumentTreeview()
		{	
			TreeviewArgsFunction.EnableGridLines = TreeViewGridLines.Both;
				
			//Ajout des colonnes dans le TreeviewVariable
			FunctionArgsNumberColumn = new Gtk.TreeViewColumn();
			FunctionArgsTypeColumn = new Gtk.TreeViewColumn();
			FunctionArgsNameColumn = new Gtk.TreeViewColumn();	
			
			//Nous donnons des titre au colonnes
			FunctionArgsNumberColumn.Title = param.ParamT("SEW_TVFunctionArgs_Number");
			FunctionArgsTypeColumn.Title = param.ParamT("SEW_TVFunctionArgs_Type");
			FunctionArgsNameColumn.Title = param.ParamT("SEW_TVFunctionArgs_Name");		
			
			//On redimensionne les colonnes
			FunctionArgsNumberColumn.Resizable = true;
			FunctionArgsTypeColumn.Resizable = true;
			FunctionArgsNameColumn.Resizable = true;	
			
			//Création des cellules
			FunctionArgsNumberCell = new Gtk.CellRendererText ();
			FunctionArgsTypeCell = new Gtk.CellRendererCombo ();
			FunctionArgsNameCell = new Gtk.CellRendererText ();
			
			FunctionArgsTypeCell.Model = ListStoreTypeVariable;
			FunctionArgsTypeCell.TextColumn = 0;
			
			//On rend éditable les cellules
			FunctionArgsTypeCell.Editable = true;
			FunctionArgsNameCell.Editable = true;	
			
			FunctionArgsTypeCell.Edited += FunctionArgsTypeCell_Edited;
			FunctionArgsNameCell.Edited += FunctionArgsNameCell_Edited;		
			
			//On associe des cellule au colonne
			FunctionArgsNumberColumn.PackStart(FunctionArgsNumberCell, true);
			FunctionArgsTypeColumn.PackStart (FunctionArgsTypeCell, true);
			FunctionArgsNameColumn.PackStart(FunctionArgsNameCell, true);		
			
			//Ajout des colonnes dans le Treeview
			TreeviewArgsFunction.AppendColumn (FunctionArgsNumberColumn);
			TreeviewArgsFunction.AppendColumn (FunctionArgsTypeColumn);	
			TreeviewArgsFunction.AppendColumn (FunctionArgsNameColumn);	
			
			//Ajout des attibuts à chaque colonne
			FunctionArgsNumberColumn.AddAttribute(FunctionArgsNumberCell,"text",param.ParamI("SEW_TVFunctionArgs_PositionNumber"));
			FunctionArgsTypeColumn.AddAttribute (FunctionArgsTypeCell, "text", param.ParamI("SEW_TVFunctionArgs_PositionType"));		
			FunctionArgsNameColumn.AddAttribute(FunctionArgsNameCell,"text",param.ParamI("SEW_TVFunctionArgs_PositionName"));	
			
			//Création d'un nouveau store, param : texte Type, texte Value
			FunctionArgsListStore = new Gtk.TreeStore(typeof (string),typeof (string), typeof (string));
			TreeviewArgsFunction.Model = FunctionArgsListStore;
			TreeviewArgsFunction.ShowAll();			
		}
		
		//Fonction UpdateFunctionArgumentTreeview
		//Fonction permettant de mettre à jour le treeview des arguments des fonctions
		public void UpdateFunctionArgumentTreeview()
		{
			FunctionArgsListStore.Clear();
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							foreach(Function fun in sce.ReturnListFunction())
							{
								if(fun.FunctionId.ToString() == MemorisationIdFunction)
								{
									TextEditorFunction.Text = fun.FunctionData;
								}
								if(fun.FunctionId.ToString() == MemorisationIdFunction && !fun.InitFunction)
								{
									
									FunctionArgsListStore.AppendValues("1".ToString(),fun.FunctionTypeArg1,fun.FunctionNameArg1);
									
									if(fun.FunctionNameArg1 != "")
									{
										FunctionArgsListStore.AppendValues("2".ToString(),fun.FunctionTypeArg2,fun.FunctionNameArg2);
									}
									else
									{
										return;
									}
									if(fun.FunctionNameArg2 != "")
									{
										FunctionArgsListStore.AppendValues("3".ToString(),fun.FunctionTypeArg3,fun.FunctionNameArg3);
									}
									else
									{
										return;
									}
									
									if(fun.FunctionNameArg3 != "")
									{
										FunctionArgsListStore.AppendValues("4".ToString(),fun.FunctionTypeArg4,fun.FunctionNameArg4);
									}
									else
									{
										return;
									}
									
									if(fun.FunctionNameArg4 != "")
									{
										FunctionArgsListStore.AppendValues("5".ToString(),fun.FunctionTypeArg5,fun.FunctionNameArg5);
									}
									else
									{
										return;										
									}
									
									if(fun.FunctionNameArg5 != "")
									{
										FunctionArgsListStore.AppendValues("6".ToString(),fun.FunctionTypeArg6,fun.FunctionNameArg6);
									}
									else
									{
										return;
									}									
								}
							}
						}
					}
				}
			}				
		}
		
			//Fonction FunctionArgsTypeCell_Edited
			//Fonction permettant de modifier un type d'arguments
			void FunctionArgsTypeCell_Edited (object o, EditedArgs args)
			{
				string NumberArgs = "";		//variable permettant de stocker l'id selectionné
				string TypeSelected ="";
			
				TreeSelection selection = TreeviewArgsFunction.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					NumberArgs = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunctionArgs_PositionNumber")); //Nous retournons l'id du noeud selectionné
					TypeSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunctionArgs_PositionType"));
				}		
			
				if(args.NewText != TypeSelected)
				{
					datamanagement.ModifyFunction(args.NewText,param.ParamI("MoFu_FunctionTypeArg" + NumberArgs),Convert.ToInt32(MemorisationIdFunction));
					UpdateFunctionArgumentTreeview();	
				}
			}
		
			//Fonction FunctionArgsNameCell_Edited
			//Fonction permettant de modifier le nom de l'argument
			void FunctionArgsNameCell_Edited (object o, EditedArgs args)
			{
				string NumberArgs = "";		//variable permettant de stocker l'id selectionné
				string NameSelected ="";
			
				TreeSelection selection = TreeviewArgsFunction.Selection; //Nous allons cree un arbre de selection
				if(selection.GetSelected(out TreeModelChildTreeView, out IterChild)) //Nous cherchons la valeur selectionné dans l'arbre de selection
				{
					NumberArgs = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunctionArgs_PositionNumber")); //Nous retournons l'id du noeud selectionné
					NameSelected = (string) TreeModelChildTreeView.GetValue (IterChild, param.ParamI("SEW_TVFunctionArgs_PositionName"));
				}		
			
				string NewText = datamanagement.ReturnCorrectName(args.NewText);
			
				if(NewText == "" && NameSelected != "")
				{
					datamanagement.ModifyFunction(NumberArgs,param.ParamI("MoFu_FunctionArgReorganisation"),Convert.ToInt32(MemorisationIdFunction));
					UpdateFunctionArgumentTreeview();	
					return;
				}
			
				if(NewText != NameSelected)
				{
				foreach(Project Pro in datamanagement.ListProject.Values) //Dans la liste des projets
					{
						foreach(Node node in Pro.ReturnListNode()) //Dans la liste des noeud de chaque projet
						{
							foreach(Scenario sce in node.ReturnListScenario())
							{
								if(sce.ScenarioId == ScenarioId)
								{
									foreach(Function fun in sce.ReturnListFunction())
									{
										if(fun.FunctionName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameVariableExistInFunction");
											return;
										}
										if(fun.FunctionId == Convert.ToInt32(MemorisationIdFunction))
										{
											if(NewText == fun.FunctionNameArg1 || NewText == fun.FunctionNameArg2 || NewText == fun.FunctionNameArg3 || NewText == fun.FunctionNameArg4 || NewText == fun.FunctionNameArg5 || NewText == fun.FunctionNameArg6)
											{
												mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameArgsExistInArgs");
												return;
											}
										}
									}
									foreach(Variable vari in sce.ReturnListVariable())
									{
										if(vari.VariableName == NewText)
										{
											mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameFunctionExistInVariable");
											return;										
										}
									}
								}
							}
							foreach(Instance ins in node.ReturnListInstance())
							{
								if(NewText == ins.Instance_Name)
								{
									mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInInstance");
									return;								
								}
							}						
						}
					}
				
					for(int i=0;i<ValuesPreferenceAction.Count;i++)
					{
						if(NewText == ValuesPreferenceAction[i])
						{
							mainwindow.AddLineOutput(param.ParamI("OutputError"),"NameExistInActionName");
							return;
						}
					}				
				
					datamanagement.ModifyFunction(NewText,param.ParamI("MoFu_FunctionNameArg" + NumberArgs),Convert.ToInt32(MemorisationIdFunction));
					UpdateFunctionArgumentTreeview();
				}		
			}
		
//##################### OTHER #######################################			
		
		//Fonction WidgetIsCorrect
		//Fcontion peremettant de saoir si un widget est correct, si il nous retourne de la data
		public bool WidgetIsCorrect()
		{
			foreach(Project Pro in datamanagement.ListProject.Values)
			{	
				foreach(Node node in Pro.ReturnListNode())
				{
					foreach(Scenario sce in node.ReturnListScenario())
					{
						if(sce.ScenarioId == ScenarioId)
						{
							return true;
						}
					}
				}
			}
			return false;
		}			
	}
}