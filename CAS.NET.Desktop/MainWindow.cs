﻿using System;
using Gtk;

//using TaskGenLib;
//using System.Collections.Generic;
//using ImEx;
//using System.Net;
//using System.Text;
using DesktopUI;

namespace CAS.NET.Desktop
{
    class MainWindow : Window
    {
        TextViewList textviews = new TextViewList();
        MenuBar menubar = new MenuBar();
        Menu menu = new Menu();
        ServerMenuItem server;
        LoginMenuItem login;
        LogoutMenuItem logout;
        StudentAddCompletedMenuItem stdAddCom;
        StudentGetAssignmentMenuItem stdGetAsm;
        StudentGetAssignmentListMenuItem stdGetAsmList;
        StudentGetFeedbackMenuItem stdGetFee;
        TeacherAddAssignmentMenuItem teaAddAsm;
        TeacherAddFeedbackMenuItem teaAddFee;
        TeacherGetAssignmentListMenuItem teaGetAsmList;
        TeacherGetCompletedMenuItem teaGetCom;
        StudentGetAssignmentMenuItem studentGetAssignmentMenuItem;

        User user = new User();

        Toolbar toolbar = new Toolbar();
        OpenToolButton open;
        SaveToolButton save;
        NewToolButton neo;

        ScrolledWindow scrolledWindow = new ScrolledWindow();

        public MainWindow()
            : base("CAS.NET")
        {
            DeleteEvent += (o, a) => Application.Quit();

            // Initiating menu elements
            server = new ServerMenuItem();
            login = new LoginMenuItem(ref user, menu);
            logout = new LogoutMenuItem(ref user, ref menu);
            stdAddCom = new StudentAddCompletedMenuItem(ref user, ref textviews);
            stdGetAsm = new StudentGetAssignmentMenuItem(ref user, ref textviews);
            stdGetAsmList = new StudentGetAssignmentListMenuItem(ref user, ref textviews);
            stdGetFee = new StudentGetFeedbackMenuItem(ref user, ref textviews);
            teaAddAsm = new TeacherAddAssignmentMenuItem(ref user, ref textviews);
            teaAddFee = new TeacherAddFeedbackMenuItem();
            teaGetAsmList = new TeacherGetAssignmentListMenuItem();
            teaGetCom = new TeacherGetCompletedMenuItem();

            // Adding elements to menu
            server.Submenu = menu;
            menu.Append(login);
            menu.Append(logout);
            menu.Append(stdAddCom);
            menu.Append(stdGetAsm);
            menu.Append(stdGetAsmList);
            menu.Append(stdGetFee);
            menu.Append(teaAddAsm);
            menu.Append(teaAddFee);
            menu.Append(teaGetAsmList);
            menu.Append(teaGetCom);

            menubar.Append(server);

            open = new OpenToolButton(textviews);
            save = new SaveToolButton(textviews);
            neo = new NewToolButton(textviews);

            toolbar.Add(open);
            toolbar.Add(save);
            toolbar.Add(neo);

            VBox vbox = new VBox();

            vbox.PackStart(menubar, false, false, 2);
            vbox.PackStart(toolbar, false, false, 2);
            scrolledWindow.Add(textviews);
            vbox.Add(scrolledWindow);

            Add(vbox);

            SetSizeRequest(600, 600);

            ShowAll();

            // Rehiding elements not ment to be shown at boot, as the
            // user is currently not logged in.
            foreach (Widget w in menu)
            {
                if (w.GetType() == typeof(StudentAddCompletedMenuItem)
                    || w.GetType() == typeof(StudentGetAssignmentListMenuItem)
                    || w.GetType() == typeof(StudentGetAssignmentMenuItem)
                    || w.GetType() == typeof(StudentGetFeedbackMenuItem)
                    || w.GetType() == typeof(TeacherAddAssignmentMenuItem)
                    || w.GetType() == typeof(TeacherAddFeedbackMenuItem)
                    || w.GetType() == typeof(TeacherGetAssignmentListMenuItem)
                    || w.GetType() == typeof(TeacherGetCompletedMenuItem)
                    || w.GetType() == typeof(LogoutMenuItem))
                {
                    w.Hide();
                }
                else if (w.GetType() == typeof(LoginMenuItem))
                {
                    w.Show();
                }
            }
        }

