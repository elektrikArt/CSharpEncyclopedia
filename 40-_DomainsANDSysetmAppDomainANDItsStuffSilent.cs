/*
 * creation date  01 aug 2021
 * last change    04 aug 2021
 * author         artur
 */
using System;
using System.Linq;

class _DomainsANDSysetmAppDomainANDItsStuff
{
    static void Main()
    {
        Console.WriteLine("***** _ *****");

        DomainsANDSysetmAppDomainANDItsStuff();

        Console.ReadLine();
    }
    static void DomainsANDSysetmAppDomainANDItsStuff()  //after SystemDiagnostics, SystemDiagnosticsProcess
    {
        Console.WriteLine(">->->->->->->->->->->->->->->->->->->   DomainsANDSysetmAppDomainANDItsStuff()\n");


        //   Теперь, когда мы достаточно хорошо понимает суть процессов Windows и умеем их использовать посредством .NET, можно переходить
        //     к концепции доменом приложений (тоже через .NET)


        // WhatItIs
        //
        //   Как оказалось, исполняемые файлы .NET размещаются не прямо в процессах Windows, как это делается в традиционных неуправляемых
        //     приложениях. Взамен *.exe попадает в отдельный логический раздел внутри процесса, что называется "доменом приложения". Ты
        //     увидишь, что один процесс может содержать несколько доменов приложения, каждый из которых обслуживает свой исполняемый файл
        //     .NET. Такое разделение процесса Windows даёт несколько преимуществ:
        //  
        //     > Домены - это ключевой механизм нейтральной природы .NET к операционным системам, т.к. за ними не видно то, как именно
        //       система представляет загруженный исполняемый файл
        //     > Домены приложений гораздо менее затратны в вычеслительных ресурсах и памяти по сравнению с полноценными процессами. Это
        //       приводит к тому, что CLR способна загружать и выгружать домены приложений намного быстрее, чем условный процесс (и, как
        //       говорит автор, это может значительно улучшить масштабируемость серверных приложений)
        //     > Домены также обеспечивают уровень изоляции для своих приложений. Если один домен процесса терпит отказ, то остальные домены
        //       остаются работоспособными
        //
        //   Я представляю процесс как контейнер, в который можно подвешивать домены. Ось общается с процессом, CLR'ка в этом контейнере ловит
        //     команды оси, переводит их в .NET'овские команды, и отправляет в подвешенные домены
        //  
        //   Да, домены одного процесса не могут обмениваться данными, т.к. они полностью изолированы друг от друга. Но передачи данных можно
        //     добиться применением какого-нибудь протокола распределённого программирования (вроде WCF)(автор просто упомянул это. что это
        //     за НЁХ? - не говорит)
        //   Хоть в одном процессе может находится и множество доменов, такого обычно не происходит. Как минимум процесс операционной
        //     стистемы будет обслуживать так называемый "статический домен приложения" (или просто "стандартный домен"). Этот специфичный
        //     домен приложения автоматически создаётся средой CLR во время запуска процесса. Затем она по мере необходимости (ну т.е. если ты
        //     напишешь нужную логику) создаст дополнительные домены приложения
        //
        //   /////////after reading: ObjectContexts///////////////////////////////////////////////////////
        //   // Схема всего этого имеется, но находится она в методе ObjectContexts. Почему? Там просто
        //   //   всё в более полном виде, та схема включает в себя контексты
        //   /////////////////////////////////////////////////////////////////////////////////////////////


        // Вырезка из одного журнала 2003 года (RSDN Magazine #1-2003) (это не копипаста, а перефразированный мной текст):
        //
        //     # В отличие от обычного, управляемый код может проверятся (верифицироваться), в том числе и на наличие небезопасных операций.
        //     # Эта верефикацися всего лишь проверяет код на запрещённые команды, но не спасает от вредящих другим приложениям или
        //     # компанентам вызовов. Чтобы избежать этого, нужно огранизовать выполнение кода в некой изолированной среде, не допускающей
        //     # подобных обращений. Этой изолированной средой стали процессы. Для неуправляемого кода эту роль выполняет исключительно они,
        //     # однако, с точки зрения скорости, взаимодействие между процессами требует больших затрат времени. Чтобы минимизировать потери,
        //     # в .NET был включён механизм доменов. Этот механизм позволяет запустить группу приложений в одном процессе, опеспечивая
        //     # относительную изоляцию друг от друга и в то же время обеспечивает значительно более быстрое взаимодействие между ними
        //     #
        //     # Для каждого домена создаётся свой собственный экземпляр сборщика мусора, свои собственные настройки безопасности и
        //     # и собственные статические переменные


        // Using (yeah, practic first)
        //
            System.AppDomain defaultAD = System.AppDomain.CurrentDomain;                         // .. = ..CurrentDomain - здесь мы получим
            Console.WriteLine("Name of domain:\t\t\t{0}", defaultAD.FriendlyName);               //   доступ к домену текущего приложения
            Console.WriteLine("Id of domain:\t\t\t{0}", defaultAD.Id);                           // defaultAD.FriendlyName - заметь, что имя
            Console.WriteLine("Is this default domain?:\t{0}", defaultAD.IsDefaultAppDomain());  //   стандартного домена идентично имени
            Console.WriteLine("Base directory of domain:\t{0}\n", defaultAD.BaseDirectory);      //   выполняющегося в нём *.exe
        //                                                                                       // ..IsDefaultAppDomain() - да, это именно
        //                                                                                       //   метод, не свойство (помним, что свойства
        //                                                                                       //   - это большая надстройка над полем)
        //
        void ListAllAssembliesInDomainApp(AppDomain theDomain)
        {
            Console.WriteLine("Here are the assemblies loaded in {0}", theDomain.FriendlyName);
            foreach (System.Reflection.Assembly curr in theDomain.GetAssemblies())
            {
                Console.WriteLine("\n-> Name:\t{0}", curr.GetName().Name);
                Console.WriteLine("-> Version:\t{0}", curr.GetName().Version);
            }
            Console.WriteLine();
        }                                                                               
        //                                                                                       // theDomain.GetAssemblies() - как ты видишь,
        //                                                                                       //   в нашем домене используется всего пара
        //                                                                                       //   сборок (именно сборок, не модулей. Модуль
        //                                                                                       //   - это файл)(это mscorlib и наш 17.3.
        //                                                                                       //   ****так было в проекте 17.3. в
        //                                                                                       //   энциклопедии, конечно, гораздо больше).
        //                                                                                       //   Помни, что в этот список попадут только
        //                                                                                       //   те сборки, что действительно используются
        //                                                                                       //   (в References этого проекта имеется
        //                                                                                       //   дополнительная сборка - некая 
        //                                                                                       //   Microsoft.Build.dll, и в списке её нет)
        Console.WriteLine("Here are all assemblies loaded in {0}", defaultAD.FriendlyName);
        ListAllAssembliesInDomainApp(defaultAD);


        //
        //
        // System.AppDomain
        //
        //   Платформа .NET позволяет программно отселживать существующие домены, создавать новые домены приложений (или выгружать их),
        //     загружать сборки в домены приложений и решать ряд других задач. Всё это поставляется в классе System.AppDomain (sealed, находится
        //     в mscorlib.dll)(заметь, что мы уже не в System.Diagnostics). Вот некоторые полезные методы в нём:
        //  
        //     > System.AppDomain.CreateDomain()        - этот статический создаст и выдаст тебе экземпляр нового домена приложения в текущем
        //                                                процесса (у него 6 разнообразных перегрузок)
        //     > System.AppDomain.GetCurrentThreadId()  - тоже статический метод, выдаст id (int) текущего домена, в котором выполняется
        //                                                приложение
        //     > System.AppDomain.Upload()              - ещё один статический метод, и он выгружает указанный домен из нашего (текущего)
        //                                                процесса
        //  
        //     > <object>.CreateInstance()              - метод делает то же, что и System.Activator.CreateInstance(), но в рамках этого
        //                                                домена (т.е. этот метод создаёт объект типа из некой заданной сборки, что загружена в
        //                                                домен)
        //     > <object>.ExecuteAssembly()             - загружает и запускает сборку *.exe внутри домена, принимая её файл (внутри себя
        //                                                вызывает знакомый нам System.Reflection.Assembly.LoadFrom())
        //     > <object>.GetAssemblies()               - этот метод выдаст массив сборок, что были загружены в домен (выдаст .NET сборки,
        //                                                COM и Си пропускаются)
        //     > <object>.Load()                        - этим динамически загружают дополнительную сборку в домен (как и все, получает сборку
        //                                                через CLR)
        //  
        //   Как ты, возможно, уже понял, .NET не позволяет выгружать конкретную сборки из памяти. Единственный способ программной выгрузки
        //     библиотек - это уничтожение всего домена посредством System.AppDomain.Upload()
        //   Вдобавок класс System.AppDomain определяет набор удобных для мониторинга свойств. Вот самые интересные из них:
        //  
        //     > System.AppDomain.CurrentDomain      - статическое свойство выдаст тебе текущий домен (в котором этот код и выполняется)
        //     > System.AppDomain.MonitoringIsEnable - тоже static (типа bool), и им можно включать мониторинг ресурсов центрального процессора
        //                                             и памяти для текущего процесса. Если мониторинг включён, отключить его невозможно
        //  
        //     > <object>.BaseDirectory              - свойство позволяет получить путь к каталогу, что CLR использует для
        //                                             зондирования (т.е. нахождения сборок)
        //     > <object>.FriendlyName               - свойство для получения дружественного имени своего домена
        //     > <object>.SetupInforamtion           - этим свойством ты можешь получить детали конфигурации (****в виде System.AppDomainSetup)
        //                                             домена приложения
        //  
        //   Ну и на десерт, класс System.AppDomain содержит набор событий (events), что соответсвуют некоторым аспектам жизненного цикла домена
        //     приложения, и самое интересное то, что ты можешь на них влиять. Вот наиболее полезные из них:
        //  
        //     > <object>.AssemblyLoad         - происходит сразу после того, как сборка загружена в память
        //     > <object>.AssemblyResolve      - происходит, если распознаватель сборок не смог найти сборку
        //     > <object>.DomainUnload         - происходит перед началом выгрузки домена из обслуживающего процесса
        //     > <object>.FirstChanceException - этот event запускается после генерации исключения в его домене, но перед тем, как CLR начнёт
        //                                       спускаться по стёку для поиска подходящего catch (да, суффикс Exception может немного сбить с
        //                                       толку)
        //     > <object>.ProcessExit          - запускается во всех доменах, когда обслуживающий их процесс завершается (начиная с .NET 2.0 по
        //                                       msdn. В книге же говорится, что данное событие происходит только в стандартном домене). В .NET
        //                                       общее время для всех event'ов ProcessExit ограничено 2 секуднами (но это можно изменить
        //                                       неуправляемым кодом, вызвав некий ICLRPolicyManager::SetTimeout() (в .NET Core лимита нет)
        //     > <object>.UnhandledException   - запускается, когда исключение не было перехвачено обработчиком исключений
        //  
        //   Интересно, что у System.Diagnostics.Process нет метода для выдачи доменов


        // New assembly in app domain notification Using
        //
        //   Если ты хочешь получить уведомление от CLR о загрузке новой сборки в один из твоих доменов, то сохрани нужную фукнцию/метод в
        //     событие AssemblyLoad, что имеется у твоего домена. Событие AssemblyLoad отностися к типу делегата
        //     System.AssemblyLoadEventHandler, что совместим с прототипом void ..(object .., AssemblyLoadEventArgs ..)
        //   System.AssemblyLoadEventArgs - это крошечный класс, что имеет один конструктор, принимающий экземпляр System.Reflection.Assembly и
        //     свойство для его хранения (почему этот класс существует? прост мелкомягкие придерживались рекомендуемого шаблона Microsoft для
        //     событий)
        //
            defaultAD.AssemblyLoad += (o, s) =>
            {
                Console.WriteLine("{0} has been loaded!", s.LoadedAssembly.GetName().Name);
            };                                  // (o, s) - напомню, что рекомендуемый Microsoft шаблон для событий предусматривает отправку
        //                                      //   ссылки на вызывающий объект и объекта какого-нибудь класса, содержащего доп. инфу
        //                                      //   (делегат должен быть таким:     .. ..EventHandler(object sender, ..EventArgs args    )
        //
        //   Теперь при каждой загрузке новых сборок в нашем приложении (что просиходит ниже) мы будем получать уведомление об этом в виде
        //     строки в cmd


        // NewAppDomain Using
        //
        //   Хоть потребность в создании новых доменов приложения возникает редко, неплохо бы знать как это делается.
        //     /////////after reading: DynamicAssemblies////////////////////////////////////////////////////
        //     // Например, динамические сборки должны устанавливаться в специальный домен,
        //     //   отдельно от потребителя.
        //     /////////////////////////////////////////////////////////////////////////////////////////////
        //     Многие API-интерфейсы, связанные с безопасностью, также требуют знания того, как для изоляции сборок конструировать новые домены
        //     (там создаются что-то вроде сборкок-хранилищ для поставляемых учётных данных)
        //   Новые домены создаются статическим методом System.AppDomain.CreateDomain() (открытых конструкторов у этого класса нету), и этот
        //     метод имеет 5 перегрузок (всего 6 весрий, и всем из них как минимум нужно отправить строку с будущим дружественным именем)
        //
        //
        //
            ListAllAssembliesInDomainApp(defaultAD);
            AppDomain newAD = AppDomain.CreateDomain("SecondAppDomain");
            ListAllAssembliesInDomainApp(newAD);
        //                                        // System.AppDomain.CreateDomain(..) - и этим методом мы подвешиваем в процесс новый домен
        //                                        // ListAllAssemblies..(newAD) - в новом, но пустом домене оказывается только mscorlib (она
        //                                        //   загружается средой CLR во все домены и всегда). Чтоб домен newAD запустил что-нибудь,
        //                                        //   надо это что-нибудь (сборку) загрузить в него    newAD.Load(..);    
        //                                        // Как говорит автор, если ты запускаешь отладку проекта (F5), то ты должен увидеть, что
        //                                        //   каждый домен имеет много других сборок, что задействованы процессом отладки VS
        //                                        //   (запустить проект просто для выполнения, т.е. без отладки, можно нажав Ctrl-F5). У меня
        //                                        //   же (а у меня VS 2019) такого не происходит, и в окне Debug -> Windows -> Modules (что,
        //                                        //   как и многие другие окна для отладки, появляется только при запущеном приложении) также
        //                                        //   их нет
        //                                        // Ещё автор говорит, что для людей, имеющих приличный опыт создания традиционных приложений
        //                                        //   для Windows, подключение одних и тех же сборок для обоих доменов может показаться
        //                                        //   нелогичным (вдруг это приведёт к проблемам), но не стоит забывать, что у каждого из этих
        //                                        //   доменов окажется свой личный набор сборок, никак не связанный с набором других


        // ManualLoadingAndUnloadingAssembliesInAppDomain
        //
        //   Да, среда CLR будет автоматически загружать сборки, как только код начнёт использовать их код, но, т.к. это занимает какое-то
        //     время (всё-таки чтение с hdd/ssd - занятие относительно весьма не быстрое), ты можешь захотеть загрузить их заранее, использовав
        //     специальный метод <object>.Load() нужного тебе домена
        //
            AppDomain newAD2 = AppDomain.CreateDomain("ThirdAppDomain");
            try
            {
                newAD2.Load("14.2");                    // 14.2 - да, это наша старая знакомая сборка с пространством CarLibrary
            }                                           //   (файл этой сборки ещё давно был помещён в ..\bin\Debug\)
            catch (System.IO.FileNotFoundException ex)  // newAD2.Load(..) - т.к. внутри используется System.Reflection.Assembly.Load(), нам
            {                                           //   придётся ловить все выпадающие исключения оттуда
                Console.WriteLine("{0}", ex.Message);
            }
            ListAllAssembliesInDomainApp(newAD2);       // ListAllAssemblies..(newAD2) - и как мы видим, эта сборка выводится в списке сборок,
        //                                              //   имеющихся у домена (наряду с mscorlib)
        //                                              // Интересно, что сборка 14.2 также загружается в стандартный домен (об этом пишет
        //                                              //   наш уведомитель, ну и я проверил)


        //   Как говорилось, CLR не может выгрузить определённую сборку из домена. Вместо этого предлагается выгрузить весь домен полностью,
        //     вместе со всеми сборками, и затем сконструировать его по новой, но без ненужной части (сборки). Делается это статическим
        //     методом System.AppDomain.Unload()
        //   Ещё у каждого домена есть довольно интересное событие DomainUnload (о котором также писалось), запускающегося, когда его домен
        //     собирается выгружаться из памяти (из процесса)(это может быть потому, что или кто-то вызвал AppDomain.Unload() для него, или
        //     в нём просто завершился код), и о (также довольно интересном) событии ProcessExit, что выполняется, когда процесс собирается
        //     завершиться. Их использование мы здесь также продемонстрируем
        //
            newAD.DomainUnload += (o, s) =>  // ..DomainUnload - это событие для содержания делегатов EventHandler (void ..(object, EventArgs))
            {                                // EventArgs - тип-пустышка, имеющий одно static readonly поле Empty и конструктор без параметров
                Console.Write("newAD is going to unload!\n\n");
            };
            AppDomain.Unload(newAD);  // ..Unload(newAD) - и здесь мы получаем ожидаемое оповещение
        //                                                          //
            AppDomain.CurrentDomain.ProcessExit += (o, s) =>        // ..ProcessExit - также для делегатов типа EventHandler. Конечно, наше
            {                                                       //   сообщение увидит только тот, у кого сразу после завершения приложения
                Console.WriteLine("Our process is going to die!");  //   не закроется cmd
            };                                                      // Если ты попытаешься изменить что-то у выгруженного домена (newAD,
        //                                                          //   например), то ты получишь AppDomainUnloadedException


        // ..DefineDynamicAssembly() - именно из объектов доменов и можно получить объект
        //   AssemblyBuilder. Как видишь, наша динамическая сборка будет находится прямо в
        //   текущем домене (но, как мы помним, в домен работает только с одним
        //   исполняемым файлом), и все наши действия с asm отразятся именно в этом домене
        //****скопировано из DynamicAssemblies_Silent


        // Afterword
        //   Итак, рассмотрение доменов приложений .NET завершено
        //     /////////after reading: ObjectContexts///////////////////////////////////////////////////////
        //     // На последок той главе мы рассмотрим ещё один уровень разделения, что
        //     // применяется для гуппирования объектов в контекстные границы
        //     /////////////////////////////////////////////////////////////////////////////////////////////


        Console.WriteLine("<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<-<   DomainsANDSysetmAppDomainANDItsStuff()");
    }
}