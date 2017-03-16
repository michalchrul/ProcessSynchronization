using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessSynchronization
{

    class Semaphore
    {

        public int value { get; set; }
        public Queue<Process> WaitingQueue;
        public string name { get; set; }

        public Semaphore(string name, int value, Queue<Process> WaitingQueue)
        {
            this.name = name;
            this.value = value;
            this.WaitingQueue = WaitingQueue;
        }
        ~Semaphore() { }

        public void Wait(Process p)
        {   
            value--;

            if (value < 0)
            {
                WaitingQueue.Enqueue(p);
                p.state = 0;
                p.BlockedBy.Add(this.name);         //zapisujemy nazwe semafora na liscie mechanizmow synchronizacyjnych, ktore blokuja proces
            }

        }

        public void Signal()
        {
            value++;
            
            if (value <=0)
            {
                Process p = WaitingQueue.Dequeue();
                if (p.BlockedBy.Count == 1)                             //pod warunkiem, ze tylko ten mech. synch. blokuje proces:
                {
                    p.state = 1;                                        //zmieniamy stan na gotowy
                }
                p.BlockedBy.Remove(this.name);     //z przypisanej procesowi listy blokujacych go mechanizmow synchronizacyjnych usuwamy semafor
                                                   //na ktorym proces wykonal operacje signal
            }


        }
    };
}
