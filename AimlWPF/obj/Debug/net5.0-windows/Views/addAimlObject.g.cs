﻿#pragma checksum "..\..\..\..\Views\addAimlObject.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E728E0A09DB1E86B850D64B2121A687AF60013AE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AimlWPF.ViewModels;
using AimlWPF.Views;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AimlWPF.Views {
    
    
    /// <summary>
    /// addAimlObject
    /// </summary>
    public partial class addAimlObject : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 13 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PatternBox;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TemplateBox;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox MoodTextBox;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox categoryComboBox;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ContentTextBox;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Views\addAimlObject.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button saveButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Aiml Editor;V1.0.0.0;component/views/addaimlobject.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\addAimlObject.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.4.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 12 "..\..\..\..\Views\addAimlObject.xaml"
            ((System.Windows.Controls.Grid)(target)).Loaded += new System.Windows.RoutedEventHandler(this.setListBox_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.PatternBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 13 "..\..\..\..\Views\addAimlObject.xaml"
            this.PatternBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.updatePatternInContent);
            
            #line default
            #line hidden
            return;
            case 3:
            this.TemplateBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 14 "..\..\..\..\Views\addAimlObject.xaml"
            this.TemplateBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.updateTemplateInContent);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MoodTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 15 "..\..\..\..\Views\addAimlObject.xaml"
            this.MoodTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.mood_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.categoryComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.ContentTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 17 "..\..\..\..\Views\addAimlObject.xaml"
            this.ContentTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.content_Changed);
            
            #line default
            #line hidden
            return;
            case 7:
            this.saveButton = ((System.Windows.Controls.Button)(target));
            
            #line 20 "..\..\..\..\Views\addAimlObject.xaml"
            this.saveButton.Click += new System.Windows.RoutedEventHandler(this.saveButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
