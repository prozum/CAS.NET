﻿using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;

namespace DesktopUI
{
    public class OpenToolButton : ToolButton
    {
        TextViewList textviews;

        public OpenToolButton(TextViewList textviews)
            : base(Stock.Open)
        {
            this.textviews = textviews;

            this.Clicked += delegate
            {
                OpenFile();
            };
        }

        public void OpenFile()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            string file = String.Empty;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        var filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            file = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        Object[] parameters = { "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept };
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", null, FileChooserAction.Open, parameters);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            file = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        Console.WriteLine("Line loaded::: " + file);

                        filechooser.Destroy();

                        break;
                    }
                default:
                    break;
            }

            Console.WriteLine(file);

            List<MetaType> metaTypeList = new List<MetaType>();

            metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(file);

            foreach (var item in metaTypeList)
            {
                Console.WriteLine("Item: " + item);
            }

            foreach (var item in metaTypeList)
            {
                if (item.type == typeof(MovableCasCalcView))
                {
                    Console.WriteLine("Is movable cas calc view");
                    Evaluator Eval = new Evaluator();
                    MovableCasCalcView movableCasCalcView = new MovableCasCalcView(Eval, textviews);
                    movableCasCalcView.calcview.input.Text = item.metastring;
                    textviews.Add(movableCasCalcView);
                }
                else if (item.type == typeof(MovableCasTextView))
                {
                    Console.WriteLine("Is movavle cas text view");
                    MovableCasTextView movableCasTextView = new MovableCasTextView(textviews, item.metastring, true);
                    textviews.Add(movableCasTextView);
                }

            }
        }
    }
}

