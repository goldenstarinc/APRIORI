   M S B A M L   `   `   `  ����U  QInterface_Preprocessor_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Interface_Preprocessor_WPFInterface_Preprocessor_WPF  5      5    /Interface_Preprocessor_WPF.RuleConstructionPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Interface_Preprocessor_WPF     5        N� Title$  RuleConstructionPage��5	      .��5      � 
������� � ColumnDefinitions 5   
   �� 5      $
��168* �6   �� 5      $	��65* �6   �� 5      $	��87* �6    � RowDefinitions 5   
   �� 5      $	��68* �6   �� 5      $
��365* �6   �� 5      $
��287* �6   .+�5   
   �� $
��Left=�6   $��68��6/   $��25,0,0,0q�6;   $
��Wrap=�6M   $��Center=�6a   $
��1245��6|   $��Segoe UI Variable Display)�6�   $��45(�6�   $
��Bold"�6�   
���   �$��3��6�   �� 6�   $�> ��6�   ���	#FF00ACFF6	  �� 6"   � Language$ ru-ru�6&  $]�WПостроение ассоциативных правил по базе данных��67  6p  �� 5   
   $��4q�6    �� CornerRadius$ 30��6%   $��
25,22,10,6q�67   ���	#FF00ACFF$��1��6c   ._�5      �� $
��Left=�6   $��53��63   $
��Wrap=�6?   $1��+Характеристики правил >��6S   $	��Top=�6u   $	��550��6�   $(��"Segoe UI Variable Display Semibold)�6�   $��35(�6�   
���   �$��	24,20,0,0q�6�   5      �� 5   
   $
��Left=�6   $	��127��6/   $��
56,106,0,0q�6<   $
��Wrap=�6P   $	��Top=�6d   $	��341��6|   $��Segoe UI Variable Text)�6�   $��24(�6�   $��1��6�   �� 6�   $ ru-ru�6�   $�Качество��6�   �� 6�   �� 6�   $ ru-ru�6�   $7�1Достаточная достоверность��6  �� 63  �� 6?  $ ru-ru�6C  $6�0Достаточная длина посылки��6T  �� 6w  �� 6�  $ ru-ru�6�  $�Номер правила��6�  6�  �� 5   
   ���	#FF00ACFF$��3q�6)   $ 30��6=   $��
364,0,10,6q�6O   $��80��6c   $��Bottom=�6o   $��1��6�   ._�5      �� -   5         ��Name$ Confirm_Button��5      $��Задать��6-   $��Center=�6>   $��60��6[   $��Center=�6g   $	��290��6�   $��Segoe UI Variable Text)�6�   $��24(�6�   $
��Bold"�6�   ��6�   Z� ��6�   Z� 6�   5      �� 5   
   $��4q�6   $ 30��6%   $��28,12,12,26q�67   ���	#FFF3F0F0���	#FFF3F0F0$��2��6{   $��2��6�   ._�5      �� $
��Left=�6   $��45��63   $
��Wrap=�6?   $,��&Пилик! Вам сообщение:��6S   $	��Top=�6p   $	��535��6�   $(��"Segoe UI Variable Display Semibold)�6�   $��35(�6�   $
��Bold"�6�   
���   �$��24,4,0,0q�6�   5       �� 5!   
   -   5         ��Name$ OutputMessage_TextBox��5!      $��56,69,29,37q�61    �� TextWrapping$
 Wrap=�6F   $��Segoe UI Variable Display)�6�   $��24(�6�   $��2��6�   $��2��6�    �� 
IsReadOnly . ��6Z   Z� ��6p   Z� 6�   �� 5"   
   $��4q�6   $ 30��6%   $��
10,22,20,6q�67   ���	#FF00ACFF$��1��6c   $��1��6p   $��2��6�   ._�5#      �� $
��Left=�6   $��53��63   $
��Wrap=�6?   $	��Top=�6S   $	��404��6k   $(��"Segoe UI Variable Display Semibold)�6w   $��35(�6�   $
��Bold"�6�   
���   �$��	29,20,0,0q�6�   �� 6�   $ ru-ru�6�   $)�#Список параметров >��6  6$  5$      �� 5%   
   -   5        	 ��Name$	 ParametrsList_ListBox��5%      $��38,106,46,22q�61   $��Segoe UI Variable Display)�6�   $��24(�6�   $��1��6�   $��1��6�   $��2��6�   ��6r   Z� 6�   �� 5&   
   
���   �$��3q�6%   $ 30��69   $��
9,12,20,26q�6K   $��2��6_   $��2��6l   ._�5'      �� -   5        $ CreateRule_Button��5'      $'��!Построить правила��60   $��Center=�6L   $	��145��6i   $��Center=�6v   $	��257��6�   $��Segoe UI Variable Text)�6�   $��24(�6�   $
��Bold"�6�   ��6�   Z� ��6�   Z� 6  5(      �� 5)   
   -   5        $ 	qualityTB��5)      $��30��6%   $��402,115,113,0q�61   $
 Wrap=�6H   $	��Top=�6\   $��Segoe UI Variable Display)�6t   $��24(�6�   ���	#FFF3F0F0$��
0, 0, 0, 2q�6�   $��1��6�   �� 5*   
   -   5        $ confidenceTB��5*      $��30��6(   $��402,147,113,0q�64   $
 Wrap=�6K   $	��Top=�6_   $��Segoe UI Variable Display)�6w   $��24(�6�   ���	#FFF3F0F0$��
0, 0, 0, 2q�6�   $��1��6�   �� 5+   
   -   5        $ sendingLengthTB��5+      $��30��6+   $��402,177,113,0q�67   $
 Wrap=�6N   $	��Top=�6b   $��Segoe UI Variable Display)�6z   $��24(�6�   ���	#FFF3F0F0$��
0, 0, 0, 2q�6�   $��1��6�   �� 5,   
   -   5        $ ruleNumberTB��5,      $��30��6(   $��402,207,113,0q�64   $
 Wrap=�6K   $	��Top=�6_   $��Segoe UI Variable Display)�6w   $��24(�6�   ���	#FFF3F0F0$��
0, 0, 0, 2q�6�   $��1��6�   5.      5/      