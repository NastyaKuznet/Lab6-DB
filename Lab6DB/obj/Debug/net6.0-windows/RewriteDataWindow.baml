   M S B A M L   `   `   `  ����A  =Lab6DB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:Lab6DBLab6DB  5   	       Lab6DB.RewriteDataWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Lab6DB     5   	     � Title$  RewriteDataWindow��5   	   $	��450��6#   $	��800��60   .��5	      � $��20q�6    � ColumnDefinitions 5
   
   �� 5      $
��0.8* �6   �� 5      $��* �6   �� 5      $
��0.4* �6   �� 5      $
��0.4* �6   �� 5      $��70 �6    � RowDefinitions 5   
   �� 5      $��0.075* �6   �� 5      $��* �6   �� 5      $
��0.1* �6   �� 5      $
��0.1* �6   .+�5   
   �� $��0��6   $��5��6$   $��0��68   ��6E   �� *	NameTable+&   System.Windows.Controls.DataGrid  5   
   $��0��6   $��5��6#   $��1��67   ��6D   �� *SelectedTable+�� 5   
   $��0��6   $��2��6#   $��2��60   $��Окно ошибок��6A   .��5      �� ��6   �� *ContentErrorWindow+5      �� 5   
   $��1��6   $��2��6$   $F��@Выберете строку по первому столбцу��61   �� 5    
   $��1��6   $��3��6#   ��60   �� *CollectDelete+ �� SelectedItem 6V   �� *SelectedDelete+�� 5!   
   $��2��6   $��3��6!   $��Удалить��6.   ��6@   �� *Delete+�� 5#   
   $��4��6   $��3��6!   $��Сохранить��6.   ��6B   �� *Save+5$      5%      