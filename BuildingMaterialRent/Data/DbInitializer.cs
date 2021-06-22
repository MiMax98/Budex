using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMaterialRent.Data
{
    public class DbInitializer
    {
        public static void Initialize(RentDbContext context, UserManager<User> userManager)
        {
            context.Database.EnsureCreated();

            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Piły i pilarki"
                    },
                    new Category
                    {
                        Name = "Myjki ciśnieniowe"
                    },
                    new Category
                    {
                        Name = "Wkrętarki"
                    },
                    new Category
                    {
                        Name = "Akcesoria i osprzęt"
                    },
                    new Category
                    {
                        Name = "Szlifierki i polerki"
                    },
                    new Category
                    {
                        Name = "Spawarki"
                    }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList();
                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Pilarka tarczowa ręczna",
                        Description = "Nieduża waga urządzenia oraz odpowiednio wyprofilowany uchwyt sprawiają, że wielogodzinna praca pilarką nie jest męcząca.",
                        CategoryId = categories.Single(c => c.Name == "Piły i pilarki").CategoryId,
                        Price = 160,
                        Image = "pilarka.jpg",
                        Stock = 20
                    },
                    new Product
                    {
                        Name = "Myjka ciśnieniowa Karcher",
                        Description = "Urządzenie to nie tylko znacznie skraca czas mycia ale również zużywa nawet do 80% mniej wody w stosunku do węża ogrodowego co pozwala użytkownikowi zaoszczędzić również pieniądze.",
                        CategoryId = categories.Single(c => c.Name == "Myjki ciśnieniowe").CategoryId,
                        Price = 290,
                        Image = "myjka.jpg",
                        Stock = 10
                    },
                    new Product
                    {
                        Name = "Wkrętarka akumlatorowa",
                        Description = "Wkrętarka akumulatorowa jest urządzeniem przeznaczonym do wiercenia w różnych materiałach oraz do wkręcania i wykręcenia śrub, wkrętów, blachowkrętów itp.",
                        CategoryId = categories.Single(c => c.Name == "Wkrętarki").CategoryId,
                        Price = 228,
                        Image = "wkrętarka.jpg",
                        Stock = 35
                    },
                    new Product
                    {
                        Name = "Komplet wierteł",
                        Description = "Komplet wierteł w mocnej kasecie 100szt",
                        CategoryId = categories.Single(c => c.Name == "Akcesoria i osprzęt").CategoryId,
                        Price = 81,
                        Image = "wiertła.jpg",
                        Stock = 15
                    },
                    new Product
                    {
                        Name = "Szlifierka kątowa",
                        Description = "Wszechstronna elektryczna szlifierka kątowa o dużej mocy, wysokiej wydajności, bezpieczeństwie i stabilności.",
                        CategoryId = categories.Single(c => c.Name == "Szlifierki i polerki").CategoryId,
                        Price = 135,
                        Image = "szlifierka.jpg",
                        Stock = 40
                    },
                    new Product
                    {
                        Name = "Spawarka MIGOMAT",
                        Description = "Stosowany jest do lutospawania blach ocynkowanych, spawania stali nierdzewnej, stali węglowych i niskostopowych oraz aluminium.",
                        CategoryId = categories.Single(c => c.Name == "Spawarki").CategoryId,
                        Price = 1699,
                        Image = "spawarka.jpg",
                        Stock = 5
                    },
                    new Product
                    {
                        Name = "Wiretarko-wkrętarka RYOBI",
                        Description = "Bezprzewodowa wiertarko-wkrętarka to idealne narzędzie do wiercenia w drewnie lub metalu oraz do wkręcania",
                        CategoryId = categories.Single(c => c.Name == "Wkrętarki").CategoryId,
                        Price=450,
                        Image = "wkrętarka2.webp",
                        Stock = 20
                    }
                };

                context.Products.AddRange(products);
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
                var admin = new Role
                {
                    Name = "Admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString("N")
                };
                var user = new Role
                {
                    Name = "User",
                    NormalizedName = "user",
                    ConcurrencyStamp = Guid.NewGuid().ToString("N")
                };
                context.Roles.Add(admin);
                context.Roles.Add(user);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com",
                    FirstName = "admin",
                    LastName = "admin"
                };
                userManager.CreateAsync(admin, "admin").Wait();
                userManager.AddToRoleAsync(admin, "Admin").Wait();
            }
        }
    }
}
