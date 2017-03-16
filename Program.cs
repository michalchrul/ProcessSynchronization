using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSynchronization
{
    class Program
    {
        //Uwaga: Program nie zostal zabezpieczony przed wprowadzeniem niepoprawnych znakow

        static void PrintProcesses(List<Process> ProcessList)
        {
            if (ProcessList.Count != 0)
            {
                Console.WriteLine("Istnieja nastepujace procesy:");
                foreach (Process process in ProcessList)
                {
                    string state = "";
                    if (process.state == 0)
                    {
                        state = "czekajacy";
                    }
                    else if (process.state == 1)
                    {
                        state = "gotowy";
                    }
                    else if (process.state == 2)
                    {
                        state = "wykonywany";
                    }

                    Console.WriteLine("Stan procesu " + process.name + " to: " + state);
                }
            }
            else
            {
                Console.WriteLine("Brak utworzonych procesow.");
            }
        }

        static void PrintSemaphores(List<Semaphore> SemaphoreList)
        {
            if (SemaphoreList.Count != 0)
            {
                Console.WriteLine("Istnieja nastepujace semafory:");
                foreach (Semaphore semaphore in SemaphoreList)
                {
                    Console.WriteLine("Semafor " + semaphore.name + " ma wartosc " + semaphore.value + ".");
                }
            }
            else
            {
                Console.WriteLine("Brak utworzonych semaforow.");
            }
        }

        static void PrintLocks(List<Lock> LockList)
        {
            if (LockList.Count != 0)
            {
                Console.WriteLine("Istnieja nastepujace zamki:");
                foreach (Lock Lock in LockList)
                {
                    if(Lock.isLocked == true)
                    {
                        Console.WriteLine("Zamek " + Lock.name + " jest zamkniety.");
                    }
                   else
                    {
                        Console.WriteLine("Zamek " + Lock.name + " jest otwarty.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Brak utworzonych zamkow.");
            }
        }


        static void Main(string[] args)
        {
            List<Process> ProcessList = new List<Process>();         //lista przechowujaca wszystkie istniejace procesy
            List<Semaphore> SemaphoreList = new List<Semaphore>();   //lista przechowujaca wszystkie istniejace semafory
            List<Lock> LockList = new List<Lock>();                  //lista przechowujaca wszystkie istniejace zamki
            Queue<Process> WaitingQueue = new Queue<Process>();      //kolejka procesow czekajacych

            string input;                                            //komenda odczytana z konsoli

            do
            {
                input = "";
                input = Console.ReadLine();
                //tworzenie procesow
                if (input == "CP")
                {
                    Console.WriteLine("Podaj nazwe procesu:");
                    string name = Console.ReadLine();
                    Process NewProcess = new Process(name, 1);
                    ProcessList.Add(NewProcess);
                    Console.WriteLine("Utworzono proces o nazwie " + NewProcess.name + ".");
                }

                //tworzenie semaforow
                else if (input == "CS")
                {
                    Console.WriteLine("Podaj nazwe semafora:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Podaj wartosc poczatkowa semafora:");
                    int value = Convert.ToInt32(Console.ReadLine());
                    Semaphore NewSemaphore = new Semaphore(name, value, WaitingQueue);
                    SemaphoreList.Add(NewSemaphore);
                    Console.WriteLine("Utworzono semafor o nazwie " + NewSemaphore.name + " oraz wartosci poczatkowej " + NewSemaphore.value + ".");
                }

                //tworzenie zamkow

                else if (input == "CL")
                {
                    Console.WriteLine("Podaj nazwe zamka:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Podaj stan poczatkowy zamka (0 - otwarty | 1 - zamkniety)");
                    int value = Convert.ToInt32(Console.ReadLine());
                    bool isLocked = true;
                    string state = "";
                    
                    if (value == 1)
                    {
                        isLocked = true;
                        state = "zamkniety";
                    }
                    else if(value == 0)
                    {
                        isLocked = false;
                        state = "otwarty";
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidlowa wartosc.");
                    }


                    Lock NewLock = new Lock(name, isLocked, WaitingQueue);
                    LockList.Add(NewLock);

                    Console.WriteLine("Utworzono " + state + " zamek o nazwie " + NewLock.name);
                }

                //wyswietlanie kolejki czekajacych procesow
                else if (input == "PQ")
                {
                    Console.WriteLine("Wybierz mechanizm synchronizacyjny do ktorego przypisana jest kolejka: (S - Semafory | L - Zamki)");
                    string SyncType = Console.ReadLine();
                    if (SyncType == "S")
                    {
                        if (SemaphoreList.Count != 0)
                        {
                            PrintSemaphores(SemaphoreList);
                            Console.WriteLine("Wskaz semafor, ktorego kolejka ma zostac wyswietlona: ");
                            string SemaphoreName = Console.ReadLine();
                            Semaphore SelectedSemaphore = SemaphoreList.Find(x => x.name.Equals(SemaphoreName));

                            if (SelectedSemaphore.WaitingQueue.Count != 0)
                            {


                                Console.WriteLine("Kolejka prcesow czekajacych:");
                                foreach (Process process in SelectedSemaphore.WaitingQueue)
                                {
                                    Console.Write(process.name + " ");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Kolejka procesow czekajacych jest pusta.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Brak utworzonych semaforow.");
                        }
                    }
                    else if (SyncType == "L")
                    {
                        if (LockList.Count != 0)
                        {
                            PrintLocks(LockList);
                            Console.WriteLine("Wskaz zamek, ktorego kolejka ma zostac wyswietlona: ");
                            string LockName = Console.ReadLine();
                            Lock SelectedLock = LockList.Find(x => x.name.Equals(LockName));

                            if (SelectedLock.WaitingQueue.Count != 0)
                            {
                                Console.WriteLine("Kolejka prcesow czekajacych:");
                                foreach (Process process in SelectedLock.WaitingQueue)
                                {
                                    Console.Write(process.name + " ");
                                }
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.WriteLine("Kolejka procesow czekajacych jest pusta.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Brak utworzonych zamkow.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wprowadzono niepoprawny symbol.");
                    }
                

                }

                //wyswietlanie istniejacych procesow
                else if (input == "PP")
                {
                    PrintProcesses(ProcessList);
                }

                //wyswietlanie istniejacych semaforow i ich wartosci
                else if (input == "PS")
                {
                    PrintSemaphores(SemaphoreList);
                }

                //wyswietlanie istniejacych zamkow i ich stanow
                else if (input == "PL")
                {
                    PrintLocks(LockList);
                }

                //Wybrany proces wykonuje operacje wait na wybranym semaforze
                else if (input == "SW")
                {
                    if (SemaphoreList.Count != 0)
                    {
                        
                        PrintSemaphores(SemaphoreList);
                        Console.WriteLine("Wskaz semafor, na ktorym ma zostac wykonana operacja wait: ");
                        string SemaphoreName = Console.ReadLine();
                        Semaphore SelectedSemaphore = SemaphoreList.Find(x => x.name.Equals(SemaphoreName));

                        PrintProcesses(ProcessList);
                        Console.WriteLine("Wskaz proces, ktory ma wykonac operacje wait: ");
                        string ProcessName = Console.ReadLine();
                        SelectedSemaphore.Wait(ProcessList.Find(x => x.name.Equals(ProcessName)));
      
                        Console.WriteLine("Wykonano operacje wait na semaforze " + SelectedSemaphore.name);
                    }
                    else
                    {
                        Console.WriteLine("Brak utworzonych semaforow.");
                    }

                }

                //Wybrany proces wykonuje operacje signal na wybranym semaforze
                else if (input == "SS")
                {
                    if (SemaphoreList.Count != 0)
                    {
                        PrintSemaphores(SemaphoreList);
                        Console.WriteLine("Wskaz semafor, na ktorym ma zostac wykonana operacja signal: ");
                        string SemaphoreName = Console.ReadLine();
                        Semaphore SelectedSemaphore = SemaphoreList.Find(x => x.name.Equals(SemaphoreName));

                        
                        PrintProcesses(ProcessList);
                        Console.WriteLine("Wskaz proces, ktory ma wykonac operacje signal: ");
                        string ProcessName = Console.ReadLine();
                        SelectedSemaphore.Signal();
                        
                        Console.WriteLine("Wykonano operacje signal na semaforze " + SelectedSemaphore.name);
                    }
                    else
                    {
                        Console.WriteLine("Brak utworzonych semaforow.");
                    }
                }

                //Wybrany proces wykonuje operacje acquire na wybranym zamku
                else if (input == "LA")
                {
                    if (LockList.Count != 0)
                    {

                        PrintLocks(LockList);
                        Console.WriteLine("Wskaz zamek, na ktorym ma zostac wykonana operacja acuqire: ");
                        string LockName = Console.ReadLine();
                        Lock SelectedLock = LockList.Find(x => x.name.Equals(LockName));

                        PrintProcesses(ProcessList);
                        Console.WriteLine("Wskaz proces, ktory ma wykonac operacje acquire: ");
                        string ProcessName = Console.ReadLine();
                        SelectedLock.Accquire(ProcessList.Find(x => x.name.Equals(ProcessName)));

                        Console.WriteLine("Wykonano operacje acquire na zamku " + SelectedLock.name);
                    }
                    else
                    {
                        Console.WriteLine("Brak utworzonych zamkow.");
                    }

                }

                //Wybrany proces wykonuje operacje release na wybranym zamku
                else if (input == "LR")
                {
                    if (LockList.Count != 0)
                    {

                        PrintLocks(LockList);
                        Console.WriteLine("Wskaz zamek, na ktorym ma zostac wykonana operacja release: ");
                        string LockName = Console.ReadLine();
                        Lock SelectedLock = LockList.Find(x => x.name.Equals(LockName));

                        string ProcessName = "";
                        if (!SelectedLock.InitStateLocked)
                        {
                            do
                            {
                                PrintProcesses(ProcessList);
                                Console.WriteLine("Wskaz proces, ktory ma wykonac operacje release: ");
                                ProcessName = Console.ReadLine();
                                if (ProcessName != SelectedLock.AcquiringProcess.name)
                                {
                                    Console.WriteLine("Wybrany proces nie moze wykonac operacji release. Operacji acquire dokonal proces " + SelectedLock.AcquiringProcess.name);
                                }
                            } while (ProcessName != SelectedLock.AcquiringProcess.name);
                        }
                        else
                        {
                            SelectedLock.InitStateLocked = false;
                            PrintProcesses(ProcessList);
                            Console.WriteLine("Wskaz proces, ktory ma wykonac operacje release: ");
                            ProcessName = Console.ReadLine();
                        }


                        SelectedLock.Release(ProcessList.Find(x => x.name.Equals(ProcessName)));

                        Console.WriteLine("Wykonano operacje release na zamku " + SelectedLock.name);
                    }
                    else
                    {
                        Console.WriteLine("Brak utworzonych zamkow.");
                    }

                }

                //Wyswietla mechanizmy synchronizacyjne na ktorych proces "zawiesil sie"
                else if(input == "PB")
                {

                    PrintProcesses(ProcessList);
                    Console.WriteLine("Wskaz proces do tej operacji: ");
                    string ProcessName = Console.ReadLine();
                    Process p = ProcessList.Find(x => x.name.Equals(ProcessName));

                    if (p.BlockedBy.Count != 0)
                    {
                        Console.WriteLine("Proces zawiesil sie na nastepujacych mechanizmach synchronizacyjnych:");
                        foreach (String name in p.BlockedBy)
                        {
                            Console.Write(name + " ");
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Proces jest w stanie gotowosci.");
                    }
                    
                }


                else if (input == "HELP")
                {
                    Console.WriteLine("Dostepne komendy:");
                    Console.WriteLine("CP - Utworz proces                             CS - Utworz semafor");
                    Console.WriteLine("PP - Wyswietl istniejace procesy               PS - Wyswietl istniejace semafory");
                    Console.WriteLine("SW - Wykonaj operacje wait na semaforze        SS - Wykonaj operacje signal na semaforze");
                    Console.WriteLine("LA - Wykonaj operacje acquire na zamku         LR - Wykonaj operacje release na zamku");
                    Console.WriteLine("PQ - Wyswietl kolejke czekajacych procesow     PB - Wyswietl mechanizmy sync. blokujace proces");
                    Console.WriteLine("HELP - Wyswietl dostepne komendy               EXIT - Wyjscie z programu");
                }



                else
                {
                    Console.WriteLine("Wpisano nieprawidlowa komnede. Wpisz HELP, aby wyswietlic liste dostepnych komend.");
                }

                
            } while (input != "EXIT");
        }
    }
}
