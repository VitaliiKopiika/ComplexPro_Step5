
// + Собрать все ошибки в отдельный файл и вынести эту функцию в EroorWindow

// + не выдает ошибку если не нашла NEW_LINE
// + сделать окно выдачи всех ошибок как в VisualStudio
// + не проверяет что на входе разрывающего элемента ничего нет.

// + сделать конечный элемент - 4го типа.

// + сформировать копию поля нетворк с записанными на нем на месте коннекторов и 
// + элементов: "1" или имена коннект.переменных.
// + разрывающий элемент является поставщиком питания (как NEW_LINE) для своей цепи
// + цепь может начинаться с разрывающего элемента, но не может сквозь него проходить сквозь него ни в каком направлении.
// + цепь может заходить в разрывающий элемент справой его стороны не выходя с левой
// + цепь может проходить через неразрывающий элемент слева направо, но не может справа налево 
// + и не может присваиваивать в свою цепь справа налево (его правую сторону)
// + Сделать контроль соединения чужих цепей
// + Ищем пока только коннекторы в FindNextConnector но надо добавить и пропускающий сквозь себя элементы.
// + доделать см.коммент к switch-default и см. записи на бумаге
// + Убрать из списка найденных элементов занесенные туда конечные элементы.
// + отладить поиск вторичных цепей
// + по drag-move сделать контроль подходитли элемент на это место.
// + сделать поиск цепи из середины цепи во всех направлениях для чего сделать все коннекторы по образцу Т-коннекоторов
// + доделать обработку ошибок прирекурсии при Т-елемнентах 
// + доделать остальные Т-элементы
// + добавить добавление нетворков на поле диаграммы
// + добавить добавление колонок к полю нетворк
// + добавить у коннекторов текстовое поле состояния их выхода
// + добавить эти поля в список TextBlockov для привязки
// + добавить класс и  кнопку анализатора
// + поcле каждого добавленияэлемента пересоздать Boll-поле и привязать его переменные к этим текстовым полям и произвести анализ 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

using System.Globalization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;

namespace ComplexPro_Step5
{
    public partial class Step5
    {


//*********************************************
public class DiagramAnalysator
    {

        //***********   CONSTRUCTOR #1    

