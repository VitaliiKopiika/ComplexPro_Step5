
//void modbus_slave( void )
word modbus_slave( Tmodbus_packet *Receive, Tmodbus_packet *Send )
{
  word start_addr, quantity, ByteCount;
  word length, i, j;
  byte ErrorCode = 0;
  int index;

  lword pAddr;
  word start, size;

  union
  {
    word  slovo;
    struct
    {
#ifdef _Union_FR
      byte b1;
      byte b2;
#else
      byte b2;
      byte b1;
#endif
    } _;
  } DATA;

  start_addr = (w)( Receive->data[0] ) ;
  start_addr = ( start_addr << 8 ) + (w)Receive->data[1] ;
  quantity   = (w)( Receive->data[2] ) ;
  quantity   = ( quantity << 8 ) + (w)Receive->data[3] ;
  Send->func = Receive->func ;

  switch ( Receive->func )
  {
    case _R_Coils :
        ErrorCode = _Code_Not_Support ;
/*                      if ( ( quantity >= 1 ) && ( quantity <= 0x07D0 ) )
                        {
                          if ( ( ( start_addr/8 ) <= kol_var ) &&
                               ( ( start_addr + quantity/8 + 1 ) <= kol_var ) )
                          {
//                           Длина вложенных данных, в байтах
                             Send->data = ( quantity + 7 ) / 8 ;

                             for ( i = 0 ; i <= s_mbf.data[0] ; i++ )
                             {
                                Send.data++;
                                Send->data[1+i] = tabl_var_tab[0][start_addr+i] ;
                             }
                             quantity = quantity - ( s_mbf.data[0] - 1 ) * 8 ;
                             Send->data[1+i] &= ~( 0xFF << quantity ) ;
//                           Подсчет длины посылки Modbus
                             length = 3 + s_mbf.data[0];
                          }
                          else
                            ErrorCode = _Address_Not_Support ;
                        }
                        else
                          ErrorCode = _Out_Of_Quantity ;
  */
        break ;

    case _R_Discrete_Inputs :
        ErrorCode = _Code_Not_Support ;
/*                      if ( ( quantity >= 1 ) && ( quantity <= 0x07D0 ) )
                        {
                          if ( ( ( start_addr/8 ) <= kol_var ) &&
                               ( ( start_addr + quantity/8 + 1 ) <= kol_var ) )
                          {
//                           Длина вложенных данных, в байтах
                             s_mbf.data[0] = ( quantity + 7 ) / 8 ;

                             for ( i = 0 ; i <= s_mbf.data[0] ; i++ )
                             {
                                s_mbf.data[1+i] = tabl_var_tab[0][start_addr+i] ;
                             }
                             quantity = quantity - ( s_mbf.data[0] - 1 ) * 8 ;
                             s_mbf.data[1+i] &= ~( 0xFF << quantity ) ;
//                           Подсчет длины посылки Modbus
                             length = 3 + s_mbf.data[0];
                          }
                          else
                            ErrorCode = _Address_Not_Support ;
                        }
                        else
                          ErrorCode = _Out_Of_Quantity ;
  */
        break;

    case _R_Holding_Registers :

        if ( ( quantity >= 1 ) && ( quantity <= 0x07D ) )
        {
          if ( ( ( start_addr ) <= kol_var ) &&
               ( ( start_addr + quantity + 1 ) <= kol_var ) )
          {
//                           Длина вложенных данных, в байтах
               length = quantity ;

//      Подсчет длины посылки Modbus
               for ( i = 0, j = 1 ; i < length ; i++, j = j+2 )
               {
				      DATA.slovo = *((w*)var_tab1[start_addr+i].addr) ;
                  Send->data[j]   = (w)DATA._.b1;
                  Send->data[j+1] = (w)DATA._.b2;
               }
               length = length * 2 ;
               Send->data[0] = (w)length ;
               length++;
          }
          else
            ErrorCode = _Address_Not_Support ;
        }
        else
          ErrorCode = _Out_Of_Quantity ;

        break;

    case _R_Input_Register :
        ErrorCode = _Code_Not_Support ;
        break;

    case _W_Single_Coils :
        ErrorCode = _Code_Not_Support ;
        break;

    case _W_Single_Register :
        ErrorCode = _Code_Not_Support ;
        break;

    case _R_Exception_Status :
        ErrorCode = _Code_Not_Support ;
        break;

    case _Diagnostic :
        ErrorCode = _Code_Not_Support ;
        break;

    case _Get_Com_Event_Counter :
        ErrorCode = _Code_Not_Support ;
        break;

    case _Get_Com_Event_Log :
        ErrorCode = _Code_Not_Support ;
        break;

    case _W_Multiple_Coils :
        ErrorCode = _Code_Not_Support ;
        break;

    case _W_Multiple_Registers :

        ByteCount = Receive->data[4] ;
        if ( ( ( quantity >= 1 ) && ( quantity <= 0x07B ) ) && ( ByteCount == ( quantity * 2 ) ) )
        {
          if ( ( ( start_addr ) <= kol_var ) &&
               ( ( start_addr + quantity + 1 ) <= kol_var ) )
          {
               length = quantity ;

               for ( i = 0, j = 5 ; i < length ; i++, j = j+2 )
               {
                  DATA._.b1 = Receive->data[j];
                  DATA._.b2 = Receive->data[j+1];
                  index = start_addr + i;
                     if ( var_tab1[index].access == _READ_WRITE_access )
                         *((w*) var_tab1[index].addr) = DATA.slovo ;
                     else
                     {
                         ErrorCode = _Address_Not_Support ;
                         break;
                     }
               }
//      Длина вложенных данных, в байтах

               Send->data[0] = Receive->data[0];
               Send->data[1] = Receive->data[1];
               Send->data[2] = Receive->data[2];
               Send->data[3] = Receive->data[3];

               length = 4 ;
          }
          else
            ErrorCode = _Address_Not_Support ;
        }
        else
          ErrorCode = _Out_Of_Quantity ;

        break;

    case _Report_Slave_ID :
        ErrorCode = _Code_Not_Support ;
        break;

    case _R_File_Record :
        ErrorCode = _Code_Not_Support ;
        break;

    case _W_File_Record :
        ErrorCode = _Code_Not_Support ;
        break;

    case _Mask_Write_Register :
        ErrorCode = _Code_Not_Support ;
        break;

    case _RW_Multiple_Registers :
        ErrorCode = _Code_Not_Support ;
        break;

    case _R_FIFO_Queue :
        ErrorCode = _Code_Not_Support ;
        break;

    case _R_Device_Ident :
        ErrorCode = _Code_Not_Support ;
        break;

	case _R_Struct :
          if (  start_addr  <= kol_var )
		  {
		     if ( ( var_tab1[start_addr].type == _CHAR_array_type ) || ( var_tab1[start_addr].type == _uCHAR_array_type ) )
			 {
                  start   = (w)( Receive->data[2] ) ;
                  start   = ( start << 8 ) + (w)Receive->data[3] ;
                  quantity   = (w)( Receive->data[4] ) ;
                  quantity   = ( quantity << 8 ) + (w)Receive->data[5] ;
				  size = var_tab1[start_addr].lon;// / sizeof(char);
				  if (( start <= size ) || ((start + quantity) <= size))
				  {
                      Send->data[0] = (quantity & 0xFF00) >> 8;
                      Send->data[1] = quantity & 0x00FF;

				      pAddr = (lword)(var_tab1[start_addr].addr);
				      pAddr = pAddr + start;
                      for ( i = 2 ; i < quantity+2 ; i++ )
                      {
                        Send->data[i] = *((b*)pAddr) ;
						(b*)pAddr++;
                      }
					  length = quantity + 2; // +length

                  }
				  else
					ErrorCode = _Out_Of_Quantity; // 0x02

			 }
			 else
				 ErrorCode = _Param_Not_Support; // 0x03;
		  }
		  else
			  ErrorCode = _Address_Not_Support; // 0x1
		  break;

    default:
        ErrorCode = _Code_Not_Support ;
        break;
  }
  if ( ErrorCode != 0 )
  {
    length = 1;
    Send->data[0] = ErrorCode;
    Send->func = Send->func + 0x80;
  }

  return length;

}


