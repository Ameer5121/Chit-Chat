using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChitChat.Models;
using System.IO;
using Newtonsoft.Json;

namespace ChitChat.Services
{
    static class CredentialsSaveService
    {

        public static void SaveCredentials(UserCredentials userCredentials) => 
            File.WriteAllText($"{Environment.CurrentDirectory}/credentials.json", JsonConvert.SerializeObject(userCredentials));


        public static UserCredentials LoadCredentials()
        {
            return null;
        }
    }
}
