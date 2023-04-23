/*
 * PageFabric.
 * Copyright Sébastien Bouez. All Rights Reserved.
*/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Coho.UI.Controls.Ribbon
{
    internal class RibbonColorSplitButton : Button, IRibbonCommandWithChildren, IRibbonCommand, IColorSupportedButton
    {
        private RoutedEventHandler onClick;
        private Popup _popup;
        private bool _isLoaded;

        internal Entities.ComplexColorItem CurrentColor
        {
            get;
            private set;
        }

        internal event EventHandler<Entities.ComplexColorItem> SelectionChanged;

        public static readonly DependencyProperty DescriptionProperty =
                    DependencyProperty.RegisterAttached("Description", typeof(string), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DisplayProperty =
                    DependencyProperty.RegisterAttached("Display", typeof(RibbonEnums.RibbonButtonDisplay), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(RibbonEnums.RibbonButtonDisplay.IconAndText, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty DropDownContentProperty =
                                DependencyProperty.Register("DropDownContent", typeof(object), typeof(RibbonColorSplitButton), null);

        public static readonly DependencyProperty GestureProperty =
            DependencyProperty.RegisterAttached("Gesture", typeof(string), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Brush), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty IsInQATProperty =
            DependencyProperty.RegisterAttached("IsInQAT", typeof(bool), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LockEnabledStateProperty =
                   DependencyProperty.RegisterAttached("LockEnabledState", typeof(bool), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty TextProperty =
                    DependencyProperty.RegisterAttached("Text", typeof(string), typeof(RibbonColorSplitButton), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        public RibbonColorSplitButton()
        {
            SetValue(ToolTipService.ShowOnDisabledProperty, true);

            Loaded += RibbonColorSplitButton_Loaded;
            Click += RibbonToggleSplitButton_Click;
        }

        private void RibbonColorSplitButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isLoaded)
            {
                _isLoaded = true;

                ApplyTemplate();
                _popup = (Popup)Template.FindName("OverflowPopup", this);

                DropDownContent = new RibbonColorSplitButtonDropDownContent()
                {
                    Owner = this
                };

                (DropDownContent as RibbonColorSplitButtonDropDownContent).SelectionChanged += RibbonColorSplitButton_SelectionChanged;
            }
        }

        private void RibbonColorSplitButton_SelectionChanged(object sender, Entities.ComplexColorItem e)
        {
            _popup.IsOpen = false;
            CurrentColor = e;
            SelectionChanged?.Invoke(this, e);
        }

        event RoutedEventHandler IRibbonCommand.OnClick
        {
            add
            {
                onClick += value;
            }
            remove
            {
                onClick -= value;
            }
        }

        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);
            }
        }

        public RibbonEnums.RibbonButtonDisplay Display
        {
            get
            {
                return (RibbonEnums.RibbonButtonDisplay)GetValue(DisplayProperty);
            }
            set
            {
                SetValue(DisplayProperty, value);
            }
        }

        public object DropDownContent
        {
            get
            {
                return GetValue(DropDownContentProperty);
            }
            set
            {
                SetValue(DropDownContentProperty, value);
            }
        }

        public string Gesture
        {
            get
            {
                return (string)GetValue(GestureProperty);
            }
            set
            {
                SetValue(GestureProperty, value);
            }
        }

        public Brush Icon
        {
            get
            {
                return (Brush)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public bool IsInQAT
        {
            get
            {
                return (bool)GetValue(IsInQATProperty);
            }
            set
            {
                SetValue(IsInQATProperty, value);
            }
        }

        public bool LockEnabledState
        {
            get
            {
                return (bool)GetValue(LockEnabledStateProperty);
            }
            set
            {
                SetValue(LockEnabledStateProperty, value);
            }
        }

        public IRibbonCommand OriginalCommand
        {
            get;
            set;
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private void RibbonToggleSplitButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is ToggleButton btn && btn.Name == "toggleButton")
            {
                e.Handled = true;
            }
            else
            {
                SelectionChanged?.Invoke(this, CurrentColor);
            }

            onClick?.Invoke(this, e);
        }

        public void SetColor(Color color)
        {
            ApplyTemplate();

            Border bdr = (Border)Template.FindName("colorIndicator", this);
            if (bdr != null)
            {
                bdr.Background = new SolidColorBrush(color);
            }
        }


        public object GetDropDownContent()
        {
            return DropDownContent;
        }

        public void SetDropDownContent(UIElement content)
        {
            DropDownContent = content;
        }

        public List<IRibbonCommand> GetSubCommands()
        {
            return default;
        }

        public void RaiseClick()
        {
            RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}