﻿#pragma checksum "..\..\ConstructRule.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B06DF4B91BF351869CC81D8ACA3EF5D5043BBBFF3A4F86C21721BEF9CAC4061A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Interface_Preprocessor_WPF;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace Interface_Preprocessor_WPF {
    
    
    /// <summary>
    /// ConstructRule
    /// </summary>
    public partial class ConstructRule : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 119 "..\..\ConstructRule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateRules_Button;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\ConstructRule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateSingleRules_Button;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\ConstructRule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateAllRules_Button;
        
        #line default
        #line hidden
        
        
        #line 127 "..\..\ConstructRule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutputRunningTime_TextBox;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\ConstructRule.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutputRules_TextBox;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Interface_Preprocessor_WPF;component/constructrule.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ConstructRule.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.CreateRules_Button = ((System.Windows.Controls.Button)(target));
            
            #line 119 "..\..\ConstructRule.xaml"
            this.CreateRules_Button.Click += new System.Windows.RoutedEventHandler(this.CreateRules_Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CreateSingleRules_Button = ((System.Windows.Controls.Button)(target));
            
            #line 122 "..\..\ConstructRule.xaml"
            this.CreateSingleRules_Button.Click += new System.Windows.RoutedEventHandler(this.CreateSingleRules_Button_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CreateAllRules_Button = ((System.Windows.Controls.Button)(target));
            
            #line 125 "..\..\ConstructRule.xaml"
            this.CreateAllRules_Button.Click += new System.Windows.RoutedEventHandler(this.CreateAllRules_Button_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.OutputRunningTime_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.OutputRules_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

