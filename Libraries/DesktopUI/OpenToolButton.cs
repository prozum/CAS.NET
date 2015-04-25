﻿using System;
using Gtk;
using System.Collections.Generic;
using ImEx;
using Ast;
using Gdk;
using System.IO;

namespace DesktopUI
{
    public class OpenToolButton : ToolButton
    {
        static Image image = new Image();
        TextViewList textviews;

        public OpenToolButton(TextViewList textviews)
            : base(/*image,*/ "open")
        {

//            SetIcon();

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

            bool hasOpenedAnything = false;

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
                            hasOpenedAnything = true;
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
                            hasOpenedAnything = true;
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    break;
            }

            if (hasOpenedAnything == true)
            {
                List<MetaType> metaTypeList = new List<MetaType>();

                metaTypeList = ImEx.Import.DeserializeString<List<MetaType>>(file);

                textviews.castextviews.Clear();

                foreach (var item in metaTypeList)
                {
                    if (item.type == typeof(MovableCasCalcView))
                    {
                        Evaluator Eval = new Evaluator();
                        MovableCasCalcView movableCasCalcView = new MovableCasCalcView(Eval, textviews);
                        movableCasCalcView.calcview.input.Text = item.metastring;
                        textviews.castextviews.Add(movableCasCalcView);
                    }
                    else if (item.type == typeof(MovableCasTextView))
                    {
                        MovableCasTextView movableCasTextView = new MovableCasTextView(textviews, item.metastring, true);
                        textviews.castextviews.Add(movableCasTextView);
                    }
                }

                textviews.Clear();
                textviews.Redraw();
                textviews.Reevaluate();
                textviews.ShowAll();

            }
        }

        void SetIcon()
        {
            OperatingSystem os = Environment.OSVersion;
            PlatformID pid = os.Platform;

            switch (pid)
            {
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                case PlatformID.Win32NT: // <- if one, this is the one we really need
                    {
                        byte[] buffer = File.ReadAllBytes("");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        byte[] buffer = File.ReadAllBytes("");
                        Pixbuf pixbuf = new Pixbuf(buffer);
                        pixbuf = pixbuf.ScaleSimple(25, 25, InterpType.Bilinear);
                        image.Pixbuf = pixbuf;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

    }
}