        public DiagramAnalysator()
        {
            try
            {
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.ToString());
            }
    
        }



//***************

static  public void Network_Analysator(Network_Of_Elements network)
{
    try
    {

        //TAB_CONTROL_PANEL.Items.MoveCurrentTo((TabItem)TAB_CONTROL_PANEL.FindName("Errors"));

//***************    Поиск всех первичных цепей - отходящих от всех NEW_LINE.

        network.Chains_list.Clear();

        //List<int[]> chain_error_elements_list = new List<int[]>();
        //List<string> error_list = new List<string>();

        int tst = 0;
        for (int y = 0; y < network.Network_panel_copy.Count; y++ )
        {
            List<int[]>  chain_elements_list = new List<int[]>();
            // Поиск очередного NEW_LINE.
            y = Find_NextNewLine(network, y);
            // Больше нет ни одного NEW_LINE.
            if (y == -1) break;

            tst = 1;

            //  Заносим NEW_LINE также в список: ее номер на экране будет не виден, т.к. он 
            //  не влазит в узкую ячейку, но для компилятора он будет.
            chain_elements_list.Add(new int[] { 0, y });

            string error = Find_ConnectorsChain(network, 1, y, "Right", chain_elements_list);

                // если была ошибка, то в последнием элементе коорд. ошибочного элемента.
                if (error == null || error != "End") 
                {
                    ERRORS.Add(error, network, chain_elements_list.Last());

                    //---  удаляем из массива элемент с ошибкой.
                    chain_elements_list.Remove(chain_elements_list.Last());
                }

            network.Chains_list.Add(chain_elements_list);

        }

        // Не было ни одного NEW_LINE.
        if (tst == 0 ) 
        {
            ERRORS.Add("No NEW_LINEs.", network, null );
        }

//**************  Поиск всех вторичных цепей - исходящих из элементов прерваших первичные цепи NEW_LINE.
        
        //  Столбец за столбцом сверху вниз сканируем ячейки на предмет неопознанных коннекторов - 
        // находя коннектор запускаем из него поиск цепи.

        for (int x = 1; x < network.Network_panel_copy[0].Count; x++)
        {
            for (int y = 0; y < network.Network_panel_copy.Count; y++)
            {
                //********************
                List<int[]> chain_elements_list = new List<int[]>();

                // Поиск очередного NEW_LINE.
                y = Find_NextConnector(network, x, y);
                // Нет ни одного NEW_LINE.
                if (y == -1) break;

                                                                //  елемент сам разберется с направлением.
                string error = Find_ConnectorsChain(network, x, y, null, chain_elements_list);

                    // если была ошибка, то в последнием элементе коорд. ошибочного элемента.
                    if (error == null || error != "End")
                    {
                        ERRORS.Add(error, network, chain_elements_list.Last());
                        
                        //---  удаляем из массива элемент с ошибкой.
                        chain_elements_list.Remove(chain_elements_list.Last());
                    }

                network.Chains_list.Add(chain_elements_list);
            }
        }

//************   СОЗДАЕМ МАССИВ ДЛЯ ДАЛЬНЕЙШЕЙ КОМПИЛЯЦИИ: делаем копию нетворк-поля с ячейками
//************               заполненными "1/0" или именами коннект-переменных.

        //   СЕЙЧАС ВМЕСТО "1/0" СТАВИМ ПЕРЕМЕННУЮ В КОТОРУЮ В С-КОДЕ ЕЛЕМЕНТ NEW_LINE ЗАПИШЕТ "1"
        //   В ДАЛЬНЕЙШЕМ НУЖНО ОПТИМИЗИРОВАТЬ И ЗАПИСЫВАТЬ "1" НО ЭТО ПОТРЕБУЕТ ПО ДВА ВИДА КАЖДОГО 
        //   ИСПОЛНИТЕЛЬНОГО ЭЛЕМЕНТА: ПРОВЕРЯЮЩЕГО СВОЙ EN И НЕ ПРОВЕРЯЮЩЕГО - ТОГО У КОГО НА ВХОДЕ "1".

        network.Chains_FullVars_panel_copy.Clear();
        network.Chains_Vars_panel_copy.Clear();
        network.Chains_Vars_list.Clear();

        // растягиваем массив и заполняем все элементы нулями, найденные затем затрутся номерами.
        foreach (List<Element_Data> list in network.Network_panel_copy)
        {
            List<string> str_list1 = new List<string>();
            List<string> str_list2 = new List<string>();
            foreach (Element_Data element in list)
            {
                str_list1.Add(null); //  должны быть гальв.развязанные заготовки, иначе 
                str_list2.Add(null); // потом разные массивы указывают на одно и тоже.
            }
            network.Chains_FullVars_panel_copy.Add(str_list1);
            network.Chains_Vars_panel_copy.Add(str_list2);
        }

        //  имя конект-переменной формируем из буквы и номера networka в диаграмме.
        int  num = network.DIAGRAM.NETWORKS_LIST.IndexOf(network);
        char chain_var_name = 'A';
        foreach (List<int[]> list in network.Chains_list)
        {
            foreach (int[] xy in list)
            {
                network.Chains_FullVars_panel_copy[xy[1]][xy[0]] = chain_var_name + num.ToString() + xy[0].ToString() + xy[1].ToString();
                network.Chains_Vars_panel_copy[xy[1]][xy[0]] = chain_var_name.ToString() + num.ToString();
            }

            network.Chains_Vars_list.Add(chain_var_name.ToString() + num.ToString());

            chain_var_name++;
        }


//******************  Распечатка подписей элементов и найденных ошибок для ознакомления.

        //  подписываем все элементы вопросами, найденные затем затрутся номерами.
        foreach (List<Element_Data> list in network.Network_panel_copy)
        {
            foreach(Element_Data element in list)
            {
                if (element != null)
                {
                    element.IO_TextBlocks_list.Last().Text = "???";
                }
            }
        }

        //   подписываем элементы номерами из цепей к которым они принадлежат.
        foreach (List<int[]> list in network.Chains_list)
        {
            foreach (int[] xy in list)
            {
                Element_Data element = network.Network_panel_copy [xy[1]] [xy[0]];
                //element.IO_TextBlocks_list.Last().Text = j.ToString();
                element.IO_TextBlocks_list.Last().Text = network.Chains_FullVars_panel_copy[xy[1]][xy[0]];
            }
        }

    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
    }
}



//***************   

static public int  Find_NextNewLine(Network_Of_Elements network, int start_row)
{
    try
    {
        for ( int row = start_row ; row < network.Network_panel_copy.Count; row++ )
        {
            List<Element_Data> elements_list = network.Network_panel_copy[row] ;

            if (elements_list[0] != null)
            {
                if (elements_list[0].Name == "NEW_LINE") return row;
            }
        }

        return -1;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return -1;
    }
}


//***************   Поиск незарегистрированного в списке элемента.

static public int Find_NextConnector(Network_Of_Elements network, int start_x, int start_y)                                     
{
    try
    {
        for (int y = start_y; y < network.Network_panel_copy.Count; y++)
        {
            List<Element_Data> elements_list = network.Network_panel_copy[y];
            Element_Data element;

            //  Найден элемент. Проверяем нет ли его уже в списке. Если есть то ищем следующий.

            element = elements_list[start_x];

            if (element != null)
            {   
                //  тип элемента: коннекторы.
                //  элементы пропускающие сигналы только слева направо сюда нельзя.
                // ищем все элементы, чтобы даже на прерывающих  if (element.Txt_Image.Type_int == 1 )
                {
                    //  Проверяем нет ли уже в коллекции элемента с такими же координатами.
                    int Ok = 1;
                    foreach (List<int[]> list in network.Chains_list)
                    {
                        foreach (int[] xy in list)
                        {
                            if (start_x == xy[0] && y == xy[1])
                            {
                                Ok = 0;  // такой элемент уже есть, искать следующий.
                                break;
                            }
                        }
                        
                        if ( Ok == 0 ) break;
                    }

                    if (Ok == 1) return y;
                }
            }
         
        }

        return -1;
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return -1;
    }
}

//***************  Рекурсивное распутывание сети: 
//          нахождение всех Коннекторов связанных в одну сеть без разрывов Контактными элементами и
//          формирование списка координат ячеек входящих в эту сеть.

static public string Find_ConnectorsChain(Network_Of_Elements network,  int start_x, int start_y, 
                                                                    string start_seek_direction,
                                                                    List<int[]> net_elements_xy)
{
    try
    {
        string error = null;

        int x = start_x, y = start_y;

        string seek_direction = start_seek_direction;

        Element_Data element = network.Network_panel_copy[y][x];


//*************    Проверяем в других цепях нет ли уже в коллекции элемента с такими же координатами.


                foreach (List<int[]> list in network.Chains_list)
                {
                    foreach (int[] xy in list)
                    {
                        if (start_x == xy[0] && y == xy[1])
                        {
                            net_elements_xy.Add(xy);
                            return "Connected chains";
                        }
                    }
                }


//***************   Проверяем замкнутый круг: нет в своей коллекции уже элемента с такими же координатами.

                foreach (int[] xy in net_elements_xy)
                {
                    if ( x == xy[0] && y == xy[1] )  return "End";
                }


//***************   ПОИСК

            //  Запись стоит впереди поиска - поэтому первый найденный ошибочный элемент тоже 
            // записывается в массив, а потом снаружи выводится в собщение и удаляется из списка.
            net_elements_xy.Add(new int[] { x, y });

            //  сети нет - стартовый индекс указывает на пустое поле.
            if ((network.Network_panel_copy[y])[x] == null)  error = "Empty cell";
            else
            {
                switch (element.Name)
                {
                    case "HORIZONTAL":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        //  "null' - означает что поиск начат с этого элемента, т.е. нет начального направления поиска.
                        if (seek_direction == null || seek_direction == "Right")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Left"; x--; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Left"; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Right"; x++; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }

                        break;
                    //---

                    case "VERTICAL":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Down")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Up"; y--; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Up")
                        {
                            seek_direction = "Up"; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Down"; y++; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        
                        break;
                    //---

                    case "T_UP":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Down")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break; 
                            //---
                            seek_direction = "Left"; x--; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Right")
                        {
                            seek_direction = "Up"; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Right"; y++; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Up"; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Left"; y++; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "T_DOWN":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Up")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Left"; x--; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Right")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Right"; y--; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Left"; y--; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                     case "T_RIGHT":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Up")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Up"; x--; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Up"; y--; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Down")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Right"; y--; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "T_LEFT":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Up")
                        {
                            seek_direction = "Left"; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Up"; x++; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Right")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Up"; y--; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Down")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                            if (error != "End") break;
                            //---
                            seek_direction = "Left"; y--; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "L_LEFT_UP":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Down")
                        {
                            seek_direction = "Left"; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Right")
                        {
                            seek_direction = "Up"; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "L_LEFT_DOWN":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Up")
                        {
                            seek_direction = "Left"; x--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Right")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "L_RIGHT_UP":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Down")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Up"; y--;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---

                    case "L_RIGHT_DOWN":
                        //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                        if (seek_direction == null || seek_direction == "Up")
                        {
                            seek_direction = "Right"; x++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else if (seek_direction == "Left")
                        {
                            seek_direction = "Down"; y++;
                            //---
                            error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                        }
                        else error = "Wrong direction";

                        break;
                    //---
                                            
                    default:

                        //  Прочие элементы пропускающие сигнал сквозь себя в горизонтальном направлении.
                        //------------------------------------------------------------------------

                        //  -  разрывающий элемент является поставщиком питания (как NEW_LINE) для своей цепи
                        //  -  цепь может начинаться вправо с разрывающего элемента, но не может сквозь него проходить ни в каком направлении.
                        //  -  цепь может заходить в разрывающий элемент справой его стороны не выходя с левой
                        //  -  цепь может проходить через неразрывающий элемент слева направо, 
                        //     но не может справа налево чтобы он не передал "1" с выхода сам себе на вход,
                        //     и цепь не может присваиваивать его в свою цепь справа налево (его правую сторону)
                                                

                        //  НЕРАЗРЫВАЮЩИЕ ЭЛЕМЕНТЫ ПРОПУСКАЮТ СИГНАЛ ТОЛЬКО СЛЕВА НАПРАВО ЧТОБЫ ИНАЧЕ НЕ 
                        //  БУДУТ РАБОТАТЬ ЭЛЕМЕНТЫ-ПРЕРЫВАТЕЛИ: СИГНАЛ БУДЕТ ПРОХОДИТЬ СПРАВА НА ЛЕВО И 
                        //  ЭЛЕМЕНТ БУДЕТ СРАБАТЫВАТЬ ОТ ПРОПУЩЕННОГО СКВОЗЬ СЕБЯ ЖЕ СИГНАЛА ОТ
                        //  ДРУГОГО ПРЕРЫВАТЕЛЯ В ЭТОЙ ЦЕПИ.


//***************       НЕРАЗРЫВАЮЩИЕ ЭЛЕМЕНТЫ

                        if (element.Txt_Image.Type_int == 2)
                        {
                            //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                            if (seek_direction == null || seek_direction == "Right")
                            {
                                seek_direction = "Right"; x++;
                                //---
                                error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);
                                if (error != "End") break;
                                //---
                                seek_direction = "Left"; x--; x--;
                                //---
                                error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);

                                break;
                                //---
                            }
                            else if (seek_direction == "Left") { } // конец цепи.
                            else error = "Wrong direction";
                        //---
                        }
                        else

//***************       РАЗРЫВАЮЩИЕ ЭЛЕМЕНТЫ

                        if (element.Txt_Image.Type_int == 3)
                        {
                            //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                            if (seek_direction == null)
                            {
                                seek_direction = "Right"; x++;
                                //---
                                error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);

                                break;
                                //---
                            }
                            else if (seek_direction == "Left")  // конец цепи.
                            {                //  этот элемент должен быть занесен в список:
                                return "End";//  не идем дальше чтобы не удалить этот элемент из списка.
                            }
                            else if (seek_direction == "Right") { }  // конец цепи.
                            else error = "Wrong direction";
                            //---
                        }
                        else

//***************       КОНЕЧНЫЙ ЭЛЕМЕНТ

                        if (element.Txt_Image.Type_int == 4)
                        {
                            //  Согласуется ли найденный элемент с предыдущим по направлению входа.
                            if (seek_direction == null)
                            {
                                seek_direction = "Left"; x--;
                                //---
                                error = Find_ConnectorsChain(network, x, y, seek_direction, net_elements_xy);

                                break;
                                    //---
                            }
                            else if (seek_direction == "Right")
                            {                //  этот элемент должен быть занесен в список:
                                return "End";//  не идем дальше чтобы не удалить этот элемент из списка.
                            }
                            else error = "Wrong direction";
                                //---
                        }

                        //  Конец поиска: Уперлись в элемент прерывающий цепь.

                        //  удаляем прерывающий элемент - последний занесенный элемент.
                        net_elements_xy.Remove(net_elements_xy[net_elements_xy.Count-1]);
                        
                        return "End";
                    //  здесь сделать отделение элементов разрывающих сеть от неразрывающих и для
                    //неразрывающих задавать направление Left
                    //break;
                    //---
                } //----- end of switch

            }

          return error;
        
    }
    catch (Exception excp)
    {
        MessageBox.Show(excp.ToString());
        return "Exception";
    }
}

//***********************    HANDLERS   **************************************


    }


    }  //*************    END of Class <Step5>
}

