   M S B A M L   `   `   `  ����F  BZipWrapper2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ZipWrapper2ZipWrapper2  5   	       ZipWrapper2.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ZipWrapper2     5   	     � Title$  Zip/Unzip Wrapper��5   	   $	��325��6#   $	��400��60   .��5	      � .+�5
   
   �� $��1��6   $��1��6"   $��10q�62   .+�5      �� $ ��Drag and drop a file here:��6   $��10,10,10,10q�6:   �� 5      -   5         ��Name$ FileDropBorder��5      
�������$��2q�6@   $	��100��6T    J� 	AllowDrop$
 True��6a   ._�5      � .+�5      �� 
������ 
�������$��2��6A   $��0,0,0,0q�6U    �� RadiusX$ 5��6f    �� RadiusY$ 5��6r   $
 True��6~   �� 5      -   5         ��Name$ FileDropTextBlock��5       $��Drop File Here��6;   $��Center=�6Q   $��Center=�6l   $
��Wrap=�5        J� IsHitTestVisible$ False��64   5      5      �� 5      $��Enter Password:(optional)��6   $��0,10,0,0q�69   $
��Left=�6K   � 5      $
��Left=�6   .+�5      G� -   5         G�Name$ PasswordBox��5      $	��300��63   $��30��6?   $��0,0,0,0q�6K   6\   �� 5      -   5         ��Name$ PlainTextPasswordBox��5      $	��300��68   $��30��6D   $��0,0,0,0q�6P   $y�	Collapsed=�6a   6x   5      �� 5       -   5        $��Show Password��5       $��0,5,0,0q�6/   $
��Left=�6@   6{   �� 5#      -   5        $��Execute��5#      $	��100��6'   $��30��63   $��0,10,0,0q�6?   $
��Left=�6Q   6l   5$      5%      5&      