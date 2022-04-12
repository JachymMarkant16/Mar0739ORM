using Mar0739ORM.Models;
using ProjektDB.ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Mar0739ORM
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("----NEVER DELETE SOMETHING WITH ID 1-----");
                Console.WriteLine("User - 1");
                Console.WriteLine("Reservation - 2");
                Console.WriteLine("Game - 3");
                Console.WriteLine("Requirement - 4");
                Console.WriteLine("Reciept - 5");
                Console.WriteLine("Stock - 6");
                var table = Convert.ToInt32(Console.ReadLine());
                switch (table)
                {
                    case 1:
                        Console.WriteLine("Create - 1");
                        Console.WriteLine("Detail - 2");
                        Console.WriteLine("Update - 3");
                        Console.WriteLine("Delete - 4");
                        var action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                User testUser = new User()
                                {
                                    City = "Ostrava",
                                    Country = "CZ",
                                    Email = "jachym.markant.st@vsb.cz",
                                    FirstName = "Jachym",
                                    LastName = "Markant",
                                    Phone = "123456789",
                                    PostalCode = "703 00",
                                    Role = "Pracovník",
                                    Street = "Nereknu"
                                };
                                UserRepository.CreateUser(testUser);
                                break;
                            case 2:
                                User detailUser = UserRepository.FindUserById(1);
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(detailUser))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(detailUser);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 3:
                                User editUser = UserRepository.FindUserById(1);
                                Console.WriteLine("User Edit");
                                editUser.Phone = new Random().Next(999999999).ToString();
                                UserRepository.UpdateUser(editUser);
                                detailUser = UserRepository.FindUserById(1);
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(detailUser))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(detailUser);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 4:
                                Console.WriteLine("Type id to delete");
                                int a = Convert.ToInt32(Console.ReadLine());
                                UserRepository.DeleteUserById(a);
                                break;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Create - 1");
                        Console.WriteLine("Update - 2");
                        Console.WriteLine("Create reciept - 3");
                        Console.WriteLine("Reservations list - 4");
                        Console.WriteLine("My reservations list - 5");
                        Console.WriteLine("Reservation cancel - 6");
                        Console.WriteLine("Reservation not used - 7");
                        Console.WriteLine("Reservation accepted - 8");
                        Console.WriteLine("Reservation completed - 9");
                        Console.WriteLine("Check not paid reservations - 10");
                        action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                Console.WriteLine("Zadejte datum ve tvaru YYYY-MM-DD");
                                string date = Console.ReadLine();
                                Reservation testReserv = new Reservation()
                                {
                                    AddInfo = "Ahojda jak je",
                                    Date = Convert.ToDateTime(date),
                                    Game = GameRepository.FindGame(1),
                                    Length = 55,
                                    State = "K potvrzení",
                                    User = UserRepository.FindUserById(1)
                                };
                                try
                                {
                                    ReservationRepository.CreateReservation(testReserv);
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Rezervace v tomto datu existuje");
                                }
                                break;
                            case 2:
                                List<Reservation> res = new List<Reservation>();
                                res.AddRange(ReservationRepository.FindAllReservations());
                                Console.WriteLine("--------BEFORE UPDATE-----");
                                foreach (var ress in res)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(ress))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(ress);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                    ress.Length = new Random().Next(75);
                                    ReservationRepository.UpdateReservation(ress);
                                }
                                res = ReservationRepository.FindAllReservations();
                                Console.WriteLine("--------AFTER UPDATE-----");
                                foreach (var ress in res)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(ress))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(ress);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 3:
                                var reservations = ReservationRepository.FindAllReservations();
                                ReservationRepository.CreateRecieptForReservation(reservations[reservations.Count - 1]);
                                break;
                            case 4:
                                res = ReservationRepository.FindAllReservations();
                                Console.WriteLine("--------ALL RESERVATIONS-----");
                                foreach (var ress in res)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(ress))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(ress);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 5:
                                res = ReservationRepository.FindAllReservationsByUserId(1);
                                Console.WriteLine("--------ALL RESERVATIONS-----");
                                foreach (var ress in res)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(ress))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(ress);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 6:
                                var currRest = ReservationRepository.FindAllReservations();
                                var toUseResb = currRest[currRest.Count - 1];
                                ReservationRepository.CancelReservation(toUseResb);
                                Console.WriteLine("--------BEFORE CANCEL-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                currRest = ReservationRepository.FindAllReservations();
                                toUseResb = currRest[currRest.Count - 1];
                                Console.WriteLine("--------AFTER CANCEL-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 7:
                                Console.WriteLine("--------USER RECIEPTS BEFORE FUNC----");
                                var reciepts = RecieptRepository.GetRecieptsForUser(new User() { Id = 1 });
                                foreach (var notpaidReciept in reciepts)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(notpaidReciept))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(notpaidReciept);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                ReservationRepository.CheckNotPaidReservations();
                                Console.WriteLine("--------USER RECIEPTS AFTER FUNC----");
                                reciepts = RecieptRepository.GetRecieptsForUser(new User() { Id = 1 });
                                foreach (var notpaidReciept in reciepts)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(notpaidReciept))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(notpaidReciept);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 8:
                                currRest = ReservationRepository.FindAllReservations();
                                toUseResb = currRest[currRest.Count - 1];
                                ReservationRepository.AcceptReservation(toUseResb);
                                Console.WriteLine("--------BEFORE ACCEPTANCE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                currRest = ReservationRepository.FindAllReservations();
                                toUseResb = currRest[currRest.Count - 1];
                                Console.WriteLine("--------AFTER ACCEPTANCE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 9:
                                currRest = ReservationRepository.FindAllReservations();
                                toUseResb = currRest[currRest.Count - 1];
                                ReservationRepository.AcceptReservation(toUseResb);
                                Console.WriteLine("--------BEFORE COMPLETION-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                currRest = ReservationRepository.FindAllReservations();
                                toUseResb = currRest[currRest.Count - 1];
                                Console.WriteLine("--------AFTER COMPLETION-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(toUseResb))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(toUseResb);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 10:
                                ReservationRepository.CheckNotPaidReservations();
                                break;
                        }
                        break;
                    case 3:

                        Console.WriteLine("List - 1");
                        Console.WriteLine("Detail - 2");
                        Console.WriteLine("Create - 3");
                        Console.WriteLine("Update - 4");
                        Console.WriteLine("State change - 5");
                        Console.WriteLine("Delete - 6");
                        action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                var games = GameRepository.FindAllGames();
                                foreach (var game in games)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(game))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(game);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 2:
                                games = GameRepository.FindAllGames();
                                var detailGame = GameRepository.FindGame(games[games.Count - 1].Id);
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(detailGame))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(detailGame);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 3:
                                Game createGame = new Game()
                                {
                                    Length = 50,
                                    Name = "Hounted house xy",
                                    Price = 500,
                                    State = "Připraveno"
                                };
                                GameRepository.CreateGame(createGame);
                                break;
                            case 4:
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----BEFORE UPDATE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(games[games.Count - 1]))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(games[games.Count - 1]);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                games[games.Count - 1].Length = new Random().Next(120);
                                GameRepository.UpdateGame(games[games.Count - 1]);
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----AFTER UPDATE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(games[games.Count - 1]))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(games[games.Count - 1]);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 5:
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----BEFORE STATE CHANGE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(games[games.Count - 1]))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(games[games.Count - 1]);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                games[games.Count - 1].Length = new Random().Next(120);
                                GameRepository.UpdateGameState("Poškozeno", games[games.Count - 1].Id);
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----AFTER STATE CHANGE-----");
                                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(games[games.Count - 1]))
                                {
                                    string name = descriptor.Name;
                                    object value = descriptor.GetValue(games[games.Count - 1]);
                                    Console.WriteLine("{0}={1}", name, value);
                                }
                                break;
                            case 6:
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----GAMES BEFORE DELETE-----");
                                foreach (var game in games)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(game))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(game);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                Console.WriteLine("Zadejte id pro smazani");
                                var idToDel = Convert.ToInt32(Console.ReadLine());
                                GameRepository.DeleteGameById(idToDel);
                                games = GameRepository.FindAllGames();
                                Console.WriteLine("-----GAMES AFTER DELETE-----");
                                foreach (var game in games)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(game))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(game);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                        }
                        break;
                    case 4:
                        Console.WriteLine("List - 1");
                        Console.WriteLine("Create - 2");
                        Console.WriteLine("Complete - 3");
                        action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                var reqs = RequirementRepository.FindAllRequirements();
                                foreach (var req in reqs)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(req))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(req);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 2:
                                Requirement newReq = new Requirement()
                                {
                                    Description = "Rozbite umyvadlo",
                                    State = "Zadáno",
                                    User = UserRepository.FindUserById(1)
                                };
                                RequirementRepository.CreateRequirement(newReq);
                                break;
                            case 3:
                                Console.WriteLine("Zadejte id pozadavku, ktery chcete dokoncit");
                                var reqId = Convert.ToInt32(Console.ReadLine());
                                RequirementRepository.CompleteReq(reqId);
                                break;
                        }
                        break;
                    case 5:
                        Console.WriteLine("List for user - 1");
                        Console.WriteLine("List with filter - 2");
                        Console.WriteLine("Create - 3");
                        Console.WriteLine("Pay - 4");
                        action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                var userWithReciept = UserRepository.FindUserById(1);
                                var recieptsForUser = RecieptRepository.GetRecieptsForUser(userWithReciept);
                                foreach (var rec in recieptsForUser)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(rec))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(rec);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 2:
                                Console.WriteLine("Zadejte filtr jmeno:");
                                var nameFilter = Console.ReadLine();
                                Console.WriteLine("Zadejte filtr prijmeni:");
                                var lastNameFilter = Console.ReadLine();
                                Console.WriteLine("Zadejte filtr email:");
                                var emailFilter = Console.ReadLine();
                                var recieptsWithFilter = RecieptRepository.GetRecieptsByFilter(nameFilter, lastNameFilter, emailFilter);
                                foreach (var rec in recieptsWithFilter)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(rec))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(rec);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 3:
                                Reciept newReciept = new Reciept()
                                {
                                    Date = new DateTime(2022, 2, 2),
                                    Description = "Nezaplatils",
                                    Price = 500,
                                    State = "K zaplacení",
                                    User = UserRepository.FindUserById(1),
                                    Stock = StockRepository.FindAllStock().Select(x => x).Where(x => x.Id == 1).FirstOrDefault()
                                };
                                RecieptRepository.CreateReciept(newReciept);
                                break;
                            case 4:
                                Console.WriteLine("Zadejte id uctenky k zaplaceni");
                                var idToPay = Convert.ToInt32(Console.ReadLine());
                                RecieptRepository.PayReciept(idToPay);
                                break;
                        }
                        break;
                    case 6:
                        Console.WriteLine("List - 1");
                        Console.WriteLine("Create - 2");
                        Console.WriteLine("Update all - 3");
                        Console.WriteLine("Stock count check - 4");
                        Console.WriteLine("Delete - 5");
                        Console.WriteLine("Emails for low stock - 6");
                        action = Convert.ToInt32(Console.ReadLine());
                        switch (action)
                        {
                            case 1:
                                var stock = StockRepository.FindAllStock();
                                foreach (var sto in stock)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(sto))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(sto);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 2:
                                Stock newStock = new Stock()
                                {
                                    Count = 5,
                                    Description = "Piticko",
                                    Name = "Dobre piticko",
                                    Price = 50
                                };
                                StockRepository.CreateStock(newStock);
                                break;
                            case 3:
                                stock = StockRepository.FindAllStock();
                                Console.WriteLine("-----BEFORE UPDATE-----");
                                foreach (var sto in stock)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(sto))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(sto);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                foreach (var sto in stock)
                                {
                                    sto.Price = new Random().Next(120);
                                }
                                StockRepository.UpdateStock(stock);
                                stock = StockRepository.FindAllStock();
                                Console.WriteLine("-----AFTER UPDATE-----");
                                foreach (var sto in stock)
                                {
                                    foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(sto))
                                    {
                                        string name = descriptor.Name;
                                        object value = descriptor.GetValue(sto);
                                        Console.WriteLine("{0}={1}", name, value);
                                    }
                                }
                                break;
                            case 4:
                                break;
                            case 5:
                                Console.WriteLine("Zadejte id polozky ke smazani");
                                var stockId = Convert.ToInt32(Console.ReadLine());
                                StockRepository.DeleteStockById(stockId);
                                break;
                            case 6:
                                var ems = StockRepository.GetEmailToSendForLowStock(3, "jaja");
                                Console.WriteLine("Email ids to send:");
                                foreach(var em in ems)
                                {
                                    Console.WriteLine(em);
                                }
                                break;
                        }
                        break;
                }

            }
        }
    }
}
