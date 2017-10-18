﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace ChatSharedRessource.Models
{
    public class ListenQueues : INotifyPropertyChanged
    {
        private static Message CurrentMessage { get; set; }
        private static Message ClientMessage { get; set; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private static ListenQueues _instance;
        private Queue<Message> Messages { get; set; }
        private Queue<string> TextMessages { get; set; }
        private Queue<string> OptionnalTextMessages { get; set; }
        public ListenQueues()
        {
           Messages = new Queue<Message>();
           TextMessages=new Queue<string>();
           OptionnalTextMessages = new Queue<string>();
        }

        public static ListenQueues MyInstance()
        {
            
                if (_instance == null)
                {
                    _instance = new ListenQueues();
                }
                return _instance;
            
        }
        public static void ServerListeningLoop()
        {
            while (true)
            {
                lock (MyInstance())
                {
                    if (MyInstance().Count() > 0)
                    {
                        CurrentMessage = _instance.PullMessage(); // removed
                        CurrentMessage?.ControlCommand.ServerTreatment(CurrentMessage);
                        CurrentMessage = null;
                    }
                }
            }
        }

      


        public void AddMessage(Message message)
        {
            string jsonString = message.EncapsulateMsg();
            if (jsonString.Length < 10000)
            Messages.Enqueue(message);
            OnPropertyChanged("newMessage");
        }
        public Message PullMessage()
        {
            if(Messages.Count != 0)
             return Messages.Dequeue();
            return null;
        }

        public void AddTextMessage(string message)
        {
                TextMessages.Enqueue(message);
            OnPropertyChanged("newTextMessage");
        }
        public string PullTextMessage()
        {
            if (TextMessages.Count != 0)
                return TextMessages.Dequeue();
            return null;
        }

        public void AddOptionnalTextMessage(string message)
        {
            TextMessages.Enqueue(message);
            OnPropertyChanged("newOptionnalTextMessage");
        }
        public string PullOptionnalTextMessage()
        {
            if (TextMessages.Count != 0)
                return TextMessages.Dequeue();
            return null;
        }

        public int Count()
        {
            return Messages.Count;
        }

        public int CountText()
        {
            return TextMessages.Count;
        }

        public int CountTextOption()
        {
            return OptionnalTextMessages.Count;
        }



    }
}