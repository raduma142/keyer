   M S B A M L   `   `   `  ����@  <Keyer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:KeyerKeyer  5   	   3clr-namespace:Keyer.ViewModelsKeyer.ViewModels  5   	       Keyer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Hi:http://schemas.microsoft.com/expression/2010/interactivity     5   	   %localclr-namespace:Keyer     5   	   -vmclr-namespace:Keyer.ViewModels     5   	   $	��450��5
   	   $	��800��6     � Title  5	   	   ��  �� Path$ Title� � DataContext 5      *  $Keyer.ViewModels.MainWindowViewModel  5   
    J� InputBindings 5      �� 5   
   
 �� Key$ O��6    �� 	Modifiers$
 Ctrlf�6    �� Command 5      �� $ OpenImageCommand��� 5   
   $ S��6   $
 Ctrlf�6    5      �� $ SaveImageCommand��� 5   
   $ Z��6   $
 Ctrlf�6    5      �� $ UndoProcessImageCommand�.��5      ]� .+�5   
   A� $��Bottom=�6    A� Rows$ 1��5      $��Center=�5      $��2q�5      .+�5       �� $��📂 Открыть��6    � ToolTip$  Открыть (CTRL + О)��5!      ��5"      �� $ OpenImageCommand��� 5#      $"��🎨 Выбрать цвет��6   $* $Выбрать цвет кеинга��5$      ��5%      �� $ SelectColorCommand��� 5&      
���   �$��1q�5'      $��30��5(      $��30��5)      ._�5*      �� -   5        	 ��Name$	 KeyingElement��5*      $, &Текущий цвет кейинга��5,      ��5+      �� $ KeyingBrush�5,      5-      � 5.      .+�5/      �� $&�� Чувствительность��6   
 �� TextAlignment$
 Center=�50      $	��Top=�51      �� 52       � Minimum$ 0Q�53       � Maximum$	 100Q�6%   $��Bottom=�55      $
��2, 0q�56       � Value 52      �� $ KeyingSensitivity� 54      �� $ KeyingSensitivity�56      57      �� 58      $!��⚒️ Обработать��6   $1 +Обработать изображение��59      ��5:      �� $ ProcessImageCommand��� 5;      $��💾 Сохранить��6   $# Сохранить (CTRL + S)��5<      ��5=      �� $ SaveImageCommand�5>      � 5@   
   .+�5A      �� $��/Data/bg.jpg��6   $��UniformToFill=�5B      �� 5C       � Cursor$
 Hand��5D      $3 -Нажмите для выбора цвета��6   ��5C      �� $ Image� 5E      d� 5F       d� MouseAction$ 	LeftClicke�6#    5G      �� $ ClickImageCommand�5I      5J      5L      5M      