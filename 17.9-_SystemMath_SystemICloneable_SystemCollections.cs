/*
 * creation date  26 jun 2021
 * last change    26 jun 2021
 * author         artur
 */
using System;
using System.Collections;
using System.Collections.Generic;

class _SystemMath_SystemICloneable_SystemCollections
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");
                
        SystemMath();
        SystemICloneable();
        SystemCollections_Silent();

        Console.ReadLine();
    }
    static void SystemMath()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemMath()\n");


        // 
        Console.WriteLine("5^3 = {0}", Math.Pow(5F, 3F));  // System.Math.Pow() - возвращает x в степени y. Работает с double


        Console.WriteLine("Floor of 4.62 is {0:f}", Math.Floor(4.62));      // System.Math.Floor() - округление некого double (или decimal во
        Console.WriteLine("Ceiling of 4.62 is {0:f}", Math.Ceiling(4.62));  //   2-ой версии) в меньшую сторону (как floor() в Си, вобщем)
        Console.WriteLine("Round of 4.62 is {0:f}", Math.Round(4.62));      // System.Math.Ceiling() - как Floor() (также имеет 1-у перегрузку,
        // Вывод:                                                           //   но округление в большую сторону (как ceil() в Си)
        //   .. 4,00                                                        // System.Math.Round() - стандартное округление к ближайшему
        //   .. 5,00                                                        //   числу (как round() в Си)(есть 8-мь версий)
        //   .. 5,00


        Math.Abs(-5.2);  // Abs() - выдаст модуль числа. Есть ещё 6-ть перегрузок
        // output: 5.2


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemMath()");
    }
    static void SystemICloneable()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemICloneable()\n");


        // System.ICloneable - этот интерфейс всего-то просит реализовать метод, что создаёт точную копию текущего объекта. Встречается он у
        //   многих несвязанных типов
        //
        //       interface ICloneable
        //       {
        //           object Clone();  	       // Clone() - единственный метод, требуемый интерфейсом System.ICloneable. Xлены интерфейсов
        //                                     //   всегда открыты, и не приемлют никаких модификаторов доступа
        //       }
        //


        Point p1 = new Point(100, 100);
        Point p2 = (Point)p1.Clone();        // = - как мы помним, простое присваивание одного экземпляра класса другому - это копирование
        p2.X = 0;                            //   ссылкок. Чтобы полностью копировать объект нужно нечто большее (обычно это метод Clone())
        Console.WriteLine("p1: {0}", p1);
        Console.WriteLine("p2: {0}\n", p2);


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemICloneable()");
    }
    class Point : ICloneable  // ICloneable - в начале главы этот интерфейс уже описывался, но здесь он применяется уже в наших классах
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point() { }
        public Point(int x, int y) { X = x; Y = y; }
        public override string ToString() => $"X: {X}, Y: {Y}";
        public object Clone() => new Point(X, Y);
        public object CloneM() => this.MemberwiseClone();  // MemberwiseClone() - также можно воспользоваться методом MemberwiseClone(),
    }   // Если ссылочные поля в классе имеются (например, //   унаследованным от System.Object. Не стоит забывать, что он копирует
        //   string) то для полной независимости           //   поверхностно (т.е. почленно). Здесь это то, что нужно, т.к. здесь нет
        //   клонированного объекта следует клонировать и  //   полей ссылочного типа
        //   их внутринности, а не просто создать новую    // Напоминаю о такой штуке, как System.Guid, хранящей случайный 128-битный id.
        //   ссылку на их участки в памяти                 //   Т.к. это структура, она всегда копируется в действительности
    static void SystemCollections_Silent()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemCollections_Silent()\n");


        // Самым элементарным контейнером считается конечно же массив. Он очень прост, и от этого удобен. Да, он имеет свой небольшой арсенал
        //   методов для удобства работы, вроде свойства Length, реализации IEnumerable, Array.Reverse() и т.д. (что уже сильно отличает его от
        //   массива Си), но имея фиксированную вместимость и тип, он сильно уступает в гибкости с другими местным контейнерами
        // В .NET определено несколько пространств имён, содержащих так называемые классы коллекций. От простого массива их отличает
        //   динамическая возможность добавлять и удалять элементы. Также многие коллекции предлагают улучшенную безопасность и довольно хорошо
        //   оптимизированны в отношении памяти. Классы коллекций можно разделить на две обширные категории:
        //
        //   > Необобщённые (в пространстве имён System.Collections)
        //   > Обобщённые (в пространстве System.Collections.Generic)
        //
        //   Необобщённые обычно работают с типом System.Object, а значит с ними постоянно используется приведение
        // Стоит понимать, что скорее всего ты никогда не будешь использовать коллекции из System.Collections, т.к. есть их же обобщённые
        //   версии из System.Collections.Generic. Необобщённые коллекции - это реликты тех времён, когда .NET не поддерживал обобщения
        //
        // Также есть пространство System.Collections.Specialized, содержащее довольно узкоспециализированные необобщённые коллекции. В ряде
        //   случаев они - то, что нужно. Ещё там хранится множество дополнительных интерфейсов и полезных абстрактных классов, на базе которых
        //   можно собрать свой собственный контейнер
        /////////after reading///////////////////////////////////////////////////////////////////////
        // Для знакомых нам интерфейсов IComparable, IEnumerable, IEnumerator, ICollection, IList, IDictionary, IComparer (но не ICloneable,
        //   что странно) есть обобщённые аналоги, которые были созданы для обобщённых коллекций
        /////////////////////////////////////////////////////////////////////////////////////////////
        //
        // И ещё ты можешь наткнуться на System.Collections.ObjectModel. Это крохотное пространство, хранящее 6 дополнительных коллекций

        //  Хотя до появления обобщений в .NET было построено не мало успешных приложений, приходилось мириться с рядом проблем:
        //  > Первая проблема в том, что при частом использовании обычных коллекций значительно проседала производительность, особенно
        //    с использованием чисел. Т.к. такие коллекции умеют работать только с System.Object, среде CLR приходится часто перемещать
        //    участки памяти, что довольно ресурсозатратно (boxing / unboxing, и да, это касается вообще всех структур)
        //  > Также из-за нацеливания для работы с System.Object приходится постоянно явно приводить типы, что очень небезопасно.
        //    System.Object может хранить вообще что угодно, поэтому тем разработчикам, которым нужна была полная безопасность типов,
        //    приходилось строить свою собственную коллекцию. Хоть это и не очень долго, но зато утомительно (на самом деле не обязательно
        //    делать с нуля, достаточно сделать надстройку)


        /////////after reading///////////////////////////////////////////////////////////////////////
        // Заметь, что в пространстве System.Collections.Generic также есть множество вспомогательных типов. Например, структура
        //   System.Collections.Generic.LinkedListNode<T> работает в сочетании с System.Collections.Generic.LinkedList<T> (для односзвязного
        //   списка такого помощника нет), исключение KeyNotFoundException генерируется словарями при неправильном ключа
        /////////////////////////////////////////////////////////////////////////////////////////////


        List<int> myGenericList = new List<int> { 4, 92, 9, 43 };  // {..} - с синтаксисом инициализации массивов тесно связан синтаксис
        ArrayList myList = new ArrayList { 5, 'r', new Char() };   //   инициализации коллекций. Он также позволяет наполнять многие
                                                                   //   контейнеры при их создании (только те, которые имеют метод Add(), т.е
                                                                   //   все коллекции, которые поддерживают ICollection или его обобщение)
                                                                   //   (готов спорить, что и тот, и тот синтаксис инициализации - это один
                                                                   //   механизм)




        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemCollections_Silent()");
    }
}