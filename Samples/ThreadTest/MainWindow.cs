﻿using System;
using System.Threading;
using Gtk;

namespace ThreadTest
{
    public class MyClass : Window
    {
        static Label l;
        static int numClick = 0;

        public static void Main()
        {
            Application.Init();
            new MyClass();

            Thread thread = new Thread(new ThreadStart(ThreadRoutine));
            thread.Start();
            Application.Run();
        }

        static void ThreadRoutine()
        {
            while (Application.EventsPending())
            {
                Gtk.Application.RunIteration();
            }
        }

        public MyClass()
            : base("Threading window")
        {
            SetDefaultSize(300, 200);

            Grid grid = new Grid();

            l = new Label(numClick.ToString());
            grid.Attach(l, 1, 1, 1, 1);

            Button b = new Button("ClickMe!");
            grid.Attach(b, 1, 2, 1, 1);
            b.Clicked += delegate
            {
                numClick++;
                l.Text = numClick.ToString();
                l.QueueDraw();
            };

            Add(grid);

            ShowAll();
        }
    }
}

