   M S B A M L   `   `   `  ����C  ?Jiracoll, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:JiracollJiracoll  5   	       Jiracoll.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Jiracoll     5   	     � Title$  
MainWindow��5   	   $	��600��6   $
��1200��6)   .��5	      � $��	1,1,-1,-1q�6    � ColumnDefinitions 5
   
   �� 5      $	��43* �6   �� 5      $	��56* �6   .+�5   
   � -   5         �Name$ ProgressBar_Historie��5      $
��Left=�64   $��24��6O   $��
10,535,0,0q�6[   $	��Top=�6o   $	��792��6�   $��2��6�   6�   �� 5   
   $
��Left=�6   $	��538��60   $��	10,10,0,0q�6=   $	��Top=�6P   $
��1146��6h   $��2��6u   .@�5      �� $
��JSON��6   .��5      � ���	#FFE5E5E5 5      �� 5      $	��62* �6+   �� 5      $
��563* �6+   �� 5      $
��381* �6+   �� 5      $
��134* �6+   .+�5      �� -   5         ��Name$ TextBox_JsonPath��5      $
��Left=�68   $��49��6S   $��
391,10,0,0q�6_    �� TextWrapping$
 Wrap=�6s   $��TextBox��6�   $	��Top=�6�   $	��162��6�   $��1��6�   �� 5      -   5         ��Name$ Button_SerlectJson��5      $��CFD from Json��69   $
��Left=�6Q   $��
50,255,0,0q�6l   $	��Top=�6�   $	��211��6�   $��57��6�    J� RenderTransformOrigin$ 0.479,2.986&�6�   ���	#FFE8CB18$��24(�6  $��Lucida Bright)�6  $��2��65  �� 5      -   5        $��Read Issues from Json��5      $
��Left=�6=   $��43��6X   $��
50,383,0,0q�6d   $	��Top=�6x   $	��211��6�   ���	#FF1FE481$��Lucida Bright)�6�   $��18(�6�   $��2��6�   �� 5      -   5        $ Button_FilePath��5      $��Choose Export Filepath��66   $
��Left=�6W   $��39��6r   $��
22,359,0,0q�6~   $	��Top=�6�   $	��139��6�    �� 	IsDefault . $��2��6�   �� 5      -   5         ��Name$ TextBlock_Filepath��5       $
��Left=�6<   $��
22,428,0,0q�6W   $
��Wrap=�6k   $��
<FilePath>��6   $	��Top=�6�   $	��425��6�   $��48��6�   $��2��6�   �� 5      -   5        $ Button_JsonFile��5      $��Select Json File��66   $
��Left=�6Q   $��
22,193,0,0q�6l   $	��Top=�6�   $	��139��6�   	 �� IsCancel	 . $��40��6�   $��2��6�   �� 5      -   5        $ TextBlock_FilePathJson��5       $
��Left=�6@   $��
22,255,0,0q�6[   $
��Wrap=�6o   $��<Json File>��6�   $	��Top=�6�   $	��398��6�   $��2��6�   �� 5      -	   5        $ TextBox_Errors��5      $��2��66   $��2��6J   $
��Left=�6Z   $	��233��6u   $��	202,0,0,0q�6�   $
 Wrap=�6�   $��<found Errors>��6�   $	��Top=�6�   $	��313��6�   �� 5       $��3��6    $
��Left=�60   $��108,10,-7,0q�6K   $	��Top=�6`   $��33��6x   $	��223��6�   (  "System.Windows.Controls.DatePicker  5!      -
   5        
  Name$
 DatePicker_DateOfFile��5!   !   $��2��6@   $
��Left=�6P   $��	10,10,0,0q�6k   $	��Top=�6~      IsDropDownOpen .    SelectedDateFormat$
 Long=�6�   �� 5"      -   5        $ Button_workflowHistory��5"      $��2��6=   $��Export Status History��6Q   $��2��6q   $
��Left=�6�   $��298,332,0,0q�6�   $	��Top=�6�   $	��195��6�   $��57��6�   $��16(�6�   $��Lucida Bright)�6�   ���	#FF8ECDFF
���   ����	#FF9967676L  �� 5#      -   5        $ Button_SelectWorkflowFile��5#      $��2��6@   $��Select Workflow��6T   $
��Left=�6n   $��	22,96,0,0q�6�   $	��Top=�6�   $	��139��6�   $��32��6�   6�   �� 5$      -   5        $  TextBlock_WorkflowFilePath��5$       $
��Left=�6D   $��
22,146,0,0q�6_   $
��Wrap=�6s   $��	TextBlock��6�   $	��Top=�6�   $��2��6�   $	��313��6�   $��19��6�   5%      5&      �� 5'      $	��API��6   .��5(      � ���	#FFE5E5E5 5)      �� 5*      $	��10* �6+   �� 5+      $��9* �6+   �� 5,      $
��411* �6+   .+�5.      �� -   5        $ TextBox_User��5.      $
��Left=�64   $��23��6O   $��1,25,0,0q�6[   $
 Wrap=�6m   $	��Top=�6�   $	��134��6�   $��2��6�   G� 5/      -   5         G�Name$ PasswordBox_pw��5/   "   $
��Left=�6:   $��181.923,25,0,0q�6U   $	��Top=�6m   $	��156��6�   $��23��6�   $��2��6�   �� 50      -   5        $ TextBox_Server_URL��50      $
��Left=�6:   $��23��6U   $��3,67,0,0q�6a   $
 Wrap=�6s   $	��Top=�6�   $	��296��6�   $ 0.5,0.5&�6�   $��2��6�   .��6�   z�51      V� 52      .~�53   "   �� �� 54   "    �� AngleX$ -2.816Q�60   �� 55   "   T� 56   "    T� X$ 0.566Q�65   57      .��58   3   59      �� 5:      -   5        $ TextBox_ProjectKey��5:      $
��Left=�6:   $��26��6U   $��	0,112,0,0q�6a   $
 Wrap=�6t   $	��Top=�6�   $	��233��6�   $��project = abd��6�   $��2��6�   �� 5;      -   5        $ Button_StartCollect��5;      $��CFD from API ��6:   $
��Left=�6R   $��83.923,186,0,0q�6m   $	��Top=�6�   $	��182��6�   $��57��6�   ���	#FF15E043$��20(�6�   $��Lucida Bright)�6�   $
��Bold"�6  #��� ��6"  $��2��6l  5<      5=      5>      5@      5A      