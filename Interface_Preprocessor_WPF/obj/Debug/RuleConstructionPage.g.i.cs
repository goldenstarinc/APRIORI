﻿#pragma checksum "..\..\RuleConstructionPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0B1F397499EE9348EFDA9BCE122077471CF093E12B21F0FC64E85DE773586E82"
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
    /// RuleConstructionPage
    /// </summary>
    public partial class RuleConstructionPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 28 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Confirm_Button;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox OutputMessage_TextBox;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ParametrsList_ListBox;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateRule_Button;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox qualityTB;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox confidenceTB;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox sendingLengthTB;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\RuleConstructionPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ruleNumberTB;
        
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
            System.Uri resourceLocater = new System.Uri("/Interface_Preprocessor_WPF;component/ruleconstructionpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\RuleConstructionPage.xaml"
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
            this.Confirm_Button = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\RuleConstructionPage.xaml"
            this.Confirm_Button.Click += new System.Windows.RoutedEventHandler(this.Confirm_Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.OutputMessage_TextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.ParametrsList_ListBox = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.CreateRule_Button = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\RuleConstructionPage.xaml"
            this.CreateRule_Button.Click += new System.Windows.RoutedEventHandler(this.CreateRule_Button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.qualityTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.confidenceTB = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.sendingLengthTB = ((System.Windows.Controls.TextBox)(target));
            
            #line 43 "..\..\RuleConstructionPage.xaml"
            this.sendingLengthTB.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.sendingLengthTB_TextChanged);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ruleNumberTB = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

