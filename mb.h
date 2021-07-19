#ifndef  _MB_H
  #define  _MB_H

// Типы запросов (функции)
#define     _R_Coils                1
#define     _R_Discrete_Inputs      2
#define     _R_Holding_Registers    3
#define     _R_Input_Register       4
#define     _W_Single_Coils         5
#define     _W_Single_Register      6
#define     _R_Exception_Status     7
#define     _Diagnostic             8
#define     _Get_Com_Event_Counter  11
#define     _Get_Com_Event_Log      12
#define     _W_Multiple_Coils       15
#define     _W_Multiple_Registers   16
#define     _Report_Slave_ID        17
#define     _R_File_Record          20
#define     _W_File_Record          21
#define     _Mask_Write_Register    22
#define     _RW_Multiple_Registers  23
#define     _R_FIFO_Queue           24
#define     _R_Device_Ident         43
#define		_R_Struct				71

// Коды ошибок
#define     _Code_Not_Support       1   // Код функции не поддерживается
#define     _Address_Not_Support    2   // Адрес регистра < 1 и > допустимого адреса
#define     _Out_Of_Quantity        3   // Количсетво битов (регистров) < 1 и > допустимого
#define     _Param_Not_Support      4   // Регистр не поддерживает эту функцию

typedef struct
{
  word func;
//  byte *data;
  word data[0x40];
} Tmodbus_packet;


#endif  //  _MB_H



