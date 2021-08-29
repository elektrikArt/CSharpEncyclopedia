/*
 * creation date  13 jul 2021
 * last change    09 aug 2021
 * author         artur
 */
using System;
using System.Diagnostics;
using System.Linq;

class _Processes_Threads_SystemDiagnosticsNamespace
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        Processes_Silent();
        Threads_Silent();
        SystemDiagnosticsNamespace();

        Console.ReadLine();
    }
    static void Processes_Silent()  //after CLR,LinkingAssemblies,Reflection
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   Processes_Silent()\n");


        // WhatIsIt
        //
        //   Ты уже видел в подробностях, как CLR находит подключаемые сборки к приложению (напомню, что через метаданные сборки
        //     и .config-файл, и, если в коде есть рефлексия System.Assembly.Load/LoadFrom(), то по ним)
        //   Здесь же ты увидишь, как CLR
        //     обслуживает сборку (во время её работы) более конкретно, а также ты откроешь перед собой детели отношений между работающими
        //     процессами, доменами приложений и некими "объектными контекстами"
        //   Да, многие повседневные задачи снова не используют эти возможности, но понимание их важно при использовании некоторых
        //     API-интерфейсов .NET
        //                                         /////////after reading:WCF///////////////////////////////////////////////////////////////////
        //                                         // (включая WCF (Windows Communication Foundation), многопоточную и параллельную обработку
        //                                         // , а также сериализацию)
        //                                         /////////////////////////////////////////////////////////////////////////////////////////////
        //
        //
        //   Как ты помнишь, "процесс" - это выполняющаяся программа. Если говорить формально, то это концепция уровня ОС. Именно по ней
        //     система может знать, как именно следует выполнять программу. Код общается с системой через процесс, в котором он находится
        //     (сколько ему нужно памяти, какие внешние библиотеки кода предоставить, какую память он хочет занять/освободить и т.д. - всё
        //     это через процесс)
        //   Для каждого запускаемого .exe ОС выделяет отдельный изолированный процесс, в котором и будет выполнятся
        //     код. Код завершается - процесс закрывается (точнее, ОС закрывает его)
        //   Изолированность процесса от всего остального - вещь довольно логичная, ведь так отказ одного процесса не влияет на работу
        //     других. При этом они могут общатся (но это делается не напрямую, а через "прокси". что такое прокси? это такой посредник, что
        //     фильтрует данные и иногда кеширует, чтобы всё шло быстрее)
        //   Каждый процесс Windows при своём создании получает от ОС собственный идентефикатор (process identifier data - PID). Процессы,
        //     что работают прямо сейчас, ты можешь просмотреть в Task Manager'е Windows (если ты используешь Windows, конечно), что можно
        //     открыть, нажав Ctrl-Shift-Esc. Во вкладке Processes имеются разнообразные статистические данные о каждом из них, а PID для
        //     каждого из них ты можешь узнать во вкладке Details (там в качестве имени процесса выведены .exe-файлы их программ. У системных
        //     процессов дополнения .exe нет)(в gnu/linux'е есть утилитка top, или её более h'уманисткий аналог - htop)


        /////////after reading: System.Diagnostics.Process///////////////////////////////////////////
        // О том, как управлять процессами средствами .NET, написано в методе о
        //   System.Diagnostics.Process
        /////////////////////////////////////////////////////////////////////////////////////////////


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   Processes_Silent()");
    }
    static void Threads_Silent()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   Threads_Silent()\n");


        // WhatItIs
        //
        //   Windows, создавая процесс, выдаёт ему начальный "поток" ("thread"), в котором затем стартанёт код. Этот начальный поток также
        //     называют "главным потоком". Если процесс создан для .NET-программы, то главный поток начинает выполнять код с метода Main()
        //     (помеченного компилятором как .entrypoint, ведь такой метод может быть сразу у нескольких классов в программе)
        //   Это однопоток, но можно отправить выполнять твой код и больше потоков. Такой однопоток безопасен, т.к. один элемент данных никогда
        //     не будет в обработке нескольких потоков одновременно (это может
        //     привести к проблеме, вроде этой: один поток записывает 42 в условный int, и сразу после этого второй поток записывает туда же 13
        //    . Как ты понял, число 42 было предназначено для чего-то, но оно уже потеряно). Тем не менее, иногда удобнее использовать сразу
        //     несколько потоков, чтобы ускорить (или сделать более удобным) работу приложения. Отличным примером служит графический
        //     интерфейс: у тебя однопоток, то он может быть или у логики пользователя (следить за мышью и клавиатурой и интерфейсом), или
        //     занят делами
        //     программы (считывать инфо с флешки). Т.е. пользователь может столкнуться с тем, что программа не будет реагировать на него,
        //     т.к. её единственная рабочая лошадка (поток) будет занята другим делом (правда, такая проблему можно решить, заставив эту
        //     лошадку постоянно поочереди перепрыгивать между своими задачами, но так сильно снизится снизится общая скорость дела)
        //   Если ты всё-таки хочешь, чтобы процесс выполнял твою программу в двух (и более) потоках, то можешь подготовить её код к этому.
        //     API Windows (а также .NET) предоставляют метод CreateThread(). Когда главный поток выполняет этот метод, он посылает
        //     запрос к API Windows на подключение нового потока (называемого "рабочим потоком"). Получая объект нового потока, ты можешь
        //     запрясти его, дав ему задание в виде имени метода (или функции), и этот новый работник начнёт его выполнять. Оба потока
        //     (первичный и вторичный) будут работать параллельно (главное, чтобы они не мешали друг другу)
        //   Собственно, с двумя рабочими лошадками производительность твоей программы может удвоится (смотря насколько хорошо ты будешь ими
        //     управлять. это сильно зависит от ситуации). Многопоточные процессы создают иллюзию того, что выполнение многочисленных
        //     действий происходит более или менее одновременно. Правда, легко написать код и так, чтобы добовление нового потока наоборот
        //     "ухудшит" производительность, ведь главный поток, управляя рабочим, тратит на него какое-то время, а рабочий поток может и не
        //     делать ничего полезного. Даже если твоя приложуха будет мастерски управляться обоими потоками, это тоже может привести к
        //     ухудшению скорости. Это происходит в случае, если ось (спрашивая процессор) не сможет выдать тебе новый поток, и, чтобы отвечать
        //     запросу,
        //     он будет (так же, как я и описал выше) переключать один имеющийся поток между двумя виртуальными (главным и рабочим твоего
        //     процесса), а такое переключение, кстати, тоже занимает время
        //   На самом деле на некоторых машинах, где процессор имеет одно ядро и не поддерживает гиперпотоки, многозадачность эмулируется
        //     ОС'кой именно этим путём. Единица времени, которую тратит настоящий поток на эмулируемый, прозвали "квантом времени". Когда
        //     кванты
        //     времени истекают для текущей эмуляции, настоящий поток приостонавливает её, и затем продолжает другую. Чтобы поток не забывал
        //     то, что происходило до приостановки, ОС предоставляет ему возможность записывать данные в локальное хранилище потоков
        //     (Thread Local Storage - TLS) и выделяет отдельный стёк вызовов. Вот как это выглядит на схеме:
        //  
        //         ┌────────────────────────────────────────────────────────────────────────────┐
        //         │ Одиночный процесс Windows                                                  │
        //         │                                                                            │
        //         │  ┌──────────────────────────────────────────────────────────────────────┐  │
        //         │  │                          Разделяемые данные                          │  │
        //         │  └──────────────────────────────────────────────────────────────────────┘  │
        //         │  ┌─────────────────────────────────┐  ┌─────────────────────────────────┐  │
        //         │  │             Поток A             │  │             Поток B             │  │
        //         │  │                                 │  │                                 │  │
        //         │  │ ┌───────────┐┌────────────────┐ │  │ ┌───────────┐┌────────────────┐ │  │
        //         │  │ │    TLS    ││  Стёк вызовов  │ │  │ │    TLS    ││  Стёк вызовов  │ │  │
        //         │  │ └───────────┘└────────────────┘ │  │ └───────────┘└────────────────┘ │  │
        //         │  └─────────────────────────────────┘  └─────────────────────────────────┘  │
        //         └────────────────────────────────────────────────────────────────────────────┘
        //  


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   Threads_Silent()");
    }
    static void SystemDiagnosticsNamespace()
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   SystemDiagnosticsNamespace()\n");


        // General info about System.Diangostics
        //
        //   Как говорит автор, чтобы начать исследование многопотока, нужна сначала иметь хорошое представление о способах взаимодействия
        //     с процессами (конечно, опять через пути .NET)
        //   В пространстве имён System.Diagnostics (во времена .NET 4.7 полная его версия жила System.dll. сейчас некоторые типы вынесли в свои
        //     .dll) имеется несколько типов, что создавались в качестве
        //     посредников между тобой и процессами. Там также есть и другие, уже действительно диагностическими типы, вроде журнала событий
        //     системы и счётчиков производительности. Здесь ты, правда, увидишь только те, что связанны с процессами. Кстати, вот они:
        //  
        //         > System.Diagnostics.Process                  - предоставляет доступ к локальным и удалённым процессам. Через него также
        //                                                         можно их создавать и останавливать
        //         > System.Diagnostics.ProcessModule            - представляет собой модуль (*.dll или *.exe), загруженный в определённый
        //                                                         процесс. Этот модуль не
        //                                                         обязательно должен быть .NET, он также может предоставлять сборки, основанные
        //                                                         на COM или традиционном C
        //         > System.Diagnostics.ProcessModuleCollection  - предоставляет строго типизированную коллекцию объектов ..ostics.ProcessModule
        //         > System.Diagnostics.ProcessStartInfo         - экземпляры этого типа имеют набор значений, что применяются при запуске
        //                                                         процесса (с помощью System.Diagnostics.Process.Start())
        //         > System.Diagnostics.ProcessThread            - представляет собой поток внутри заданного процесса. Имей ввиду, что
        //                                                         экзмемпляры будут просто
        //                                                         набором информации о потоках, но ты не сможешь создать этим типом новый поток
        //                                                         для процесса. Не стоит путать с System.Threading.Thread
        //         > System.Diagnostics.ProcessThreadCollection  - тоже предоставляет строго типизированную коллекцию объектов ProcessThread
        //
        //    Подробнее о этих классах ты можешь почитать в методах о каждом из них (****но на данный момент не о каждом)


        // Using 2
        //
            void ListOfModules(int pid)  // ListOfModules() - выводит имена модулей (типа .dll или .exe), загруженных в процесс с этим pid
            {
                Process process;
                try               // try - если запущенного процесса с таким pid не имеется, сообщить об этом пользователю
                {
                    process = Process.GetProcessById(pid);
                }
                catch (ArgumentException ex)
                {
                    Console.Write("{0}\n\n", ex.Message);
                    return;
                }

                Console.WriteLine($"Mudules that the process with pid {pid} (is {process.ProcessName}) has:");
                foreach (ProcessModule curr in process.Modules)               // process.Modules - свойство (что, кстати только для чтения)
                {                                                             //   выдаст нам коллекцию типа ProcessModuleCollection сюда
                    Console.WriteLine($"-> Module Name: {curr.ModuleName}");  //   войдут модули .NET, COM и написанные на Си
                }                                                             // process.Modules - как оказалось, есть процессы (System,
                Console.WriteLine();                                          //   например), что не выдают свои модули
            }                                                                 //   (генерируется System.ComponentModel.Win32Exception)
        //                                                                    //
            Console.Write("Your pid: ");
            int thePid = int.Parse(Console.ReadLine());
            ListOfModules(thePid);  // thePid - если ты отправишь на исследование pid этого приложения, то увидишь, что даже в нём
                                    //   используется очень даже внушительный список модулей
        //                          //


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   SystemDiagnosticsNamespace()\n");
    }
}