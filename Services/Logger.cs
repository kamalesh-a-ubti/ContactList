using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactList.Services
{
    public class Logger{

    private static string logFilePath = "log.txt";
    
        public static void log(string Msg){
            try{
                using (StreamWriter writer = new StreamWriter(logFilePath,true)){
                    writer.WriteLine($"{DateTime.Now} :{Msg}");
                }
            }catch(Exception ex){
                Console.WriteLine($"logging Failed : {ex.Message}");
            }
        }
    }
}