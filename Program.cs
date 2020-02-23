using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./questions.db";
            List<ClassInfo> allClasses = new List<ClassInfo>();
            PlayerClass playerAnswer = new PlayerClass();
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                 var questionDBConnect = connection.CreateCommand();
                questionDBConnect.CommandText = "SELECT * FROM Questions"; 
                
                 using (var questionReader = questionDBConnect.ExecuteReader())
                {
                    playerAnswer.Combat = 0;
                    playerAnswer.Magic = 0;
                    playerAnswer.Stealth = 0;
                    for (int i = 1; questionReader.Read(); i++)
                    {
                        string[] answer = new string[4];
                        answer[0] = questionReader.GetString(1);
                        answer[1] = questionReader.GetString(2);
                        answer[2] = questionReader.GetString(3);
                        answer[3] = questionReader.GetString(4);

                        foreach (var item in answer)
                        {
                            Console.WriteLine(item + "\n");
                        }
                        int userAnswer = 0;
                        while (true)
                        {
                            int.TryParse(Console.ReadLine(),out userAnswer);
                            if (userAnswer > 0 && userAnswer < 4)
                            {
                                break;
                            }else
                            {
                                Console.WriteLine("Введите корректный номер ответа");
                            }
                        }
                        playerAnswer.QuestionNum = i;
                        if (userAnswer == 1 && playerAnswer.Combat < 7)
                        {
                            playerAnswer.Combat++;

                        }else if(userAnswer == 2 && playerAnswer.Magic < 7)
                        {
                            playerAnswer.Magic++;
                        }else if(userAnswer == 3 && playerAnswer.Stealth < 7)
                        {
                            playerAnswer.Stealth++;
                        }
                        
                    }
                }
                
                var classesDBConnect = connection.CreateCommand();
                classesDBConnect.CommandText = "SELECT * FROM Classes";
                
                using (var classesReader = classesDBConnect.ExecuteReader())
                {
                    
                    while (classesReader.Read())
                    {
                        ClassInfo tmpClassInfo = new ClassInfo();
                        tmpClassInfo.ClassName = classesReader.GetString(1);
                        tmpClassInfo.Combat = classesReader.GetString(2);
                        tmpClassInfo.Magic = classesReader.GetString(3);
                        tmpClassInfo.Stealth = classesReader.GetString(4);
                        allClasses.Add(tmpClassInfo);
                    }
                }

                connection.Close();
            }
             /* foreach (var item in allClasses)
            {
                    System.Console.Write(item.ClassName + " ");
                    System.Console.Write(item.Combat + " ");
                    System.Console.Write(item.Magic + " ");
                    System.Console.Write(item.Stealth + " \n");
            }
            System.Console.Write(playerAnswer.Combat + " ");
            System.Console.Write(playerAnswer.Magic + " ");
            System.Console.Write(playerAnswer.Stealth + " \n"); */

            #region Check
                if (playerAnswer.Combat == 7)
                {
                    Console.WriteLine("Ваш класс войн");
                }
                if (playerAnswer.Magic == 7)
                {
                    Console.WriteLine("Ваш класс Маг");
                }
                if (playerAnswer.Stealth == 7)
                {
                    Console.WriteLine("Ваш класс Вор");
                }
                if(playerAnswer.Combat == 4 && playerAnswer.Magic < 7 && playerAnswer.Stealth < 7)
                {
                    Console.WriteLine("Ваш класс жулик");
                }
                foreach (var item in allClasses)
                {
                    int combat = -1;
                    int magic = -1;
                    int stealth = -1;
                    if(int.TryParse(item.Combat, out combat) && int.TryParse(item.Magic, out magic) && int.TryParse(item.Stealth, out stealth))
                    {
                        int.TryParse(item.Combat, out combat);
                        int.TryParse(item.Magic, out magic);
                         int.TryParse(item.Stealth, out stealth);
                    }
                    
                    if (playerAnswer.Combat == combat && playerAnswer.Magic == magic && playerAnswer.Stealth == stealth)
                    {
                        System.Console.WriteLine(item.ClassName);
                    }
                }
            #endregion
        }
    }
}
