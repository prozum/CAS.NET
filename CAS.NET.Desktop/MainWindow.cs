﻿using Gtk;

public class MainWindow : Window
{
    static void Main(string[] args)
    {
        Application.Init ();
        new MainWindow ();
        Application.Run();
    }
        

    public MainWindow() : base("MainWindow")
    {
        DeleteEvent += delegate {
            Application.Quit();
        };

        var textview = new TextView();
        Add (textview);

        var tag = new TextTag ("helloworld-tag");
        tag.Scale = Pango.Scale.XXLarge;
        tag.Style = Pango.Style.Italic;
        tag.Underline = Pango.Underline.Double;
        tag.Foreground = "blue";
        tag.Background = "pink";
        tag.Justification = Justification.Center;
        var buffer = textview.Buffer;
        buffer.TagTable.Add (tag);

        var insertIter = buffer.StartIter;
        buffer.InsertWithTagsByName (ref insertIter, "Hello Desktop!\n", "helloworld-tag");

        ShowAll ();
    }
}
