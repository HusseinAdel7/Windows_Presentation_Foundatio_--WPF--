using ChatApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChatApp.MVVM.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<ContactModel> Contacts { get; set; }
        public MainViewModel()
        {

            Messages = new ObservableCollection<MessageModel>();
            Contacts = new ObservableCollection<ContactModel>();
            Messages.Add(new MessageModel
            {
                Username="Hussein Adel",
                UsernameColor="#409aff",
                ImageSource= "https://i.imgur.com/i2szTsp.png",
                Message ="Test",
                Time=DateTime.Now, 
                IsNativeOrigin=false,
                FirstMessage=true
            });
            for(int i=0; i<3; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Hussein Adel",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/i2szTsp.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = false,
                    FirstMessage = false
                });
            }
            for (int i = 0; i < 4; i++)
            {
                Messages.Add(new MessageModel
                {
                    Username = "Mohamed Ahmed",
                    UsernameColor = "#409aff",
                    ImageSource = "https://i.imgur.com/i2szTsp.png",
                    Message = "Test",
                    Time = DateTime.Now,
                    IsNativeOrigin = true,

                });
            }
            Messages.Add(new MessageModel
            {
                Username = "Mohamed Ahmed",
                UsernameColor = "#409aff",
                ImageSource = "https://i.imgur.com/i2szTsp.png",
                Message = "Last",
                Time = DateTime.Now,
                IsNativeOrigin = true,

            });
            for (int i = 0;i < 5;i++)
            {
                Contacts.Add(new ContactModel
                {
                    Username=$"Hussein {i}",
                    ImageSource = "https://i.imgur.com/i2szTsp.png",
                    Messages =Messages
                });
            }
        }
    }

}
