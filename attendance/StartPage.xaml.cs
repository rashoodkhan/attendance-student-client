using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace attendance
{

    public class UserCredentials
    {
        public int id { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }
    }

    public partial class StartPage : PhoneApplicationPage
    {
        private MobileServiceCollection<UserCredentials,UserCredentials> credentials;

        private IMobileServiceTable<UserCredentials> credentialTable = App.MobileService.GetTable<UserCredentials>();

        private string username;
        private string password;

        public StartPage()
        {
            InitializeComponent();
        }

        private async void GetData()
        {
            username = USERNAME.Text;
            password = PASSWORD.Password;

            if(username.Equals("") || password.Equals("")){
                MessageBox.Show("Username or Password cannot be empty","Error",MessageBoxButton.OK);
            }
            else{
                try
                {
                    credentials = await credentialTable.
                        Where(userCredentials => userCredentials.username == username && userCredentials.password == password).
                        ToCollectionAsync();
                }
                catch(MobileServiceInvalidOperationException e)
                {
                    MessageBox.Show(e.Message,"Database Error",MessageBoxButton.OK);
                }

                int count = credentials.Count();
                if (count == 1)
                {
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GetData();

        }

    }
}