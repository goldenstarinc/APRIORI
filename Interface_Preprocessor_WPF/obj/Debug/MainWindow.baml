   M S B A M L   `   `   `  ����U  QInterface_Preprocessor_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Interface_Preprocessor_WPFInterface_Preprocessor_WPF  5   	   +    %Interface_Preprocessor_WPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Interface_Preprocessor_WPF     5   	     � Title$  
MainWindow��5   	   $	��720��6   $
��1280��6)    � WindowStartupLocation$ CenterScreen=�66    � WindowState$ Normal=�5	   	    � 	Resources 5
         ShowMenu �� 
TargetName �� TargetProperty	 W� To g� Duration	 �� To  HideMenu��%'  &	        &	 
    �� 5   
   .��5      W� $ 
MenuBorder��6   $ (Canvas.Left)�5      $ 0X�5      $ 0:0:0.3D�6%   �� 5      $ 
MenuBorder��6   $1 +(Border.Background).(SolidColorBrush.Color)�5      $ 	#FFF3F0F0X�5      $ 0:0:0.3D�6,   5      �� 5   
   .��5      W� $ 
MenuBorder��6   $ (Canvas.Left)�5      $
 -330X�5      $ 0:0:0.3D�6(   �� 5      $ 
MenuBorder��6   $1 +(Border.Background).(SolidColorBrush.Color)�5      $ BlackX�5      $ 0:0:0.3D�6(   5      5      .��5      � 
�������.+�5   
    � -   5        $��	MainFrame��5      	  � NavigationUIVisibility$	 Hidden=�6!   �� 5   
   -   5        
������ $��79��5   *   $
��Left=�65   6P   �� 5   
   .+�5       �� -   5        
 ��Name$
 
MenuBorder��5        �� CornerRadius$ 15��6)   ���	#FFEBE7E7$	��340��6R   $	��345��6^    �� Left$
 -341��6k   $��Center=�5!   8   
 �� Top$	 203��6U   $	��Top=�6f    J� RenderTransformOrigin$ 0.5,0.5&�6~   z�5#      V� 5$      .~�5%      �� �� 5&       �� AngleX$ -0.182Q�6(   �� 5'      T� 5(       T� X$ -0.263Q�6-   5)      ._�5+      �� $��Center=�6   $	��318��6:   .+�5,      �� -   5        $&�� Выбор базы данных��5,      
 � Tag$ Page1��6N   $��Center=�6Z   $	��289��6w   ���	#FFCBCBCB$��0,0,0,1q�6�   $��Segoe UI Variable Display)�6�   $��24(�6�   $��86��6   ��6�   Z� 6   �� 5-      -   5        $,��&Просмотр базы данных��5-      $ Page2��6Q   $��Center=�6]   $	��289��6z   $��86��6�   ���	#FFCBCBCB$��0,0,0,1q�6�   $��Segoe UI Variable Display)�6�   $��24(�6  ��6�   Z� 6  �� 5.      -   5        $'��!Построение правил��5.      $ Page3��6N   $��Center=�6Z   $	��289��6w   $��86��6�   ���	#FFCBCBCB$��0,0,0,1q�6�   $��Segoe UI Variable Display)�6�   $��24(�6�   ��6�   Z� 6�   �� 5/      -   5        $��Справка��5/      $ Page4��6D   $��86��6P   $	��289��6\   $��Center=�6h   ���	#FFCBCBCB$��0,0,0,0q�6�   $��Segoe UI Variable Display)�6�   $��24(�6�   ��6�   Z� 6�   50      51      52      54      55      