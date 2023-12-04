﻿using FarAway2._0.Entities.Enums;
using FarAway2._0.Tools.Collections;
using ModernWpf.Controls;

namespace FarAway2._0.Content.Controls
{
    internal class MyNavigationViewItem : NavigationViewItem
    {
        public TableNames DatabaseTable
        {
            get { return (TableNames)GetValue(DatabaseTableProperty); }
            set { SetValue(DatabaseTableProperty, value); }
        }

        public static readonly DependencyProperty DatabaseTableProperty =
            DependencyProperty.Register("DatabaseTable", typeof(TableNames), typeof(MyNavigationViewItem), new PropertyMetadata(TableNames.Null));



        public MainWindowActions Action
        {
            get { return (MainWindowActions)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(MainWindowActions), typeof(MyNavigationViewItem), new PropertyMetadata(MainWindowActions.Tables));


    }
}
