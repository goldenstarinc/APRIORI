   M S B A M L   `   `   `  ����U  QInterface_Preprocessor_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Interface_Preprocessor_WPFInterface_Preprocessor_WPF  5      )    #Interface_Preprocessor_WPF.ViewPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:Interface_Preprocessor_WPF     5        N� Title$  ViewPage��5	      .��5      � 
������� � RowDefinitions 5   
   �� 5      $	��41* �6   �� 5      $	��41* �6   �� 5      $
��278* �6    � ColumnDefinitions 5   
   �� 5      $
��629* �6   �� 5      $
��311* �6   �� 5      $
��340* �6   .+�5   
   �� $
��Left=�6   $��
25,10,0,13q�6/   $
��Wrap=�6C   $	��521��6W   $(��"Segoe UI Variable Display Semibold)�6c   $��35(�6�   $��1��6�   �� 6�    � Language$ ru-ru�6�   $8�2Какую базу данных показать?��6�   6�   �� 5   
   $
��Left=�6   $��69��6/   $��25,0,0,0q�6;   $
��Wrap=�6M   $	��Top=�6a   $
��1245��6y   $��Segoe UI Variable Display)�6�   $��45(�6�   $
��Bold"�6�   
���   �$��3��6�   �� 6�   ���	#FF00ACFF$�> ��6  �� 6  $ ru-ru�6#  $�Просмотр базы��64  �� 6K  $� данных��6O  6a  �� 5   
   ���	#FF00ACFF$��3q�6)    �� CornerRadius$ 30��6=   $��15,10,16,13q�6O   $��1��6d   $��1��6t   ._�5      �� -   5        $��ShowMetaDB_Button��5      $��С метаданными��6.   $��Center=�6F   $��50��6c   $��Center=�6o   $	��217��6�   $��Segoe UI Variable Text)�6�   $��25(�6�   ��6�   Z� ��6�   Z� 6�   5      �� 5   
   ���	#FF00ACFF$��3q�6)   $ 30��6=   $��20,10,40,13q�6O   $��2��6d   $��1��6t   ._�5      �� -   5        $��ShowDB_Button��5      $��Основную��6*   $��Center=�6=   $��50��6Z   $��Center=�6f   $	��217��6�   $��Segoe UI Variable Text)�6�   $��25(�6�   ��6�   Z� ��6�   Z� 6�   5      &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dataGrid��5      $��25,10,25,23q�6%   $��18(�6e   $��Segoe UI Variable Display)�6s   $��2��6�   $��3��6�   ��6�   Z� 5       5!      