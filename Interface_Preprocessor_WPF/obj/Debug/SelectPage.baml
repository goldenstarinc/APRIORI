   M S B A M L   `   `   `  ����U  QInterface_Preprocessor_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Interface_Preprocessor_WPFInterface_Preprocessor_WPF  5      +    %Interface_Preprocessor_WPF.SelectPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Interface_Preprocessor_WPF     5        N� Title$  
SelectPage��5	       N� 
Background
 �����.��5      �  � RowDefinitions 5   
   �� 5      $
��205* �6   �� 5      $
��114* �6   �� 5      $	��77* �6   �� 5      $
��111* �6   �� 5      $
��213* �6    � ColumnDefinitions 5   
   �� 5      $	��57* �6   �� 5      $
��199* �6   .+�5   
   �� $
��Left=�6   $��69��6/   $��25,0,0,0q�6;   $
��Wrap=�6M   $	��Top=�6a   $
��1245��6y   $��Segoe UI Variable Display)�6�   $��45(�6�   $
��Bold"�6�   
���   �$��2��6�   �� 6�   $�> ��6�   ���	#FF00ACFF6  �� 6   � Language$ ru-ru�6#  $&� Выбор базы данных��64  6P  �� 5   
   $
��Left=�6   $��57��6/   $��
25,147,0,0q�6;   $
��Wrap=�6O   $	��Top=�6c   $
��1091��6{   $(��"Segoe UI Variable Display Semibold)�6�   $��35(�6�   $��2��6�   �� 6�   $,�&Выберите базу данных��6�   �� 6�   $�, ��6  �� 6  $ ru-ru�6  $@�:содержащую метаданные исходной��6"  �� 6J  $�:��6N  6Z  �� 5   
   -   5        $��MetaDBPath_TextBox��5      $��60��6,   $��	9,0,81,15q�68    �� TextWrapping$
 Wrap=�6K   $��Bottom=�6_   ���	#FF00ACFF �� SelectionBrush �	#FF00ACFF$��0,0,0,1q�6�   $��24(�6�   $��Cascadia Code SemiBold)�6�   
���   �$��1��6"  $��1��62  ��6z   Z� 62  �� 5   
   ���	#FF00ACFF$��3q�6)    �� CornerRadius$ 30��6=   $��25,15,11,15q�6O   $��1��6d   ._�5      �� -   5        $��ChooseMetaDB_Button��5      $��Выбрать��60   $��Center=�6B   $��64��6_   $��Center=�6k   $	��217��6�   $��Segoe UI Variable Text)�6�   $��25(�6�   ��6�   Z� ��6�   Z� 6�   5      �� 5   
   $
��Left=�6   $��47��6/   $��	25,23,0,0q�6;   $
��Wrap=�6N   $	��Top=�6b   $	��739��6z   $(��"Segoe UI Variable Display Semibold)�6�   $��35(�6�   $��2��6�   $��2��6�   �� 6�   $ ru-ru�6�   $>�8Выберите исходную базу данных:��6�   6$  �� 5   
   -   5        $��PathDB_TextBox��5      $��
9,41,81,10q�6(   $
 Wrap=�6<   ���	#FF00ACFF �	#FF00ACFF$��0,0,0,1q�6�   $��24(�6�   $��Cascadia Code SemiBold)�6�   
���   �$��1��6�   $��3��6  ��6P   Z� 6  �� 5   
   ���	#FF191919$��3q�6)   $ 30��6=   $��	0,0,31,32q�6O   $��1��6b   $��4��6r   $��69��6   $��Bottom=�6�   $��Right=�6�   $	��266��6�   ._�5       �� -   5        $��Save_Button��5       $��Сохранить��6(   $��Center=�6<   $��58��6Y   $��Center=�6e   $	��219��6�   $��Segoe UI Variable Text)�6�   $��25(�6�   $
��Bold"�6�   ��6�   Z� ��6�   Z� 6�   5!      �� 5"   
   ���	#FF00ACFF$��3q�6)   $ 30��6=   $��25,17,11,10q�6O   $��3��6d   ._�5#      �� -   5        $��ChooseDB_Button��5#      $��Выбрать��6,   $��Center=�6>   $��64��6[   $��Center=�6g   $	��215��6�   $��Segoe UI Variable Text)�6�   $��25(�6�   ��6�   Z� ��6�   Z� 6�   5$      5&      5'      