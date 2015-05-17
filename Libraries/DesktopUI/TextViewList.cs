﻿using System;
using System.Threading;
using Ast;
using System.Collections.Generic;
using Gtk;

namespace DesktopUI
{
    public class TextViewList : Grid
    {
        public List<MovableCasTextView> castextviews = new List<MovableCasTextView>();
        Grid ButtonGrid = new Grid();
        Evaluator Eval;
        User user;
        Window window;

        public TextViewList(User user, Evaluator Eval, Window window)
            : base()
        {
            this.Eval = Eval;
            this.user = user;
            this.window = window;

            ColumnSpacing = 10;
            RowSpacing = 10;

            window.ResizeChecked += delegate
            {
                foreach (Widget widget in castextviews)
                {
                    if(widget is MovableCasCalcView)
                    {
                        (widget as MovableCasCalcView).calcview.input.WidthRequest = window.Window.Width - 60; 
                    }
                    else if (widget is MovableDrawCanvas)
                    {
                        (widget as MovableDrawCanvas).canvas.WidthRequest = window.Window.Width - 60;
                    }
                    else if(widget is MovableCasTextView) // <- This shall always be last as all other widgets inherit from it, but not all use it.
                    {
                        (widget as MovableCasTextView).textview.WidthRequest = window.Window.Width - 60;
                    }
                }
            };

            ShowAll();
        }

