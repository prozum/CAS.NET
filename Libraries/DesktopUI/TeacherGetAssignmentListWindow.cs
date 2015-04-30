﻿using System;
using Gtk;

namespace DesktopUI
{
    public class TeacherGetAssignmentListWindow : Window
    {
        User user;
        TextViewList textviews;

        public TeacherGetAssignmentListWindow(ref User user, ref TextViewList textviews)
            : base("Assignment List")
        {
            this.user = user;
            this.textviews = textviews;

            string[] assignmentList = user.teacher.GetAssignmentList();

            ScrolledWindow scrolledWindow = new ScrolledWindow();
            VBox vbox = new VBox(false, 2);

            foreach (var item in assignmentList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    Label label = new Label(item);
                    Button GetCompleted = new Button("Get Completed Assignments");
                    GetCompleted.Clicked += (sender, e) => new TeacherGetCompletedListWindow(this.user, this.textviews, item);

                    HBox hbox = new HBox(false, 2);

                    hbox.Add(label);
                    hbox.Add(GetCompleted);               
                    vbox.Add(hbox);
                }
            }

            scrolledWindow.Add(vbox);
            Add(scrolledWindow);

            SetDefaultSize(300, 200);

            ShowAll();
        }
    }
}