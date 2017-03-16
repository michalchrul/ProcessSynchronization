using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSynchronization
{
    class Lock
    {
        public string name { get; set; }
        public bool isLocked { get; set; }
        public Queue<Process> WaitingQueue;
        public Process AcquiringProcess;
        public bool InitStateLocked;

        public Lock(string name, bool isLocked, Queue<Process> WaitingQueue)
        {
            this.name = name;
            this.isLocked = isLocked;
            this.WaitingQueue = WaitingQueue;
            if(isLocked == true)
            {
                this.InitStateLocked = true;
            }
            else
            {
                InitStateLocked = false;
            }
        }
        ~Lock() { }

        public void Accquire(Process p)
        {
            // zajecie zamka, jesli wolny
            if (!this.isLocked)
            {
                isLocked = true;
                AcquiringProcess = p;
                p.BlockedBy.Add(this.name);
            }

            // zmiana stanu watku na oczekujacy i dodanie do kolejki, jesli jest zajety
            else
            {
                WaitingQueue.Enqueue(p);
                p.state = 0;
                p.BlockedBy.Add(this.name);
            }


        }
        public void Release(Process p)
        {
            //jesli kolejka jest pusta: zwolnienie zamka
            if (WaitingQueue.Count == 0)
            {
                isLocked = false;
            }

            //jezeli kolejka nie jest pusta: wybranie watku z niepustej kolejki i zmiana stanu na gotowy
            else
            {
                Process DequeuedProcess = WaitingQueue.Dequeue();       //usuwamy pierwszy proces z kolejki
                if (p.BlockedBy.Count == 1)                             //pod warunkiem, ze tylko ten mech. synch. blokuje proces:
                {
                    DequeuedProcess.state = 1;                          //zmieniamy stan procesu na gotowy
                }

                AcquiringProcess = DequeuedProcess;                     //podmieniamy proces ktory zamknal zamek na ten usuniety z kolejki

            }

            p.BlockedBy.Remove(this.name);     //z przypisanej procesowi listy blokujacych go mechanizmow synchronizacyjnych usuwamy zamek
                                               //na ktorym proces wykonal operacje release

        }

        //public void TryLock(Lock myLock)
        //{
        //    //proba zajecia zamka, jesli jest wolny
        //    //nie powoduje zablokowania watku, gdy zamek jest zajety
        //}
    }
    
}
