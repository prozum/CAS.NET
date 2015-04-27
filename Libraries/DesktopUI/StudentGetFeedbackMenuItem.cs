﻿using System;
using Gtk;
using ImEx;
using System.Collections.Generic;
using Ast;

namespace DesktopUI
{
    public class StudentGetFeedbackMenuItem : MenuItem
    {
        TextViewList textviews;
        User user;

        public StudentGetFeedbackMenuItem(ref User user, ref TextViewList textviews)
            : base("Get Feedback")
        {
            this.user = user;
            this.textviews = textviews;

            this.Activated += delegate
            {
                OnClicked();
            };
        }

        void OnClicked()
        {
			StudentGetFeedbackWindow window = new StudentGetFeedbackWindow (ref user, ref textviews);
        }
    }
}