        public void InsertTextView(string serializedString, bool locked, int pos)
        {
            MovableCasTextView movableCasTextView = new MovableCasTextView(serializedString, locked);
            movableCasTextView.textview.LockTextView(locked);

            movableCasTextView.Attach(AddLockCheckButton(movableCasTextView), 1, 100, 1, 1);
            movableCasTextView.Attach(AddCommandButtons(movableCasTextView), 100, 1, 1, 1);

            if (pos == -1)
            {
                castextviews.Add(movableCasTextView);
            }
            else
            {
                castextviews.Insert(pos, movableCasTextView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertTaskGenTextView(string TaskString)
        {
            MovableCasTextView movableCasTextView = new MovableCasTextView(TaskString);
            movableCasTextView.textview.LockTextView(true);

            movableCasTextView.Attach(AddLockCheckButton(movableCasTextView), 1, 100, 1, 1);
            movableCasTextView.Attach(AddCommandButtons(movableCasTextView), 100, 1, 1, 1);

            castextviews.Add(movableCasTextView);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcView(int pos)
        {
            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
            MovCasCalcView.calcview.input.Activated += delegate
            {
                MovCasCalcView.calcview.Eval.Locals.Clear();
                MovCasCalcView.calcview.Evaluate();
                Reevaluate();
                MovCasCalcView.ShowAll();
            };

            MovCasCalcView.Attach(AddLockCheckButton(MovCasCalcView), 1, 100, 1, 1);
            MovCasCalcView.Attach(AddCommandButtons(MovCasCalcView), 100, 1, 1, 1);

            if (pos == -1)
            {
                castextviews.Add(MovCasCalcView);
            }
            else
            {
                castextviews.Insert(pos, MovCasCalcView);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertCalcView(string input, bool locked)
        {
            MovableCasCalcView MovCasCalcView = new MovableCasCalcView(Eval);
            MovCasCalcView.calcview.input.Text = input;
            MovCasCalcView.calcview.input.IsEditable = !locked;
            MovCasCalcView.calcview.input.Activated += delegate
            {
                    MovCasCalcView.calcview.Eval.Scope.Locals.Clear();
                    MovCasCalcView.calcview.Evaluate();
                    MovCasCalcView.ShowAll();
            };

            MovCasCalcView.Attach(AddLockCheckButton(MovCasCalcView), 1, 100, 1, 1);
            MovCasCalcView.Attach(AddCommandButtons(MovCasCalcView), 100, 1, 1, 1);

            if(user.privilege <= 0 && locked == true)
            {
                MovCasCalcView.calcview.input.IsEditable = false;
            }

            castextviews.Add(MovCasCalcView);

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertDrawCanvas(int pos)
        {
            MovableDrawCanvas movableDrawCanvas = new MovableDrawCanvas();

            movableDrawCanvas.Attach(AddLockCheckButton(movableDrawCanvas), 1, 100, 1, 1);
            movableDrawCanvas.Attach(AddCommandButtons(movableDrawCanvas), 100, 1, 1, 1);

            if (pos == -1)
            {
                castextviews.Add(movableDrawCanvas);
            }
            else
            {
                castextviews.Insert(pos, movableDrawCanvas);
            }

            Clear();
            Redraw();
            ShowAll();
        }

        public void InsertResult(string answer, string facit)
        {
            MovableCasResult MovableCasResult = new MovableCasResult(user, answer, facit);

            MovableCasResult.Attach(AddLockCheckButton(MovableCasResult), 1, 100, 1, 1);
            MovableCasResult.Attach(AddCommandButtons(MovableCasResult), 100, 1, 1, 1);

            castextviews.Add(MovableCasResult);

            Clear();
            Redraw();
            ShowAll();
        }

        public void Move(int ID, int UpOrDown)
        {
            int i = 0;

            while (ID != castextviews[i].id_)
            {
                i++;
            }

            // move down
            if (UpOrDown == 1 && i + 1 < castextviews.Count)
            {
                castextviews.Insert(i + 2, castextviews[i]);
                castextviews.RemoveAt(i);
            }
			// move up
			else if (UpOrDown == -1 && i - 1 >= 0)
            {
                castextviews.Insert(i - 1, castextviews[i]);
                castextviews.RemoveAt(i + 1);
            }

            Clear();
            Reevaluate();
            Redraw();
        }

        public void Delete(int ID)
        {
            int i = 0;

            while (ID != castextviews[i].id_)
            {
                i++;
            }

            castextviews.RemoveAt(i);

            Clear();
            Reevaluate();
            Redraw();
        }

        public void Clear()
        {
            foreach (Widget widget in this)
            {
                Remove(widget);
            }
        }

        public void Redraw()
        {
            foreach (Widget widget in castextviews)
            {
                Attach(widget, 1, Children.Length, 1, 1);
            }

            Attach(ButtonGrid, 1, Children.Length, 1, 1);
        }

        public void Reevaluate()
        {
            foreach (Widget widget in castextviews)
            {
                if(widget is MovableCasCalcView)
                {
                    (widget as MovableCasCalcView).calcview.Evaluate();
                }
            }
        }

        public void AddNew(Widget w)
        {
            int _id = castextviews.IndexOf((MovableCasTextView)w);

            Button buttonCalcel = new Button("Cancel");
            Button buttonTextView = new Button("TextView");
            Button buttonCalcView = new Button("CalcView");
            Button buttonDrawCanvas = new Button("DrawCanvas");

            Window window = new Window("Insert new widget");

            window.SetSizeRequest(300, 300);

            VBox vbox = new VBox();

            vbox.PackStart(buttonTextView, false, false, 2);
            vbox.PackStart(buttonCalcView, false, false, 2);
            vbox.PackStart(buttonDrawCanvas, false, false, 2);

            vbox.PackEnd(buttonCalcel, false, false, 2);

            window.Add(vbox);

            window.ShowAll();

            buttonCalcel.Clicked += delegate
            {
                window.Destroy();
            };

            buttonTextView.Clicked += delegate
            {
                InsertTextView("", false, _id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };

            buttonCalcView.Clicked += delegate
            {
                InsertCalcView(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };

            buttonDrawCanvas.Clicked += delegate
            {
                InsertDrawCanvas(_id + 1);
                Clear();
                Reevaluate();
                Redraw();

                _id++;
                buttonCalcel.Click();
            };
        }

        CheckButton AddLockCheckButton(MovableCasTextView movableCasTextView)
        {
            if(user.privilege == 1)
            {
                CheckButton checkbutton = new CheckButton("Lock for students");

                if (movableCasTextView.textview.locked == true)
                {
                    checkbutton.Active = true;
                }

                checkbutton.Toggled += delegate
                {
                    movableCasTextView.textview.locked = !movableCasTextView.textview.locked;
                };

                return checkbutton;
            }
            else
            {
                return null;
            }
        }

        VBox AddCommandButtons(MovableCasTextView movableCasTextView)
        {
            Button ButtonMoveUp = new Button("↑");
            Button ButtonMoveDown = new Button("↓");
            Button ButtonDelete = new Button("X");
            Button ButtonAddNew = new Button("+");

            VBox vbox = new VBox();

            if (user.privilege == 1 || (user.privilege == 0 && movableCasTextView.textview.locked == false))
            {

                ButtonMoveUp.Clicked += delegate
                {
                    Move(movableCasTextView.id_, -1);
                };

                ButtonMoveDown.Clicked += delegate
                {
                    Move(movableCasTextView.id_, 1);
                };

                ButtonDelete.Clicked += delegate
                {
                    Delete(movableCasTextView.id_);
                };

                ButtonAddNew.Clicked += delegate
                {
                    AddNew(movableCasTextView);
                };

                HBox hbox = new HBox();
                Toolbar tb = new Toolbar();
                hbox.Add(ButtonMoveUp);
                hbox.Add(ButtonMoveDown);
                hbox.Add(ButtonDelete);
                hbox.Add(ButtonAddNew);
                vbox.PackStart(hbox, false, false, 2);
            }
            else if (user.privilege <= 0)
            {
                ButtonAddNew.Clicked += delegate
                {
                    AddNew(movableCasTextView);
                };

                vbox.PackStart(ButtonAddNew, false, false, 2);
            }

            return vbox;
        }
    }
}

