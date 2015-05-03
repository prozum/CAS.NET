﻿using System;
using Gtk;
using System.Collections.Generic;
using ImEx;

namespace DesktopUI
{
    public class TeacherAddAssignmentMenuItem : MenuItem
    {
        User user;
        TextViewList textviews;

        public TeacherAddAssignmentMenuItem(User user, TextViewList textviews)
            : base("Add Assignment")
        {
            this.user = user;
            this.textviews = textviews;

            Activated += (sender, e) => new TeacherAddAssignmentWindow(user, textviews);
        }
    }
}

