﻿/*
 * creation date  13 jan 2021
 * last change    26 jun 2021
 * author         artur
 */
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;

class _SystemArrayANDSystemCollectionsIComparerANDSystemCollectionsGenericIComparerANDSystemStringComparer
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        SystemArrayANDSystemCollectionsIComparerANDSystemCollectionsGenericIComparerANDSystemStringComparer();

        Console.ReadLine();
    }
    static void SystemArrayANDSystemCollectionsIComparerANDSystemCollectionsGenericIComparerANDSystemStringComparer()
    {
        Console.Write(">->->->->->->->->->->->->->->->->->->   ");
        Console.WriteLine("SystemArrayANDSystemCollectionsIComparerANDSystemCollectionsGenericIComparerANDSystemStringComparer()\n");


        // Абстрактный класс System.Array - это батя всех array'ев в .NET, т.к. все массивы (что делаются посредсством подставления [] к имени
        //   типа) - это его производные. System.Array поставляет комплект общих для всех массивов членов (вроде методов сортировки)
        //   /////////after reading///////////////////////////////////////////////////////////////////////
        //   // (или свойства Length)
        //   /////////////////////////////////////////////////////////////////////////////////////////////
        // Вообще, местные массивы сильно отличаются от привычных Си-стайл массивов, т.к. они сами по себе -
        //   объекты, и что-то весят. Т.к. System.Array - это ссылочный тип (ведь это класс), размещаются массивы всегда в управляемой куче. За
        //   помятью т.н. overhead'а (память, что занимается самим массивом без элементов) следует цепочка байтов, занимаемых самими элементами
        //   . От того, что хранит массив - value или ссылочные типы - напрямую зависит устройство этой самой цепочки элементов. Если массив
        //   хранит ссылочные типы, то его элементы будут ссылками. "А если массив хранит value type, то и сам массив будет размещён в стёке?"
        //   - можешь предоположить ты. Но нет, нет такой возможности в CLR размещать ссылочные типы в стёке (правда, в .NET есть). Если же
        //   массив должен хранить value типы (т.е. структуры), то эта цепочка памяти в управляемой куче будет хранить именно структуры (они то
        //   могут размещаться в куче)
        //
        // Массивы значений (т.е. массивы в value типами) - это весьмя эффективная штука, ведь для GC они считаются одним большим объектом!
        /////////after reading///////////////////////////////////////////////////////////////////////
        // System.Array хоть и не находится в System.Collections, но всё-равно считается коллекцией, т.к. реализует IList
        /////////////////////////////////////////////////////////////////////////////////////////////


        // Массивы имеют не могут менять свою длину
        //   /////////after reading///////////////////////////////////////////////////////////////////////
        //   // в отличие от типов из System.Collections
        //   /////////////////////////////////////////////////////////////////////////////////////////////
        //
        // Максимальный размер массива - 4 миллиарда элементов на всех измерениях. Максимальный индекс одного имзерения - 0X7FEFFFFF (это
        //   2146435071), или 0X7FFFFFC7, т.е. 2147483591, для массива, элементы которого занимают 1 байт (подходят только структуры)
        // В .NET Framework'е (который уже умер) по-умолчанию вес массива не мог превышать 2GiB. В x64 системах это ограничение можно было
        //   обойти, вписав    <gcAllowVeryLargeObjects enabled="true" />    в теле <configuration><runtime> .config файла
        // Массив может иметь максимум 32 измерения

        // Хоть это и открытый абстрактный класс, только система и компилятор способны напрямую наследовать от этого класса
        //
        // Вообще, в неуправляемом мире (мире динозавров, мире с повсеместным UB (Undefined Behavior)) массивы разделяются на статические и
        //   динамические. Статические - это те, чья длина известна ещё во время компиляции, и поэтому они размещаются в стёке как обычные
        //   переменные и мало чем от них отличаются. Динамические - это те, для которых программист сам вручную выделяет память через
        //   обращение к куче. Это нужно, когда ты не знаешь, какого именно размера тебе нужен массив. Массивы System.Array - динамические


        // В C# есть аж 4-е способа инициализации массива

        int[] nums1 = new int[4] { 1, 2, 3, 5 };                     // int[..] { .. } - полная запись. Здесь указан и тип, и размер массива.
        string[] nums2 = new string[] { "Tom", "Tommy", "Thomas" };  //   Число элементов в { .. } всегда должно совпадать с числом в [..]
        bool[] nums3 = new[] { false, true, true };                  // int[] { .. } - здесь указан тип, но не размер массива. Размер выведет
        float[] nums4 = { 72.9F, 90.0F, 5.5F };                      //   компилятор на основе числа элементов в { .. }
                                                                     // [] { .. } - здесь не указан ни тип, ни размер массива. Всё это выведет
                                                                     //   компилятор на основе тех же значений { .. } (размер массива задать
                                                                     //   таки не получится)
                                                                     // { .. } - это, фактически, сокращённая запись new[] { .. }. Работает она
                                                                     //   только при инициализации (только после =), и нигде больше
                                                                     //
        int[] nums5 = new int[4];                                    // int[..] - вот так можно просто выделить память под массив. В этом
                                                                     //   случае все элементы получат своё default значение (у нас получится
                                                                     //   массив из 4-ёх 0-ей (массив ссылочного типа будет заполнен null'ами)


        var varArray = new[] { "peace", null, "world" };  // var - можно подумать, что var может привестись к object, но это не так (получится
                                                          //   string массив. Готов спорить, что компилятор отталкивается от типа первого
                                                          //   элемента, а дальше просто поднимается по его иерархии)


        string[] myCats = new string[] { "Musya", "Tosya" };
        Console.WriteLine(myCats.Length);                     // Length - это свойство выдаст полное число элементов со всех измерений массива


        object[] objectArray = new object[4];  // object - массивы с его типом могут хранить всё (что из .NET)
        objectArray[0] = true;
        objectArray[1] = "Costa Rica";
        objectArray[2] = 3;
        objectArray[3] = new DateTime(1821, 9, 15);
        for (int i = 0; i < objectArray.Length; i++)
        {
            Console.WriteLine($"objectArray: (type {objectArray[i].GetType()}) {objectArray[i]}");
        }
        Console.WriteLine();


        int[,] myMatrix = new int[3, 4];                     // [,] - такой массив зовётся прямоугольным (C# поддерживает 2-а вида многомерных
        for (int y = 0; y < -3; y++)                         //   массивов). В прямоугольных массивах строки в каждом измерении имеют одну и ту
        {                                                    //   же длину:
            for (int x = 0; x < -4; x++)                     //
                myMatrix[y, x] = y * x;                      //                   ┌────────────┐
        }                                                    //                 ┌  [x, x, x, x]  - 4 элемента
        for (int y = 0; y < 3; y++)                          //       3 элемента┤  [x, x, x, x]  - 4 элемента
        {                                                    //                 └  [x, x, x, x]  - 4 элемента
            for (int x = 0; x < 4; x++)                      //                   └────────────┘
                Console.Write(myMatrix[y, x] + "\t");        //
            Console.WriteLine();                             //   Следует сказать, что прямоугольный массив поддерживает больше 2-х измерений,
        }                                                    //   а элементы в памяти выстрояны друг за другом (т.е. в нашем случае в цепочка
        Console.WriteLine();                                 //   в памяти состовляет 12 элементов)(его ещё называют линейным). Ещё знай, что
                                                             //   что такого рода массивы - это всё-ещё массивы System.Array, хранящие элементы
                                                             //   int, а не массивы System.Array, хранящие другие массивы System.Array.
                                                             //   Многомерность в них эмулируется самими средствами System.Array (а если точнее
                                                             //   - оператором this[])
                                                             //
                                                             //
        int[][] myJaggArray = new int[5][];                  // [][] - это jagged (зубчатый или ступенчатый) массив. Он гораздо ближе к
        myJaggArray[0] = new int[] { 9, 25, 12, 52, 28 };    //   массивам Си, т.к. это уже массив массивов. Это значит, что внутренние массивы
        myJaggArray[1] = new int[] { 1, 2, };                //   могут иметь проивзольную длину:
        for (int y = 2; y < myJaggArray.Length; y++)         //
        {                                                    //                   ┌────────────┐
            myJaggArray[y] = new int[5 + y];                 //                 ┌  [x, x, x]     - 3 элемента
            for (int x = 0; x < myJaggArray[y].Length; x++)  //       3 элемента┤  [x, x, x, x]  - 4 элемента
                myJaggArray[y][x] = 50 + y * 10 + x;         //                 └  [x, x]        - 2 элемента
        }                                                    //                   └────────────┘
        for (int y = 0; y < myJaggArray.Length; y++)         //
        {
            for (int x = 0; x < myJaggArray[y].Length; x++)
                Console.Write(myJaggArray[y][x] + "\t");
            Console.WriteLine();
        }
        Console.WriteLine();


        // Сам по себе класс System.Array не имеет ни индексаторного метода, ни конструкторов


        string[] favorites = { "Peter Gabriel", "Dead Or Alive", "Shakira", "Boney M" };
        Array.Reverse(favorites);                     // Reverse() - статический void метод Array, переворачивающий массивы. Ммеется
        Console.Write("The array after reverse:  ");  //   перегрузка для задания участка в массиве, только который у будет реверсирован)
        for (int i = 0; i < favorites.Length; i++)
        {
            Console.Write(favorites[i] + ",  ");
        }
        Console.WriteLine();


        Array.Clear(favorites, 1, 2);                 // Clear() - переводит в default заданный участок массива (да, для него перегрузку решили
        Console.Write("The array after clear:    ");  //   не создавать)
        for (int i = 0; i < favorites.Length; i++)
        {
            Console.Write(favorites[i] + ",  ");
        }
        Console.Write("\n\n");


        int[,] nums2D = new int[,] { { 1, 2, 3, 4 },
                                     { 9, 8, 7, 6 } };
        Console.WriteLine("0th dimensional length of {0} is {1}", nameof(nums2D), nums2D.GetLength(0));
        Console.WriteLine("1th dimensional length of {0} is {1}", nameof(nums2D), nums2D.GetLength(1));
        //                          // nums2D.GetLength() - этот метод есть только у многомерных прямоугольных массивов, и он выдаёт длину
        nums2D.GetLongLength(1);    //   заданного по индексу измерения (в виде int)
                                    // nums2D.GetLongLength() - то же самое, но выдаёт это число в виде long


        Array.Sort(new int[] { 4, 2, 9, 0 });  // Sort<T>() - общий для всех массивов класс Array имеет статический метод Sort(), сортирующий
        // output: { 0, 2, 4, 9 }              //   массив. Имеется аж 17 версий
        //                                     /////////after reading///////////////////////////////////////////////////////////////////////
        //                                     //   при условии, что хотя бь один из его объектов поддерживал IComparable
        //                                     /////////////////////////////////////////////////////////////////////////////////////////////
        // Этот Sort<T> не даёт возможности сравнивать числа по каким-то другим правилам, но что если тебе это требуется? Неужели придётся
        //   делать отдельную функцию для решения этой проблемы? Не в этот раз! Специально для такой проблемы придумали интерфейс
        //   System.Collections.IComparer, с которым работают подобного рода методы, основанные на сравнение. Суть в том, что это каждый тип,
        //   поддерживающий это интерфейс, будет иметь по своей реализации сравнивающего метода. Получается, что сколько механизмов для
        //   сравнения тебе нужно, столько типов ты и создаёшь. Одна из перегрузок метода Sort<T> также принимает объект этого интерфейса
        //
        //       interface IComparer
        //       {
        //          int Compare(object o1, object o2);  // Compare() - этот метод должен сравнивать несколько объектов. Что возвращает
        //       }                                      //   Compare()? Общепринято, что если сравнивающий метод возвращает число меньше нуля,
        //                                              //   то 1-ый объект как бы меньше другого, если возвращается 0-ль, то объекты
        //                                              //   равнопраны. Если же вернувшийся int больше 0-ля, то 1-ый объект - выше (обычно
        //                                              //   используются просто -1, 0, 1)
        //
        // У IComparer есть обобщённый аналог - System.Collections.Generic.IComparer
        // Как автор говорит, интерфейс IComparer обычно не реализуется типом, объекты которого сравниваются. Вместо этого используются
        //   вспомогательные классы, по одному на механизм
        //
        Array.Sort(new string[] { "musya", "Tosya" }, StringComparer.OrdinalIgnoreCase);
        //                                     // System.StringComparer - .NET'чики решили выделить отдельный класс, хранящий несколько готовых
        //                                     //   comparer'ов для строк. Этот класс реализует IComparer<string>, а его статические свойства
        //                                     //   возвращают готовые объект его же типа (в нём даже есть простенький конструктор! Но он не
        //                                     //   нужен)


        //*****Array.Resize() (правда, для массива существует метод Array.Resize<>, но он просто строит
        //   новый массив на основе принимаемого)


        Array arr = new int[] { 1, 2, 3 };
        int[] arr1 = new int[10];
        //
        arr.CopyTo(arr1, 0);  // arr.CopyTo() - этот метод копирует содержимое arr'а в массив arr1, начиная с заданного индекса. Есть 2-е
                              //   версии. Эта принимает index в виде int, 2-ая - в виде long


        /////////after reading///////////////////////////////////////////////////////////////////////
        // 
        int[] myInts = new int[] { 1, 2, 4, 5, 5, 9 };
        //
        Array.Find(myInts, (n) => n % 3 == 0);  // System.Array.Find() - этот static метод принимает массив и делегат. Возвратится первый же
        // output: 9                            //   элемент, к которому отправленная в делегате функция возвратит true
        //
        //
        Array.FindAll(myInts, (n) => n % 2 == 0);  // System.Array.FindAll() - а этот идёт дальше и выдаёт вообще все подходящие элементы
        // output: { 2, 4 }
        //
        //
        Array.FindIndex(myInts, (n) => n % 5 == 0);  // System.Array.FindIndex() - этот метод уже выдаёт индекс первого подходящего элемента
        // output: 3
        //
        //
        Array.FindLast(myInts, (n) => n % 2 == 0);  // System.Array.FindLast() - этот выдаст первый подходящий элемент с конца
        // output: 4
        //
        //
        Array.FindLastIndex(myInts, (n) => n % 3 == 1);  // System.Array.FindLastIndex() - как ..FindIndex(), но ищет с конца
        // output: 2
        /////////////////////////////////////////////////////////////////////////////////////////////


        Console.Write("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   ");
        Console.WriteLine("SystemArrayANDSystemCollectionsIComparerANDSystemCollectionsGenericIComparerANDSystemStringComparer()");
    }
}