        /*

        public CASGui()
            : base("CAS.Net gui")
        {

            DeleteEvent += (o, a) => Application.Quit();

            globVarMax = 10;
            globVarMin = 1;
            globVarNum = 2;

            SetSizeRequest(300, 500);
            oVB = new VBox(false, 2);
            iVB = new VBox(false, 2);
            Table table1 = new Table(2, 2, false);
            ScrolledWindow scroll = new ScrolledWindow();

            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;

            SetPosition(WindowPosition.Center);

            #region Menuer
            MenuBar mb = new MenuBar();
            Menu filemenu = new Menu();
            Menu addFileMenu = new Menu();
            Menu serverMenu = new Menu();


            MenuItem file = new MenuItem("File");
            file.Submenu = filemenu;

            MenuItem newFile = new MenuItem("New File");
            newFile.Activated += (object sender, EventArgs e) => ClearWindow();

            MenuItem openFile = new MenuItem("Open File");
            openFile.Activated += (o, a) => OpenFile();

            MenuItem saveFile = new MenuItem("Save File");
            saveFile.Activated += (o, a) => SaveFile();

            MenuItem properties = new MenuItem("Properties");
            properties.Activated += (o, a) => OnActivatedProperties(globVarMin, globVarMax, globVarNum);

            MenuItem exit = new MenuItem("Exit");
            exit.Activated += OnActivated;



            MenuItem addItem = new MenuItem("Add Item");
            addItem.Submenu = addFileMenu;

            MenuItem gen = new MenuItem("Generate Assignment");
            gen.Activated += (o, a) => OnActivatedGen();

            MenuItem head = new MenuItem("Add headline");
            head.Activated += (object sender, EventArgs e) => AddHeadlineBox();

            MenuItem text = new MenuItem("Text Field");
            text.Activated += (object sender, EventArgs e) => AddTextBox();



            MenuItem server = new MenuItem("Server");
            server.Submenu = serverMenu;

            MenuItem login = new MenuItem("Login");
            login.Activated += (object sender, EventArgs e) => LoginScreen();

            filemenu.Append(newFile);
            filemenu.Append(openFile);
            filemenu.Append(saveFile);
            filemenu.Append(properties);
            filemenu.Append(exit);

            addFileMenu.Append(gen);
            addFileMenu.Append(head);
            addFileMenu.Append(text);

            serverMenu.Append(login);

            mb.Append(file);
            mb.Append(addItem);
            mb.Append(server);
            #endregion Menuer

            table1.Attach(SetupLabAss(), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table1.Attach(entry, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table1.Attach(SetupTV(100, 100, ""), 0, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR


            iVB.Add(table1);
            oVB.PackStart(mb, false, false, 8);
            oVB.Add(scroll);
            scroll.Add(iVB);

            Add(oVB);
            ShowAll();
        }

        // Menu to set number minimum, maximum and number of numbers used for autogenerated equations.
        void OnActivatedProperties(int min, int max, int num)
        {
            Window PropertiesWindow = new Window("This is a window");
            PropertiesWindow.SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            // string smin = min.ToString();
            // string smax = max.ToString();
            // string snum = num.ToString();

            Label varMin = new Label("VarMin");
            Label varMax = new Label("varMax");
            Label varNum = new Label("varNum");

            Table table = new Table(2, 4, false);

            table.Attach(varMin, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varMax, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(varNum, 0, 1, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            SpinButton sbMin = new SpinButton(0, 100000000, 1);
            sbMin.Value = globVarMin;
            sbMin.WidthRequest = 10;
            SpinButton sbMax = new SpinButton(0, 100000000, 1);
            sbMax.Value = globVarMax;
            sbMax.WidthRequest = 10;
            SpinButton sbNum = new SpinButton(2, 5, 1);
            sbNum.Value = globVarNum;
            sbNum.WidthRequest = 10;

            table.Attach(sbMin, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbMax, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(sbNum, 1, 2, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Button ok = new Button("Confirm");
            Button cancel = new Button("Cancel");

            cancel.Clicked += (object sender, EventArgs e) => PropertiesWindow.Destroy();

            ok.Clicked += delegate(object sender, EventArgs e)
            {
                globVarMin = sbMin.ValueAsInt;
                globVarMax = sbMax.ValueAsInt;
                globVarNum = sbNum.ValueAsInt;
                PropertiesWindow.Destroy();
            };

            hbox.Add(cancel);
            hbox.Add(ok);

            vbox.Add(table);
            vbox.Add(hbox);

            PropertiesWindow.Add(vbox);
            PropertiesWindow.ShowAll();
        }

        // Quits the application on call
        void OnActivated(object sender, EventArgs args)
        {
            Application.Quit();
        }

        // Generates new auto-generated equation
        void OnActivatedGen()
        {
            Table table = new Table(2, 2, false);
            Entry entry = new Entry();
            entry.HeightRequest = 20;
            entry.WidthRequest = 100;
            entry.Buffer.Text = "";

            table.Attach(SetupLabAss(), 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //Assignment
            table.Attach(entry, 4, 5, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); //answer
            table.Attach(SetupTV(100, 100, ""), 0, 5, 2, 3, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 3, 3); // MR
            iVB.Add(table);
            ShowAll();

        }

        // Adds a oneline headline box
        void AddHeadlineBox()
        {
            Table table = new Table(1, 1, false);
            Entry entry = new Entry();

            entry.HeightRequest = 20;
            entry.WidthRequest = 100;
            entry.Buffer.Text = "";

            table.Attach(entry, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 3, 3);
            iVB.Add(table);
            ShowAll();
        }

        // Adds a textbox
        // Please note that it currently writes all text on one line only.
        void AddTextBox()
        {
            Table table = new Table(1, 1, false);
            TextView textView = new TextView();

            textView.HeightRequest = 100;
            textView.WidthRequest = 200;
            textView.Visible = true;

            table.Attach(textView, 0, 1, 0, 1, AttachOptions.Fill, AttachOptions.Fill, 3, 3);
            iVB.Add(table);
            ShowAll();
        }

        // Generates the calculation for the equation
        public Label SetupLabAss()
        {
            Label labAss = new Label(TaskGen.MakeCalcTask(globVarMin, globVarMax, globVarNum));
            return labAss;
        }

        // Generates textbox
        public TextView SetupTV(int h, int w, string text)
        {
            TextView t = new TextView();
            t.HeightRequest = h;
            t.WidthRequest = w;
            t.Buffer.Text = text;
            t.Visible = true;
            return t;
        }

        void OpenFile()
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
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.OpenFileDialog filechooser = new System.Windows.Forms.OpenFileDialog();

                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.FileName);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Open file...", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            casFile = System.IO.File.ReadAllText(filechooser.Filename);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        void SaveFile()
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
                        System.IO.Stream myStream = null;
                        System.Windows.Forms.SaveFileDialog filechooser = new System.Windows.Forms.SaveFileDialog();
                       
                        filechooser.InitialDirectory = "c:\\";
                        filechooser.Filter = "cas files (*.cas)|*.cas";
                        filechooser.FilterIndex = 2;
                        filechooser.RestoreDirectory = true;

                        if (filechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            System.IO.File.WriteAllText(filechooser.FileName, casFile);
                        }

                        break;
                    }
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    {
                        FileChooserDialog filechooser = new FileChooserDialog("Save File...", this, FileChooserAction.Save, "Cancel", ResponseType.Cancel, "Save", ResponseType.Accept);

                        filechooser.Filter = new FileFilter();
                        filechooser.Filter.AddPattern("*.cas");

                        if (filechooser.Run() == (int)ResponseType.Accept)
                        {
                            System.IO.File.WriteAllText(filechooser.Filename, casFile);
                        }

                        filechooser.Destroy();

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        void LoginScreen()
        {
            Window loginWindow = new Window("Login to CAS.NET");
            loginWindow.SetDefaultSize(200, 200);

            VBox vbox = new VBox(false, 2);
            HBox hbox = new HBox(false, 2);

            Label labUsername = new Label("Username");
            Label labPassword = new Label("Password");

            Table table = new Table(2, 3, false);

            table.Attach(labUsername, 0, 1, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(labPassword, 0, 1, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Entry entryUsername = new Entry();
            entryUsername.HeightRequest = 20;
            entryUsername.WidthRequest = 100;
            entryUsername.Buffer.Text = "";

            Entry entryPassword = new Entry();
            entryPassword.HeightRequest = 20;
            entryPassword.WidthRequest = 100;
            entryPassword.Buffer.Text = "";

            table.Attach(entryUsername, 1, 2, 0, 1, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);
            table.Attach(entryPassword, 1, 2, 1, 2, Gtk.AttachOptions.Fill, Gtk.AttachOptions.Fill, 5, 5);

            Button buttonLogin = new Button("Login");
            buttonLogin.Clicked += delegate(object sender, EventArgs e)
            {
                username = entryUsername.Text;
                password = entryPassword.Text;
                loginWindow.Destroy();
            };

            Button buttonCancel = new Button("Cancel");
            buttonCancel.Clicked += (object sender, EventArgs e) => loginWindow.Destroy();

            hbox.Add(buttonCancel);
            hbox.Add(buttonLogin);

            vbox.Add(table);
            vbox.Add(hbox);

            loginWindow.Add(vbox);
            loginWindow.ShowAll();
        }

        void ClearWindow()
        {
            foreach (Widget item in iVB)
            {
                iVB.Remove(item);
            }
        }
        */
    }
}