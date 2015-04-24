﻿using System;
using Gtk;

namespace DesktopUI
{
    public class LogoutMenuItem : MenuItem
    {
        Menu menu;
        User user;

        public LogoutMenuItem(ref User user, ref Menu menu)
            : base("Logout")
        {
            this.menu = menu;
            this.user = user;

            this.Activated += delegate
            {
                LogoutWrapper();
            };
        }

        void LogoutWrapper()
        {
            user.Logout();

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
    }
}

